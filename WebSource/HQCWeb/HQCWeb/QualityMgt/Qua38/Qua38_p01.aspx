<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua38_p01.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua38.Qua38_p01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_txtDate").val() == "") {
                alert("발생일을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlPartNo").val() == "") {
                alert("품번을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDefectCompany").val() == "") {
                alert("발생처를 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtDefectCnt").val() == "") {
                alert("불량수량을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlDecomposeType").val() == "") {
                alert("분해유형을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlStorageCd").val() == "") {
                alert("창고를 선택하세요.");
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
            <h1><asp:Label ID="lbTitle" runat="server">Qua38</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDefectDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPartNo" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbLotNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtLotNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDefectCompany" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDefectCompany" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDefectCnt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDefectCnt" type="number" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDefectReason" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDefectReason" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDecomposeType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDecomposeType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStorageCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <%--<tr>
                    <th><asp:Label ID="lbDeleteDesc" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDeleteDesc" runat="server"></asp:TextBox>
                    </td>
                </tr>--%>
            </table>
            <!-- Table //-->

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
