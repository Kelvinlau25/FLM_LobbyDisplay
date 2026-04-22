<%@ Page Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true" CodeFile="MM_VerticalScreenFull.aspx.cs" Inherits="acc_MstMain_MM_VerticalScreenFull" title="Master Maintenance Screen Video" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
<script type ="text/javascript" src="../../js/jquery.colorbox.js"></script>

    <style type ="text/css">
			    body{font:12px/1.2 Verdana, sans-serif; padding:0 10px;}
			    a:link, a:visited{text-decoration:none; color:#416CE5; /*border-bottom:1px solid #416CE5;*/}
			    h2{font-size:13px; margin:15px 0 0 0;}
    </style>
    
	<link rel="stylesheet" href ="../../css/colorbox.css" />

    <script type="text/javascript" >
			    $(document).ready(function(){
				    $(".youtube").colorbox({iframe:true, innerWidth:640, innerHeight:390});
			    });
    </script>
    
    <style type="text/css">
        
        .none{display:none;}
        
    </style>
    
    <link rel="stylesheet" type="text/css" href="../../css/smoothness/jquery-ui-1.8.17.custom.css" />
    <link rel="stylesheet" type="text/css" href="../../css/cssmobile.css" />
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <h2><asp:Label ID="lblTitle" runat="server" Text="Master Maintenance Main Screen Video"></asp:Label></h2>
 
  <asp:Panel ID="pnl1" runat="server">
        <table>
            <tr>
                <td>
                  <asp:DropDownList ID="ddlsearch" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="ddlsearch_OnSelectedIndexChanged">
                  <asp:ListItem Selected ="True" Text ="-" Value ="-"></asp:ListItem>
                  <asp:ListItem  Text ="Title" Value ="Upper(title_name)"></asp:ListItem>
                  <asp:ListItem  Text ="Topic" Value ="Upper(topic_name)"></asp:ListItem>
                  </asp:DropDownList>
                </td>
              
                 
                <td>
                  <asp:DropDownList ID="ddlcondition" runat="server" Visible ="false">
                  <asp:ListItem Selected ="True" Text ="LIKE" Value ="LIKE"></asp:ListItem>
                  <asp:ListItem  Text ="EQUAL" Value ="="></asp:ListItem>
                  <asp:ListItem Text ="NOT EQUAL" Value ="<>"></asp:ListItem>
                  </asp:DropDownList>
                </td>
                
                <td><asp:TextBox ID="txtsearch" runat="server" Enabled ="false"  ></asp:TextBox></td>
                <td>
                  <asp:DropDownList ID="ddloperate" runat="server" Visible ="false" Enabled ="false" >
                  <asp:ListItem Selected ="True" Text ="-" Value =" "></asp:ListItem>
                  <asp:ListItem  Text ="AND" Value ="AND"></asp:ListItem>
                  <asp:ListItem Text ="OR" Value ="OR"></asp:ListItem>
                  </asp:DropDownList>
                </td>
                
                <td>
                   <asp:Button ID="ButtonAdd" runat="server" Text="Add" OnClick ="ButtonAdd_Click" Visible ="false"  />
                </td>
                
                <td>
                   <asp:Button ID="ButtonExecute" runat="server" Text="Search" OnClick ="ButtonExecute_Click" />
                </td>
                
                <td>
                   <asp:LinkButton ID="lnkbtnbasic" runat="server" Text="Basic Search" OnClick ="lnkbtnbasic_Click" Visible ="false" />
                </td>
                
                <td>
                   <asp:LinkButton ID="lnkbtnadv" runat="server" Text="Advance Search"  OnClick ="Advance_Click" />
                </td>
           </tr>
          
        </table>
                   <asp:ListBox ID="lstboxsearch" runat ="server" Visible ="false" Width ="30%" SelectionMode ="Single"  ></asp:ListBox>
                   <asp:Button ID="ButtonClear" runat="server" Text="Clear" OnClick ="ButtonClear_Click"  />
   </asp:Panel>
        
     <hr /><br />
     
          <div style="display:block;float:right;">
                   <asp:LinkButton ID="linkbtn" runat="server" Text ="Add Item" />
          </div>
          
                   <asp:gridview ID="Gridview1" Width="100%" PageSize ="10" runat="server" ShowFooter="false" AutoGenerateColumns="false" AllowPaging="true"  cssclass="tblbg"
                    OnRowDeleting="OnRowDeleting" OnRowDataBound = "OnRowDataBound"  OnPageIndexChanging="gridView_PageIndexChanging"  HeaderStyle-CssClass="title_bar" background="white" >
                   <PagerSettings PageButtonCount ="5" Mode="NextPreviousFirstLast" NextPageText ="Next" LastPageText ="Last" FirstPageText ="First" PreviousPageText ="Previous" />
                        <Columns>
                            <asp:CommandField  HeaderStyle-Width="1%" ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl ="~/image/delete.gif" />
                            
                            <asp:BoundField HeaderStyle-Width="1%" HeaderText="ID" DataField="ID_MM_VIDEOS" HeaderStyle-CssClass ="none" ItemStyle-CssClass ="none">
                                <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                            
                            <asp:BoundField HeaderStyle-Width="1%" HeaderText="SCR_ID" DataField="SCR_ID" HeaderStyle-CssClass ="none" ItemStyle-CssClass ="none">
                                <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                        
                            <asp:TemplateField HeaderStyle-Width="79%" HeaderText="Video Files" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                <a class="youtube" target="_blank" href="../PopUp/ImgVideoPreview.aspx?id='<%#Eval("Attach_File")%>'&scr=1"><%#Eval("Attach_File")%></a>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Seek Start" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("SEEK_START")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Seek End" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("SEEK_END")%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                                                  
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Start Time" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("PERIOD_START").Substring(0, Eval("PERIOD_START").Length - 3)%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="End Time" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("PERIOD_END").Substring(0, Eval("PERIOD_END").Length - 3)%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                        
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="3%">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <a class=" btnEditcoqueue btn btn-primary" style="padding: 2px 7px; margin-bottom: 0px;"
                                 target="_self" href="FullMainScreen_Dtl.aspx?action=3&id=<%#Eval("ID_MM_VIDEOS")%>">
                                 Edit</a>
                            </ItemTemplate>
                            </asp:TemplateField> 
                                
                       </Columns>
                            
                            <EmptyDataRowStyle VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" ForeColor="Red" />
                            <EmptyDataTemplate>Record Not Found</EmptyDataTemplate>
                            <PagerStyle CssClass ="title_bar" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="title_bar"></HeaderStyle>
                     
                   </asp:gridview>
                    
         <div style="display:block;float:right;" runat ="server" id="divchange">
                   <asp:Label ID="Label1" runat ="server" Text ="Go To:" />
                   <asp:DropDownList ID="ddlgrdpage" runat ="server" AutoPostBack ="true"  OnSelectedIndexChanged ="ddlgridView_OnSelectedIndexChanged" />
         </div>
         
                   <asp:Label ID="lblerror" runat ="server" ></asp:Label><br />
                   <asp:Panel ID="pnldisplaypage" runat="server"  >
                        <i><!--You are viewing page-->
                            <%=Gridview1.PageIndex + 1%>
                            of
                            <%=Gridview1.PageCount%>
                        </i>
                    </asp:Panel>

    <br />


    <h2><asp:Label ID="lblTitle2" runat="server" Text="Master Maintenance Second Screen Upper Video"></asp:Label></h2>
    
      <asp:Panel ID="pnl2" runat="server">
        <table>
            <tr>
                <td>
                  <asp:DropDownList ID="ddlsearch2" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="ddlsearch_OnSelectedIndexChanged">
                  <asp:ListItem Selected ="True" Text ="-" Value ="-"></asp:ListItem>
                  <asp:ListItem  Text ="Title" Value ="Upper(title_name)"></asp:ListItem>
                  <asp:ListItem  Text ="Topic" Value ="Upper(topic_name)"></asp:ListItem>
                  </asp:DropDownList>
                </td>
              
                 
                <td>
                  <asp:DropDownList ID="ddlcondition2" runat="server" Visible ="false">
                  <asp:ListItem Selected ="True" Text ="LIKE" Value ="LIKE"></asp:ListItem>
                  <asp:ListItem  Text ="EQUAL" Value ="="></asp:ListItem>
                  <asp:ListItem Text ="NOT EQUAL" Value ="<>"></asp:ListItem>
                  </asp:DropDownList>
                </td>
                
                <td><asp:TextBox ID="txtsearch2" runat="server" Enabled ="false"  ></asp:TextBox></td>
                <td>
                  <asp:DropDownList ID="ddloperate2" runat="server" Visible ="false" Enabled ="false" >
                  <asp:ListItem Selected ="True" Text ="-" Value =" "></asp:ListItem>
                  <asp:ListItem  Text ="AND" Value ="AND"></asp:ListItem>
                  <asp:ListItem Text ="OR" Value ="OR"></asp:ListItem>
                  </asp:DropDownList>
                </td>
                
                <td>
                   <asp:Button ID="ButtonAdd2" runat="server" Text="Add" OnClick ="ButtonAdd_Click" Visible ="false"  />
                </td>
                
                <td>
                   <asp:Button ID="ButtonExecute2" runat="server" Text="Search" OnClick ="ButtonExecute_Click" />
                </td>
                
                <td>
                   <asp:LinkButton ID="lnkbtnbasic2" runat="server" Text="Basic Search" OnClick ="lnkbtnbasic_Click" Visible ="false" />
                </td>
                
                <td>
                   <asp:LinkButton ID="lnkbtnadv2" runat="server" Text="Advance Search"  OnClick ="Advance_Click" />
                </td>
           </tr>
          
        </table>
                   <asp:ListBox ID="lstboxsearch2" runat ="server" Visible ="false" Width ="30%" SelectionMode ="Single"  ></asp:ListBox>
                   <asp:Button ID="ButtonClear2" runat="server" Text="Clear" OnClick ="ButtonClear_Click"  />
   </asp:Panel>
        
     <hr /><br />
     
          <div style="display:block;float:right;">
                   <asp:LinkButton ID="linkbtn2" runat="server" Text ="Add Item" />
          </div>
          
                   <asp:gridview ID="Gridview2" Width="100%" PageSize ="10" runat="server" ShowFooter="false" AutoGenerateColumns="false" AllowPaging="true"  cssclass="tblbg"
                    OnRowDeleting="OnRowDeleting2" OnRowDataBound = "OnRowDataBound"  OnPageIndexChanging="gridView2_PageIndexChanging"  HeaderStyle-CssClass="title_bar" background="white" >
                   <PagerSettings PageButtonCount ="5" Mode="NextPreviousFirstLast" NextPageText ="Next" LastPageText ="Last" FirstPageText ="First" PreviousPageText ="Previous" />
                        <Columns>
                            <asp:CommandField  HeaderStyle-Width="1%" ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl ="~/image/delete.gif" />
                            
                            <asp:BoundField HeaderStyle-Width="1%" HeaderText="ID" DataField="ID_MM_VIDEOS" HeaderStyle-CssClass ="none" ItemStyle-CssClass ="none">
                                <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                            
                            <asp:BoundField HeaderStyle-Width="1%" HeaderText="SCR_ID" DataField="SCR_ID" HeaderStyle-CssClass ="none" ItemStyle-CssClass ="none">
                                <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                        
                            <asp:TemplateField HeaderStyle-Width="79%" HeaderText="Video Files" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                <a class="youtube" target="_blank" href="../PopUp/ImgVideoPreview.aspx?ID='<%#Eval("Attach_File")%>'&scr=2"><%#Eval("Attach_File")%></a>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Seek Start" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("SEEK_START")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Seek End" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("SEEK_END")%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Start Time" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("PERIOD_START").Substring(0, Eval("PERIOD_START").Length - 3)%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="End Time" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("PERIOD_END").Substring(0, Eval("PERIOD_END").Length - 3)%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                        
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="3%">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <a class=" btnEditcoqueue btn btn-primary" style="padding: 2px 7px; margin-bottom: 0px;"
                                 target="_self" href="Upper2ndScreen_Dtl.aspx?action=3&id=<%#Eval("ID_MM_VIDEOS")%>">
                                 Edit</a>
                            </ItemTemplate>
                            </asp:TemplateField> 
                                
                       </Columns>
                            
                            <EmptyDataRowStyle VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" ForeColor="Red" />
                            <EmptyDataTemplate>Record Not Found</EmptyDataTemplate>
                            <PagerStyle CssClass ="title_bar" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="title_bar"></HeaderStyle>
                     
                   </asp:gridview>
                    
         <div style="display:block;float:right;" runat ="server" id="divchange2">
                   <asp:Label ID="Label2" runat ="server" Text ="Go To:" />
                   <asp:DropDownList ID="ddlgrdpage2" runat ="server" AutoPostBack ="true"  OnSelectedIndexChanged ="ddlgridView2_OnSelectedIndexChanged" />
         </div>
         
                   <asp:Label ID="lblerror2" runat ="server" ></asp:Label><br />
                   <asp:Panel ID="pnldisplaypage2" runat="server"  >
                        <i><!--You are viewing page-->
                            <%=Gridview2.PageIndex + 1%>
                            of
                            <%=Gridview2.PageCount%>
                        </i>
                    </asp:Panel>

    <br />
    
    
    
    <h2><asp:Label ID="lblTitle3" runat="server" Text="Master Maintenance Second Screen Lower Video"></asp:Label></h2>
    
      <asp:Panel ID="pnl3" runat="server">
        <table>
            <tr>
                <td>
                  <asp:DropDownList ID="ddlsearch3" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="ddlsearch_OnSelectedIndexChanged">
                  <asp:ListItem Selected ="True" Text ="-" Value ="-"></asp:ListItem>
                  <asp:ListItem  Text ="Title" Value ="Upper(title_name)"></asp:ListItem>
                  <asp:ListItem  Text ="Topic" Value ="Upper(topic_name)"></asp:ListItem>
                  </asp:DropDownList>
                </td>
              
                 
                <td>
                  <asp:DropDownList ID="ddlcondition3" runat="server" Visible ="false">
                  <asp:ListItem Selected ="True" Text ="LIKE" Value ="LIKE"></asp:ListItem>
                  <asp:ListItem  Text ="EQUAL" Value ="="></asp:ListItem>
                  <asp:ListItem Text ="NOT EQUAL" Value ="<>"></asp:ListItem>
                  </asp:DropDownList>
                </td>
                
                <td><asp:TextBox ID="txtsearch3" runat="server" Enabled ="false"  ></asp:TextBox></td>
                <td>
                  <asp:DropDownList ID="ddloperate3" runat="server" Visible ="false" Enabled ="false" >
                  <asp:ListItem Selected ="True" Text ="-" Value =" "></asp:ListItem>
                  <asp:ListItem  Text ="AND" Value ="AND"></asp:ListItem>
                  <asp:ListItem Text ="OR" Value ="OR"></asp:ListItem>
                  </asp:DropDownList>
                </td>
                
                <td>
                   <asp:Button ID="ButtonAdd3" runat="server" Text="Add" OnClick ="ButtonAdd_Click" Visible ="false"  />
                </td>
                
                <td>
                   <asp:Button ID="ButtonExecute3" runat="server" Text="Search" OnClick ="ButtonExecute_Click" />
                </td>
                
                <td>
                   <asp:LinkButton ID="lnkbtnbasic3" runat="server" Text="Basic Search" OnClick ="lnkbtnbasic_Click" Visible ="false" />
                </td>
                
                <td>
                   <asp:LinkButton ID="lnkbtnadv3" runat="server" Text="Advance Search"  OnClick ="Advance_Click" />
                </td>
           </tr>
          
        </table>
                   <asp:ListBox ID="lstboxsearch3" runat ="server" Visible ="false" Width ="30%" SelectionMode ="Single"  ></asp:ListBox>
                   <asp:Button ID="ButtonClear3" runat="server" Text="Clear" OnClick ="ButtonClear_Click"  />
   </asp:Panel>
        
     <hr /><br />
     
          <div style="display:block;float:right;">
                   <asp:LinkButton ID="linkbtn3" runat="server" Text ="Add Item" />
          </div>
          
                   <asp:gridview ID="Gridview3" Width="100%" PageSize ="10" runat="server" ShowFooter="false" AutoGenerateColumns="false" AllowPaging="true"  cssclass="tblbg"
                    OnRowDeleting="OnRowDeleting3" OnRowDataBound = "OnRowDataBound"  OnPageIndexChanging="gridView3_PageIndexChanging"  HeaderStyle-CssClass="title_bar" background="white" >
                   <PagerSettings PageButtonCount ="5" Mode="NextPreviousFirstLast" NextPageText ="Next" LastPageText ="Last" FirstPageText ="First" PreviousPageText ="Previous" />
                        <Columns>
                            <asp:CommandField  HeaderStyle-Width="1%" ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl ="~/image/delete.gif" />
                            
                            <asp:BoundField HeaderStyle-Width="1%" HeaderText="ID" DataField="ID_MM_VIDEOS" HeaderStyle-CssClass ="none" ItemStyle-CssClass ="none">
                                <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                            
                            <asp:BoundField HeaderStyle-Width="1%" HeaderText="SCR_ID" DataField="SCR_ID" HeaderStyle-CssClass ="none" ItemStyle-CssClass ="none">
                                <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundField>
                        
                            <asp:TemplateField HeaderStyle-Width="79%" HeaderText="Video Files" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                <a class="youtube" target="_blank" href="../PopUp/ImgVideoPreview.aspx?ID='<%#Eval("Attach_File")%>'&scr=3"><%#Eval("Attach_File")%></a>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Seek Start" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("SEEK_START")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Seek End" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("SEEK_END")%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Start Time" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("PERIOD_START").Substring(0, Eval("PERIOD_START").Length - 3)%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="End Time" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                <%#Eval("PERIOD_END").Substring(0, Eval("PERIOD_END").Length - 3)%>
                                 </ItemTemplate>
                            </asp:TemplateField>
                        
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="3%">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <a class=" btnEditcoqueue btn btn-primary" style="padding: 2px 7px; margin-bottom: 0px;"
                                 target="_self" href="Lower2ndScreen_Dtl.aspx?action=3&id=<%#Eval("ID_MM_VIDEOS")%>">
                                 Edit</a>
                            </ItemTemplate>
                            </asp:TemplateField> 
                    
                                
                       </Columns>
                            
                            <EmptyDataRowStyle VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" ForeColor="Red" />
                            <EmptyDataTemplate>Record Not Found</EmptyDataTemplate>
                            <PagerStyle CssClass ="title_bar" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="title_bar"></HeaderStyle>
                     
                   </asp:gridview>
                    
         <div style="display:block;float:right;" runat ="server" id="divchange3">
                   <asp:Label ID="Label3" runat ="server" Text ="Go To:" />
                   <asp:DropDownList ID="ddlgrdpage3" runat ="server" AutoPostBack ="true"  OnSelectedIndexChanged ="ddlgridView3_OnSelectedIndexChanged" />
         </div>
         
                   <asp:Label ID="lblerror3" runat ="server" ></asp:Label><br />
                   <asp:Panel ID="pnldisplaypage3" runat="server">
                        <i><!--You are viewing page-->
                            <%=Gridview3.PageIndex + 1%>
                            of
                            <%=Gridview3.PageCount%>
                        </i>
                    </asp:Panel>

       <hr /><br />
       
    <asp:Label ID="Label4" runat="server" Text="Scrolling Text" Font-Bold="True"></asp:Label>&nbsp;<asp:Label ID="Label5" runat="server" Text=":"></asp:Label>
    &nbsp;<asp:TextBox ID="txtFooter" runat="server" Width="90%" TextMode="MultiLine"></asp:TextBox>&nbsp;<asp:Button ID="btnFooter" runat="server" style="height: 26px;vertical-align: top;float: right;" Text="Save" OnClick="btnFooter_Click" />
    
    

    <asp:Button ID="Button1" runat="server" Text="Display at Lobby" style="display:none;" OnClick="Button1_Click"/>
    
    
    
</asp:Content>

