<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info05_p01.aspx.cs" Inherits="HQCWeb.InfoMgt.Info05.Info05_p01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlCarType").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtPartNo").val() == "") {
                alert("품번을 입력하세요.");
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
            <h1><asp:Label ID="lbTitle" runat="server">Info05</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">
            <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
               <ContentTemplate>
                <!--// Table -->
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlCarType" runat="server" ></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><b style="color:red;">*</b><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtPartNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbPartDesc" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtPartDesc" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbCardCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtCardCd" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbCardDesc" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtCardDesc" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbPartType" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtPartType" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbQty" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtQty" runat="server" type="number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbRemark1" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtRemark1" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
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
