Imports System.Threading

Public Class AddWeatherSourceViewModel
    Inherits ViewModelBase

    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _geocodeService As Services.IGeocodeService
    Private _searchCancelTokenSource As CancellationTokenSource

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(messageBus As IMessageBus, dialogService As IDialogService, navigationService As INavigationService, geocodeService As Services.IGeocodeService)
        _messageBus = messageBus
        _dialogService = dialogService
        _navigationService = navigationService
        _geocodeService = geocodeService

        PostalCode = "46845"
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
            Status = "Searching for " & PostalCode & "..."

            _searchCancelTokenSource = New CancellationTokenSource
            Dim location As Models.Location = Await _geocodeService.GetLocationByPostalCodeAsync(PostalCode)
            If _searchCancelTokenSource.IsCancellationRequested Then
                Status = "User canceled the search."
                Return
            End If

            If location Is Nothing Then
                _dialogService.ShowOkDialog("No Location Found", "There was no location found at the zip code: " & PostalCode & "." & Environment.NewLine & "Please try again.")
                Return
            End If

            Status = "Search Complete."
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
#End Region




End Class
