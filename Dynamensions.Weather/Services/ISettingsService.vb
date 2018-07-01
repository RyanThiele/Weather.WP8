Imports System.Threading.Tasks

Public Interface ISettingsService
    Function ResetDatabaseAsync() As Task
    Function RefreshStationsAsync() As Task

End Interface
