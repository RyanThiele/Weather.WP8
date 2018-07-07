Imports System.Threading.Tasks

Namespace Services

    Public Interface ISettingsService
        Function ResetDatabaseAsync() As Task
        Function RefreshLocationsAsync(deleteExisting As Boolean) As Task
        Function RefreshStationsAsync(deleteExisting As Boolean) As Task
        Function SetSelectedLocationsAsync(locations As IEnumerable(Of Models.Location)) As Task
        Function GetSelectedLocationsAsync() As Task(Of IEnumerable(Of Models.Location))
    End Interface

End Namespace
