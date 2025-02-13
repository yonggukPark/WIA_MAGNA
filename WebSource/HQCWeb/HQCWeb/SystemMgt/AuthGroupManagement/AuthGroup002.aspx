<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="AuthGroup002.aspx.cs" Inherits="HQCWeb.SystemMgt.AuthGroupManagement.AuthGroup002" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_ModifyConfirm() {

            if (confirm("수정 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_DeleteConfirm() {

            if (confirm("삭제 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_Modify() {
            $("#PopupContent_btnModify").click();
        }

        function fn_Delete() {
            $("#PopupContent_btnDelete").click();
        }
    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00090</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><asp:Label ID="lbAuthGroupID" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetAuthGroupID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbAuthGroupKR" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtAuthGroupKR" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbAuthGroupEN" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtAuthGroupEN" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbAuthGroupLO" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtAuthGroupLO" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbUseYN" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                    </td>   
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
