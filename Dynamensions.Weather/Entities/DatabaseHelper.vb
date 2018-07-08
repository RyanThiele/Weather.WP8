Imports System.IO.IsolatedStorage
Imports System.ComponentModel
Imports Dynamensions.Weather.Entities
Imports System.IO
Imports System.Reflection

Friend Module DatabaseHelper

    Friend Enum DatabaseDataOptions
        Refresh
        Delete
        None
    End Enum


    Private _settings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings

    Private Const RESET_STATUS As String = "Resetting Data. This could take a while depending on your phone speed."
    Private Const METAR_STATIONS_ADDRESS As String = "https://www.aviationweather.gov/static/adds/metars/stations.txt"

    Private Event StatusUpdated(status As StatusMessage)

    Friend Sub MoveReferenceDatabase(databaseFileName As String, replaceExisting As Boolean, Optional messageBus As IMessageBus = Nothing)
        Dim iso = IsolatedStorageFile.GetUserStoreForApplication()
        Dim dbExists As Boolean = iso.FileExists(databaseFileName)

        If replaceExisting AndAlso dbExists Then iso.DeleteFile(databaseFileName)
        If Not replaceExisting AndAlso dbExists Then Return

        ' if we get here, the user want to replace the database.
        Using input = Application.GetResourceStream(New Uri(databaseFileName, UriKind.Relative)).Stream
            Using output As IsolatedStorageFileStream = iso.CreateFile(databaseFileName)
                Dim readBuffer As Byte() = New Byte(4096) {}
                Dim bytesRead As Integer = -1

                bytesRead = input.Read(readBuffer, 0, readBuffer.Length)
                While bytesRead > 0
                    output.Write(readBuffer, 0, bytesRead)
                    bytesRead = input.Read(readBuffer, 0, readBuffer.Length)
                End While
            End Using
        End Using
    End Sub

    Friend Async Function MoveReferenceDatabaseAsync(databaseFileName As String, replaceExisting As Boolean, messageBus As IMessageBus) As Task
        Dim iso = IsolatedStorageFile.GetUserStoreForApplication()
        Dim exists As Boolean = iso.FileExists(databaseFileName)
        If exists Then
            If replaceExisting Then
                iso.DeleteFile(databaseFileName)
            Else
                Using input = Application.GetResourceStream(New Uri(databaseFileName, UriKind.Relative)).Stream
                    Using output As IsolatedStorageFileStream = iso.CreateFile(databaseFileName)
                        Dim readBuffer As Byte() = New Byte(4096) {}
                        Dim bytesRead As Integer = -1
                        input.Seek(0, IO.SeekOrigin.Begin)

                        bytesRead = Await input.ReadAsync(readBuffer, 0, readBuffer.Length)
                        While bytesRead > 0
                            Await output.WriteAsync(readBuffer, 0, bytesRead)
                            bytesRead = Await input.ReadAsync(readBuffer, 0, readBuffer.Length)
                        End While
                    End Using
                End Using
            End If
        End If
    End Function

    Friend Async Function RefreshDataAsync(deleteExistingDatabase As Boolean, stationOption As DatabaseDataOptions, locationOption As DatabaseDataOptions, messageBus As IMessageBus) As Task
        If deleteExistingDatabase Then
            Using db As New DbDataContext
                Await db.DeleteDatabaseAsync
                Await db.CreateDatabaseAsync

                'db.Stations.InsertOnSubmit(New Station With {
                '                           .AutoFlag = "A",
                '                           .AviationFlag = "A",
                '                           .CountryCode = "CC",
                '                           .Elevation = 1,
                '                           .IATA = "I",
                '                           .ICAO = "I",
                '                           .Latitude = 1.0,
                '                           .Longitude = 1.0,
                '                           .METAR = False,
                '                           .NEXRAD = True,
                '                           .OfficeFlag = "O",
                '                           .Priority = 0,
                '                           .StateOrProvince = "IN",
                '                           .Station = "S",
                '                           .SYNOP = 1,
                '                           .UpperAirOrWindFlag = "U",
                '                           .LastUpdated = DateTime.Now})

                'Await db.SubmitChangesAsync

                'Dim station As Station = db.Stations.SingleOrDefault(Function(s) s.Priority = 0)
                'db.Locations.InsertOnSubmit(New Location With {
                '                            .City = "C",
                '                            .County = "Co",
                '                            .Latitude = 0,
                '                            .Longitude = 1,
                '                            .PostalCode = 1,
                '                            .StateOrProvince = "S",
                '                            .StateOrProvinceAbbreviation = "ss",
                '                            .StationId = station.Id,
                '                            .LastUpdated = DateTime.Now})

                'db.Locations.InsertOnSubmit(New Location With {
                '                            .City = "C",
                '                            .County = "Co",
                '                            .Latitude = 0,
                '                            .Longitude = 1,
                '                            .PostalCode = 2,
                '                            .StateOrProvince = "S",
                '                            .StateOrProvinceAbbreviation = "ss",
                '                            .StationId = station.Id,
                '                            .LastUpdated = DateTime.Now})


                'db.SubmitChanges()
            End Using
        End If

        'Return

        ' Refresh the stations.
        Select Case stationOption
            Case DatabaseDataOptions.Refresh
                Await RefreshStations_NewAsync(False, 10, messageBus)
            Case DatabaseDataOptions.Delete
                Await RefreshStations_NewAsync(True, 10, messageBus)
            Case DatabaseDataOptions.None
        End Select

        ' Refresh the locations.
        Select Case locationOption
            Case DatabaseDataOptions.Refresh
                Await RefreshLocations_NewAsync(False, 10, messageBus)
            Case DatabaseDataOptions.Delete
                Await RefreshLocations_NewAsync(True, 10, messageBus)
            Case DatabaseDataOptions.None
        End Select

    End Function

#Region "Working"


    Friend Async Function RefreshStationsAsync(deleteExisting As Boolean, messageBus As IMessageBus) As Task

        If deleteExisting Then
            Using db As New DbDataContext
                db.Stations.DeleteAllOnSubmit(db.Stations)
                Await db.SubmitChangesAsync
            End Using
        End If

        ' download the stations data.
        Dim client As New WebClient
        Dim fileContent As String

        messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Checking for an updated list of stations from NOAA..."})
        fileContent = Await client.DownloadStringTaskAsync("https://www.aviationweather.gov/static/adds/metars/stations.txt")
        If fileContent Is Nothing Then Return

        Dim lines() As String = fileContent.Split(vbLf).Where(Function(line) Not line.StartsWith("!")).Where(Function(line) line.Length >= 83).ToArray
        Dim totalLocationsCount As Decimal = lines.Count
        Dim currentLocationCount As Decimal = 0
        Dim duplicatedLocationCount As Integer = 0

        For currentLocationCount = 1 To totalLocationsCount
            Dim line As String = lines(currentLocationCount - 1)
            Dim station As New Station()
            Dim synopValue As String = Nothing
            Dim latitudeValue As String = Nothing
            Dim longitudeValue As String = Nothing
            Dim elevationValue As String = Nothing
            Dim priorityValue As String = Nothing

            If line.Length >= 2 Then station.StateOrProvince = line.Substring(0, 2).Trim
            If line.Length >= 19 Then station.Station = line.Substring(3, 16).Trim
            If line.Length >= 25 Then station.ICAO = line.Substring(20, 4).Trim
            If line.Length >= 30 Then station.IATA = line.Substring(26, 3).Trim
            If line.Length >= 38 Then synopValue = line.Substring(32, 5).Trim
            If line.Length >= 45 Then latitudeValue = line.Substring(39, 7).Trim
            If line.Length >= 53 Then longitudeValue = line.Substring(47, 7).Trim
            If line.Length >= 59 Then elevationValue = line.Substring(55, 4).Trim
            If line.Length >= 64 Then station.METAR = line.Substring(62, 2).Trim = "X"
            If line.Length >= 67 Then station.NEXRAD = line.Substring(65, 2).Trim = "X"
            If line.Length >= 70 Then station.AviationFlag = line.Substring(68, 2).Trim
            If line.Length >= 73 Then station.UpperAirOrWindFlag = line.Substring(71, 2).Trim
            If line.Length >= 76 Then station.AutoFlag = line.Substring(74, 2).Trim
            If line.Length >= 79 Then station.OfficeFlag = line.Substring(77, 2).Trim
            If line.Length >= 81 Then priorityValue = line.Substring(79, 2).Trim
            If line.Length >= 83 Then station.CountryCode = line.Substring(81, 2).Trim

            Dim synop As Integer = 0
            Int32.TryParse(synopValue, synop)
            station.SYNOP = synop

            Dim latitude As Decimal = 0, longitude As Decimal = 0, elevation As Decimal = 0
            latitudeValue = latitudeValue.Replace(" ", ".")
            longitudeValue = longitudeValue.Replace(" ", ".")

            Decimal.TryParse(latitudeValue.Substring(0, latitudeValue.Length - 1), latitude)
            If latitudeValue.EndsWith("S") Then latitude *= -1
            station.Latitude = latitude

            Decimal.TryParse(longitudeValue.Substring(0, longitudeValue.Length - 1), longitude)
            If longitudeValue.EndsWith("W") Then longitude *= -1
            station.Longitude = longitude

            Decimal.TryParse(elevationValue, elevation)
            station.Elevation = elevation

            Dim priority As Integer = 0
            Int32.TryParse(priorityValue, priority)
            station.Priority = priority

            Using db As New DbDataContext
                Dim existingEntity = db.Stations.Where(Function(entity) (Not String.IsNullOrWhiteSpace(station.ICAO) AndAlso entity.ICAO.Equals(station.ICAO)) OrElse
                                                           (Not String.IsNullOrWhiteSpace(station.IATA) AndAlso entity.ICAO.Equals(station.IATA))).SingleOrDefault
                If existingEntity Is Nothing Then
                    station.LastUpdated = DateTime.Now
                    db.Stations.InsertOnSubmit(station)
                Else
                    duplicatedLocationCount += 1

                    existingEntity.StateOrProvince = station.StateOrProvince
                    existingEntity.ICAO = station.ICAO
                    existingEntity.IATA = station.IATA
                    existingEntity.SYNOP = station.SYNOP
                    existingEntity.Latitude = station.Latitude
                    existingEntity.Longitude = station.Longitude
                    existingEntity.Elevation = station.Elevation
                    existingEntity.METAR = station.METAR
                    existingEntity.NEXRAD = station.NEXRAD
                    existingEntity.AviationFlag = station.AviationFlag
                    existingEntity.UpperAirOrWindFlag = station.UpperAirOrWindFlag
                    existingEntity.AutoFlag = station.AutoFlag
                    existingEntity.OfficeFlag = station.OfficeFlag
                    existingEntity.Priority = station.Priority
                    existingEntity.CountryCode = station.CountryCode
                    existingEntity.LastUpdated = DateTime.Now
                End If

                Await db.SubmitChangesAsync
            End Using

            Dim subStatus As String
            If duplicatedLocationCount = 1 Then
                subStatus = "Parsing stations (" & currentLocationCount & " of " & totalLocationsCount & "). " & vbLf & duplicatedLocationCount & " updated. " & currentLocationCount - duplicatedLocationCount & " added."
            Else
                subStatus = "Parsing stations (" & currentLocationCount & " of " & totalLocationsCount & "). " & vbLf & duplicatedLocationCount & " updated. " & currentLocationCount - duplicatedLocationCount & " added."
            End If

            messageBus.Publish(New StatusMessage With {
                               .Status = RESET_STATUS,
                               .SubStatus = subStatus,
                               .Progress = Decimal.Divide(currentLocationCount, totalLocationsCount) * 100})

        Next
    End Function

    Friend Async Function RefreshLocationsAsync(deleteExisting As Boolean, messageBus As IMessageBus) As Task
        If deleteExisting Then
            Using db As New DbDataContext
                db.Stations.DeleteAllOnSubmit(db.Stations)
                Await db.SubmitChangesAsync
            End Using
        End If


        Using stream As IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Dynamensions.Weather.ZipCodes.csv")
            Using reader As New StreamReader(stream)
                Dim fileContent = Await reader.ReadToEndAsync
                If fileContent Is Nothing Then Return

                Dim lines() As String = fileContent.Split(vbLf).ToArray
                Dim totalLineCount As Decimal = lines.Count
                Dim currentLineCount As Decimal = 0
                Dim duplicatedLineCount As Integer = 0

                For currentLineCount = 2 To totalLineCount
                    Dim line As String = lines(currentLineCount - 1)
                    If line = "Zip Code,Place Name,State,State Abbreviation,County,Latitude,Longitude" & vbLf Then Continue For
                    ' this is a csv so we will split the line with that.
                    Dim values As String() = line.Split(","c)
                    If values.Length = 0 Then Continue For
                    Dim model As New Location()
                    If values.Length >= 1 Then model.PostalCode = values(0).PadLeft(5, "0")
                    If values.Length >= 2 Then model.City = values(1).Replace("""", "")
                    If values.Length >= 3 Then model.StateOrProvince = values(2).Replace("""", "")
                    If values.Length >= 4 Then model.StateOrProvinceAbbreviation = values(3).Replace("""", "")
                    If values.Length >= 5 Then model.County = values(4).Replace("""", "")
                    If values.Length >= 6 Then model.Latitude = values(5).Replace("""", "").ToDecimal()
                    If values.Length >= 7 Then model.Longitude = values(6).Replace("""", "").ToDecimal()

                    Dim closestStation As Station = Await GetClosesStationsByLatLongAsync(model.Latitude, model.Longitude)

                    If closestStation IsNot Nothing Then
                        Using db As New DbDataContext
                            Dim location As Location = db.Locations.SingleOrDefault(Function(o) o.PostalCode.Equals(model.PostalCode))
                            If location Is Nothing Then
                                model.StationId = closestStation.Id
                                model.LastUpdated = DateTime.Now
                                db.Locations.InsertOnSubmit(model)
                            Else
                                location.LastUpdated = DateTime.Now
                                location.StationId = closestStation.Id
                            End If

                            Await db.SubmitChangesAsync()
                        End Using
                    End If

                    Dim subStatus As String
                    If duplicatedLineCount = 1 Then
                        subStatus = "Parsing stations (" & currentLineCount & " of " & totalLineCount & "). " & vbLf & duplicatedLineCount & " updated. " & currentLineCount - duplicatedLineCount & " added."
                    Else
                        subStatus = "Parsing stations (" & currentLineCount & " of " & totalLineCount & "). " & vbLf & duplicatedLineCount & " updated. " & currentLineCount - duplicatedLineCount & " added."
                    End If

                    messageBus.Publish(New StatusMessage With {
                                       .Status = RESET_STATUS,
                                       .SubStatus = subStatus,
                                       .Progress = Decimal.Divide(currentLineCount, totalLineCount) * 100})
                Next
            End Using
        End Using
    End Function

    Private Function GetClosesStationsByLatLongAsync(latitude As Decimal, longitude As Decimal) As Task(Of Station)
        Dim tcs As New TaskCompletionSource(Of Station)
        Dim worker As New BackgroundWorker
        Dim foundStation As Station = Nothing

        AddHandler worker.DoWork, Sub(s, e)
                                      Using db As New DbDataContext
                                          Dim query = From station In db.Stations
                                                      Order By Math.Abs(Math.Abs(station.Latitude.Value) - Math.Abs(latitude)) + Math.Abs(Math.Abs(station.Longitude.Value) - Math.Abs(longitude)), station.Priority
                                                      Select station

                                          foundStation = query.FirstOrDefault

                                          'While foundStation Is Nothing OrElse range >= 5
                                          '    'foundStation = db.Stations.
                                          '    '    Where(Function(station) station.Latitude <= latitude + range).
                                          '    '    Where(Function(station) station.Latitude >= latitude - range).
                                          '    '    Where(Function(station) station.Longitude <= longitude + range).
                                          '    '    Where(Function(station) station.Longitude >= longitude - range).
                                          '    '    OrderByDescending(Function(station) (station.Latitude - latitude) + (station.Longitude - longitude)).
                                          '    '    FirstOrDefault

                                          '    range += 0.02
                                          'End While
                                      End Using
                                  End Sub

        AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                  tcs.SetResult(foundStation)
                                              End Sub

        worker.RunWorkerAsync()
        Return tcs.Task
    End Function


#End Region

#Region "Experimental"

    Private Sub UpdateStatus(messageBus As IMessageBus, parsingObject As String, currentCount As Integer, duplicatedCount As Integer, totalCount As Integer, totalExecutionTasksCount As Integer, startTime As DateTime)
        Dim subStatus As String
        If duplicatedCount = 1 Then
            subStatus = "Parsing stations (" & currentCount & " of " & totalCount & "). " & vbLf & duplicatedCount & " updated. " & currentCount - duplicatedCount & " added. " & totalExecutionTasksCount & " tasks."
        Else
            subStatus = "Parsing stations (" & currentCount & " of " & totalCount & "). " & vbLf & duplicatedCount & " updated. " & currentCount - duplicatedCount & " added. " & totalExecutionTasksCount & " tasks."
        End If

        Dim timeTaken As TimeSpan = DateTime.Now - startTime
        Dim timeLeft As Double = (timeTaken.TotalSeconds / currentCount) * (totalCount - currentCount)


        messageBus.Publish(New StatusMessage With {
                          .Status = RESET_STATUS,
                          .SubStatus = subStatus,
                          .Progress = Decimal.Divide(currentCount, totalCount) * 100,
                          .TimeRemaining = TimeSpan.FromSeconds(timeLeft)})
    End Sub

    Private Async Function RefreshStations_NewAsync(deleteExisting As Boolean, totalExecutions As Integer, messageBus As IMessageBus) As Task
        If deleteExisting Then
            Using db As New DbDataContext
                db.Stations.DeleteAllOnSubmit(db.Stations)
                Await db.SubmitChangesAsync
            End Using
        End If

        ' download the stations data.
        Dim client As New WebClient
        Dim fileContent As String

        messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Checking for an updated list of stations from NOAA..."})
        fileContent = Await client.DownloadStringTaskAsync("https://www.aviationweather.gov/static/adds/metars/stations.txt")
        If fileContent Is Nothing Then Return

        Dim lines() As String = fileContent.Split(vbLf).Where(Function(line) Not line.StartsWith("!")).Where(Function(line) line.Length >= 83).ToArray
        Dim totalLocationsCount As Decimal = lines.Count
        Dim currentLocationCount As Decimal = 0
        Dim duplicatedLocationCount As Integer = 0
        Dim startTime As DateTime = DateTime.Now

        Dim parsingTasks As New List(Of Task)

        ' add 10 tasks to the list
        While currentLocationCount < totalExecutions
            parsingTasks.Add(ParseStationAsync(lines(currentLocationCount)))
            currentLocationCount += 1
        End While

        While currentLocationCount < totalLocationsCount - 1

            UpdateStatus(messageBus, "stations", currentLocationCount, duplicatedLocationCount, totalLocationsCount, parsingTasks.Count, startTime)

            Dim finishedTask As Task = Await TaskEx.WhenAny(parsingTasks)
            parsingTasks.Remove(finishedTask)
            finishedTask = Nothing

            currentLocationCount += 1
            parsingTasks.Add(ParseStationAsync(lines(currentLocationCount)))
        End While

        Await TaskEx.WhenAll(parsingTasks)
        UpdateStatus(messageBus, "stations", currentLocationCount, duplicatedLocationCount, totalLocationsCount, parsingTasks.Count, startTime)
    End Function

    Private Async Function ParseStationAsync(line As String) As Task
        Dim station As New Station()
        Dim synopValue As String = Nothing
        Dim latitudeValue As String = Nothing
        Dim longitudeValue As String = Nothing
        Dim elevationValue As String = Nothing
        Dim priorityValue As String = Nothing

        If line.Length >= 2 Then station.StateOrProvince = line.Substring(0, 2).Trim
        If line.Length >= 19 Then station.Station = line.Substring(3, 16).Trim
        If line.Length >= 25 Then station.ICAO = line.Substring(20, 4).Trim
        If line.Length >= 30 Then station.IATA = line.Substring(26, 3).Trim
        If line.Length >= 38 Then synopValue = line.Substring(32, 5).Trim
        If line.Length >= 45 Then latitudeValue = line.Substring(39, 7).Trim
        If line.Length >= 53 Then longitudeValue = line.Substring(47, 7).Trim
        If line.Length >= 59 Then elevationValue = line.Substring(55, 4).Trim
        If line.Length >= 64 Then station.METAR = line.Substring(62, 2).Trim = "X"
        If line.Length >= 67 Then station.NEXRAD = line.Substring(65, 2).Trim = "X"
        If line.Length >= 70 Then station.AviationFlag = line.Substring(68, 2).Trim
        If line.Length >= 73 Then station.UpperAirOrWindFlag = line.Substring(71, 2).Trim
        If line.Length >= 76 Then station.AutoFlag = line.Substring(74, 2).Trim
        If line.Length >= 79 Then station.OfficeFlag = line.Substring(77, 2).Trim
        If line.Length >= 81 Then priorityValue = line.Substring(79, 2).Trim
        If line.Length >= 83 Then station.CountryCode = line.Substring(81, 2).Trim

        Dim synop As Integer = 0
        Int32.TryParse(synopValue, synop)
        station.SYNOP = synop

        Dim latitude As Decimal = 0, longitude As Decimal = 0, elevation As Decimal = 0
        latitudeValue = latitudeValue.Replace(" ", ".")
        longitudeValue = longitudeValue.Replace(" ", ".")

        Decimal.TryParse(latitudeValue.Substring(0, latitudeValue.Length - 1), latitude)
        If latitudeValue.EndsWith("S") Then latitude *= -1
        station.Latitude = latitude

        Decimal.TryParse(longitudeValue.Substring(0, longitudeValue.Length - 1), longitude)
        If longitudeValue.EndsWith("W") Then longitude *= -1
        station.Longitude = longitude

        Decimal.TryParse(elevationValue, elevation)
        station.Elevation = elevation

        Dim priority As Integer = 0
        Int32.TryParse(priorityValue, priority)
        station.Priority = priority

        Using db As New DbDataContext
            Dim existingEntity = db.Stations.Where(Function(entity) (Not String.IsNullOrWhiteSpace(station.ICAO) AndAlso entity.ICAO.Equals(station.ICAO)) OrElse
                                                       (Not String.IsNullOrWhiteSpace(station.IATA) AndAlso entity.ICAO.Equals(station.IATA))).SingleOrDefault
            If existingEntity Is Nothing Then
                station.LastUpdated = DateTime.Now
                db.Stations.InsertOnSubmit(station)
            Else
                existingEntity.StateOrProvince = station.StateOrProvince
                existingEntity.ICAO = station.ICAO
                existingEntity.IATA = station.IATA
                existingEntity.SYNOP = station.SYNOP
                existingEntity.Latitude = station.Latitude
                existingEntity.Longitude = station.Longitude
                existingEntity.Elevation = station.Elevation
                existingEntity.METAR = station.METAR
                existingEntity.NEXRAD = station.NEXRAD
                existingEntity.AviationFlag = station.AviationFlag
                existingEntity.UpperAirOrWindFlag = station.UpperAirOrWindFlag
                existingEntity.AutoFlag = station.AutoFlag
                existingEntity.OfficeFlag = station.OfficeFlag
                existingEntity.Priority = station.Priority
                existingEntity.CountryCode = station.CountryCode
                existingEntity.LastUpdated = DateTime.Now
            End If

            Await db.SubmitChangesAsync
        End Using
    End Function

    Friend Async Function RefreshLocations_NewAsync(deleteExisting As Boolean, totalExecutions As Integer, messageBus As IMessageBus) As Task
        If deleteExisting Then
            Using db As New DbDataContext
                db.Stations.DeleteAllOnSubmit(db.Stations)
                Await db.SubmitChangesAsync
            End Using
        End If

        Dim lines() As String
        Dim totalLineCount As Decimal
        Dim currentLineCount As Decimal = 0
        Dim duplicatedLineCount As Integer = 0
        Dim startTime As DateTime = DateTime.Now

        Using stream As IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Dynamensions.Weather.ZipCodes.csv")
            Using reader As New StreamReader(stream)
                Dim fileContent = Await reader.ReadToEndAsync
                If fileContent Is Nothing Then Return

                lines = fileContent.Split(vbLf).ToArray
                totalLineCount = lines.Length
            End Using
        End Using

        ' add 10 tasks to the list
        Dim parsingTasks As New List(Of Task)

        While currentLineCount < totalExecutions
            parsingTasks.Add(ParseLocationAsync(lines(currentLineCount)))
            currentLineCount += 1
        End While

        While currentLineCount < totalLineCount - 1
            UpdateStatus(messageBus, "stations", currentLineCount, duplicatedLineCount, totalLineCount, parsingTasks.Count, startTime)

            Dim finishedTask As Task = Await TaskEx.WhenAny(parsingTasks)
            parsingTasks.Remove(finishedTask)
            finishedTask = Nothing

            currentLineCount += 1
            parsingTasks.Add(ParseLocationAsync(lines(currentLineCount)))
        End While

        Await TaskEx.WhenAll(parsingTasks)
        UpdateStatus(messageBus, "stations", currentLineCount, duplicatedLineCount, totalLineCount, parsingTasks.Count, startTime)
    End Function

    Private Async Function ParseLocationAsync(line As String) As Task
        If line = "Zip Code,Place Name,State,State Abbreviation,County,Latitude,Longitude" & vbCr & "" Then Return
        ' this is a csv so we will split the line with that.
        Dim values As String() = line.Split(","c)
        If values.Length = 0 Then Return
        Dim model As New Location()
        If values.Length >= 1 Then model.PostalCode = values(0).PadLeft(5, "0")
        If values.Length >= 2 Then model.City = values(1).Replace("""", "")
        If values.Length >= 3 Then model.StateOrProvince = values(2).Replace("""", "")
        If values.Length >= 4 Then model.StateOrProvinceAbbreviation = values(3).Replace("""", "")
        If values.Length >= 5 Then model.County = values(4).Replace("""", "")
        If values.Length >= 6 Then model.Latitude = values(5).Replace("""", "").ToDecimal()
        If values.Length >= 7 Then model.Longitude = values(6).Replace("""", "").ToDecimal()

        Dim closestStation As Station = Await GetClosesStationsByLatLongAsync(model.Latitude, model.Longitude)

        If closestStation IsNot Nothing Then
            Using db As New DbDataContext
                Dim location As Location = db.Locations.SingleOrDefault(Function(o) o.PostalCode.Equals(model.PostalCode))
                If location Is Nothing Then
                    model.StationId = closestStation.Id
                    model.LastUpdated = DateTime.Now
                    db.Locations.InsertOnSubmit(model)
                Else
                    location.LastUpdated = DateTime.Now
                    location.StationId = closestStation.Id
                End If

                Await db.SubmitChangesAsync()
            End Using
        End If
    End Function


#End Region


End Module
