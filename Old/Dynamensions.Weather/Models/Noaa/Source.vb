Namespace Models.Noaa

    Public NotInheritable Class Source

        Private _moreInformation As String
        ''' <summary>
        ''' A link to the web page of the forecast’s source or a more complete forecast (R2.1.13).
        ''' </summary>
        Public Property MoreInformation() As String
            Get
                Return _moreInformation
            End Get
            Friend Set(ByVal value As String)
                _moreInformation = value
            End Set
        End Property

        Private _productionCenter As String
        ''' <summary>
        ''' Identifies which organization creates the product (R2.1.7).
        ''' </summary>
        Public Property ProductionCenter() As String
            Get
                Return _productionCenter
            End Get
            Friend Set(ByVal value As String)
                _productionCenter = value
            End Set
        End Property

        Private _subCenter As String
        ''' <summary>
        ''' Part of the production center that prepared the product (R2.1.8).
        ''' </summary>
        Public Property SubCenter() As String
            Get
                Return _subCenter
            End Get
            Set(ByVal value As String)
                _subCenter = value
            End Set
        End Property

        Private _disclaimer As String
        ''' <summary>
        ''' A URL containing a disclaimer regarding the data (R2.1.9).
        ''' </summary>
        Public Property Disclaimer() As String
            Get
                Return _disclaimer
            End Get
            Friend Set(ByVal value As String)
                _disclaimer = value
            End Set
        End Property

        Private _credit As String
        ''' <summary>
        ''' A URL used to credit the source of the data (R2.1.10).
        ''' </summary>
        Public Property Credit() As String
            Get
                Return _credit
            End Get
            Friend Set(ByVal value As String)
                _credit = value
            End Set
        End Property

        Private _creditLogo As String
        ''' <summary>
        ''' An image link used with the credit URL to acknowledge the data source (R.2.11).
        ''' </summary>
        Public Property CreditLogo() As String
            Get
                Return _creditLogo
            End Get
            Friend Set(ByVal value As String)
                _creditLogo = value
            End Set
        End Property

        Private _feedback As String
        ''' <summary>
        ''' A URL to a web page used to provide the production center comments on the product (R2.1.12).
        ''' </summary>
        Public Property Feedback() As String
            Get
                Return _feedback
            End Get
            Friend Set(ByVal value As String)
                _feedback = value
            End Set
        End Property

    End Class

End Namespace
