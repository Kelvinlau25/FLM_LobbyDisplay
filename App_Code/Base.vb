Namespace Control
    ''' <summary>
    ''' Handler All Page Common Function 
    ''' 1 ) Retrieve and Determine Key
    ''' 2 ) Retrieve and Determine Action(Insert / Update / Delete)
    ''' 3 ) Generate (Insert / Edit / Delete / List / View ) URL Based on the Setup Key
    ''' 4 ) Generete Title Based on the Setup Key
    ''' 5 ) Generate Action Desc 
    ''' 6 ) Retrieve and Determine Sort Field , Sort Direction 
    ''' 7 ) Generate the List View URL (Include Sort Field , Sort Value and Page No )
    ''' 8 ) Function Control property , Default = true , if false all the generate list will not auto generate, this is to igone the error wish the page doest not have setting in the resource page
    ''' 9 ) Delete Control property , Default = true, if false then the show deleted Check box will be disappear.
    ''' 
    ''' Remark : The default sort property will determine wherether the url is check or not
    ''' check ( if the url failed retrieve the sort field then will generete and redirect its
    ''' -------------------------------------------------------------------------------
    ''' C.C.Yeon    25 April 2011   initial Version
    ''' C.C.Yeon    12 May   2011   Add FucntionControl Property
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Base
        Inherits Library.Root.Control.Base

        Public MustOverride Sub BindData()

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)
            Me.BindData()
        End Sub

        ''' <summary>
        ''' Retrieve the Detail Path
        ''' </summary>
        Public Overrides ReadOnly Property DetailPage() As String
            Get
                Return GetGlobalResourceObject("DetailPage", Me.SetupKey)
            End Get
        End Property

        ''' <summary>
        ''' Retrieve Title
        ''' </summary>
        Public Overrides ReadOnly Property DisplayTitle() As String
            Get
                Return GetGlobalResourceObject("Title", Me.SetupKey)
            End Get
        End Property

        ''' <summary>
        ''' Retrieve List Page
        ''' </summary>
        Public Overrides ReadOnly Property ListPage() As String
            Get
                Return GetGlobalResourceObject("ListPage", Me.SetupKey)
            End Get
        End Property

        ''' <summary>
        ''' Retrieve Log Page
        ''' </summary>
        Public Overrides ReadOnly Property LogPage() As String
            Get
                Return GetGlobalResourceObject("ListPage", "History")
            End Get
        End Property

        ''' <summary>
        ''' Retrieve other path based on the key
        ''' </summary>
        Protected Function RetrieveOthersDetail(ByVal key As String) As String
            Return GetGlobalResourceObject("DetailPage", key)
        End Function

        Public ReadOnly Property DateTimeFormat() As String
            Get
                Return "dd / MMM / yyyy hh:mm:ss"
            End Get
        End Property

        Public ReadOnly Property UserID() As Integer
            Get
                Return IIf(Session("gstrUserID") IsNot Nothing, Session("gstrUserID"), 0)
            End Get
        End Property

        ''' <summary>
        ''' Retrieve Print Page Path
        ''' </summary>
        Public Overrides ReadOnly Property PrintPage() As String
            Get
                Return GetGlobalResourceObject("ListPage", "Print")
            End Get
        End Property
    End Class
End Namespace

