<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Menu001.aspx.cs" Inherits="HQCWeb.SystemMgt.MenuManagement.Menu001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_txtMenuID").val() == "") {
                alert("메뉴 아이디를 입력하세요.");
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

    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00010</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->
    
        <div class="box">
            <table  cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbMenuID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMenuID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuNM" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMenuNM" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbAssamblyID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtAssamblyID" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbViewID" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtViewID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuIDX" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtMenuIDX" runat="server" style="IME-MODE:disabled;" MaxLength="3" onkeydown="return onlyNumber(event);"  onkeyup="removeChar(event);"  width="20%"></asp:TextBox>
                    </td>   
                </tr>
                <tr>
                    <th><asp:Label ID="lbUseYN" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                    </td>   
                </tr>
            </table>

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Save();" class="btn" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>
                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;"  />
            </div>
            <!-- Btn //-->
        </div>
    </div>

</asp:Content>
