Public Class StartupViewModel
    Inherits ViewModelBase

    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _settingsService As ISettingsService

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(navigationService As INavigationService, settingsService As ISettingsService)
        _navigationService = navigationService
        _settingsService = settingsService
    End Sub

#End Region

#Region "Properties"

#Region "Status"

    Dim _Status As String
    Public Property Status As String
        Get
            Return _Status
        End Get
        Set(value As String)
            _Status = value
            OnPropertyChanged("Status")
        End Set
    End Property

#End Region

#End Region

#Region "Commands"
#End Region

#Region "Methods"

    Public Overrides Async Function InitializeAsync(Optional parameter As Object = Nothing) As System.Threading.Tasks.Task
        Status = "Checking Data Integrity..."
        Dim zipCodes As IEnumerable(Of String) = Await _settingsService.LoadZipCodesAsync()
        If (zipCodes Is Nothing) Then
            Status = "No Zip Codes Found. Navigating to Zip Codes"
            _navigationService.NavigateTo(Of ZipCodeListViewModel)()
            Return
        End If

        Status = "Done!"
    End Function

#End Region

End Class
