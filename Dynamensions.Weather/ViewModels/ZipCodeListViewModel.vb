
Public Class ZipCodeListViewModel
    Inherits ViewModelBase

    ' Services
    Private ReadOnly _navigationService As INavigationService


#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(navigationService As INavigationService)
        _navigationService = navigationService
    End Sub

#End Region

#Region "Properties"

    Property ZipCodes As ObservableCollection(Of String)

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

    End Sub

#End Region

#End Region

#Region "Methods"

    Public Overrides Function InitializeAsync(Optional parameter As Object = Nothing) As System.Threading.Tasks.Task
        Return MyBase.InitializeAsync(parameter)
    End Function

#End Region




End Class
