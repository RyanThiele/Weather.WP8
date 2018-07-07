Imports System.Data.Linq.Mapping
Imports System.ComponentModel

Namespace Entities

    <Table(Name:="Stations")>
    Public Class Station

        <Column(IsPrimaryKey:=True, CanBeNull:=False, AutoSync:=AutoSync.OnInsert, DbType:="int NOT NULL IDENTITY(1,1)", IsDbGenerated:=True)> Public Property Id As Integer
        <Column(CanBeNull:=False, DbType:="nvarchar(2) NOT NULL")> Public Property StateOrProvince As String
        <Column(CanBeNull:=False, DbType:="nvarchar(16) NOT NULL")> Public Property Station As String
        <Column(CanBeNull:=True, DbType:="nvarchar(4) NULL")> Public Property ICAO As String
        <Column(CanBeNull:=True, DbType:="nvarchar(3) NULL")> Public Property IATA As String
        <Column(CanBeNull:=True, DbType:="int NULL")> Public Property SYNOP As Integer?
        <Column(CanBeNull:=True, DbType:="numeric(18,2) NULL")> Public Property Latitude As Decimal?
        <Column(CanBeNull:=True, DbType:="numeric(18,2) NULL")> Public Property Longitude As Decimal?
        <Column(CanBeNull:=True, DbType:="int NULL")> Public Property Elevation As Integer?
        <Column(CanBeNull:=True, DbType:="bit NOT NULL"), DefaultValue(0)> Public Property METAR As Boolean
        <Column(CanBeNull:=True, DbType:="bit NOT NULL"), DefaultValue(0)> Public Property NEXRAD As Boolean
        <Column(CanBeNull:=True, DbType:="nchar(1) NULL")> Public Property AviationFlag As String
        <Column(CanBeNull:=True, DbType:="nchar(1) NULL")> Public Property UpperAirOrWindFlag As String
        <Column(CanBeNull:=True, DbType:="nchar(1) NULL")> Public Property AutoFlag As String
        <Column(CanBeNull:=True, DbType:="nchar(1) NULL")> Public Property OfficeFlag As String
        <Column(CanBeNull:=True, DbType:="int NULL")> Public Property Priority As Integer?
        <Column(CanBeNull:=True, DbType:="nchar(2) NULL")> Public Property CountryCode As String
        <Column(CanBeNull:=True, DbType:="datetime NOT NULL")> Public Property LastUpdated As DateTime

    End Class

End Namespace
