<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Pln04_p03.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln04.Pln04_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
     
	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Pln04</asp:Label> - <asp:Label ID="lbWorkName" runat="server">Print</asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
