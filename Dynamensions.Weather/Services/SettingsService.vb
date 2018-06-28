Imports System.Threading.Tasks
Imports System.Reflection
Imports System.IO
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Linq

<Table>
Public Class Test
    <Column> Public Property Stuff
End Class

Public Class SettingsService
    Implements ISettingsService

    Private ReadOnly _messageBus As IMessageBus
    Private Const RESET_STATUS As String = "Resetting Data. This could take a while depending on your phone speed."


    Public Sub New(messageBus As IMessageBus)
        _messageBus = messageBus
    End Sub

#Region "Helpers"

    Private Async Function ParseLocationsAsync() As Task
        Dim total As Long
        Dim zipCodeData As New List(Of Location)
        Using stream As IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Dynamensions.Weather.ZipCodes.csv")
            Using reader As New StreamReader(stream)
                While reader.Peek() <> -1

                    Dim line As String = Await reader.ReadLineAsync()
                    If line = "Zip Code,Place Name,State,State Abbreviation,County,Latitude,Longitude" Then Continue While
                    ' this is a csv so we will split the line with that.
                    Dim values As String() = line.Split(","c)
                    If values.Length = 0 Then Continue While
                    Dim model As New Location()
                    If values.Length >= 1 Then model.ZipCode = values(0).PadLeft(5, "0")
                    If values.Length >= 2 Then model.City = values(1).Replace("""", "")
                    If values.Length >= 3 Then model.StateOrProvince = values(2).Replace("""", "")
                    If values.Length >= 4 Then model.StateOrProvinceAbbreviation = values(3).Replace("""", "")
                    If values.Length >= 5 Then model.County = values(4).Replace("""", "")
                    If values.Length >= 6 Then model.Latitude = values(5).Replace("""", "").ToDecimal()
                    If values.Length >= 7 Then model.Longitude = values(6).Replace("""", "").ToDecimal()

                    Await AddLocationAsync(model)
                    total += 1
                    _messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Parsing Zip Codes (" & total & ")..."})
                End While
            End Using
        End Using
    End Function

    Private Function AddLocationAsync(location As Location) As Task
        Dim tcs As New TaskCompletionSource(Of Object)
        Dim worker As New BackgroundWorker
        AddHandler worker.DoWork, Sub()
                                      Using db As New DbDataContext(DbDataContext.DBConnectionString)
                                          Dim existingLocation As Location = db.Locations.SingleOrDefault(Function(m) m.ZipCode.Equals(location.ZipCode))
                                          If existingLocation IsNot Nothing Then
                                              existingLocation.City = location.City
                                              existingLocation.StateOrProvince = location.StateOrProvince
                                              existingLocation.Latitude = location.Latitude
                                              existingLocation.Longitude = location.Longitude
                                          Else
                                              db.Locations.InsertOnSubmit(location)
                                          End If

                                          db.SubmitChanges()
                                      End Using
                                  End Sub

        'AddLocation(location)
        AddHandler worker.RunWorkerCompleted, Sub()
                                                  tcs.SetResult(Nothing)
                                              End Sub
        worker.RunWorkerAsync()
        Return tcs.Task
    End Function

#End Region


    Public Async Function LoadLocationsAsync() As Task(Of IEnumerable(Of String)) Implements ISettingsService.LoadLocationsAsync

        ''Return tcs.Task
    End Function


    'Public Function SaveZipCodesAsync(zipCodes As IEnumerable(Of String)) As Task Implements ISettingsService.SaveZipCodesAsync
    '    Dim tcs As New TaskCompletionSource(Of Boolean)

    '    Dim settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings
    '    settings("ZipCodes") = zipCodes.ToList()

    '    Return tcs.Task
    'End Function

    Public Function GetWeatherSourcesAsync() As Task(Of IEnumerable(Of WeatherSource)) Implements ISettingsService.GetWeatherSourcesAsync
        Dim tcs As New TaskCompletionSource(Of IEnumerable(Of WeatherSource))

        Dim settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings
        If settings.Contains("WeatherSources") Then
            tcs.SetResult(CType(settings("WeatherSources"), IEnumerable(Of WeatherSource)))
        Else
            tcs.SetResult(Nothing)
        End If

        Return tcs.Task
    End Function

    Public Function SaveWeatherSourcesAsync(weatherSources As IEnumerable(Of WeatherSource)) As Task Implements ISettingsService.SaveWeatherSourcesAsync
        Dim tcs As New TaskCompletionSource(Of Boolean)

        Dim settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings
        settings("WeatherSources") = weatherSources.ToList()

        Return tcs.Task
    End Function

    Public Function GetStationsAsync() As Task(Of Location) Implements ISettingsService.GetStationsAsync

    End Function

    Public Async Function ResetDatabaseAsync() As Task Implements ISettingsService.ResetDatabaseAsync

        _messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Resetting Database..."})
        Await DatabaseHelper.MoveReferenceDatabaseAsync("Weather.sdf")

        _messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Parsing Zip Codes..."})
        Await ParseLocationsAsync()

    End Function
End Class
