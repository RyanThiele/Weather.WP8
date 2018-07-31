Imports System.ComponentModel
Imports System.Runtime.CompilerServices


''' <summary>
''' The base of all observables (view models, etc..) that data may change on.
''' </summary>
''' <remarks></remarks>
Public Class ObservableObject
    Implements INotifyPropertyChanged

#Region "INotifyPropertyChanged Implementation"

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
        If String.IsNullOrWhiteSpace(propertyName) Then Return
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region

End Class
