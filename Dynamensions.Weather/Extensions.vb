Imports System.Runtime.CompilerServices

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
End Module
