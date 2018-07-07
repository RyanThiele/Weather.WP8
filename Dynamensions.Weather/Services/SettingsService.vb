Imports System.Threading.Tasks
Imports System.Reflection
Imports System.IO
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.IO.IsolatedStorage
Imports Dynamensions.Weather.Models


Namespace Services

    Public Class SettingsService
        Implements ISettingsService


        Private ReadOnly _messageBus As IMessageBus
        Private Const RESET_STATUS As String = "Resetting Data. This could take a while depending on your phone speed."
        Private Const METAR_STATIONS_ADDRESS As String = "https://www.aviationweather.gov/static/adds/metars/stations.txt"
        Private _settings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings


#Region "Helpers"

        Private Async Function DownloadStationsAsync() As Task(Of String)
            Dim client As New WebClient
            Return Await client.DownloadStringTaskAsync(METAR_STATIONS_ADDRESS)
            client = Nothing
        End Function

        'Private Async Function ParseLocationsAsync(statusType As StatusTypes) As Task
        '    Dim total As Long
        '    Dim zipCodeData As New List(Of Location)
        '    Using stream As IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Dynamensions.Weather.ZipCodes.csv")
        '        Using reader As New StreamReader(stream)
        '            While reader.Peek() <> -1

        '                Dim line As String = Await reader.ReadLineAsync()
        '                If line = "Zip Code,Place Name,State,State Abbreviation,County,Latitude,Longitude" Then Continue While
        '                ' this is a csv so we will split the line with that.
        '                Dim values As String() = line.Split(","c)
        '                If values.Length = 0 Then Continue While
        '                Dim model As New Location()
        '                If values.Length >= 1 Then model.ZipCode = values(0).PadLeft(5, "0")
        '                If values.Length >= 2 Then model.City = values(1).Replace("""", "")
        '                If values.Length >= 3 Then model.StateOrProvince = values(2).Replace("""", "")
        '                If values.Length >= 4 Then model.StateOrProvinceAbbreviation = values(3).Replace("""", "")
        '                If values.Length >= 5 Then model.County = values(4).Replace("""", "")
        '                If values.Length >= 6 Then model.Latitude = values(5).Replace("""", "").ToDecimal()
        '                If values.Length >= 7 Then model.Longitude = values(6).Replace("""", "").ToDecimal()

        '                Await AddLocationAsync(model)
        '                total += 1
        '                RaiseEvent StatusUpdated(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Parsing Zip Codes (" & total & ")..."}, statusType)
        '            End While
        '        End Using
        '    End Using
        'End Function

        'Private Function AddLocationAsync(location As Location) As Task



        'Dim tcs As New TaskCompletionSource(Of Object)
        'Dim worker As New BackgroundWorker
        'AddHandler worker.DoWork, Sub()
        '                              Using db As New DbDataContext(DbDataContext.DBConnectionString)
        '                                  Dim existingLocation As Location = db.Locations.SingleOrDefault(Function(m) m.ZipCode.Equals(location.ZipCode))
        '                                  If existingLocation IsNot Nothing Then
        '                                      existingLocation.City = location.City
        '                                      existingLocation.StateOrProvince = location.StateOrProvince
        '                                      existingLocation.Latitude = location.Latitude
        '                                      existingLocation.Longitude = location.Longitude
        '                                  Else
        '                                      db.Locations.InsertOnSubmit(location)
        '                                  End If

        '                                  db.SubmitChanges()
        '                              End Using
        '                          End Sub

        ''AddLocation(location)
        'AddHandler worker.RunWorkerCompleted, Sub()
        '                                          tcs.SetResult(Nothing)
        '                                      End Sub
        'worker.RunWorkerAsync()
        'Return tcs.Task
        'End Function

#End Region

        Public Async Function ResetDatabaseAsync() As Task Implements ISettingsService.ResetDatabaseAsync
            _messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Resetting Database..."})
            'Await DatabaseHelper.MoveReferenceDatabaseAsync("Weather.sdf")
            Using db As New Entities.DbDataContext
                Await db.DeleteDatabaseAsync
                Await db.CreateDatabaseAsync
            End Using


            Await RefreshStationsAsync(True)
            Await RefreshLocationsAsync(True)

        End Function

        Public Async Function RefreshStationsAsync(deleteExisting As Boolean) As Task Implements ISettingsService.RefreshStationsAsync
            Await DatabaseHelper.RefreshStationsAsync(deleteExisting, _messageBus)
        End Function

        Public Async Function RefreshLocationsAsync(deleteExisting As Boolean) As Task Implements ISettingsService.RefreshLocationsAsync
            Await DatabaseHelper.RefreshLocationsAsync(deleteExisting, _messageBus)
        End Function

        Public Function GetSelectedLocationsAsync() As Task(Of IEnumerable(Of Location)) Implements ISettingsService.GetSelectedLocationsAsync
            Dim tcs As New TaskCompletionSource(Of IEnumerable(Of Models.Location))
            Dim worker As New BackgroundWorker
            Dim models As IEnumerable(Of Models.Location) = Nothing

            AddHandler worker.DoWork, Sub(s, e)
                                          If _settings.Contains("SelectedLocations") Then models = CType(_settings("SelectedLocations"), IEnumerable(Of Models.Location))
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.SetResult(models)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function

        Public Function SetSelectedLocationsAsync(locations As IEnumerable(Of Location)) As Task Implements ISettingsService.SetSelectedLocationsAsync
            Dim tcs As New TaskCompletionSource(Of Object)
            Dim worker As New BackgroundWorker

            AddHandler worker.DoWork, Sub(s, e)
                                          _settings("SelectedLocations") = locations
                                      End Sub

            AddHandler worker.RunWorkerCompleted, Sub(s, e)
                                                      tcs.SetResult(Nothing)
                                                  End Sub

            worker.RunWorkerAsync()
            Return tcs.Task
        End Function
    End Class

End Namespace
