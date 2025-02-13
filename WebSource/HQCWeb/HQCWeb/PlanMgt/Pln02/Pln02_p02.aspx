<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Pln02_p02.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln02.Pln02_p02" %>
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

        function fn_Modify() {
            $("#PopupContent_btnModify").click();
        }

        function fn_Delete() {
            $("#PopupContent_btnDelete").click();
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            // 숫자와 백스페이스, 삭제 키를 허용
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>    

    <asp:HiddenField ID="hidPlanSeq"          runat="server" />
	<asp:HiddenField ID="hidOrderType"        runat="server" />
    <asp:HiddenField ID="hidPopDefaultValue2" runat="server" /> 

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Pln02</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0" >
                <tr>
                    <th><asp:Label ID="lbOrderNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetOrderNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPlanDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetPlanDt" runat="server"></asp:Label>
                    </td>
                </tr>
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
                    <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetCarType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetPartNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbStatusFlg" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStatusFlg" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPlanQty" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPlanQty" runat="server" type="number" min="0" step="1" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPlanDQty" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPlanDQty" runat="server" type="number" min="0" step="1" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPlanNQty" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPlanNQty" runat="server" type="number" min="0" step="1" ></asp:TextBox>
                    </td>
                </tr>
                <tr style="display:none;">
                    <th><asp:Label ID="lbFinishFlg" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlFinishFlg" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPlanCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPlanCd" runat="server" OnSelectedIndexChanged="ddlPlanCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPlanDetailCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPlanDetailCd" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlPlanCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbErpSend" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlErpSend" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
