<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua34_p02.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua34.Qua34_p02" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_ConfirmConfirm() {
            if ($("#PopupContent_txtReworkMsg").val() == "") {
                alert("사유를 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlInspCd").val() == "") {
                alert("검사유형을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlReturnFlag").val() == "") {
                alert("불량판정을 선택하세요.");
                return false;
            } else {
                if (confirm("판정 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }
        }

        function fn_Confirm() {
            $("#PopupContent_btnConfirm").click();
        }
    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Qua34</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReworkMsg" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReworkMsg" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbStartStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetStartStorageCd" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbInspCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlInspCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReturnFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlReturnFlag" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Confirm();" class="btn ml10" id="aConfirm" runat="server" visible="false">Confirm</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnConfirm" runat="server" OnClientClick="javascript:return fn_ConfirmConfirm();" OnClick="btnConfirm_Click" Text="Confirm" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
