Public Class AddWeatherSourceViewModel
    Inherits ViewModelBase

    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _navigationService As INavigationService

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(messageBus As IMessageBus, dialogService As IDialogService, navigationService As INavigationService)
        _messageBus = messageBus
        _dialogService = dialogService
        _navigationService = navigationService
    End Sub

#End Region

#Region "Properties"
#End Region

#Region "Commands"
#End Region

#Region "Methods"
#End Region




End Class
