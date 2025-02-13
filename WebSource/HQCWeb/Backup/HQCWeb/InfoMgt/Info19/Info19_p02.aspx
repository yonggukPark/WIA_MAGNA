<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info19_p02.aspx.cs" Inherits="HQCWeb.InfoMgt.Info19.Info19_p02" %>
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
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info19</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStnCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbWorkSeq" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetWorkSeq" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbWorkNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWorkNm" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbWorkCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWorkCd" runat="server" MaxLength="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbToolType" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtToolType" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbQty" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtQty" runat="server" type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbTorqueType" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtTorqueType" runat="server" MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPset" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPset" runat="server" MaxLength="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbToolHole" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtToolHole" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDevID" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevID" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbWorkType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlWorkType" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbScanCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlScanCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMatchCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlMatchCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbModeFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlModeFlag" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark1" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark2" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark2" runat="server"></asp:TextBox>
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
