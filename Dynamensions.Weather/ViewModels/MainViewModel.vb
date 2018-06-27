Public Class MainViewModel
    Inherits ViewModelBase

    Private _messageBus As IMessageBus
    Private _navigationService As INavigationService
    Private _settingsService As ISettingsService

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(messageBus As IMessageBus, navigationService As INavigationService, settingsService As ISettingsService)
        _messageBus = messageBus
        _navigationService = navigationService
        _settingsService = settingsService
    End Sub

#End Region

#Region "Properties"

    Public Property WeatherSourcesViewModel As New WeatherSourcesViewModel

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

            ' Do we have weather sources?
            Dim weatherSources As DataResult(Of IEnumerable(Of WeatherSource)) = Await _settingsService.GetWeatherSourcesAsync()
            If weatherSources.IsSuccessful AndAlso weatherSources.Result IsNot Nothing Then
                Me.WeatherSourcesViewModel.WeatherSources = New ObservableCollection(Of WeatherSource)(weatherSources.Result)
            Else
                Status = weatherSources.ErrorMessage
            End If
        Catch ex As Exception
            ' TODO: We need to recover here!
            Throw
        End Try

    End Function

#End Region



End Class
