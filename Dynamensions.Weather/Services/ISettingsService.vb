Imports System.Threading.Tasks

Public Interface ISettingsService
    Function GetWeatherSourcesAsync() As Task(Of IEnumerable(Of WeatherSource))
    Function SaveWeatherSourcesAsync(weatherSources As IEnumerable(Of WeatherSource)) As Task


    Function LoadZipCodesAsync() As Task(Of IEnumerable(Of String))
    Function SaveZipCodesAsync(zipCodes As IEnumerable(Of String)) As Task
End Interface
