<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info40_p03.aspx.cs" Inherits="HQCWeb.InfoMgt.Info40.Info40_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("SHOP 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("LINE 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlStnCd").val() == "") {
                alert("공정 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDevCd").val() == "") {
                alert("장비 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlCarType").val() == "") {
                alert("차종 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtInsCd").val() == "") {
                alert("항목코드를 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtInsNm").val() == "") {
                alert("항목명을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlDivFlag").val() == "") {
                alert("구분을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtInsNm").val() == "") {
                alert("등록순번을 입력하세요.");
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
            <h1><asp:Label ID="lbTitle" runat="server">Info40</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <ContentTemplate>
            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:250px;">
                    <col style="width:60px;">
                    <col style="width:250px;">
                </colgroup>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStnCd" runat="server" OnSelectedIndexChanged="ddlStnCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDevCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbInsCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtInsCd" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbInsNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtInsNm" runat="server"></asp:TextBox>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDivFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDivFlag" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbSeqId" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtSeqId" runat="server" type="number"></asp:TextBox>
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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>

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
