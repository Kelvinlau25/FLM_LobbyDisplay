Imports System.Threading

Namespace Other
    ''' <summary>
    ''' Handler the Common Business Logic Function
    ''' -------------------------------------------
    ''' C.C.Yeon    25 April 2011   Initial Version
    ''' </summary>
    Public MustInherit Class BusinessLogicBase

        Public Enum LanguagePack
            English = 0
            Malay = 1
        End Enum

        Public Shared ReadOnly Property Language() As LanguagePack
            Get
                Dim lp As LanguagePack = LanguagePack.English

                If Thread.CurrentThread.CurrentCulture.ToString.Equals("ms-MY") Then
                    lp = LanguagePack.Malay
                End If

                Return lp
            End Get
        End Property

        ''' <summary>
        ''' Max Quantity per Page
        ''' </summary>
        Public Shared ReadOnly Property MaxQuantityPerPage() As Integer
            Get
                Return CInt(System.Configuration.ConfigurationManager.AppSettings("MaxRowPerPage").ToString)
            End Get
        End Property

        ''' <summary>
        ''' Generate and Calculate the Number
        ''' </summary>
        Public Shared ReadOnly Property FromRowNo(ByVal PageNo As Integer) As Integer
            Get
                If PageNo = 1 Then
                    Return 1
                Else
                    Return ((PageNo - 1) * MaxQuantityPerPage) + 1
                End If
            End Get
        End Property

        ''' <summary>
        ''' Generate and Calculate the Number
        ''' </summary>
        Public Shared ReadOnly Property ToRowNo(ByVal PageNo As Integer) As Integer
            Get
                If PageNo = 1 Then
                    Return MaxQuantityPerPage
                Else
                    Return ((PageNo - 1) * MaxQuantityPerPage) + MaxQuantityPerPage
                End If
            End Get
        End Property

    End Class
End Namespace
