Imports Dynamensions.Weather.Services

Public Class SettingsViewModel
    Inherits ViewModelBase

    Private ReadOnly _settingsService As ISettingsService
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _messageBus As IMessageBus

#Region "Constructors"

    Public Sub New(messageBus As IMessageBus, settingsService As ISettingsService, dialogService As IDialogService)
        _messageBus = messageBus
        _settingsService = settingsService
        _dialogService = dialogService

        _messageBus.Subscribe(Of StatusMessage)(Sub(message)
                                                    Status = message.Status
                                                    SubStatus = message.SubStatus
                                                    Progress = message.Progress
                                                    TimeRemaining = message.TimeRemaining
                                                End Sub)
    End Sub

#End Region

#Region "Properties"

#Region "SubStatus"

    Dim _SubStatus As String
    Public Property SubStatus As String
        Get
            Return _SubStatus
        End Get
        Private Set(value As String)
            _SubStatus = value
            OnPropertyChanged("SubStatus")
        End Set
    End Property

#End Region


#Region "TimeRemaining"

    Dim _TimeRemaining As TimeSpan
    Public Property TimeRemaining As TimeSpan
        Get
            Return _TimeRemaining
        End Get
        Set(value As TimeSpan)
            _TimeRemaining = value
            OnPropertyChanged("TimeRemaining")
        End Set
    End Property

#End Region


#Region "Progress"

    Dim _Progress As Double
    Public Property Progress As Double
        Get
            Return _Progress
        End Get
        Private Set(value As Double)
            _Progress = value
            OnPropertyChanged("Progress")
        End Set
    End Property

#End Region

#Region "IsResetting"

    Dim _IsResetting As Boolean
    Public Property IsResetting As Boolean
        Get
            Return _IsResetting
        End Get
        Private Set(value As Boolean)
            _IsResetting = value
            OnPropertyChanged("IsResetting")
        End Set
    End Property

#End Region

#End Region

#Region "Commands"


#Region "ResetDatabaseCommand"
    Dim _ResetDatabaseCommand As ICommand
    Public ReadOnly Property ResetDatabaseCommand As ICommand
        Get
            If _ResetDatabaseCommand Is Nothing Then
                _ResetDatabaseCommand = New Commands.RelayCommand(AddressOf ExecuteResetDatabase, AddressOf CanExecuteResetDatabase)
            End If

            Return _ResetDatabaseCommand
        End Get
    End Property

    Private Function CanExecuteResetDatabase() As Boolean
        Return True
    End Function

    Private Async Sub ExecuteResetDatabase()
        If _dialogService.ShowYesNoDialog("Warning! Data Will Be Deleted!", "Are you sure you want to reset the database? All data will be rebuilt!") Then
            IsResetting = True
            Await DatabaseHelper.RefreshDataAsync(true, DatabaseDataOptions.Refresh, DatabaseDataOptions.Refresh, _messageBus)
            IsResetting = False
        End If
    End Sub

#End Region


#End Region

#Region "Methods"



#End Region



End Class
