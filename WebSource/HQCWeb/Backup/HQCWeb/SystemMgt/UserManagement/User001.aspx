<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="User001.aspx.cs" Inherits="HQCWeb.SystemMgt.UserManagement.User001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            var bChk = true;
            
            if ($("#PopupContent_txtUserID").val() == "") {
                alert("아이디를 입력하세요.");
                bChk = false;
                return false;
            }

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
                if (confirm("등록 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            return;
        }

        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }

    </script>

	<!--// POPUP -->
    <div class="popup_wrap">

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
                    <th width="150"><b style="color:red;">*</b><asp:Label ID="lbUserID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtUserID" runat="server" style="width:90%;"></asp:TextBox>
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
                        <asp:TextBox ID="txtMobile" runat="server" style="width:90%;" MaxLength="11" onkeydown="return onlyNumber(event);"  onkeyup="removeChar(event);"></asp:TextBox>
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
				<a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>
                <asp:Button ID="btnSave" runat="server" OnClientClick="javascript:return fn_Validation();"  OnClick="btnSave_Click" Text="Save" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
