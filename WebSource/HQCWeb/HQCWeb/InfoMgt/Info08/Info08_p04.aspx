<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info08_p04.aspx.cs" Inherits="HQCWeb.InfoMgt.Info08.Info08_p04" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">

    <style>
    .multilineLabel {
        display: block; /* 블록 요소로 설정하여 전체 너비를 차지하게 합니다. */
        width: 1400px; /* 필요한 너비로 설정하세요 */
        word-wrap: break-word; /* 단어가 너무 길 경우 줄 바꿈이 되도록 합니다. */
        white-space: normal; /* 공백 처리 방식을 일반 텍스트처럼 합니다. */
    }
</style>

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info08</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                    <col style="width:60px;" />
                    <col style="width:250px;" />
                </colgroup>
                
                <%--<tr>
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDevId" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevId" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDevKind" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevKind" runat="server"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                    </td>
                </tr>--%>
                <tr>
                    <th><asp:Label ID="lbZpl" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:Label ID="lbGetZpl" runat="server" CssClass="multilineLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <img id="imgBarcode" runat="server" src="none" style="max-height:500px" />
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
