Imports System.Data.Linq.Mapping

Namespace Entities

    <Table>
    Public Class PostalCodeStation

        <Column(IsPrimaryKey:=True, CanBeNull:=False, AutoSync:=AutoSync.OnInsert, DbType:="int NOT NULL IDENTITY(1,1)", IsDbGenerated:=True)> Public Property Id As Integer
        <Column(DbType:="int NOT NULL")> Public Property StationId As Integer
        <Association(Storage:="Station", ThisKey:="StationId", OtherKey:="StationId", IsForeignKey:=True)>
        Private _station As Station
        Public Property Station() As Station
            Get
                Return _station
            End Get
            Set(ByVal value As Station)
                _station = value
            End Set
        End Property

    End Class

End Namespace
