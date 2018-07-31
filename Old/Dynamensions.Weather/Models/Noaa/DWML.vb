Namespace Models.Noaa

    ''' <summary>
    ''' A base to hold all Noaa DWML responses.
    ''' </summary>
    Public Class DWML

        Private _version As Integer
        Public ReadOnly Property Version() As Integer
            Get
                Return _version
            End Get
        End Property

        Private _head As Head
        ''' <summary>
        ''' The head contains the metadata for the DWML instance. 
        ''' </summary>
        Public ReadOnly Property Head() As Head
            Get
                Return _head
            End Get
        End Property


    End Class

End Namespace
