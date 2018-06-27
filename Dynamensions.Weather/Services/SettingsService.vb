Imports System.Threading.Tasks

Public Class SettingsService
    Implements ISettingsService



    Public Function LoadZipCodesAsync() As Task(Of IEnumerable(Of String)) Implements ISettingsService.LoadZipCodesAsync
        Dim tcs As New TaskCompletionSource(Of IEnumerable(Of String))

        Dim settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings
        If settings.Contains("ZipCodes") Then
            tcs.SetResult(CType(settings("ZipCodes"), IEnumerable(Of String)))
        Else
            tcs.SetResult(Nothing)
        End If

        Return tcs.Task
    End Function


    Public Function SaveZipCodesAsync(zipCodes As IEnumerable(Of String)) As Task Implements ISettingsService.SaveZipCodesAsync
        Dim tcs As New TaskCompletionSource(Of Boolean)

        Dim settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings
        settings("ZipCodes") = zipCodes.ToList()

        Return tcs.Task
    End Function

    Public Function GetWeatherSourcesAsync() As Task(Of DataResult(Of IEnumerable(Of WeatherSource))) Implements ISettingsService.GetWeatherSourcesAsync
        Dim tcs As New TaskCompletionSource(Of DataResult(Of IEnumerable(Of WeatherSource)))

        Dim items As IEnumerable(Of WeatherSource) = Nothing
        Dim errorMessage As String = Nothing
        Dim exception As Exception = Nothing

        Try
            Dim settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings
            If settings.Contains("WeatherSources") Then
                items = (CType(settings("WeatherSources"), IEnumerable(Of WeatherSource)))
            End If
        Catch ex As Exception
            errorMessage = "Cannot load Weather Sources from settings: " & ex.Message & "."
            exception = ex
        End Try

        tcs.SetResult(New DataResult(Of IEnumerable(Of WeatherSource))(items, errorMessage, exception))
        Return tcs.Task
    End Function
End Class
