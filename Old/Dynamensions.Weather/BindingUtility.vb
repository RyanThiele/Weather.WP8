Public Class BindingUtility
    Public Shared Function GetUpdateSourceOnChange(ByVal d As DependencyObject) As Boolean
        Return CBool(d.GetValue(UpdateSourceOnChangeProperty))
    End Function

    Public Shared Sub SetUpdateSourceOnChange(ByVal d As DependencyObject, ByVal value As Boolean)
        d.SetValue(UpdateSourceOnChangeProperty, value)
    End Sub

    Public Shared ReadOnly UpdateSourceOnChangeProperty As DependencyProperty =
        DependencyProperty.RegisterAttached("UpdateSourceOnChange", GetType(Boolean), GetType(BindingUtility), New PropertyMetadata(False, AddressOf OnPropertyChanged))

    Private Shared Sub OnPropertyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim textBox = TryCast(d, TextBox)
        If textBox Is Nothing Then Return

        If CBool(e.NewValue) Then
            AddHandler textBox.TextChanged, AddressOf OnTextChanged
        Else
            RemoveHandler textBox.TextChanged, AddressOf OnTextChanged
        End If
    End Sub

    Private Shared Sub OnTextChanged(ByVal s As Object, ByVal e As TextChangedEventArgs)
        Dim textBox = TryCast(s, TextBox)
        If textBox Is Nothing Then Return
        Dim bindingExpression = textBox.GetBindingExpression(textBox.TextProperty)

        If bindingExpression IsNot Nothing Then
            bindingExpression.UpdateSource()
        End If
    End Sub
End Class
