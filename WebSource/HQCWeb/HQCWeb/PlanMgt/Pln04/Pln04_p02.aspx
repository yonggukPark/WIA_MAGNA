<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Pln04_p02.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln04.Pln04_p02" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 40%;
            position : absolute; 
        }
    </style>
    <script type="text/javascript">

        var cPrint;

        function fn_ModifyConfirm() {
            if ($("#PopupContent_txtPrintReasonHidden").val() == "") {
                alert("발행용도를 선택하세요.");
                return false;
            } else {
                if (confirm("출력 하시겠습니까?")) {
                    return true;
                } else {
                    return false;
                }
            }

            return true;
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

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            fn_Set_Print();
        });

        //부품코드 재설정
        function fn_Set_Print() {
            $("#PopupContent_txtPrintReason").comboTree({
                source: cPrint,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "PopupContent_txtPrintReasonHidden"
            });

            $(".ct-drop-down-container").css("height", "200px");
        }
    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Pln04</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbShipNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetShipNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPrintReason" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo" style="width:290px; font-size:12px;">
                            <input type="text" ID="txtPrintReason" runat="server" style="background-color:white; color:black;" readonly/>
                            <asp:TextBox id="txtPrintReasonHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbShipOutType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShipOutType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />

                </Triggers>

            </asp:UpdatePanel>



            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Print</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Print" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
