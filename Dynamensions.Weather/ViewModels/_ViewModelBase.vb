Public Class ViewModelBase
    Inherits ObservableObject

    Public Overridable Async Function InitializeAsync(Optional parameter As Object = Nothing) As Task
        Await DelayAsync(20)
    End Function

    Protected Function DelayAsync(milliseconds As Integer) As Task
        Dim tcs As New TaskCompletionSource(Of Boolean)
        Dim timer As New System.Threading.Timer(Sub()
                                                    tcs.TrySetResult(True)
                                                End Sub, Nothing, milliseconds, 0)

        Return tcs.Task
    End Function

#Region "Properties"


#Region "Title"

    Dim _Title As String
    Public Property Title As String
        Get
            Return _Title
        End Get
        Set(value As String)
            _Title = value
            OnPropertyChanged("Title")
        End Set
    End Property

#End Region

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

End Class
