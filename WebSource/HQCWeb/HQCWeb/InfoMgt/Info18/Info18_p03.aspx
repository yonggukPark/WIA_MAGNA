<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info18_p03.aspx.cs" Inherits="HQCWeb.InfoMgt.Info18.Info18_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_체크할 항목의 컨트롤 아이디").val() == "") {
                alert("체크할 항목의 메세지를 입력하세요.");
                return false;
            } else {
                if (confirm("등록 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            return true;
        }

        function fn_Save() {
            $("#PopupContent_btnSave").click();
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
                <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
