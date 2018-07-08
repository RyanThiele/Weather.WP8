Imports System.Windows.Data

Namespace Converters

    Public Class TimeSpanToStringConverter
        Implements IValueConverter


        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            If value Is Nothing Then Return Nothing
            If value.GetType IsNot GetType(TimeSpan) Then Return Nothing

            Dim timespan As TimeSpan = CType(value, TimeSpan)

            If timespan.TotalMinutes > 0 Then
                Return timespan.ToString("hh\:mm\:ss")
            Else
                Return timespan.Seconds & " seconds"
            End If


            'If timespan.Days > 0 Then returnString += timespan.Days.ToString("00")

            'If timespan.Hours > 0 Then
            '    If String.IsNullOrWhiteSpace(returnString) Then
            '        returnString = timespan.Hours.ToString
            '    Else
            '        returnString += ":" & timespan.Hours.ToString("00")
            '    End If
            'End If

            'If timespan.Minutes > 0 Then
            '    If String.IsNullOrWhiteSpace(returnString) Then
            '        returnString = timespan.Minutes.ToString
            '    Else
            '        returnString += ":" & timespan.Minutes.ToString("00")
            '    End If
            'End If

            'If timespan.Seconds > 0 Then
            '    If String.IsNullOrWhiteSpace(returnString) Then
            '        returnString = timespan.Seconds.ToString & " seconds"
            '    Else
            '        returnString += ":" & timespan.Seconds.ToString("00")
            '    End If
            'End If

            'Return returnString
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException
        End Function
    End Class

End Namespace