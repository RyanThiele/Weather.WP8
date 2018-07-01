Public Class WeatherSourceListItemViewModel
    Inherits ViewModelBase

    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _messageBus As IMessageBus
    'Private ReadOnly _model As WeatherSource

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(dialogService As IDialogService, messageBus As IMessageBus)
        _dialogService = dialogService
        _messageBus = messageBus

        'ZipCode = model.ZipCode

        DisplayString = ZipCode & " (" & WeatherStationId & ")"
    End Sub

#End Region

#Region "Properties"

#Region "WeatherStationId"

    Dim _WeatherStationId As String
    Public Property WeatherStationId As String
        Get
            Return _WeatherStationId
        End Get
        Private Set(value As String)
            _WeatherStationId = value
            OnPropertyChanged("WeatherStationId")
        End Set
    End Property

#End Region

#Region "City"

    Dim _City As String
    Public Property City As String
        Get
            Return _City
        End Get
        Private Set(value As String)
            _City = value
            OnPropertyChanged("City")
        End Set
    End Property

#End Region

#Region "State"

    Dim _State As String
    Public Property State As String
        Get
            Return _State
        End Get
        Private Set(value As String)
            _State = value
            OnPropertyChanged("State")
        End Set
    End Property

#End Region

#Region "ZipCode"

    Dim _ZipCode As String
    Public Property ZipCode As String
        Get
            Return _ZipCode
        End Get
        Private Set(value As String)
            _ZipCode = value
            OnPropertyChanged("ZipCode")
        End Set
    End Property

#End Region

#Region "DisplayString"

    Dim _DisplayString As String
    Public Property DisplayString As String
        Get
            Return _DisplayString
        End Get
        Private Set(value As String)
            _DisplayString = value
            OnPropertyChanged("DisplayString")
        End Set
    End Property

#End Region


#End Region

#Region "Commands"

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
            '_messageBus.Publish(New RemoveWeatherSourceMessage With {.WeatherSource = _model})
        End If
    End Sub

#End Region


#End Region

#Region "Methods"


#End Region




End Class
