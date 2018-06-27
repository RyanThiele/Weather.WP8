
Public Class ZipCodeListViewModel
    Inherits ViewModelBase

    ' Services
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

    Property ZipCodesCollection As New ObservableCollection(Of String)

#End Region

#Region "Commands"


#Region "AddCommand"

    Dim _AddCommand As ICommand

    Public ReadOnly Property AddCommand As System.Windows.Input.ICommand
        Get
            If _AddCommand Is Nothing Then
                _AddCommand = New Commands.RelayCommand(AddressOf ExecuteAdd, AddressOf CanExecuteAdd)
            End If

            Return _AddCommand
        End Get
    End Property

    Private Function CanExecuteAdd() As Boolean
        Return True
    End Function

    Private Sub ExecuteAdd()
        _navigationService.ShowOkDialog("Title", "Test")
    End Sub

#End Region


#Region "RemoveCommand"
    Dim _RemoveCommand As ICommand
    Public ReadOnly Property RemoveCommand As ICommand
        Get
            If _RemoveCommand Is Nothing Then
                _RemoveCommand = New Commands.RelayCommand(AddressOf ExecuteRemove, AddressOf CanExecuteRemove)
            End If

            Return _RemoveCommand
        End Get
    End Property

    Private Function CanExecuteRemove() As Boolean
        Return True
    End Function

    Private Sub ExecuteRemove()
        If (_navigationService) Then
    End Sub

#End Region


#End Region

#Region "Methods"

    Public Overrides Async Function InitializeAsync(Optional parameter As Object = Nothing) As System.Threading.Tasks.Task
        Try
            Dim zipCodes As IEnumerable(Of String) = Await _settingsService.LoadZipCodesAsync()
            ZipCodesCollection = New ObservableCollection(Of String)(zipCodes)
            OnPropertyChanged("ZipCodesCollection")
        Catch ex As Exception
            Status = "There was an error retrieving the zip codes: " & ex.Message
        End Try
    End Function

#End Region




End Class
