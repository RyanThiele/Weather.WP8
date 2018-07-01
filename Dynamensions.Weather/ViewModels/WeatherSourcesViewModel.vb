Public Class WeatherSourcesViewModel
    Inherits ViewModelBase

    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _settingsService As ISettingsService

#Region "Constructors"

    Public Sub New()
        Title = "Weather Sources"
    End Sub

    Public Sub New(messagebus As IMessageBus, dialogService As IDialogService, navigationService As INavigationService, settingsService As ISettingsService)
        _messageBus = messagebus
        _dialogService = dialogService
        _navigationService = navigationService
        _settingsService = settingsService

        _messageBus.Subscribe(Of RemoveWeatherSourceMessage)(AddressOf OnRemoveWeatherSource)
    End Sub

#End Region

#Region "Properties"

    Private _weatherSources As ObservableCollection(Of WeatherSourceListItemViewModel)
    Public Property WeatherSources() As ObservableCollection(Of WeatherSourceListItemViewModel)
        Get
            Return _weatherSources
        End Get
        Set(ByVal value As ObservableCollection(Of WeatherSourceListItemViewModel))
            _weatherSources = value
            OnPropertyChanged()

            If WeatherSources IsNot Nothing Then
                Title = "Weather Sources (" & WeatherSources.Count.ToString & ")"
            Else
                Title = "Weather Sources"
            End If
        End Set
    End Property

#End Region

#Region "Commands"

#Region "AddWeatherSourceCommand"
    Dim _AddWeatherSourceCommand As ICommand
    Public ReadOnly Property AddWeatherSourceCommand As ICommand
        Get
            If _AddWeatherSourceCommand Is Nothing Then
                _AddWeatherSourceCommand = New Commands.RelayCommand(AddressOf ExecuteAddWeatherSource, AddressOf CanExecuteAddWeatherSource)
            End If

            Return _AddWeatherSourceCommand
        End Get
    End Property

    Private Function CanExecuteAddWeatherSource() As Boolean
        Return True
    End Function

    Private Sub ExecuteAddWeatherSource()
        _navigationService.NavigateTo(Of AddWeatherSourceViewModel)()
    End Sub

#End Region

#End Region

#Region "Methods"

    Public Overrides Async Function InitializeAsync(Optional parameter As Object = Nothing) As Task
        Try
            ' Get the selected sources
            'Dim selectedWeatherSources = Await _settingsService.GetSelectedWeatherSourcesAsync()
            'If selectedWeatherSources IsNot Nothing Then
            '    Dim weatherSourceListItemViewModels As IEnumerable(Of WeatherSourceListItemViewModel) =
            '        selectedWeatherSources.Select(Function(model) New WeatherSourceListItemViewModel(_dialogService, _messageBus, model))

            '    WeatherSources = New ObservableCollection(Of WeatherSourceListItemViewModel)(weatherSourceListItemViewModels)
            'End If
        Catch ex As Exception
            Status = "There was an error loading the Local Weather Data."
        End Try
    End Function

    Private Async Sub OnRemoveWeatherSource(message As RemoveWeatherSourceMessage)
        Try
            'Dim weatherSources As New List(Of WeatherSource)(Me.WeatherSources.Select(Function(viewModel) New WeatherSource() With {.ZipCode = viewModel.ZipCode}))
            'weatherSources.Remove(message.WeatherSource)
            'Dim weatherSourceViewModels As IEnumerable(Of WeatherSourceListItemViewModel)

            'weatherSourceViewModels = weatherSources.Select(Function(model) New WeatherSourceListItemViewModel(_dialogService, _messageBus, model))
            'Me.WeatherSources = New ObservableCollection(Of WeatherSourceListItemViewModel)(weatherSourceViewModels)

            'Await _settingsService.SetSelectedWeatherSourcesAsync(weatherSources)

        Catch ex As Exception

        End Try
    End Sub

#End Region



End Class
