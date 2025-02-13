<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info37_p01.aspx.cs" Inherits="HQCWeb.InfoMgt.Info37.Info37_p01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlStnCd").val() == "") {
                alert("공정코드를 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlCarType").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtChkData").val() == "") {
                alert("체크데이터를 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtBrcdCheck").val() == "") {
                alert("스캔체크를 입력하세요.");
                return false;
            } else if ($("#PopupContent_txtChkData").val().length != $("#PopupContent_txtBrcdCheck").val().length) {
                alert("데이터와 스캔체크 길이가 불일치합니다.");
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

        function updateLabels() {
            var chkText = $("#PopupContent_txtBrcdCheck").val();
            var barcodeText = $("#PopupContent_txtChkData").val();
            var maxLength = Math.max(barcodeText.length, chkText.length);
            var seqContainer = $("#divSeq");
            var chkContainer = $("#divBrcdCheck");
            var brcdContainer = $("#divChkData");
            seqContainer.empty();
            chkContainer.empty();
            brcdContainer.empty();

            for (var i = 0; i < maxLength; i++) {
                var chkChar = chkText[i] || ' ';
                var brcdChar = barcodeText[i] || ' ';
                var brcdClass = brcdChar === ' ' ? 'char-label empty-char' : 'char-label';
                var chkClass = chkChar === ' ' ? 'char-label empty-char' : (chkChar === '0' ? 'char-label char-zero' : 'char-label char-set');
                seqContainer.append('<span class="char-label">' + (i + 1) + '</span>');
                chkContainer.append('<span class="' + chkClass + '">' + chkChar + '</span>');
                brcdContainer.append('<span class="' + brcdClass + '">' + brcdChar + '</span>');
            }
        }

        $(document).ready(function () {
        //function init() {
            var replaceNotInt = /[^0-1]/gi;

            $("#PopupContent_txtChkData").on('input', function () {
                updateLabels();
            });

            $("#PopupContent_txtChkData").on('paste', function (e) {
                setTimeout(function () {
                    updateLabels();
                }, 100);
            });

            $("#PopupContent_txtBrcdCheck").on('input', function () {

                if ($(this).val().length > 60) {
                    $(this).val($(this).val().slice(0, 60));
                }
                else if (!replaceNotInt.test($(this).val()))
                    updateLabels();
                else
                    $(this).val($(this).val().replace(replaceNotInt, ""));
            });

            $("#PopupContent_txtBrcdCheck").on('paste', function (e) {
                setTimeout(function () {
                    updateLabels();
                }, 100);
            });

            $("#PopupContent_txtBrcdCheck").on("focusout", function () {
                var x = $(this).val();
                if (x.length > 0) {
                    if (x.match(replaceNotInt)) {
                        x = x.replace(replaceNotInt, "");
                    }
                    $(this).val(x);
                }
            }).on("keyup", function () {
                $(this).val($(this).val().replace(replaceNotInt, ""));
            });
        //};
        });

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info37</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                    <th><b style="color:red;">*</b><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                         <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStnCd" runat="server" ></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                         <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCarType" runat="server" OnSelectedIndexChanged="ddlCarType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td>
                         <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPartNo" runat="server" OnSelectedIndexChanged="ddlPartNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbScanPartNo" runat="server"></asp:Label></th>
                    <td>
                         <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlScanPartNo" runat="server" ></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlPartNo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbSeq" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <div id="divSeq"></div>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbChkData2" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <div id="divChkData"></div>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbBrcdCheck2" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <div id="divBrcdCheck"></div>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbChkData" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:TextBox ID="txtChkData" runat="server" MaxLength="60"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbBrcdCheck" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:TextBox ID="txtBrcdCheck" runat="server" MaxLength="60" type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbScanNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtScanNm" runat="server"></asp:TextBox>
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
