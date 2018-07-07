Imports System.Xml.Linq
Imports <xmlns:ns="http://schemas.microsoft.com/search/local/ws/rest/v1">

Namespace Models

    Public Class Location

        Public Property Name As String

        Public Property Address() As Address

        Public Property WeatherStation As WeatherStation

    End Class


End Namespace
