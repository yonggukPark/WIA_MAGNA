<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info06_p02.aspx.cs" Inherits="HQCWeb.InfoMgt.Info06.Info06_p02" %>
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

        function fn_RestoreConfirm() {

            if (confirm("복구 하시겠습니까?")) {
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
            <h1><asp:Label ID="lbTitle" runat="server">Info06</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetStnCd" runat="server"></asp:Label>
                    </td>
                    <th><asp:Label ID="lbStnNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtStnNm" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbNgCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtNgCd" runat="server"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbMulti" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMulti" runat="server" type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbStn" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:CheckBoxList ID="chkListStn" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Item 1</asp:ListItem>
                            <asp:ListItem Value="2">Item 2</asp:ListItem>
                            <asp:ListItem Value="3">Item 3</asp:ListItem>
                            <asp:ListItem Value="4">Item 4</asp:ListItem>
                            <asp:ListItem Value="5">Item 5</asp:ListItem>
                            <asp:ListItem Value="6">Item 6</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbReworkStn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlReworkStn" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbPrStn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPrStn" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbTotStn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlTotStn" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbMergeStn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlMergeStn" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbReinputStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReinputStnCd" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbViewYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlViewYn" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark1" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark1" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbRemark2" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark2" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:fn_Restore();" class="btn ml10" id="aRestore" runat="server" visible="false">Restore</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
                <asp:Button ID="btnRestore"  runat="server" OnClientClick="javascript:return fn_RestoreConfirm();" OnClick="btnRestore_Click" Text="Restore" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
