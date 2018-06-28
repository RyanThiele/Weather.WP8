Imports System.Text
Imports System.Data.Linq.Mapping

<Table>
Public Class Location
    <Column(IsPrimaryKey:=True, CanBeNull:=False, AutoSync:=AutoSync.OnInsert, DbType:="nvarchar(20) NOT NULL")> Public Property ZipCode As String
    <Column(DbType:="nvarchar(100) NOT NULL")> Public Property City As String
    <Column(DbType:="nvarchar(100) NOT NULL")> Public Property StateOrProvince As String
    <Column(DbType:="nvarchar(5) NOT NULL")> Public Property StateOrProvinceAbbreviation As String
    <Column(DbType:="nvarchar(100) NOT NULL")> Public Property County As String
    <Column(DbType:="decimal(5,2) NOT NULL")> Public Property Latitude As Decimal
    <Column(DbType:="decimal(5,2) NOT NULL")> Public Property Longitude As Decimal

    Public ReadOnly Property DisplayString As String

        Get
            Dim sb As New StringBuilder
            If Not String.IsNullOrWhiteSpace(City) Then sb.Append(City)
            If Not String.IsNullOrWhiteSpace(StateOrProvinceAbbreviation) Then
                If Not String.IsNullOrWhiteSpace(City) Then sb.Append(", ")
                sb.Append(StateOrProvinceAbbreviation)
            End If

            If Not String.IsNullOrWhiteSpace(ZipCode) Then
                Dim isEmpty As Boolean = String.IsNullOrWhiteSpace(sb.ToString())
                If Not isEmpty Then sb.Append(" (")
                sb.Append(ZipCode)
                If Not isEmpty Then sb.Append(")")
            End If

            Return sb.ToString()
        End Get
    End Property

End Class