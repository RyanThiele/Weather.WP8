Imports System.ComponentModel
Imports System.Reflection
Imports System.Globalization
Imports System.Linq
Imports System.Threading

Public Class NavigationService
    Implements INavigationService

    ' Navigation stack
    Private ReadOnly _viewModels As New List(Of Type)

    Private Function CreateView(viewModelType As Type) As Page
        Dim viewType As Type = GetViewTypeForViewModel(viewModelType)
        If viewType Is Nothing Then Throw New Exception("Cannot locate page type for " & viewModelType.ToString & ".")

        Dim view As Page = CType(Activator.CreateInstance(viewType), Page)
        Return view
    End Function

    Private Function GetViewTypeForViewModel(viewModelType As Type) As Type
        Dim viewName As String = viewModelType.FullName.Replace("Model", String.Empty)
        Dim viewModelAssemblyName As String = viewModelType.Assembly.FullName
        Dim viewAssemblyName As String = String.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName)
        Dim viewType As Type = Type.GetType(viewAssemblyName)
        Return viewType
    End Function

    Public Sub NavigatePrevious() Implements INavigationService.NavigatePrevious
        CType(Application.Current, App).RootFrame.GoBack()
    End Sub

    Public Sub NavigateTo(Of TViewModel As ViewModelBase)() Implements INavigationService.NavigateTo
        ' get the view model type
        Dim viewModelType As Type = GetType(TViewModel)

        Dim vmType = GetType(TViewModel)
        Dim viewName As String = vmType.Name.Replace("Model", String.Empty)
        Dim viewUri As New Uri("/Views/" & viewName & ".xaml", UriKind.Relative)
        Dim rootFrame = CType(Application.Current, App).RootFrame
        Dim isGood = rootFrame.Navigate(viewUri)
    End Sub

    Public Sub RemoveBackStack() Implements INavigationService.RemoveBackStack
        Dim frame As PhoneApplicationFrame = CType(Application.Current, App).RootFrame

        Dim entry As Object = frame.RemoveBackEntry()

        While (entry IsNot Nothing)
            entry = frame.RemoveBackEntry()
        End While
    End Sub

    Public Sub RemoveLastFromBackStack() Implements INavigationService.RemoveLastFromBackStack
        CType(Application.Current, App).RootFrame.RemoveBackEntry()
    End Sub

End Class


