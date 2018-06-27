Public Class WeatherSourcesViewModel
    Inherits ObservableObject

    Private ReadOnly _messageBus As IMessageBus

#Region "Constructors"

    Public Sub New()
        Title = "Weather Sources"
    End Sub

    Public Sub New(messagebus As IMessageBus)
        _messageBus = messagebus
    End Sub

#End Region

#Region "Properties"

    Private _weatherSources As ObservableCollection(Of WeatherSourceListItemViewModel)
    Public Property WeatherSources() As ObservableCollection(Of WeatherSourceListItemViewModel)
        Get
            Return _weatherSources
        End Get
        Set(ByVal value As ObservableCollection(Of WeatherSourceListItemViewModel))
            _weatherSources = value
            OnPropertyChanged()

            If WeatherSources IsNot Nothing Then
                Title = "Weather Sources (" & WeatherSources.Count.ToString & ")"
            Else
                Title = "Weather Sources"
            End If
        End Set
    End Property

#Region "Title"

    Dim _Title As String
    Public Property Title As String
        Get
            Return _Title
        End Get
        Private Set(value As String)
            _Title = value
            OnPropertyChanged("Title")
        End Set
    End Property

#End Region


#End Region

#Region "Commands"
#End Region

#Region "Methods"


#Region "AddWeatherSourceCommand"
    Dim _AddWeatherSourceCommand As ICommand
    Public ReadOnly Property AddWeatherSourceCommand As ICommand
        Get
            If _AddWeatherSourceCommand Is Nothing Then
                _AddWeatherSourceCommand = New Commands.RelayCommand(AddressOf ExecuteAddWeatherSource, AddressOf CanExecuteAddWeatherSource)
            End If

            Return _AddWeatherSourceCommand
        End Get
    End Property

    Private Function CanExecuteAddWeatherSource() As Boolean
        Return True
    End Function

    Private Sub ExecuteAddWeatherSource()

    End Sub

#End Region


#End Region



End Class
