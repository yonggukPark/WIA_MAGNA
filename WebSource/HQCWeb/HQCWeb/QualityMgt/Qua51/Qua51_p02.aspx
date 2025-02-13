<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua51_p02.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua51.Qua51_p02" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">

        function fn_ModifyConfirm() {

            if ($("#PopupContent_ddlManDept").val() == "") {
                alert("관리부서를 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtStandard").val() == "") {
                alert("규격을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtSerialNo").val() == "") {
                alert("제품시리얼을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlInspCycle").val() == "") {
                alert("점검주기를 선택하세요.");
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

        function fn_Modify() {
            $("#PopupContent_btnModify").click();
        }

        function fn_Delete() {
            $("#PopupContent_btnDelete").click();
        }

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Qua51</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbManNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetManNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbManDept" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlManDept" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStandard" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtStandard" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbPartSerialNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPartSerialNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbInspCycle" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlInspCycle" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPartDesc" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPartDesc" runat="server"></asp:TextBox>
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
