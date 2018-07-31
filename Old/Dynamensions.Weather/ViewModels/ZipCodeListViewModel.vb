Imports System.Linq

Public Class ZipCodeListViewModel
    Inherits ViewModelBase

    ' Services
    Private ReadOnly _navigationService As INavigationService
    Private ReadOnly _dialogService As IDialogService
    Private ReadOnly _messageBus As IMessageBus
    Private ReadOnly _settingsService As ISettingsService

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(navigationService As INavigationService, messageBus As IMessageBus, dialogService As IDialogService, settingsService As ISettingsService)
        _navigationService = navigationService
        _settingsService = settingsService
        _dialogService = dialogService
        _messageBus = messageBus
        _messageBus.Subscribe(Of RemoveZipCodeMessage)(Async Sub(message)
                                                           ' find the zip code
                                                           Dim zipCodeToRemove As ZipCodeListItemViewModel = ZipCodesCollection.SingleOrDefault(Function(item) item.ZipCode = message.ZipCodeToRemove)
                                                           ZipCodesCollection.Remove(zipCodeToRemove)
                                                           Await _settingsService.SaveZipCodesAsync(ZipCodesCollection.Select(Function(item) item.ZipCode))
                                                       End Sub)
    End Sub

#End Region

#Region "Properties"

    Property ZipCodesCollection As New ObservableCollection(Of ZipCodeListItemViewModel)

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

    Public Overrides Async Function InitializeAsync(Optional parameter As Object = Nothing) As System.Threading.Tasks.Task
        Try
            Dim zipCodes As IEnumerable(Of String) = Await _settingsService.LoadZipCodesAsync()
            If zipCodes IsNot Nothing Then
                ZipCodesCollection = New ObservableCollection(Of ZipCodeListItemViewModel)(zipCodes.Select(Function(item) New ZipCodeListItemViewModel(_dialogService, _messageBus) With {.ZipCode = item}))
                OnPropertyChanged("ZipCodesCollection")
            End If
        Catch ex As Exception
            Status = "There was an error retrieving the zip codes: " & ex.Message
        End Try
    End Function

#End Region




End Class
