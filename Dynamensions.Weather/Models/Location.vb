Imports System.Xml.Linq
Imports <xmlns:ns="http://schemas.microsoft.com/search/local/ws/rest/v1">

Namespace Models

    Public Class Location

        Public Property Name As String

        Private _address As New Address
        Public ReadOnly Property Address() As Address
            Get
                Return _address
            End Get
        End Property

        Private _boundingBox As New LocationBoundingBox
        Public ReadOnly Property BoundingBox() As LocationBoundingBox
            Get
                Return _boundingBox
            End Get
        End Property

        Private _point As New LocationPoint
        Public ReadOnly Property Point() As LocationPoint
            Get
                Return _point
            End Get
        End Property

    End Class



    'Public Class BoundingBox


    '    Private _southLatitude As Decimal
    '    Public ReadOnly Property SouthLatitude() As Decimal
    '        Get
    '            Return _southLatitude
    '        End Get
    '    End Property

    '    Private _westLongitude As Decimal
    '    Public ReadOnly Property WestLongitude() As Decimal
    '        Get
    '            Return _westLongitude
    '        End Get
    '    End Property

    '    Private _northLatitude As Decimal
    '    Public ReadOnly Property NorthLatitude() As Decimal
    '        Get
    '            Return _northLatitude
    '        End Get
    '    End Property

    '    Private _eastLongitude As Decimal
    '    Public ReadOnly Property EastLongitude() As Decimal
    '        Get
    '            Return _eastLongitude
    '        End Get
    '    End Property

    '    Public Sub New(southlatitude As Decimal, westLongitude As Decimal, northLatitude As Decimal, eastLongitude As Decimal)
    '        _southLatitude = southlatitude
    '        _westLongitude = westLongitude
    '        _northLatitude = northLatitude
    '        _eastLongitude = eastLongitude
    '    End Sub

    '    Public Sub New(boundingBoxXml As XElement)
    '        _southLatitude = boundingBoxXml.<SouthLatitude>.Value.ToDecimal()
    '        _westLongitude = boundingBoxXml.<WestLongitude>.Value.ToDecimal()
    '        _northLatitude = boundingBoxXml.<NorthLatitude>.Value.ToDecimal()
    '        _eastLongitude = boundingBoxXml.<EastLongitude>.Value.ToDecimal()
    '    End Sub
    'End Class
End Namespace
