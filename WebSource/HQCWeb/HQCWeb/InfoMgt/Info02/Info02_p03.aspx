<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info02_p03.aspx.cs" Inherits="HQCWeb.InfoMgt.Info02.Info02_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtCdGrp").val() == "") {
                alert("코드그룹을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtCode").val() == "") {
                alert("코드를 입력하세요.");
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
            <h1><asp:Label ID="lbTitle" runat="server">Info02</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                    <col style="width:60px;" />
                    <col style="width:250px;" />
                </colgroup>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLineCd" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCdGrp" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdGrp" runat="server"></asp:TextBox>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCdNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdNm" runat="server"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbCdDesc1" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdDesc1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCdDesc2" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdDesc2" runat="server"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbCdDesc3" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdDesc3" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCdDesc4" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdDesc4" runat="server"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbCdDesc5" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdDesc5" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCdDesc6" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdDesc6" runat="server"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbCdDesc7" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCdDesc7" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark1" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark1" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbRemark2" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark2" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
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
