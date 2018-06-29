Public Class AddWeatherSourceViewModel
    Inherits ViewModelBase

    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _geocodeService As Services.IGeocodeService


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
        Dim location As Models.Location = Await _geocodeService.GetLocationByPostalCodeAsync(PostalCode)
        If location Is Nothing Then
            _dialogService.ShowOkDialog("No Location Found", "There was no location found at the zip code: " & PostalCode & "." & Environment.NewLine & "Please try again.")
        Else
            _dialogService.ShowOkDialog("Location Found", "There was a location found at the zip code: " & PostalCode & "." & Environment.NewLine & location.Address.County & ", " & location.Address.StateOrRegion)
        End If

    End Sub

#End Region


#End Region

#Region "Methods"
#End Region




End Class
