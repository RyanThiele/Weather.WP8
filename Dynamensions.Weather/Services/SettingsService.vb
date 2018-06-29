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

<Table>
Public Class Test
    <Column> Public Property Stuff
End Class

Public Class SettingsService
    Implements ISettingsService


    Private ReadOnly _messageBus As IMessageBus
    Private Const RESET_STATUS As String = "Resetting Data. This could take a while depending on your phone speed."
    Private Const METAR_STATIONS_ADDRESS As String = "https://www.aviationweather.gov/static/adds/metars/stations.txt"
    Private _settings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings

    Private Enum StatusTypes
        [Global]
        Stations
    End Enum
    Private Event StatusUpdated(status As StatusMessage, statusType As StatusTypes)

    Public Sub New(messageBus As IMessageBus)
        _messageBus = messageBus

        AddHandler StatusUpdated, Sub(status, statusType)
                                      Select Case statusType
                                          Case StatusTypes.Global
                                              _messageBus.Publish(status)
                                          Case StatusTypes.Stations
                                              _messageBus.Publish(New RefreshStationsStatusMessage(status))
                                      End Select
                                  End Sub
    End Sub

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

    Private Function AddLocationAsync(location As Location) As Task



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
    End Function

#End Region

    Public Async Function ResetDatabaseAsync() As Task Implements ISettingsService.ResetDatabaseAsync

        _messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Resetting Database..."})
        Await DatabaseHelper.MoveReferenceDatabaseAsync("Weather.sdf")

        '_messageBus.Publish(New StatusMessage With {.Status = RESET_STATUS, .SubStatus = "Parsing Zip Codes..."})
        'Await ParseLocationsAsync(StatusTypes.Global)

    End Function

    Public Async Function RefreshStationsAsync() As Task Implements ISettingsService.RefreshStationsAsync
        'Dim stationsText As String = Await DownloadStationsAsync()
        'If String.IsNullOrWhiteSpace(stationsText) Then Await ParseLocationsAsync(StatusTypes.Stations)
    End Function

    Public Function GetSelectedWeatherSourcesAsync() As Task(Of IEnumerable(Of WeatherSource)) Implements ISettingsService.GetSelectedWeatherSourcesAsync
        Dim tcs As New TaskCompletionSource(Of IEnumerable(Of WeatherSource))
        Dim worker As New BackgroundWorker
        Dim models As IEnumerable(Of WeatherSource) = Nothing

        AddHandler worker.DoWork, Sub()
                                      If _settings.Contains("SelectedWeatherSources") Then
                                          models = (CType(_settings("SelectedWeatherSources"), IEnumerable(Of WeatherSource)))
                                      End If
                                  End Sub

        AddHandler worker.RunWorkerCompleted, Sub()
                                                  tcs.SetResult(models)
                                              End Sub

        worker.RunWorkerAsync()
        Return tcs.Task
    End Function

    Public Function SetSelectedWeatherSourcesAsync(selectedWeatherSources As IEnumerable(Of WeatherSource)) As Task Implements ISettingsService.SetSelectedWeatherSourcesAsync
        Dim tcs As New TaskCompletionSource(Of Object)
        Dim worker As New BackgroundWorker
        Dim models As IEnumerable(Of WeatherSource) = Nothing

        AddHandler worker.DoWork, Sub()
                                      _settings("SelectedWeatherSources") = selectedWeatherSources
                                  End Sub

        AddHandler worker.RunWorkerCompleted, Sub()
                                                  tcs.SetResult(Nothing)
                                              End Sub

        worker.RunWorkerAsync()
        Return tcs.Task
    End Function
End Class
