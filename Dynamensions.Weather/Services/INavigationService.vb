Imports System.Threading.Tasks

Public Interface INavigationService
    Sub NavigatePrevious()
    Sub NavigateTo(Of TViewModel As ViewModelBase)()
    Sub RemoveLastFromBackStack()
    Sub RemoveBackStack()
End Interface

