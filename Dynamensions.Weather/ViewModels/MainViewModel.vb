Imports Dynamensions.Weather.Services

Public Class MainViewModel
    Inherits ViewModelBase

    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _settingsService As ISettingsService
    Private ReadOnly _weatherService As IWeatherService

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(messageBus As IMessageBus, dialogService As IDialogService, navigationService As INavigationService, settingsService As ISettingsService, weatherService As IWeatherService)
        _messageBus = messageBus
        _dialogService = dialogService
        _navigationService = navigationService
        _settingsService = settingsService
        _weatherService = weatherService

        _messageBus.Subscribe(Of StatusMessage)(Sub(message)
                                                    Status = message.Status
                                                    SubStatus = message.SubStatus
                                                    Progress = message.Progress
                                                End Sub)

        WeatherSourcesViewModel = New WeatherSourcesViewModel(messageBus, dialogService, navigationService, settingsService)
    End Sub

#End Region

#Region "Properties"

    Public Property WeatherSourcesViewModel As New WeatherSourcesViewModel

#Region "SubStatus"

    Dim _SubStatus As String
    Public Property SubStatus As String
        Get
            Return _SubStatus
        End Get
        Set(value As String)
            _SubStatus = value
            OnPropertyChanged("SubStatus")
        End Set
    End Property

#End Region

#Region "Progress"

    Dim _Progress As Integer
    Public Property Progress As Integer
        Get
            Return _Progress
        End Get
        Set(value As Integer)
            _Progress = value
            OnPropertyChanged("Progress")
        End Set
    End Property

#End Region

#End Region

#Region "Commands"

#Region "SettingsCommand"
    Dim _SettingsCommand As ICommand
    Public ReadOnly Property SettingsCommand As ICommand
        Get
            If _SettingsCommand Is Nothing Then
                _SettingsCommand = New Commands.RelayCommand(AddressOf ExecuteSettings, AddressOf CanExecuteSettings)
            End If

            Return _SettingsCommand
        End Get
    End Property

    Private Function CanExecuteSettings() As Boolean
        Return True
    End Function

    Private Sub ExecuteSettings()
        _
        _navigationService.NavigateTo(Of SettingsViewModel)()
    End Sub

#End Region


#End Region

#Region "Methods"

    ''' <summary>
    ''' Overridden. Method that is invoked when the view has loaded.
    ''' </summary>
    ''' <param name="parameter">Optional. A parameter that can be passed into the view model as the time of initialization.</param>
    ''' <remarks>
    ''' This is the trunk logic for the view model. So, the try/catch will go here.
    ''' </remarks>
    Public Overrides Async Function InitializeAsync(Optional parameter As Object = Nothing) As Task
        If _IsIntilizing Then Return
        _IsIntilizing = True
        Try
            Dim locations As IEnumerable(Of Models.Location) = Await _settingsService.GetSelectedLocationsAsync
            If locations IsNot Nothing Then
                ' when was the last check.
                Dim locationLastChecked As DateTime? = (From location In locations
                                                       Order By location.LastChecked Descending
                                                       Select location.LastChecked).FirstOrDefault


                If Not locationLastChecked.HasValue OrElse
                    (locationLastChecked.HasValue AndAlso locationLastChecked.Value.AddHours(1) < DateTime.Now) Then
                    ' refresh the data.
                    Await _weatherService.GetCurrentObservationByIataAsync(locations(0).WeatherStation.ICAO, Nothing)

                End If
            End If
        Catch ex As Exception
            ' TODO: We need to recover here!
            Throw
        End Try

        _IsIntilizing = False
    End Function




#End Region



End Class
