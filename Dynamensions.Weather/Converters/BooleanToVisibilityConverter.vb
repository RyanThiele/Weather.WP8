Imports System.Windows.Data

Namespace Converters

    Public Class BooleanToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            If value Is Nothing Then Return False

            Dim isInverted As Boolean = False
            Dim isVisible As Boolean = False

            Boolean.TryParse(parameter, isInverted)
            Boolean.TryParse(value, isVisible)

            ' if inverted flip the flag.
            If isInverted Then isVisible = Not isVisible

            If isVisible Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException
        End Function
    End Class

End Namespace
