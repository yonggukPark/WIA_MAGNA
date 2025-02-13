<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Mon14.aspx.cs" Inherits="HQCWeb.MonitorMgt.Mon14.Mon14" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function fn_Validation() {
            fn_WatingCall();
            return true;
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, '팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data, CLine;
        var container, dataProvider, gridView;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);
            dataProvider.setFields(field);
            dataProvider.setRows(data);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("PART_DESC").styleName = "string-column"
            gridView.columnByName("STORAGE_NM").styleName = "string-column"

            gridView.columnByName("KEY_HID").visible = false; 
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
        }

        /* 다중콤보 시작 */
        // 검색어 콤보
        jQuery(document).ready(function ($) {

            $("#MainContent_txtSearchCombo").comboTree({
                source: [],
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                hidCon: "MainContent_txtSearchComboHidden"
            });
            fn_Set();
            $("#type2").hide();
        });

        function fn_searchType_change(ddl) {
            if (ddl.value === '1') {
                $("#type1").show();
                $("#type2").hide();
            }
            else {
                $("#type1").hide();
                $("#type2").show();
            }
        }

        function fn_SearchSetStyle() {
            if ($("#divSearchSet").css("display") == "none") {
                $("#divSearchSet").show();
            }
            else {
                $("#divSearchSet").hide();
            }
        }

        function fn_Set() {
            $("#MainContent_txtSearchCombo").comboTree({
                source: [],
                comboReload: true
            });

            var txtSearch = $("#txtSearchValue").val();

            txtSearch = txtSearch.replace(/(?:\r\n|\r|\n)/g, ',');

            const arr = txtSearch.split(',');
            const arrTxtSearch = arr.filter((el, index) => arr.indexOf(el) === index);

            var tList = new Array();

            var iLen = 0;

            for (var i = 0; i < arrTxtSearch.length; i++) {
                if (arrTxtSearch[i] != "") {
                    iLen++;

                    var comboData = new Object();

                    comboData.id = arrTxtSearch[i];
                    comboData.title = arrTxtSearch[i];

                    tList.push(comboData);
                }
            }

            if (iLen > 50) {
                alert("검색어 셋팅 가능 숫자는 50개 입니다.");

                $("#MainContent_txtSearchCombo").comboTree({
                    source: [],
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true,
                    onLoadAllChk: true
                });

                return false;
            } else {
                $("#MainContent_txtSearchCombo").comboTree({
                    source: tList,
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true,
                    onLoadAllChk: true,
                    hidCon: "MainContent_txtSearchComboHidden"
                });

                $("#divSearchSet").hide();
            }
        }

        /* 다중콤보 끝 */

    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 13%;
            position : absolute; 
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Mon14</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Mon14'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col style="width:60px;">
                    <col style="width:60px;">
                    <col style="width:100px;">
                    <col style="width:400px;">
                    <col>
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStorageCd" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbSearchType" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlSearchType2" runat="server" onchange="fn_searchType_change(this)" AutoPostBack="false"></asp:DropDownList>
                    </td>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlSearchType" runat="server" ></asp:DropDownList>
                        <asp:TextBox id="txtTabCount" runat="server" style="display:none"></asp:TextBox>
                    </td>
                    <td id="type1">
                        <div class="searchCombo" style="width:270px; font-size:12px;">
                            <asp:TextBox ID="txtSearchCombo" runat="server" ReadOnly="true" style="background-color:white; color:black;"></asp:TextBox>
                            <asp:TextBox id="txtSearchComboHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                        <img src="/img/gnb_on_arrow.png" style="float:left; padding-left:270px;" onclick="javascript:fn_SearchSetStyle();" />
                        <div id="divSearchSet" class="searchCombo" style="display:none;">
                            <textarea id="txtSearchValue" rows="5" style="width:270px;"></textarea>
                            <br />
                            <input type="button" value="적용" onclick="javascript:fn_Set();" />
                            <input type="button" value="닫기" onclick="javascript:fn_SearchSetStyle();" />
                        </div>
                    </td>
                    <td id="type2">
                        <asp:TextBox id="txtSearchBarcode" runat="server" Width="270"></asp:TextBox>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" class="realgrid"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Mon14'); return false;" style="display:none; margin-top:10px; float:left" />
									<div class="al-r total" ondragstart="return false">Total : <div id="rowCnt" class="f02" style="float:right"></div></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>