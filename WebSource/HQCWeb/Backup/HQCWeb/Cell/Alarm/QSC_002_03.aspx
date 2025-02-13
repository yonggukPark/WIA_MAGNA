<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="QSC_002_03.aspx.cs" Inherits="HQCWeb.Cell.Alarm.QSC_002_03" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <style>
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 55%;
            position : absolute; 
        }
    </style>     

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_61000</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:80px;" />
                    <col style="width:150px;" />
                    <col style="width:80px;" />
                    <col style="width:270px;" />
                    <col style="width:80px;" />
                    <col style="width:220px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbSearchDate" runat="server"></asp:Label></th>
                    <td colspan="6">
                        <asp:TextBox ID="txtFromDt" runat="server" style="width:80px; z-index:2;"></asp:TextBox> &nbsp;
                        <asp:DropDownList ID="ddlFromTime" runat="server"></asp:DropDownList> : <asp:DropDownList ID="ddlFromMin" runat="server"></asp:DropDownList> &nbsp;
                        <asp:TextBox ID="txtToDt" runat="server" style="width:80px;  z-index:2;"></asp:TextBox> &nbsp;
                        <asp:DropDownList ID="ddlToTime" runat="server"></asp:DropDownList> : <asp:DropDownList ID="ddlToMin" runat="server"></asp:DropDownList> 
                    </td>                    
                </tr>
                <tr>
                    <th><asp:Label ID="lbSearchStandard" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo" style="width:125px;">
                            <asp:TextBox id="txtStandard" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbSearchOpration" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo" style="width:250px;">
                            <asp:TextBox id="txtOperation" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                            <asp:TextBox id="txtOperationHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>                        
                    </td>
                    <th><asp:Label ID="lbSearchEquipment" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo" style="width:200px;">
                            <asp:TextBox id="txtEquipment" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:GridControl runat="server" ID="grid" Height="595" Horizontal="Y" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

           <%-- <uc1:PagingControl runat="server" id="PagingControl" onPageSizeChanged="PageSizeChanged" Visible="false"  />--%>
            
        </div>
    </div>

    <script>
        jQuery(document).ready(function ($) {
            $("#MainContent_txtStandard").comboTree({
                source: <%=strStandardJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: false
            });

            $("#MainContent_txtOperation").comboTree({
                source: <%=strOperationJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true, 
                valueChange: true, 
                hidCon: "MainContent_txtOperationHidden"
            });

            $("#MainContent_txtEquipment").comboTree({
                source: <%=strEquipmentJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: false
            });
        });
    </script>

</asp:Content>