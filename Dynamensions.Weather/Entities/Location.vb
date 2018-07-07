Imports System.Data.Linq.Mapping
Imports System.Text

Namespace Entities

    <Table(Name:="Locations")>
    Public Class Location

        <Column(IsPrimaryKey:=True, CanBeNull:=False, AutoSync:=AutoSync.OnInsert, DbType:="nvarchar(20) NOT NULL")> Public Property PostalCode As String
        <Column(DbType:="int NOT NULL")> Public Property StationId As Integer
        <Column(DbType:="nvarchar(100) NOT NULL")> Public Property City As String
        <Column(DbType:="nvarchar(100) NOT NULL")> Public Property StateOrProvince As String
        <Column(DbType:="nvarchar(5) NOT NULL")> Public Property StateOrProvinceAbbreviation As String
        <Column(DbType:="nvarchar(100) NOT NULL")> Public Property County As String
        <Column(DbType:="decimal(5,2) NOT NULL")> Public Property Latitude As Decimal
        <Column(DbType:="decimal(5,2) NOT NULL")> Public Property Longitude As Decimal
        <Column(CanBeNull:=True, DbType:="datetime NOT NULL")> Public Property LastUpdated As DateTime


        <Association(Storage:="Station", ThisKey:="StationId", OtherKey:="Id", IsForeignKey:=True)>
        Private _station As Station
        Public Property Station() As Station
            Get
                Return _station
            End Get
            Set(ByVal value As Station)
                _station = value
            End Set
        End Property



        Public ReadOnly Property DisplayString As String
            Get
                Dim sb As New StringBuilder
                If Not String.IsNullOrWhiteSpace(City) Then sb.Append(City)
                If Not String.IsNullOrWhiteSpace(StateOrProvinceAbbreviation) Then
                    If Not String.IsNullOrWhiteSpace(City) Then sb.Append(", ")
                    sb.Append(StateOrProvinceAbbreviation)
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