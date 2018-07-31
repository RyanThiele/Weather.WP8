Namespace Services

    Public Class DialogService
        Implements IDialogService

        Public Function ShowYesNoDialog(title As String, message As String) As Boolean Implements IDialogService.ShowYesNoDialog
            Dim result As MessageBoxResult = MessageBox.Show(message, title, MessageBoxButton.OKCancel)
            Select Case result
                Case MessageBoxResult.Yes, MessageBoxResult.OK
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        Public Function ShowOkDialog(title As String, message As String) As Boolean Implements IDialogService.ShowOkDialog
            Dim result As MessageBoxResult = MessageBox.Show(message, title, MessageBoxButton.OK)
            Select Case result
                Case MessageBoxResult.Yes, MessageBoxResult.OK
                    Return True
                Case Else
                    Return False
            End Select
        End Function
    End Class

End Namespace
