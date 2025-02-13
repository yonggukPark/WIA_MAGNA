<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="User002.aspx.cs" Inherits="HQCWeb.SystemMgt.UserManagement.User002" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_ModifyConfirm() {

            var bChk = true;


            if ($("#PopupContent_ddlUserDept").val() == "") {
                alert("부서를 선택하세요.");
                bChk = false;
                return false;
            }

            if ($("#PopupContent_txtMobile").val() == "") {
                alert("핸드폰 번호를 입력하세요.");
                bChk = false;
                return false;
            }

            if ($("#PopupContent_txtMobile").val().length < 11) {
                alert("핸드폰 번호 11자리를 정확히 입력하세요.");
                bChk = false;
                return false;
            }

            if (bChk) {
                if (confirm("수정 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
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

        function fn_SendPWDConfirm() {
            if (confirm("임시비밀번호를 발급 하시겠습니까?")) {
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

        function fn_SendTempPWD() {
            $("#PopupContent_btnSendPWD").click();
        }

    </script>

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">

        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00040</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th width="150"><asp:Label ID="lbUserID" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetUserID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbUserNM" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtUserNM" runat="server" style="width:90%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbUserDept" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUserDept" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbTel" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtTel" runat="server" style="width:90%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbMobile" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" style="width:90%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbEmail" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" style="width:90%;"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
				<a href="javascript:fn_Modify();"       class="btn ml10" id="aModify"   runat="server" visible="false">Modify</a>
                <a href="javascript:fn_SendTempPWD();"  class="btn ml10" id="aTempPWD"  runat="server" visible="false" style="width:100px;">Send Temp PWD</a>
                <a href="javascript:fn_Delete();"       class="btn ml10" id="aDelete"   runat="server" visible="false">Delete</a>
				<a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();"    OnClick="btnModify_Click" Text="Modify" style="display:none;"/>&nbsp;
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();"    OnClick="btnDelete_Click" Text="Delete" style="display:none;"/>&nbsp;
                <asp:Button ID="btnSendPWD" runat="server" OnClientClick="javascript:return fn_SendPWDConfirm();"   OnClick="btnSendPWD_Click" Text="Send Temp PWD" style="display:none;"/>&nbsp;
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
