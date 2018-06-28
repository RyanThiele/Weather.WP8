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


#End Region

#Region "Commands"
#End Region

#Region "Methods"
#End Region




End Class
