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


#End Region

#Region "Commands"
#End Region

#Region "Methods"

    Public Overrides Async Function InitializeAsync(Optional parameter As Object = Nothing) As System.Threading.Tasks.Task
        Status = "Checking Data Integrity..."
        Try
            Dim zipCodes As IEnumerable(Of String) = Await _settingsService.LoadZipCodesAsync()
            If (zipCodes Is Nothing) Then
                Status = "No Zip Codes Found. Navigating to Zip Codes"
                _navigationService.NavigateTo(Of ZipCodeListViewModel)()
                Return
            End If
            Status = "Done!"
        Catch ex As Exception
            Status = "There was an error verifying the integrety of the data: " & ex.Message
        End Try
    End Function

#End Region

End Class
