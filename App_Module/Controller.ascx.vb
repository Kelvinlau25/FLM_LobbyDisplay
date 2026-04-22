
Partial Class App_Module_Controller
    Inherits System.Web.UI.UserControl

    Public Enum DisplayType
        Full = 3
        Half = 2
        Name = 1
        ID = 0
    End Enum

#Region "Audit"
    Private _createdcompanycode As String = String.Empty
    Public Property CreatedCompanyCode() As String
        Get
            Return _createdcompanycode
        End Get
        Set(ByVal value As String)
            _createdcompanycode = value
        End Set
    End Property

    Private _createdby As String = String.Empty
    Public Property CreatedBy() As String
        Get
            Return _createdby
        End Get
        Set(ByVal value As String)
            _createdby = value
        End Set
    End Property

    Private _createdDate As DateTime = Now
    Public Property CreatedDate() As DateTime
        Get
            Return _createdDate
        End Get
        Set(ByVal value As DateTime)
            _createdDate = value
        End Set
    End Property

    Private _createdLoc As String = String.Empty
    Public Property CreatedLoc() As String
        Get
            Return _createdLoc
        End Get
        Set(ByVal value As String)
            _createdLoc = value
        End Set
    End Property

    Private _UpdatedCompanyCode As String = String.Empty
    Public Property UpdatedCompanyCode() As String
        Get
            Return _UpdatedCompanyCode
        End Get
        Set(ByVal value As String)
            _UpdatedCompanyCode = value
        End Set
    End Property

    Private _UpdatedBy As String = String.Empty
    Public Property UpdatedBy() As String
        Get
            Return _UpdatedBy
        End Get
        Set(ByVal value As String)
            _UpdatedBy = value
        End Set
    End Property

    Private _UpdatedDate As DateTime = Now
    Public Property UpdatedDate() As DateTime
        Get
            Return _UpdatedDate
        End Get
        Set(ByVal value As DateTime)
            _UpdatedDate = value
        End Set
    End Property

    Private _UpdatedLoc As String = String.Empty
    Public Property UpdatedLoc() As String
        Get
            Return _UpdatedLoc
        End Get
        Set(ByVal value As String)
            _UpdatedLoc = value
        End Set
    End Property
#End Region

    Private _connectionstring As String = "ORCL_ACL"
    Public Property connectionstring() As String
        Get
            Return _connectionstring
        End Get
        Set(ByVal value As String)
            _connectionstring = value
        End Set
    End Property

    Private _datetimeformat As String = "dd / MMM / yyyy hh:mm:ss"
    Public Property DateTimeFormat() As String
        Get
            Return _datetimeformat
        End Get
        Set(ByVal value As String)
            _datetimeformat = value
        End Set
    End Property

    ''' <summary>
    ''' Full - Display Company Code + Employee Name + ID
    ''' Half - Display Employee Name + ID
    ''' Name - Display Employee Name
    ''' ID - Display ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _AuditTrailDisplayType As DisplayType = DisplayType.ID
    Public Property AuditTrailDisplayType() As DisplayType
        Get
            Return _AuditTrailDisplayType
        End Get
        Set(ByVal value As DisplayType)
            _AuditTrailDisplayType = value
        End Set
    End Property

    ''' <summary>
    ''' Control the ability the button
    ''' </summary>
    ''' <remarks></remarks>
    Private _editMode As Boolean = True
    Public Property EditMode() As Boolean
        Get
            Return _editMode
        End Get
        Set(ByVal value As Boolean)
            _editMode = value
        End Set
    End Property

    Private _validationGroup As String
    Public Property ValidationGroup() As String
        Get
            Return _validationGroup
        End Get
        Set(ByVal value As String)
            _validationGroup = value
        End Set
    End Property

    Private _add As Boolean = True
    Public Property Add() As Boolean
        Get
            Return _add
        End Get
        Set(ByVal value As Boolean)
            _add = value
        End Set
    End Property

    Private _edit As Boolean = True
    Public Property Edit() As Boolean
        Get
            Return _edit
        End Get
        Set(ByVal value As Boolean)
            _edit = value
        End Set
    End Property

    Private _delete As Boolean = True
    Public Property Delete() As Boolean
        Get
            Return _delete
        End Get
        Set(ByVal value As Boolean)
            _delete = value
        End Set
    End Property

    Private _History As Boolean = True
    Public Property History() As Boolean
        Get
            Return _History
        End Get
        Set(ByVal value As Boolean)
            _History = value
        End Set
    End Property

    Private _Listkey As String = String.Empty
    Public Property ListKey() As String
        Get
            Return _Listkey
        End Get
        Set(ByVal value As String)
            _Listkey = value
        End Set
    End Property

    Public Event AddAction()
    Public Event EditAction()
    Public Event DeleteAction()
    Public Event AddResetAction()
    Public Event EditResetAction()
    Public Event ViewEditAction()
    Public Event ModifyMode()
    Public Event DisplayMode()
    Public Event CancelAction() '' Add By Elven, Handler custom event



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        AddHandler CancelAction, AddressOf OnCancelAction
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim setting As Control.Base = CType(Me.Page, Control.Base)

        hpLink.NavigateUrl = setting.GetUrl(Control.Base.EnumAction.History, Me.ListKey)

        Select Case setting.Action
            Case Control.Base.EnumAction.Add
                btnDelete.Visible = False
                hpLink.Visible = False
                pnconfirmation.Visible = False
                pninfo.Visible = False

                RaiseEvent ModifyMode()

                If Add = False Then
                    Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None))
                End If
            Case Control.Base.EnumAction.Delete
                hpLink.Visible = False
                btnReset.Visible = False
                btnDelete.Visible = False

                RaiseEvent DisplayMode()

                If Delete = False Then
                    Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None))
                End If
            Case Control.Base.EnumAction.Edit
                hpLink.Visible = False
                btnDelete.Visible = False
                pnconfirmation.Visible = False

                RaiseEvent ModifyMode()

                If Edit = False Then
                    Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None))
                End If
            Case Control.Base.EnumAction.View
                btnSubmit.Text = "Edit"
                btnReset.Visible = False
                btnSubmit.CausesValidation = False
                btnDelete.Visible = Delete
                pnconfirmation.Visible = False

                RaiseEvent DisplayMode()
        End Select

        ''check for emc
        'If Session("checkEMC") = "1" Then
        '    Session.Remove("checkEMC")
        '    Dim year As String = Date.Today.Year
        '    Dim month As String = Date.Today.Month
        '    If month < 10 Then
        '        month = "0" + month
        '    End If
        '    Dim dtemc As Data.DataTable = Library.Database.M1S.BLL.SpiningEMCService.CheckEmc(year + month)
        '    If dtemc.Rows.Count > 0 Then
        '        If dtemc.Rows(0)("CLOSED").ToString = "Y" Then
        '            btnSubmit.Visible = False
        '            btnDelete.Visible = False
        '        End If
        '    End If
        'End If


        If Me.ValidationGroup <> String.Empty Then
            btnSubmit.ValidationGroup = ValidationGroup
            cvdeleteyes.ValidationGroup = ValidationGroup
            cvdeleteyes.ErrorMessage = Resources.Message.Deletemessage
        End If

        If pninfo.Visible Then
            Dim _createdtemp As ACL.Object.User = Nothing
            Dim _updatedtemp As ACL.Object.User = Nothing

            'Validation here 


            If Not IsPostBack Then
                'If AuditTrailDisplayType = DisplayType.Full Then
                '    If Me.CreatedCompanyCode = String.Empty Then
                '        Throw New Exception("Please set value into properties created company code")
                '    End If

                '    If Me.UpdatedCompanyCode = String.Empty Then
                '        Throw New Exception("Please set value into properties updated company code")
                '    End If
                'End If

                If AuditTrailDisplayType = DisplayType.Full Or DisplayType.Half Or DisplayType.Name Then
                    If Me.CreatedBy = String.Empty Then
                        Throw New Exception("Please set value into properties created by")
                    End If

                    If Me.UpdatedBy = String.Empty Then
                        Throw New Exception("Please set value into properties updated by")
                    End If
                End If

                'Retrieve Data
                'Select Case AuditTrailDisplayType
                '    Case DisplayType.Full
                '        _createdtemp = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, Me.CreatedCompanyCode, Me.CreatedBy)
                '        _updatedtemp = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, Me.CreatedCompanyCode, Me.CreatedBy)
                '    Case DisplayType.Half, DisplayType.Name
                '        If Me.CreatedCompanyCode = String.Empty Then
                '            _createdtemp = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, Me.CreatedCompanyCode, Me.CreatedBy)
                '        Else
                '            _createdtemp = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, Me.CreatedBy)
                '        End If

                '        If Me.UpdatedCompanyCode = String.Empty Then
                '            _updatedtemp = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, Me.UpdatedCompanyCode, Me.UpdatedBy)
                '        Else
                '            _updatedtemp = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, Me.UpdatedBy)
                '        End If
                'End Select

                'Assigning Data
                Dim _createdtext As String = String.Empty
                Dim _updatedtext As String = String.Empty

                'If AuditTrailDisplayType = DisplayType.Full Then
                '    If _createdtemp.UserCom <> String.Empty Then
                '        trcreatecom.Visible = True
                '        lblcreatedcom.Text = ACL.OracleClass.User.GetCompany(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, _createdtemp.UserCom)
                '    End If

                '    If _updatedtemp.UserCom <> String.Empty Then
                '        trupdatecom.Visible = True
                '        lblupdatedcom.Text = ACL.OracleClass.User.GetCompany(ConfigurationManager.ConnectionStrings(connectionstring).ConnectionString, _updatedtemp.UserCom)
                '    End If
                'End If

                If AuditTrailDisplayType <> DisplayType.Name Then
                    _createdtext = GenerateText(_createdtext, " ID : " & Me.CreatedBy)
                    _updatedtext = GenerateText(_updatedtext, " ID : " & Me.UpdatedBy)
                End If

                If AuditTrailDisplayType = DisplayType.Full Or DisplayType.Half Or DisplayType.Name Then
                    If _createdtemp IsNot Nothing Then
                        If _createdtemp.EmployeeName <> String.Empty Then
                            _createdtext = GenerateText(_createdtext, " Name : " & _createdtemp.EmployeeName)
                        End If

                    End If

                    If _updatedtemp IsNot Nothing Then
                        If _updatedtemp.EmployeeName <> String.Empty Then
                            _updatedtext = GenerateText(_updatedtext, " Name : " & _updatedtemp.EmployeeName)
                        End If
                    End If
                End If

                lblcreatedby.Text = _createdtext
                lblcreateddate.Text = Me.CreatedDate.ToString(Me.DateTimeFormat)
                lblcreatedloc.Text = Me.CreatedLoc

                lblupdatedby.Text = _updatedtext
                lblupdateddate.Text = Me.UpdatedDate.ToString(Me.DateTimeFormat)
                lblUpdatedloc.Text = Me.UpdatedLoc
            End If


        End If


        ''@ Handle User Permission By Form, Add By Elven 19-Aug-2013
        'If Not setting.VersionIsDevelopment Then
        '    Dim oc As Library.Database.ENotice.BLL.ACL.Permission.ObjectControl = setting.ReadPermission()

        '    btnSubmit.Enabled = oc.ADD
        '    If setting.Action = Library.Root.Control.Base.EnumAction.Edit Then
        '        btnSubmit.Enabled = oc.EDIT
        '    End If
        '    btnDelete.Enabled = oc.DELETE
        'End If


        'If Me.EditMode = False Then
        '    btnSubmit.Visible = False
        '    btnDelete.Visible = False
        'End If
        If Not btnSubmit.Text = "Edit" Then
            btnSubmit.OnClientClick = "if(!confirm('Confirm submit the form?')){ return false; } else{showLoading();};"
        End If

    End Sub

    Private Function GenerateText(ByVal Value As String, ByVal AddText As String) As String
        GenerateText = Value

        If Value.Length > 0 Then
            GenerateText = GenerateText & " - " & AddText
        Else
            GenerateText = AddText
        End If
    End Function

    '' Add by Elven, handler existing event, if no override by page
    Public Overridable Sub OnCancelAction()
        Dim setting As Control.Base = CType(Me.Page, Control.Base)
        Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None))
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Dim setting As Control.Base = CType(Me.Page, Control.Base)
        'Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None))

        RaiseEvent CancelAction()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        RaiseEvent DeleteAction()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim setting As Control.Base = CType(Me.Page, Control.Base)
        Select Case setting.Action
            Case Control.Base.EnumAction.Delete
                RaiseEvent DeleteAction()
            Case Control.Base.EnumAction.Add
                RaiseEvent AddAction()
            Case Control.Base.EnumAction.Edit
                RaiseEvent EditAction()
            Case Control.Base.EnumAction.View
                RaiseEvent ViewEditAction()
                Response.Redirect(setting.GetUrl(Control.Base.EnumAction.Edit))
        End Select
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim setting As Control.Base = CType(Me.Page, Control.Base)
        Select Case setting.Action
            Case Control.Base.EnumAction.Add
                RaiseEvent AddResetAction()
            Case Control.Base.EnumAction.Edit
                RaiseEvent EditResetAction()
        End Select
    End Sub

    Protected Sub cvdeleteyes_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvdeleteyes.ServerValidate
        args.IsValid = rbyes.Checked
    End Sub

End Class
