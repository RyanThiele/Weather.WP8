Imports System.Threading
Imports System.Threading.Tasks
Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class ViewModelBase
    Implements INotifyPropertyChanged


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
        If String.IsNullOrWhiteSpace(propertyName) Then Return
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

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



End Class
