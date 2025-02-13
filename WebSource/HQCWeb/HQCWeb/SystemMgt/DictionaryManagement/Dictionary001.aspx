<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Dictionary001.aspx.cs" Inherits="HQCWeb.SystemMgt.DictionaryManagement.Dictionary001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_txtDictionaryID").val() == "") {
                alert("사전 아이디를 입력하세요.");
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
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00020</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th style="width:25%;"><b style="color:red;">*</b><asp:Label ID="lbDictionaryID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDictionaryID" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDictionaryKR" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDictionaryKR" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDictionaryEN" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDictionaryEN" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDictionaryLO" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDictionaryLO" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>   
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
				<a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />&nbsp;
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
