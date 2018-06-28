Public Class MainViewModel
    Inherits ViewModelBase

    Private _messageBus As IMessageBus
    Private _dialogService As IDialogService
    Private _navigationService As INavigationService
    Private _settingsService As ISettingsService
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

        _messageBus.Subscribe(Of RemoveWeatherSourceMessage)(AddressOf OnRemoveWeatherSource)
        _messageBus.Subscribe(Of StatusMessage)(Sub(message)
                                                    Status = message.Status
                                                    SubStatus = message.SubStatus
                                                    Progress = message.Progress
                                                End Sub)
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
        Try

            Await _weatherService.GetClosestWeatherSourceByZipCodeAsync("46845")

            Dim locations = Await _settingsService.LoadLocationsAsync

            Await _settingsService.ResetDatabaseAsync
        Catch ex As Exception
            ' TODO: We need to recover here!
            Throw
        End Try

    End Function

    Private Async Function LoadWeatherSourcesAsync() As Task
        ' Do we have weather sources?
        Dim weatherSources As IEnumerable(Of WeatherSource) = Await _settingsService.GetWeatherSourcesAsync()
        If weatherSources IsNot Nothing Then
            Dim weatherSourceViewModels As IEnumerable(Of WeatherSourceListItemViewModel)

            weatherSourceViewModels = weatherSources.Select(Function(model) New WeatherSourceListItemViewModel(_dialogService, _messageBus, model))
            WeatherSourcesViewModel.WeatherSources = New ObservableCollection(Of WeatherSourceListItemViewModel)(weatherSourceViewModels)
        End If
    End Function

    Private Async Sub OnRemoveWeatherSource(message As RemoveWeatherSourceMessage)
        Try
            Dim weatherSources As New List(Of WeatherSource)(WeatherSourcesViewModel.WeatherSources.Select(Function(viewModel) New WeatherSource() With {
                                                                                                               .ZipCode = viewModel.ZipCode}))
            weatherSources.Remove(message.WeatherSource)
            Dim weatherSourceViewModels As IEnumerable(Of WeatherSourceListItemViewModel)

            weatherSourceViewModels = weatherSources.Select(Function(model) New WeatherSourceListItemViewModel(_dialogService, _messageBus, model))
            WeatherSourcesViewModel.WeatherSources = New ObservableCollection(Of WeatherSourceListItemViewModel)(weatherSourceViewModels)

            Await _settingsService.SaveWeatherSourcesAsync(weatherSources)

        Catch ex As Exception

        End Try
    End Sub

#End Region



End Class
