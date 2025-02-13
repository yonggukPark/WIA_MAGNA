<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info16_p03.aspx.cs" Inherits="HQCWeb.InfoMgt.Info16.Info16_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtNoworkTimeHour").val() == "") {
                alert("제외 시작시간(시)를 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtNoworkTimeMin").val() == "") {
                alert("제외 시작시간(분)을 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtStopMin").val() == "") {
                alert("제외시간(분)을 입력하세요.");
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

        $(document).ready(function () {

            $("#PopupContent_txtNoworkTimeHour").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 입력값이 0에서 23 사이의 값인지 검증
                if (value !== '' && (parseInt(value, 10) < 0 || parseInt(value, 10) > 23) || value.length > 2) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);
            });

            $("#PopupContent_txtNoworkTimeMin").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 입력값이 0에서 59 사이의 값인지 검증
                if (value !== '' && (parseInt(value, 10) < 0 || parseInt(value, 10) > 59) || value.length > 2) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);
            });

            $("#PopupContent_txtStopMin").on('input', function () {
                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 입력값이 0에서 1440 사이의 값인지 검증
                if (value !== '' && (parseInt(value, 10) < 0 || parseInt(value, 10) > 1440)) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);
            });

        });

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info16</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbProdDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbNoworkTimeHour" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtNoworkTimeHour" runat="server" type="number" placeholder="0 ~ 23" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbNoworkTimeMin" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtNoworkTimeMin" runat="server" type="number" placeholder="0 ~ 59" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStopMin" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtStopMin" runat="server" type="number" placeholder="0 ~ 1440"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbNoworkType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlNoworkType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
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
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark2" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark2" runat="server" TextMode="MultiLine"></asp:TextBox>
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
