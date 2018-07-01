Namespace Models.Noaa

    Public Class Data

        Private _key As String
        ''' <summary>
        ''' Used to relate the location to its corresponding parameters (R2.2.2).
        ''' </summary>
        Public Property Key() As String
            Get
                Return _key
            End Get
            Set(ByVal value As String)
                _key = value
            End Set
        End Property


        Private _point As Point
        ''' <summary>
        ''' Used to define the grid point for which the data is valid (R2.2.2).
        ''' </summary>
        Public Property Point() As Point
            Get
                Return _point
            End Get
            Friend Set(ByVal value As Point)
                _point = value
            End Set
        End Property


    End Class

End Namespace

