<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="ComCode001.aspx.cs" Inherits="HQCWeb.SystemMgt.ComCodeManagement.ComCode001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_txtComType").val() == "") {
                alert("Com Type을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtComCD").val() == "") {
                alert("Com CD를 입력하세요.");
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

        function InpuOnlyNumber(event) {

            if ((event.keyCode < 48) || (event.keyCode > 57)) { //숫자키만 입력        
                event.returnValue = false;
            } 
        }

        function onlyNumber(event) {
            event = event || window.event;

            var keyID = (event.which) ? event.which : event.keyCode;

            if ((keyID >= 48 && keyID <= 57) || (keyID >= 96 && keyID <= 105) || keyID == 8 || keyID == 46 || keyID == 37 || keyID == 39)
                return;
            else
                return false;
        }

        function removeChar(event) {
            event = event || window.event;

            var keyID = (event.which) ? event.which : event.keyCode;

            if (keyID == 8 || keyID == 46 || keyID == 37 || keyID == 39)
                return;
            else
                event.target.value = event.target.value.replace(/[^0-9]/g, "");
        }

        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }


    </script>    

	<!--// POPUP -->
    <div class="popup_wrap">

        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00050</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbComType" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtComType" runat="server" width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbComCD" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtComCD" runat="server" width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbComNM" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtComNM" runat="server" width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbComSeq" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtComSeq" runat="server" style="IME-MODE:disabled;" MaxLength="3" onkeydown="return onlyNumber(event);"  onkeyup="removeChar(event);"  width="20%"></asp:TextBox> 
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
				<a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave" runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />&nbsp;
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
   
</asp:Content>
