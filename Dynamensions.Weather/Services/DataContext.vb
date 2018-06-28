Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Threading
Imports System


Public Class DbDataContext
    Inherits DataContext

    Public Shared DBConnectionString As String = "Data Source=isostore:/Weather.sdf"

    Public Sub New(connectionString As String)
        MyBase.New(connectionString)
    End Sub

    ' Specify tables.
    Public Locations As Table(Of Location)

    Private Delegate Sub SubmitChangesDelegate()
    Public Function SubmitChangesAsync() As Task
        Dim caller As New SubmitChangesDelegate(AddressOf SubmitChanges)
        Return Task.Factory.FromAsync(AddressOf caller.BeginInvoke, AddressOf caller.EndInvoke, Nothing)
    End Function


End Class
