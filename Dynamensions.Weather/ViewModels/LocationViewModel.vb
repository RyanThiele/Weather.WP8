Imports System.Threading
Imports Dynamensions.Weather.Services
Imports Dynamensions.Weather.Models

Public Class LocationViewModel
    Inherits ViewModelBase

    Private ReadOnly _settingsService As ISettingsService
    Private ReadOnly _weatherService As IWeatherService
    Private ReadOnly _model As Models.Location

    Private _cancelationTokenSource As CancellationTokenSource

    Public Enum TemperatureUnits
        Celsius
        Fahrenheit
    End Enum

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(model As Models.Location, settingsService As ISettingsService, weatherService As IWeatherService)
        _model = model
        _settingsService = settingsService
        _weatherService = weatherService

        Title = _model.Address.DisplayString
    End Sub

#End Region

#Region "Properties"

#Region "TemperatureUnit"

    Dim _TemperatureUnit As TemperatureUnits
    Public Property TemperatureUnit As TemperatureUnits
        Get
            Return _TemperatureUnit
        End Get
        Set(value As TemperatureUnits)
            _TemperatureUnit = value
            OnPropertyChanged("TemperatureUnit")
        End Set
    End Property

#End Region

#Region "Location"

    Dim _Location As String
    Public Property Location As String
        Get
            Return _Location
        End Get
        Set(value As String)
            _Location = value
            OnPropertyChanged("Location")
        End Set
    End Property

#End Region

#Region "LastChecked"

    Dim _LastChecked As String
    Public Property LastChecked As String
        Get
            Return _LastChecked
        End Get
        Set(value As String)
            _LastChecked = value
            OnPropertyChanged("LastChecked")
        End Set
    End Property

#End Region

#Region "CurrentObservations"

    Dim _CurrentObservations As CurrentObservations
    Public Property CurrentObservations As CurrentObservations
        Get
            Return _CurrentObservations
        End Get
        Set(value As CurrentObservations)
            _CurrentObservations = value
            OnPropertyChanged("CurrentObservations")
        End Set
    End Property

#End Region


#Region "IsUsingImperial"

    Dim _IsUsingImperial As Boolean
    Public Property IsUsingImperial As Boolean
        Get
            Return _IsUsingImperial
        End Get
        Set(value As Boolean)
            _IsUsingImperial = value
            OnPropertyChanged("IsUsingImperial")
        End Set
    End Property

#End Region


#End Region

#Region "Commands"

#Region "ChooseFahrenheitCommand"
    Dim _ChooseFahrenheitCommand As ICommand
    Public ReadOnly Property ChooseFahrenheitCommand As ICommand
        Get
            If _ChooseFahrenheitCommand Is Nothing Then
                _ChooseFahrenheitCommand = New Commands.RelayCommand(AddressOf ExecuteChooseFahrenheit, AddressOf CanExecuteChooseFahrenheit)
            End If

            Return _ChooseFahrenheitCommand
        End Get
    End Property

    Private Function CanExecuteChooseFahrenheit() As Boolean
        Return True
    End Function

    Private Sub ExecuteChooseFahrenheit()

    End Sub

#End Region

#Region "ChooseCelsiusCommand"
    Dim _ChooseCelsiusCommand As ICommand
    Public ReadOnly Property ChooseCelsiusCommand As ICommand
        Get
            If _ChooseCelsiusCommand Is Nothing Then
                _ChooseCelsiusCommand = New Commands.RelayCommand(AddressOf ExecuteChooseCelsius, AddressOf CanExecuteChooseCelsius)
            End If

            Return _ChooseCelsiusCommand
        End Get
    End Property

    Private Function CanExecuteChooseCelsius() As Boolean
        Return True
    End Function

    Private Sub ExecuteChooseCelsius()

    End Sub

#End Region

#End Region

#Region "Methods"

    Public Overrides Function InitializeAsync(Optional parameter As Object = Nothing) As Task
        UpdateDataAsync()

        Dim timer As New System.Windows.Threading.DispatcherTimer
        timer.Interval = TimeSpan.FromMinutes(1)
        AddHandler timer.Tick, Sub(s, e)
                                   UpdateDataAsync()
                               End Sub
        timer.Start()

        Return TaskEx.Delay(0)
    End Function

    Private Async Sub UpdateDataAsync()
        Try
            LastChecked = DateTime.Now
            _cancelationTokenSource = New CancellationTokenSource

            Await UpdateCurrentObservationsAsync()
        Catch ex As Exception
            Status = "There was an error retrieving updates to " & _model.Address.DisplayString & ": " & ex.Message & Environment.NewLine &
                "Last Checked: " & LastChecked
        End Try
    End Sub


    Private Async Function UpdateCurrentObservationsAsync() As Task
        If CurrentObservations IsNot Nothing AndAlso CurrentObservations.LastChecked.AddMinutes(CurrentObservations.RefreshTime.TotalMinutes) < DateTime.Now Then Return
        ' get the current observations
        CurrentObservations = Await _weatherService.GetCurrentObservationByIcaoAsync(_model.WeatherStation.ICAO, _cancelationTokenSource.Token)
    End Function

#End Region



End Class
