Namespace Models.Noaa

    ''' <summary>
    ''' The head of the DWML response
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class Head

        Private _product As New Product
        ''' <summary>
        ''' The product holds meta information about the product.
        ''' </summary>
        Public ReadOnly Property Product() As Product
            Get
                Return _product
            End Get
        End Property

        Private _source As New Source
        ''' <summary>
        ''' The source holds information about the product’s source and links to credit and disclaimer information.
        ''' </summary>
        Public ReadOnly Property Source() As Source
            Get
                Return _source
            End Get
        End Property


    End Class

End Namespace
