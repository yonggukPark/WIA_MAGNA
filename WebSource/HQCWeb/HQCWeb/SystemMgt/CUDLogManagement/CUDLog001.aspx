<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="CUDLog001.aspx.cs" Inherits="HQCWeb.SystemMgt.CUDLogManagement.CUDLog001" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00060</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><asp:Label ID="lbSeq" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:Label ID="lbGetSeq" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuNM" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetMenuNM" runat="server"></asp:Label>
                    </td>

                    <th><asp:Label ID="lbType" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbData" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <div id="divData" runat="server" style="height:280px; overflow-y:scroll; width:100%;">
                            <asp:Literal ID="ltGetData" runat="server"></asp:Literal>    
                        </div>
                    </td>
                </tr>
                
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>