Public Class CurrentObserevations

    Public Property StationId As String
    Public Property MoreInformation As String
    Public Property RefreshTime As TimeSpan
    Public Property Location As String
    Public Property Latitude As Decimal
    Public Property Longitude As Decimal

    Public Property UpdatedTime As DateTime
    Public Property UpdatedTimeString As String

    Public Property Weather As String

    Public Property TemperatureString As String
    Public Property TemerpatureF As Decimal
    Public Property TemerpatureC As Decimal

    Public Property RelativeHumidity As Integer

    Public Property WindString As String
    Public Property WindDirection As String
    Public Property WindDegrees As Integer
    Public Property WindMph As Decimal
    Public Property WindKnots As Decimal

    Public Property PressureIn As Decimal

    Public Property DewPointString As String
    Public Property DewPointF As Decimal
    Public Property DewPointC As Decimal

    Public Property HeatIndexString As String
    Public Property HeatIndexF As Decimal
    Public Property HeatIndexC As Decimal

    Public Property VisibilityMi As Decimal

    Public Property Icon As Uri
    Public Property LastChecked As DateTime


End Class
