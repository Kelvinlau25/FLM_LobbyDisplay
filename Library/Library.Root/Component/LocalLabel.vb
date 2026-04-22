Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Resources
Imports System.Globalization
Imports System.Reflection
Imports System.Threading


Namespace LacalizeLabel

    <ToolboxData("<{0}:LocalLabel ID=""LocalLabel"" runat=""server""></{0}:LocalLabel>")> _
    Public Class LocalLabel
        Inherits WebControl

        Friend _rm As ResourceManager



        <Bindable(True)> _
        <Category("Appearance")> _
        <DefaultValue("")> _
        <Localizable(True)> _
        Public Property Key() As [String]
            Get
                Return If((ViewState("LocalLabel_Text") IsNot Nothing), DirectCast(ViewState("LocalLabel_Text"), [String]), "")
            End Get
            Set(ByVal value As [String])
                ViewState("LocalLabel_Text") = value
            End Set
        End Property

        <Bindable(True)> _
        <Category("Appearance")> _
        <DefaultValue("")> _
        <Localizable(True)> _
        Public ReadOnly Property Text() As [String]
            Get
                If _rm Is Nothing Then
                    _rm = New ResourceManager("resources.Language", Assembly.Load("App_GlobalResources"))
                End If

                Return If((Me.Key <> ""), _rm.GetString(Me.Key, Thread.CurrentThread.CurrentCulture).Trim(), "")
            End Get
        End Property






        'Protected Overrides ReadOnly Property TagKey() As HtmlTextWriterTag
        '    Get
        '        Return "" 'HtmlTextWriterTag.Label
        '    End Get
        'End Property

        Public Overrides Sub RenderBeginTag(ByVal writer As HtmlTextWriter)
            'if (_rm == null)
            '{
            '    _rm = new ResourceManager("resources.FormText", Assembly.Load("App_GlobalResources"));
            '}
            MyBase.RenderBeginTag(writer)
        End Sub

        Protected Overrides Sub RenderContents(ByVal output As HtmlTextWriter)
            output.WriteEncodedText(Me.Text)
        End Sub

        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            'if (_rm == null)
            '{
            '    _rm = new ResourceManager("resources.FormText", Assembly.Load("App_GlobalResources"));
            '}
            MyBase.OnInit(e)
        End Sub

    End Class


End Namespace