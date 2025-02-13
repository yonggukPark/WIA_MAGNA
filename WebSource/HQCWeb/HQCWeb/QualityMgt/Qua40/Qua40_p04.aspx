<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua40_p04.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua40.Qua40_p04" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">

        function fn_ModifyConfirm() {

            if ($("#PopupContent_ddlDRespCd").val() == "") {
                alert("귀책처를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDCode").val() == "") {
                alert("불량 현상을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDReasonCode").val() == "") {
                alert("불량 원인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtDefectCnt").val() == "") {
                alert("불량수량을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtReworkMsg").val() == "") {
                alert("사유를 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlDecomposeType").val() == "") {
                alert("분해유형을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlResultCd").val() == "") {
                alert("결과를 선택하세요.");
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

            if ($("#PopupContent_ddlDRespCd").val() == "") {
                alert("귀책처를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDCode").val() == "") {
                alert("불량 현상을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDReasonCode").val() == "") {
                alert("불량 원인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtReworkMsg").val() == "") {
                alert("사유를 입력하세요.");
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

        function fn_Modify() {
            $("#PopupContent_btnModify").click();
        }

        function fn_Delete() {
            $("#PopupContent_btnDelete").click();
        }

        jQuery(document).ready(function ($) {
            $("#PopupContent_txtReturnDt").datepicker({
                dateFormat: "yy-mm-dd"
            });
        });

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
                <tr>
                    <th><asp:Label ID="lbDefectDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetDefectDt" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetPartNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbLotNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetLotNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDRespCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDRespCd" runat="server" ></asp:DropDownList>
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbDefectCnt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDefectCnt" type="number" runat="server" Attributes="min=1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReturnDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReturnDt" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReworkMsg" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReworkMsg" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDecomposeType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDecomposeType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbResultCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlResultCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbSStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlSStorageCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDStorageCd" runat="server"></asp:DropDownList>
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
