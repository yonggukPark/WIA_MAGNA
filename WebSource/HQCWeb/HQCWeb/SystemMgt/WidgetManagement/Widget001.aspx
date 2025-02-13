<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Widget001.aspx.cs" Inherits="HQCWeb.SystemMgt.WidgetManagement.Widget001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_txtWidgetNM").val() == "") {
                alert("위젯 이름을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtWidgetURL").val() == "") {
                alert("위젯 경로을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlWidgetSize").val() == "") {
                alert("위젯 사이즈을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtDisplaySeq").val() == "") {
                alert("노출순서를 입력하세요.");
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
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00110</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbWidgetNM" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWidgetNM" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbWidgetURL" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWidgetURL" runat="server" style="width:95%;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbWidgetSize" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlWidgetSize" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDisplaySeq" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDisplaySeq" runat="server" style="width:15%;" MaxLength="1" onkeydown="return onlyNumber(event);"  onkeyup="removeChar(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbUseYN" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
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
