<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua40_p05.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua40.Qua40_p05" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">

        function fn_DeleteConfirm() {

            if ($("#PopupContent_ddlDCode").val() == "") {
                alert("불량 현상을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDReasonCode").val() == "") {
                alert("불량 원인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDRespCd").val() == "") {
                alert("귀책처를 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtReworkMsg").val() == "") {
                alert("삭제 사유를 입력하세요.");
                return false;
            } else {
                if (confirm("삭제 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }
        }

        function fn_Delete() {
            $("#PopupContent_btnDelete").click();
        }

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Qua40</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;" />
                    <col style="width:250px;" />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbDeleteCnt" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetDeleteCnt" runat="server"></asp:Label>
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbDRespCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDRespCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReworkMsg" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReworkMsg" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
