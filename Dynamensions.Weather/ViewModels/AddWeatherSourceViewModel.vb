Imports System.Threading
Imports Dynamensions.Weather.Services

Public Class AddWeatherSourceViewModel
    Inherits ViewModelBase

    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _weatherService As IWeatherService
    Private ReadOnly _settingsService As ISettingsService

    Private _searchCancelTokenSource As CancellationTokenSource

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(messageBus As IMessageBus,
                   dialogService As IDialogService,
                   navigationService As INavigationService,
                   weatherService As IWeatherService,
                   settingsService As ISettingsService)

        _messageBus = messageBus
        _dialogService = dialogService
        _navigationService = navigationService
        _weatherService = weatherService
        _settingsService = settingsService

#If DEBUG Then
        PostalCode = "46845"
#End If
    End Sub

#End Region

#Region "Properties"

#Region "PostalCode"

    Dim _PostalCode As String
    Public Property PostalCode As String
        Get
            Return _PostalCode
        End Get
        Set(value As String)
            _PostalCode = value
            OnPropertyChanged("PostalCode")
        End Set
    End Property

#End Region

#Region "IsSearching"

    Dim _IsSearching As Boolean
    Public Property IsSearching As Boolean
        Get
            Return _IsSearching
        End Get
        Set(value As Boolean)
            _IsSearching = value
            OnPropertyChanged("IsSearching")
        End Set
    End Property

#End Region

#End Region

#Region "Commands"

#Region "SearchCommand"

    Dim _SearchCommand As ICommand
    Public ReadOnly Property SearchCommand As ICommand
        Get
            If _SearchCommand Is Nothing Then
                _SearchCommand = New Commands.RelayCommand(AddressOf ExecuteSearch, AddressOf CanExecuteSearch)
            End If

            Return _SearchCommand
        End Get
    End Property

    Private Function CanExecuteSearch() As Boolean
        Return True
    End Function

    Private Async Sub ExecuteSearch()
        IsSearching = True

        Try
            Status = "Searching..."

            ' search for the location
            _searchCancelTokenSource = New CancellationTokenSource
            Dim location As Models.Location = Await _weatherService.GetLocationByPostalCodeAsync(PostalCode, _searchCancelTokenSource.Token)

            ' location is not found. notify user and bail.
            If location Is Nothing Then
                Status = "Could not find a weather station for postal code: " & PostalCode & ". Please try again."
                Return
            End If

            ' check if location already in cache.
            Dim locations As IEnumerable(Of Models.Location) = Await _settingsService.GetSelectedLocationsAsync
            If locations IsNot Nothing Then
                Dim existingLocation As Models.Location = locations.Where(Function(o) o.Address.PostalCode.Equals(PostalCode)).SingleOrDefault
                ' location already exits, notify user and bail.
                If existingLocation IsNot Nothing Then
                    Status = PostalCode & " already exists. Please try again."
                    Return
                End If
            End If

            ' if we get to here, all conditions are met to enter the location.
            ' ask the user if they want to add the location.
            If _dialogService.ShowYesNoDialog("Weather Station Found!", "Found a station for '" & PostalCode & ": " & location.WeatherStation.Name & ". Do you want to use this station?") Then
                Dim locationList As New List(Of Models.Location)
                If locations IsNot Nothing Then locationList = New List(Of Models.Location)(locations)
                locationList.Add(location)
                Await _settingsService.SetSelectedLocationsAsync(locationList)
            End If

            Status = "Added " & PostalCode & " to cache."

        Catch ex As Exception
            Status = "There was a problem with the search: " & ex.Message
        End Try

        IsSearching = False
    End Sub

#End Region

#Region "CancelCommand"
    Dim _CancelCommand As ICommand
    Public ReadOnly Property CancelCommand As ICommand
        Get
            If _CancelCommand Is Nothing Then
                _CancelCommand = New Commands.RelayCommand(AddressOf ExecuteCancel, AddressOf CanExecuteCancel)
            End If

            Return _CancelCommand
        End Get
    End Property

    Private Function CanExecuteCancel() As Boolean
        Return True
    End Function

    Private Sub ExecuteCancel()
        If Not IsSearching Then Return
        If _searchCancelTokenSource Is Nothing OrElse Not _searchCancelTokenSource.IsCancellationRequested Then Return
        _searchCancelTokenSource.Cancel()
    End Sub

#End Region


#End Region

#Region "Methods"

    Private Function CheckSearchTokenStatus() As Boolean
        If _searchCancelTokenSource.IsCancellationRequested Then
            Status = "User canceled the search."
        End If

        Return _searchCancelTokenSource.IsCancellationRequested
    End Function

#End Region

End Class
