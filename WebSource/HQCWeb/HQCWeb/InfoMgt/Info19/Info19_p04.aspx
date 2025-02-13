<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info19_p04.aspx.cs" Inherits="HQCWeb.InfoMgt.Info19.Info19_p04" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlCarType").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtStnCd").val() == "") {
                alert("공정코드를 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlWorkType").val() == "T") {
                if ($("#PopupContent_txtPset").val() == "") {
                    alert("TOOL은 P Set을 입력하세요.");
                    return false;
                }
                else if ($("#PopupContent_ddlDevID").val() == "") {
                    alert("TOOL은 장비코드를 선택하세요.");
                    return false;
                }
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
            <h1><asp:Label ID="lbTitle" runat="server">Info19</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">
            <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                <ContentTemplate>
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
                        <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCarType" runat="server" OnSelectedIndexChanged="ddlCarType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStnCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbWorkSeq" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWorkSeq" runat="server" type="number"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbWorkNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWorkNm" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbWorkCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWorkCd" runat="server" MaxLength="5"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbToolType" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtToolType" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbQty" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtQty" runat="server" type="number"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbTorqueType" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtTorqueType" runat="server" MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPset" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPset" runat="server" MaxLength="3"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbToolHole" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtToolHole" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDevID" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevID" runat="server" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbWorkType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlWorkType" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbScanCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlScanCd" runat="server" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbMatchCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlMatchCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbModeFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlModeFlag" runat="server" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
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
            </table>
            <!-- Table //-->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Save();" class="btn ml10">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
