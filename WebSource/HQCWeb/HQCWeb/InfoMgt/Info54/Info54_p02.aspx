<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info54_p02.aspx.cs" Inherits="HQCWeb.InfoMgt.Info54.Info54_p02" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">

        function fn_ModifyConfirm() {

            if (confirm("수정 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
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

        function fn_RestoreConfirm() {

            if (confirm("복구 하시겠습니까?")) {
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

        function fn_Restore() {
            $("#PopupContent_btnRestore").click();
        }

        $(document).ready(function () {

            $("#PopupContent_txtWorkCd").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 4) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);

            });

            $("#PopupContent_txtPSet").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 3) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);

            });

            $("#PopupContent_txtResultLoc").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 4) {
                    value = value.slice(0, -1);
                }

                $(this).val(value);

            });

            $("#PopupContent_txtToolCnt").on('input', function () {

                var value = $(this).val();

                // 입력값이 숫자만 포함하도록 필터링
                value = value.replace(/[^0-9]/g, '');

                // 길이 검증
                if (value.length > 4) {
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
            <h1><asp:Label ID="lbTitle" runat="server">Info54</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStnCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDevId" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevId" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbDevChk" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDevChk" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbInsertTable" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetInsertTable" runat="server" MaxLength="30"></asp:Label>
                    </td>
                    <th><asp:Label ID="lbItemSeq" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetItemSeq" runat="server" type="number"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbItemCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtItemCd" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbItemNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtItemNm" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbItemMin" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtItemMin" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbItemMax" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtItemMax" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbHkmcTransInsItemNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHkmcTransInsItemNm" runat="server" MaxLength="4000"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbWorkCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtWorkCd" runat="server" type="number"></asp:TextBox>
                    </td>
                </tr><tr>
                    <th><asp:Label ID="lbShopCdT" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtShopCdT" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbLineCdT" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtLineCdT" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCarTypeT" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCarTypeT" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbPSet" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPSet" runat="server" type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbResultLoc" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtResultLoc" runat="server" type="number"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbHkmcCompany" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHkmcCompany" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbHkmcRegion" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHkmcRegion" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbHkmcSupplier" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHkmcSupplier" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbHkmcShop" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHkmcShop" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbHkmcLine" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHkmcLine" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbHkmcCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtHkmcCarType" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbToolCnt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtToolCnt" runat="server" type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPdaChkYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPdaChkYn" runat="server"></asp:DropDownList>
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
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:fn_Restore();" class="btn ml10" id="aRestore" runat="server" visible="false">Restore</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
                <asp:Button ID="btnRestore"  runat="server" OnClientClick="javascript:return fn_RestoreConfirm();" OnClick="btnRestore_Click" Text="Delete" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
