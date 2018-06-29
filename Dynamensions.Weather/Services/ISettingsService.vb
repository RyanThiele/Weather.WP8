Imports System.Threading.Tasks

Public Interface ISettingsService
    Function ResetDatabaseAsync() As Task
    Function RefreshStationsAsync() As Task

    Function GetSelectedWeatherSourcesAsync() As Task(Of IEnumerable(Of WeatherSource))
    Function SetSelectedWeatherSourcesAsync(selectedWeatherSources As IEnumerable(Of WeatherSource)) As Task

End Interface
