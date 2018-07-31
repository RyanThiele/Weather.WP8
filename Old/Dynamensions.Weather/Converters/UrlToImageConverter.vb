Imports System.Windows.Data
Imports System.Windows.Media.Imaging

Namespace Converters
    Public Class UrlToImageConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            If value Is Nothing Then Return Nothing
            If value.GetType IsNot GetType(Uri) Then Return Nothing

            Dim uri As Uri = TryCast(value, Uri)
            If uri Is Nothing Then Return Nothing

            Dim x As New BitmapImage(uri)
            Dim image As New Image
            Image.Source = x

            Return x
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException
        End Function
    End Class
End Namespace
