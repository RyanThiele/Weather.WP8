Imports System.Windows.Input

Public Class ZipCodeListItemViewModel
    Inherits ViewModelBase

    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _messageBus As IMessageBus

    Public Sub New(dialogService As IDialogService, messageBus As IMessageBus)
        _dialogService = dialogService
        _messageBus = messageBus
    End Sub

#Region "ZipCode"

    Dim _ZipCode As String
    Public Property ZipCode As String
        Get
            Return _ZipCode
        End Get
        Set(value As String)
            _ZipCode = value
            OnPropertyChanged("ZipCode")
        End Set
    End Property

#End Region



#Region "RemoveCommand"
    Dim _RemoveCommand As ICommand
    Public ReadOnly Property RemoveCommand As ICommand
        Get
            If _RemoveCommand Is Nothing Then
                _RemoveCommand = New Commands.RelayCommand(AddressOf ExecuteRemove, AddressOf CanExecuteRemove)
            End If

            Return _RemoveCommand
        End Get
    End Property

    Private Function CanExecuteRemove() As Boolean
        Return True
    End Function

    Private Sub ExecuteRemove()
        If _dialogService.ShowYesDialog("Confirm Delete", "Are you sure you want to delete " & ZipCode & "? The action cannot be undone.") Then
            _messageBus.Publish(New RemoveZipCodeMessage With {.ZipCodeToRemove = ZipCode})
        End If
    End Sub

#End Region


End Class
