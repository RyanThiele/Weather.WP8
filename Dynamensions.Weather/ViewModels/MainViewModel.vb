Imports Dynamensions.Weather.Services
Imports System.Threading

Public Class MainViewModel
    Inherits ViewModelBase

    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _settingsService As ISettingsService
    Private ReadOnly _weatherService As IWeatherService
    Private _locationTokenSource As CancellationTokenSource

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

    Public Property LocationViewModels As New ObservableCollection(Of LocationViewModel)


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

#Region "AddLocationCommand"
    Dim _AddLocationCommand As ICommand
    Public ReadOnly Property AddLocationCommand As ICommand
        Get
            If _AddLocationCommand Is Nothing Then
                _AddLocationCommand = New Commands.RelayCommand(AddressOf ExecuteAddLocation, AddressOf CanExecuteAddLocation)
            End If

            Return _AddLocationCommand
        End Get
    End Property

    Private Function CanExecuteAddLocation() As Boolean
        Return True
    End Function

    Private Sub ExecuteAddLocation()
        _navigationService.NavigateTo(Of AddWeatherSourceViewModel)()
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
            ' We do no have a location. Detect the current location as accept that.
            If locations Is Nothing Then
                Dim tokenSource As New CancellationTokenSource
                Dim currentGeoCoordinate As Models.GeoCoordinate = Await _settingsService.GetCurrentLocationAsync
                Dim location As Models.Location = Await _weatherService.GetLocationByLatitudeLongitudeAsync(currentGeoCoordinate.Latitude, currentGeoCoordinate.Longitude, tokenSource.Token)

                If location Is Nothing Then Return
                locations = {location}
                Await _settingsService.SetSelectedLocationsAsync(locations)
            End If

            Dim currentObservationsTasks As New List(Of Task)
            LocationViewModels.Clear()
            For index = 0 To locations.Count - 1
                Dim viewModel As New LocationViewModel(locations(index), _settingsService, _weatherService)
                LocationViewModels.Add(viewModel)
                currentObservationsTasks.Add(viewModel.InitializeAsync(locations(index)))
            Next

            Await TaskEx.WhenAll(currentObservationsTasks)
        Catch ex As Exception
            ' TODO: We need to recover here!

        End Try

        _IsIntilizing = False
    End Function




#End Region



End Class
