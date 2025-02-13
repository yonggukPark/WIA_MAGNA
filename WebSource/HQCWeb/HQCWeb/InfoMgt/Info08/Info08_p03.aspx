<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info08_p03.aspx.cs" Inherits="HQCWeb.InfoMgt.Info08.Info08_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("Shop 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlCarType").val() == "") {
                alert("차종 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDevId").val() == "") {
                alert("프린터 ID를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDevKind").val() == "") {
                alert("해상도 ID를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlType").val() == "") {
                alert("출력 타입을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtCode").val() == "") {
                alert("식별자를 입력하세요.");
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

            $("#PopupContent_txtWidth").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 10) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);

            });

            $("#PopupContent_txtHeight").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 10) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);

            });

            $("#PopupContent_txtFontWidth").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 10) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);

            });

            $("#PopupContent_txtFontHeight").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 10) {
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
            <h1><asp:Label ID="lbTitle" runat="server">Info08</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDevId" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlDevId" runat="server" OnSelectedIndexChanged="ddlDevId_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDevKind" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlDevKind" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbType" runat="server"></asp:Label></th>
                    <td><asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlDevId" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbCodeNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCodeNm" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbWidth" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWidth" runat="server" type="number"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbHeight" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHeight" runat="server" type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbFontWidth" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtFontWidth" runat="server" type="number"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbFontHeight" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtFontHeight" runat="server" type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbZpl" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:TextBox ID="txtZpl" runat="server" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbValue" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtValue" runat="server" MaxLength="30"></asp:TextBox>
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
