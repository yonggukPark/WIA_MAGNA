<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info18_p02.aspx.cs" Inherits="HQCWeb.InfoMgt.Info18.Info18_p02" %>
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

        function fn_DeleteConfirm() {

            if (confirm("복원 하시겠습니까?")) {
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

        function fn_Restore() {
            $("#PopupContent_btnRestore").click();
        }
    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info18</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDetail_01" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDetail" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDetail_02" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDetail" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDetail_03" runat="server"></asp:Label></th>
                    <td>
                        <asp:CheckBox ID="chkDetail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDetail_04" runat="server"></asp:Label></th>
                    <td>
                        <asp:CheckBoxList ID="chkListDetail" runat="server"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDetail_05" runat="server"></asp:Label></th>
                    <td>
                        <asp:RadioButton ID="rbDetail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDetail_06" runat="server"></asp:Label></th>
                    <td>
                        <asp:RadioButtonList ID="rbListDetail" runat="server"></asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:fn_Restore();" class="btn ml10" id="a1" runat="server" visible="false">Restore</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
                <asp:Button ID="btnRestore"  runat="server" OnClientClick="javascript:return fn_RestoreConfirm();" OnClick="btnRestore_Click" Text="Delete" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
