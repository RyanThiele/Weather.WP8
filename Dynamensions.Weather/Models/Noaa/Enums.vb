Namespace Models.Noaa

    Public Enum OperationalModes
        ''' <summary>
        ''' The “test” product indicates that this is an instance of an existing DWML 
        ''' product that contains some change being evaluated by a DWML development team.
        ''' Users will typically not process this product (R2.1.4.1).
        ''' </summary>
        Test
        ''' <summary>
        ''' The "developmental" product represents a new product that is not yet ready 
        ''' for public evaluation or use (R2.1.4.2).
        ''' </summary>
        Developmental
        ''' <summary>
        ''' This product is available for testing and evaluation for a specified, 
        ''' limited time period for the explicit purpose of obtaining customer feedback.(R2.1.4.3).
        ''' </summary>
        Experimental
        ''' <summary>
        ''' The "official" product identifies an instance of an established DWML product.
        ''' This DWML instance is part of the approved product suite available from the NWS (R2.1.4.4).
        ''' </summary>
        Official
    End Enum

End Namespace
