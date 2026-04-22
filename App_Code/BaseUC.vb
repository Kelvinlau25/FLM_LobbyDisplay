Imports System.Resources
Imports System.Threading
Imports System.Reflection
Imports System.Globalization

Namespace Control
    Public MustInherit Class BaseUC
        Inherits System.Web.UI.UserControl

        Dim _rm As ResourceManager
        Dim _ci As CultureInfo

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)

            ''// Set Language
            Dim lang As String = "en-US"
            If Request.Cookies("MalaysiaTorayNaviLanguage") IsNot Nothing Then
                lang = Request.Cookies("MalaysiaTorayNaviLanguage").Value
            End If
            SetCulture(lang, lang)
        End Sub

        Public Sub VirtualSessionClear()
            If Session("KioskVirtualSession") IsNot Nothing Then
                Session("KioskVirtualSession") = Nothing
            End If
        End Sub

        Public Function VirtualSessionRead() As String
            If Session("KioskVirtualSession") Is Nothing Then
                Session("KioskVirtualSession") = DateTime.Now.Year & DateTime.Now.DayOfYear.ToString("000") & DateTime.Now.Hour.ToString("00") & DateTime.Now.Minute.ToString("00") & DateTime.Now.Second.ToString("00")
            End If

            Return Session("KioskVirtualSession")
        End Function

        ''' <summary>
        ''' Set Language Culture
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="locale"></param>
        ''' <remarks></remarks>
        Protected Sub SetCulture(ByVal name As String, ByVal locale As String)

            Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo(name)
            Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo(locale)

            _ci = Threading.Thread.CurrentThread.CurrentCulture
        End Sub

        Public Function LocalizeText(ByVal Key As String) As String
            If _rm Is Nothing Then
                _rm = New ResourceManager("resources.Language", Assembly.Load("App_GlobalResources"))
            End If

            Return If((Key <> ""), _rm.GetString(Key, Thread.CurrentThread.CurrentCulture).Trim(), "")
        End Function

        Public Enum LanguagePack
            English = 0
            Malay = 1
        End Enum

        Public ReadOnly Property Language() As LanguagePack
            Get
                Dim lp As LanguagePack = LanguagePack.English

                If Thread.CurrentThread.CurrentCulture.ToString.Equals("ms-MY") Then
                    lp = LanguagePack.Malay
                End If

                Return lp
            End Get
        End Property

        Public ReadOnly Property ImageLanguagePrefix() As String
            Get
                Return IIf(Threading.Thread.CurrentThread.CurrentCulture.ToString().Equals("ms-MY"), "_m", "")
            End Get
        End Property
    End Class
End Namespace
