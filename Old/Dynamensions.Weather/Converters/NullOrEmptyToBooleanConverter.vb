Imports System.Windows.Data

Namespace Converters

    Public Class NullOrEmptyToBooleanConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            If String.IsNullOrWhiteSpace(value) Then
                Return Visibility.Collapsed
            Else
                Return Visibility.Visible
            End If
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException
        End Function
    End Class

End Namespace
