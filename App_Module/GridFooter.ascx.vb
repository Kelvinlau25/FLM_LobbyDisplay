
Partial Class UserControl_GridFooter
    Inherits System.Web.UI.UserControl

    Private setting As Control.Base
    Private logSetting As Control.LogBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.BindPage()
            Me.setNavigator()
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.BindPage()
        Me.setNavigator()
    End Sub

    Private _audit As Boolean = False
    Public WriteOnly Property Audit() As Boolean
        Set(ByVal value As Boolean)
            _audit = value
        End Set
    End Property

    Private _total As Integer = 0
    Public Property TotalRecords() As Integer
        Get
            Return _total
        End Get
        Set(ByVal Value As Integer)
            _total = Value
            Me._totalPage = Math.Ceiling(_total / Library.Root.Other.BusinessLogicBase.MaxQuantityPerPage)
        End Set
    End Property

    Private _totalPage As Integer = 0
    Public ReadOnly Property TotalPage() As Integer
        Get
            Return Math.Ceiling(_total / Library.Root.Other.BusinessLogicBase.MaxQuantityPerPage)
        End Get
    End Property

    Protected Sub ddlPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPage.SelectedIndexChanged
        If _audit Then
            logSetting = CType(Me.Page, Control.LogBase)
            logSetting.PageNo = ddlPage.SelectedItem.Text
            Response.Redirect(logSetting.GenerateList)
        Else
            setting = CType(Me.Page, Control.Base)
            setting.PageNo = ddlPage.SelectedItem.Text
            Response.Redirect(setting.GenerateList)
        End If
    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNext.Click
        If _audit Then
            logSetting = CType(Me.Page, Control.LogBase)

            If ddlPage.Items.Count > 0 Then
                If (logSetting.PageNo >= ddlPage.Items(ddlPage.Items.Count - 1).Value) Then
                    Return
                End If
            Else
                Return
            End If

            logSetting.PageNo = logSetting.PageNo + 1
            Response.Redirect(logSetting.GenerateList)
        Else
            setting = CType(Me.Page, Control.Base)

            If ddlPage.Items.Count > 0 Then
                If (setting.PageNo >= ddlPage.Items(ddlPage.Items.Count - 1).Value) Then
                    Return
                End If
            Else
                Return
            End If

            setting.PageNo = setting.PageNo + 1
            Response.Redirect(setting.GenerateList)
        End If
    End Sub

    Protected Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrevious.Click
        If _audit Then
            logSetting = CType(Me.Page, Control.LogBase)
            If (logSetting.PageNo <= 0) Then
                Return
            End If

            logSetting.PageNo = logSetting.PageNo - 1
            Response.Redirect(logSetting.GenerateList)
        Else
            setting = CType(Me.Page, Control.Base)
            If (setting.PageNo <= 0) Then
                Return
            End If

            setting.PageNo = setting.PageNo - 1
            Response.Redirect(setting.GenerateList)
        End If
    End Sub

    Protected Sub btnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLast.Click
        If _audit Then
            logSetting = CType(Me.Page, Control.LogBase)
            logSetting.PageNo = ddlPage.Items(ddlPage.Items.Count - 1).Value
            Response.Redirect(logSetting.GenerateList)
        Else
            setting = CType(Me.Page, Control.Base)
            setting.PageNo = ddlPage.Items(ddlPage.Items.Count - 1).Value
            Response.Redirect(setting.GenerateList)
        End If
    End Sub

    Protected Sub btnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFirst.Click
        If _audit Then
            logSetting = CType(Me.Page, Control.LogBase)
            logSetting.PageNo = 1
            Response.Redirect(logSetting.GenerateList)
        Else
            setting = CType(Me.Page, Control.Base)
            setting.PageNo = 1
            Response.Redirect(setting.GenerateList)
        End If
    End Sub

    Private Sub BindPage()
        If _audit Then
            logSetting = CType(Me.Page, Control.LogBase)
            If logSetting.PageNo > Me._totalPage Then
                logSetting.PageNo = Me._totalPage
            End If
        Else
            setting = CType(Me.Page, Control.Base)
            If setting.PageNo > Me._totalPage Then
                setting.PageNo = Me._totalPage
            End If
        End If

        ddlPage.Items.Clear()
        For i As Integer = 1 To Me._totalPage
            ddlPage.Items.Add(i)
        Next

        lblTotalRecord.Text = String.Format("Total of {0} records found", Me.TotalRecords)
    End Sub

    Protected Sub setNavigator()
        If _audit Then
            logSetting = CType(Me.Page, Control.LogBase)
        Else
            setting = CType(Me.Page, Control.Base)
        End If


        If (Me._total <= 1) Then
            Me.btnFirst.Visible = False
            Me.btnLast.Visible = False
            Me.btnNext.Visible = False
            Me.btnPrevious.Visible = False
            Return
        Else
            Me.btnFirst.Visible = True
            Me.btnLast.Visible = True
            Me.btnNext.Visible = True
            Me.btnPrevious.Visible = True
        End If

        Me.ddlPage.SelectedIndex = If(_audit, logSetting.PageNo - 1, setting.PageNo - 1)
        If ddlPage.SelectedIndex = 0 Then
            Me.btnFirst.Visible = False
            Me.btnPrevious.Visible = False
        Else
            Me.btnFirst.Visible = True
            Me.btnPrevious.Visible = True
        End If

        Dim iPage As Integer = Me.TotalPage
        If ddlPage.SelectedIndex >= iPage - 1 Then
            Me.btnLast.Visible = False
            Me.btnNext.Visible = False
        Else
            Me.btnLast.Visible = True
            Me.btnNext.Visible = True
        End If
    End Sub
End Class
