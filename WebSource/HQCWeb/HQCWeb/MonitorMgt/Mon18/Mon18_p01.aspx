<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Mon18_p01.aspx.cs" Inherits="HQCWeb.MonitorMgt.Mon18.Mon18_p01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlGubunCd").val() == "") {
                alert("구분을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtBarcode").val() == "") {
                alert("바코드를 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlBlockFlag").val() == "") {
                alert("차단 여부를 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtReworkMsg").val() == "") {
                alert("등록 사유를 입력하세요.");
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
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Mon18</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbGubunCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlGubunCd" runat="server" Width="100"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbBarcode" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtBarcode" runat="server" Width="270" MaxLength="60"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbBlockFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlBlockFlag" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReworkMsg" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReworkMsg" runat="server"></asp:TextBox>
                    </td>
                </tr>
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
