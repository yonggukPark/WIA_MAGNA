<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua34_p01.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua34.Qua34_p01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">

        function fn_ModifyConfirm() {
            if ($("#PopupContent_txtDate").val() == "") {
                alert("발생일을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlDRespCd").val() == "") {
                alert("귀책처를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDReasonCode").val() == "") {
                alert("불량 원인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDCode").val() == "") {
                alert("불량 현상을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtReworkMsg").val() == "") {
                alert("사유를 입력하세요.");
                return false;
            } else {
                if (confirm("수정 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }
        }

        function fn_Modify() {
            $("#PopupContent_btnModify").click();
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbRegDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbReturnDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetReturnDt" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDRespCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDRespCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDRespDetail" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDRespDetail" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetCarType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCompleteNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetCompleteNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDCode" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDReasonCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDReasonCode" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReworkMsg" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReworkMsg" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbEvCheck" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEvCheck" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbEolCheck" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEolCheck" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbShipFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShipFlag" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
