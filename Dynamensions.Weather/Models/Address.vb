Imports System.Text

Namespace Models
    Public Class Address
        Public Property StateOrRegion As String
        Public Property County As String
        Public Property City As String
        Public Property PostalCode As String
        Public Property Country As String

        Public ReadOnly Property DisplayString As String
            Get
                Dim sb As New StringBuilder
                If Not String.IsNullOrWhiteSpace(City) Then sb.Append(City)
                If Not String.IsNullOrWhiteSpace(StateOrRegion) Then
                    If Not String.IsNullOrWhiteSpace(City) Then sb.Append(", ")
                    sb.Append(StateOrRegion)
                End If

                If Not String.IsNullOrWhiteSpace(PostalCode) Then
                    Dim isEmpty As Boolean = String.IsNullOrWhiteSpace(sb.ToString())
                    If Not isEmpty Then sb.Append(" (")
                    sb.Append(PostalCode)
                    If Not isEmpty Then sb.Append(")")
                End If

                Return sb.ToString()
            End Get
        End Property

    End Class
End Namespace
