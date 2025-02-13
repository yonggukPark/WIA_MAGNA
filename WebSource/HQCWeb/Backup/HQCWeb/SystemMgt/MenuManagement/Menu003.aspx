<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Menu003.aspx.cs" Inherits="HQCWeb.SystemMgt.MenuManagement.Menu003" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_ModifyConfirm() {

            if ($("#PopupContent_txtMenuNM").val() == "") {
                alert("메뉴명을 입력하세요.");
                $("#PopupContent_txtMenuNM").focus();
                return false;
            }
            else if ($("#PopupContent_txtMenuParentID").val() == "") {
                if ($("#PopupContent_ddlMenuLevel").val() != "1") {
                    alert("부모 메뉴 아이디를 입력하세요.");
                    $("#PopupContent_txtMenuParentID").focus();
                    return false;
                }
            } else if (confirm("수정 하시겠습니까?")) {
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

        function fn_levelChage() {
            var levelVal = $("#PopupContent_hidMenuLevel").val();

            if ($("#PopupContent_ddlMenuLevel").val() == "1") {
                if (confirm("대메뉴로 이동시 부모 아이디가 삭제됩니다. 변경하시겠습니까?")) {
                    $("#PopupContent_txtMenuParentID").val("");
                } else {
                    $("#PopupContent_ddlMenuLevel").val(levelVal);
                }
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
    <div class="popup_wrap">

        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00010</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><asp:Label ID="lbMenuID" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetMenuID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuNM" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMenuNM" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuParentID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMenuParentID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuLevel" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlMenuLevel" runat="server" onchange="javascript:fn_levelChage();"></asp:DropDownList>
                        <asp:HiddenField ID="hidMenuLevel" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuIDX" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMenuIDX" runat="server" style="IME-MODE:disabled;" MaxLength="3" onkeydown="return onlyNumber(event);"  onkeyup="removeChar(event);"  width="20%">0</asp:TextBox>
                    </td>   
                </tr>
                <tr>
                    <th><asp:Label ID="lbAssamblyID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtAssamblyID" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbViewID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtViewID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbUseYN" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                    </td>   
                </tr>
                <tr>
                    <th><asp:Label ID="lbApproval" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlApprovalYN" runat="server"></asp:DropDownList>
                    </td>   
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn" id="aModify" runat="server" visible="false">Modify</a>
				<a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
				<a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;"/>
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;"/>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
