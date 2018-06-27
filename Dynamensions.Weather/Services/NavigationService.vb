Imports System.ComponentModel
Imports System.Reflection
Imports System.Globalization
Imports System.Linq
Imports System.Threading

Public Class NavigationService
    Implements INavigationService


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


