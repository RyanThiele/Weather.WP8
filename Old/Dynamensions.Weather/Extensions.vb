Imports System.Runtime.CompilerServices
Imports System.Xml.Linq
Imports System.Globalization

Friend Module Extensions

    <Extension>
    Friend Function ToDouble(s As String, Optional defaultValue As Double = 0) As Double
        Dim value As Double = defaultValue
        If Not String.IsNullOrWhiteSpace(s) Then Double.TryParse(s, value)
        Return value
    End Function

    <Extension>
    Friend Function ToDecimal(s As String, Optional defaultValue As Decimal = 0) As Decimal
        Dim value As Decimal = defaultValue
        If Not String.IsNullOrWhiteSpace(s) Then Decimal.TryParse(s, value)
        Return value
    End Function

    <Extension>
    Friend Function ToInteger(s As String, Optional defaultValue As Integer = 0) As Integer
        Dim value As Integer = defaultValue
        If Not String.IsNullOrWhiteSpace(s) Then Integer.TryParse(s, value)
        Return value
    End Function

    <Extension>
    Friend Function ValueIfExists(element As XElement) As String
        If element IsNot Nothing Then
            Return element.Value
        Else
            Return Nothing
        End If
    End Function

    <Extension>
    Friend Function ValueIfExists(attribute As XAttribute) As String
        If attribute IsNot Nothing Then
            Return attribute.Value
        Else
            Return Nothing
        End If
    End Function

    <Extension>
    Friend Function FromXmlDateTimeToDateTime(s As String, Optional defaultValue As DateTime? = Nothing) As DateTime
        If Not defaultValue.HasValue Then defaultValue = New DateTime
        Dim value As DateTime? = XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local)
        If value Is Nothing Then value = defaultValue.Value
        Return value
    End Function

    <Extension>
    Friend Function FromRfc22StringToDateTime(s As String, Optional defaultValue As DateTime? = Nothing) As DateTime
        Dim provider As CultureInfo = CultureInfo.InvariantCulture
        '                              Sun, 08 Jul 2018 02:55:00 -0400
        '                              Sat, 07 Jul 2018 17:48:00 -0400
        Return DateTime.ParseExact(s, "ddd, dd MMM yyyy HH:mm:ss zzz", provider)
    End Function
End Module
