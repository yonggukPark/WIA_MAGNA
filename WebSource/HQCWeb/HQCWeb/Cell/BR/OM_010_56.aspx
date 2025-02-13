<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="OM_010_56.aspx.cs" Inherits="HQCWeb.Cell.BR.OM_010_56" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo_2{
            text-align: center;
            z-index : 10;
            top : 40%;
            position : absolute; 
        }
    </style>

    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        function fn_hide() {
            if ($("#div_Search").css("display") == "none") {
                $("#div_Search").show();

                if (screen.width == 1280) {
                    $("#divMainGrid").attr("style", "height:245px;");
                    $("#divDetailGrid").attr("style", "height:245px;");

                    $("#MainContent_upMain .dxgvCSD").attr("style", "height:230px;");
                    $("#MainContent_upDetail .dxgvCSD").attr("style", "height:255px;  overflow: scroll;");
                }
                else {
                    $("#divMainGrid").attr("style", "height:285px;");
                    $("#divDetailGrid").attr("style", "height:285px;");

                    $("#MainContent_upMain .dxgvCSD").attr("style", "height:275px;");
                    $("#MainContent_upDetail .dxgvCSD").attr("style", "height:280px;");
                }
            } else {
                $("#div_Search").hide();

                if (screen.width == 1280) {
                    $("#divMainGrid").attr("style", "height:320px;");
                    $("#divDetailGrid").attr("style", "height:330px;");

                    $("#MainContent_upMain .dxgvCSD").attr("style", "height:305px;");
                    $("#MainContent_upDetail .dxgvCSD").attr("style", "height:316px;  overflow: scroll;");
                }
                else {
                    $("#divMainGrid").attr("style", "height:350px;");
                    $("#divDetailGrid").attr("style", "height:350px;");

                    $("#MainContent_upMain .dxgvCSD").attr("style", "height:340px;");
                    $("#MainContent_upDetail .dxgvCSD").attr("style", "height:352px;");
                }
            }
        }
    </script>

    <asp:TextBox ID="hidMainGridHeight" runat="server" style="display:none;" />
    <asp:TextBox ID="hidDetailGriddHeight" runat="server" style="display:none;" />

    <asp:TextBox ID="hidScreenType" runat="server" style="display:none;"></asp:TextBox>

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_21100</asp:Label></h3>
            <ul class="win_btn">
                <li><a href="javascript:fn_hide();" class="reduce">축소</a></li>
            </ul>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13" id="div_Search">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:250px;" />
                    <col style="width:100px;" />
                    <col style="width:180px;" />
                    <col style="width:100px;" />
                    <col style="width:200px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbWorkDate" runat="server"></asp:Label></th>
                    <td colspan="5"><asp:TextBox ID="txtFromDt" runat="server" style="width:80px; z-index:200;"></asp:TextBox> ~ <asp:TextBox ID="txtToDt" runat="server" style="width:80px; z-index:200;"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbOperation" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:230px;">
                            <asp:TextBox id="txtOperation" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                            <asp:TextBox id="txtOperationHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbLine" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:150px;">
                            <asp:TextBox id="txtLine" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbEquipment" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:170px;">
                            <asp:TextBox id="txtEquipment" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                            <asp:TextBox id="txtEquipmentHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbProductSpec" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlProductSpec" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbProductType" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlProductType" runat="server" onclick="javascript:fn_ddlValChk();"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_SearchValChk(); fn_WatingCall();" Visible="false" />
                        <asp:Button ID="btnDetailSearch" runat="server" OnClick="btnDetailSearch_Click" Text="Search" OnClientClick="fn_WatingCall();" style="display:none;" />
                        <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
                        <asp:Button ID="btnGridReload" runat="server" Text="POCall" OnClick="btnGridReload_Click"  style="display:none;"  />
                    </td>
                </tr>
            </table>
        </div>

        <br />

        <!--// 통계수치 테이블 01 -->
		<p class="sub_tit">조별 등록 수량</p>

        <div style="height:285px;" id="divMainGrid">
            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:GridControl runat="server" ID="MainGrid" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnGridReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <br />
        <br />

        <p class="sub_tit">공정별 세부내역</p><!--통계수치 테이블의 타이틀-->

        <div style="height:285px;" id="divDetailGrid">
            <asp:HiddenField ID="hidParams" runat="server" />

            <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:GridControl runat="server" ID="DetailGrid"/>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnDetailSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnGridReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <script>

        if (screen.width == 1280) {

            $("#divMainGrid").attr("style", "height:245px;");
            $("#divDetailGrid").attr("style", "height:245px;");

            $("#MainContent_hidMainGridHeight").val("230");
            $("#MainContent_hidDetailGriddHeight").val("255");

            fn_ScreenType("T");
        }
        else {
            $("#divMainGrid").attr("style", "height:285px;");
            $("#divDetailGrid").attr("style", "height:285px;");

            $("#MainContent_hidMainGridHeight").val("275");
            $("#MainContent_hidDetailGriddHeight").val("280");

            fn_ScreenType("W");
        }


        var comboTree1;

        jQuery(document).ready(function ($) {

            comboTree3 = $("#MainContent_txtOperation").comboTree({
                source: <%=strOperationJSON%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: true,
                hidCon: "MainContent_txtOperationHidden", 
                functionCall: true,
                functionCallFunc: "FuncCall"

            });

            comboTree1 = $("#MainContent_txtLine").comboTree({
                source: <%=strLineJSON%>,
                isMultiple: true,
                cascadeSelect: true,
                collapse: false,
                selectAll: true,
                valueChange: false, 
                comboClick: true, 
                targetlID: "MainContent_txtLine",
                valChkID: "MainContent_txtOperation", 
                valChkLabel: "MainContent_lbOperation",
                valChkFunc: "fn_ValChk", 
                functionCall: true,
                functionCallFunc: "FuncCall"
            });

            comboTree2 = $("#MainContent_txtEquipment").comboTree({
                source: [],
                isMultiple: true,
                cascadeSelect: true,
                collapse: false,
                selectAll: true,
                valueChange: false, 
                comboClick: true,
                targetlID: "MainContent_txtEquipment", 
                valChkID: "MainContent_txtLine", 
                valChkLabel: "MainContent_lbLine", 
                valChkFunc:"fn_ValChk"
            });
        });


        function fn_ValChk(targetConID, valChkConID, valChkLabelID) {
            var divTagID = $("#" + targetConID).parent().parent().attr('id');

            var valChk = $("#" + valChkConID).val();
            var valLabel = $("#" + valChkLabelID).text();

            if ($('#' + divTagID).children('div').hasClass("ct-drop-down-container")) {

                if (valChk == "") {
                    alert(valLabel + "을 먼저 선택하세요.");

                    $('#' + divTagID).children('div:eq(1)').attr("style", "display:block;");
                }
            }
        }

        function fn_SearchValChk() {
            var _val = $("#MainContent_txtEquipmentHidden").val();
            
            var _valChkLabel = $("#MainContent_lbEquipment").text();

            if (_val == "") {
                alert(_valChkLabel + "를 선택하세요.");
                return false;
            }
        }

        function fn_ddlValChk() {
            var _val = $("#MainContent_ddlProductSpec option:selected").val();

            var _valChkLabel = $("#MainContent_lbProductSpec").text();

            if (_val == "") {
                alert(_valChkLabel + "을 먼저 선택하세요.");
                return false;
            }
        }

        function FuncCall() {
            if (comboTree1 != undefined) {
                if (comboTree1._selectedItems.length == 0 || comboTree3._selectedItems.length == 0) {
                    $("#MainContent_txtEquipment").comboTree({
                        source: []
                        , comboReload: true
                    });

                    $("#MainContent_txtEquipment").comboTree({
                        source: [],
                        isMultiple: true,
                        cascadeSelect: true,
                        collapse: false,
                        selectAll: true,
                        valueChange: true,
                        comboClick: true,
                        targetlID: "MainContent_txtEquipment",
                        valChkID: "MainContent_txtLine",
                        valChkLabel: "MainContent_lbLine",
                        valChkFunc: "fn_ValChk"
                    });
                }
                else {
                    $("#MainContent_btnFunctionCall").click();
                }
            }
        }

        function fn_SetDDL(jsonval) {
            $("#MainContent_txtEquipment").comboTree({
                source: []
                , comboReload : true
            });

            $("#MainContent_txtEquipment").comboTree({
                source: jsonval,
                isMultiple: true,
                cascadeSelect: true,
                collapse: false,
                selectAll: true,
                valueChange: true,
                hidCon: "MainContent_txtEquipmentHidden",
                comboReload: false, 
                comboClick: true,
                targetlID: "MainContent_txtEquipment",
                valChkID: "MainContent_txtLine",
                valChkLabel: "MainContent_lbLine"
            });
        }

        function fn_Detail(_val) {
            $("#MainContent_hidParams").val(_val);

            $("#MainContent_btnDetailSearch").click();
        }

        function fn_ScreenType(_val) {
            $("#MainContent_hidScreenType").val(_val);

            $("#MainContent_btnGridReload").click();
        }

    </script>


</asp:Content>
