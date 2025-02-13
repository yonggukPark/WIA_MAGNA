<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info40_p02.aspx.cs" Inherits="HQCWeb.InfoMgt.Info40.Info40_p02" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">

        function fn_ModifyConfirm() {
            if ($("#PopupContent_txtInsCd").val() == "") {
                alert("항목코드를 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtInsNm").val() == "") {
                alert("항목명을 입력하세요.");
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

        function fn_Save() {
            $("#PopupContent_btnSave").click();
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
            <h1><asp:Label ID="lbTitle" runat="server">Info40</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStnCd" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbDevCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbInsCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtInsCd" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbInsNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtInsNm" runat="server"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbDivFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDivFlag" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbSeqId" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetSeqId" runat="server"></asp:Label>
                    </td>
                    <th><asp:Label ID="lbTableNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtTableNm" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbInsCdMin" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlInsCdMin" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbInsCdMax" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlInsCdMax" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbfinishFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlFinishFlag" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbRomidFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlRomidFlag" runat="server"></asp:DropDownList>
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
                <tr>
                    <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
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
                <asp:Button ID="btnRestore"  runat="server" OnClientClick="javascript:return fn_RestoreConfirm();" OnClick="btnRestore_Click" Text="Delete" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
