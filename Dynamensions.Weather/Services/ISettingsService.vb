Imports System.Threading.Tasks

Public Interface ISettingsService
    Function ResetDatabaseAsync() As Task
    Function RefreshStationsAsync() As Task


    Function GetWeatherSourcesAsync() As Task(Of IEnumerable(Of WeatherSource))
    Function SaveWeatherSourcesAsync(weatherSources As IEnumerable(Of WeatherSource)) As Task

    Function GetStationsAsync() As Task(Of Location)
    Function LoadLocationsAsync() As Task(Of IEnumerable(Of String))
    'Function SaveZipCodesAsync(zipCodes As IEnumerable(Of String)) As Task
End Interface
