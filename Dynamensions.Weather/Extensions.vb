Imports System.Runtime.CompilerServices

Friend Module Extensions
    <Extension>
    Friend Function ToDecimal(s As String, Optional defaultValue As Decimal = 0) As Decimal
        Dim value As Decimal = defaultValue
        If Not String.IsNullOrWhiteSpace(s) Then Decimal.TryParse(s, value)
        Return value
    End Function
End Module
