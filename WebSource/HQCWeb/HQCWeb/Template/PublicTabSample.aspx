<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="PublicTabSample.aspx.cs" Inherits="HQCWeb.Template.PublicTabSample" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else {
                fn_WatingCall();
                return true;
            }
            return true;
        }

        function fn_Register() {
            fn_OpenPop('팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, '팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column1, field1, data1,
            column2, field2, data2,
            column3, field3, data3,
            column4, field4, data4,
            column5, field5, data5;

        var container, dataProvider, gridView;

        //dataProvider, gridView 저장용
        var dP1, gV1,
            dP2, gV2,
            dP3, gV3,
            dP4, gV4,
            dP5, gV5;

        var tabNum = 1;//초기값 1

        //통합 엑셀 출력
        function fn_excelExport_override(menuId) {
            //fn_excelExport(menuId + '_' + tabNum);

            var excelType = true;
            var showProgress = true;
            var itemType = true;//숨겨진값 
            var pageType = true;//전체페이지 내보내기
            var indicator = 'default';
            var header = 'default';
            var footer = 'hidden';//아래 footer

            RealGrid.exportGrid({
                type: "excel",
                target: "local",
                fileName: menuId + ".xlsx",
                showProgress: showProgress,
                progressMessage: "Exporting....",
                indicator: indicator,
                header: header,
                footer: footer,
                compatibility: excelType,
                allItems: itemType,
                pagingAllItems: pageType,
                exportGrids: [
                    { grid: gV1, sheetName: $("#MainContent_lb1").text() },
                    { grid: gV2, sheetName: $("#MainContent_lb2").text() },
                    { grid: gV3, sheetName: $("#MainContent_lb3").text() },
                    { grid: gV4, sheetName: $("#MainContent_lb4").text() },
                    { grid: gV5, sheetName: $("#MainContent_lb5").text() }
                ]
            });
        }

        //탭에 따라 세팅 변경
        function getCol_override(menuId) {
            getCol(menuId + '_' + tabNum);
        }

        //탭 열기
        function openTab(evt, tabName) {
            var i, tabContent, tabButtons;

            // Hide all tab content
            tabContent = document.getElementsByClassName("grid-tab-content");
            for (i = 0; i < tabContent.length; i++) {
                tabContent[i].style.display = "none";
                tabContent[i].classList.remove("active");
            }

            // Remove the 'active' class from all buttons
            tabButtons = document.getElementsByClassName("grid-tab-button");
            for (i = 0; i < tabButtons.length; i++) {
                tabButtons[i].classList.remove("active");
            }

            // Show the current tab and add 'active' class to the button
            document.getElementById(tabName).style.display = "block";
            document.getElementById(tabName).classList.add("active");
            evt.currentTarget.classList.add("active");

            //탭번호로 그리드 변경
            updateGridReferences(tabName);
        }

        // 탭에 따라 기타사항 변경
        function updateGridReferences(tabName) {
            $(".rg-group-panel-message").css("top", "10px");//버그 수정을 위해 강제전환
            const tabNumber = tabName.replace('Tab', '');
            var tmpData = window['data' + tabNumber];
            dataProvider = window['dP' + tabNumber];
            gridView = window['gV' + tabNumber];
            tabNum = tabNumber;

            if (tmpData !== undefined)
                document.getElementById('rowCnt').innerHTML = tmpData.length;

            setPaging();
            $("#MainContent_txtTabCount").val(tabNumber);
        }

        /* 그리드 시작 */

        // 그리드 생성
        function createGrid1(_val) {
            container = document.getElementById('realgrid1');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column1);
            dataProvider.setFields(field1);
            dataProvider.setRows(data1);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid1(_val); if (data1 != undefined && $("#MainContent_txtTabCount").val() == '1') document.getElementById('rowCnt').innerHTML = data1.length;
            dP1 = dataProvider;
            gV1 = gridView;
        }

        function settingGrid1(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid2(_val) {
            container = document.getElementById('realgrid2');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column2);
            dataProvider.setFields(field2);
            dataProvider.setRows(data2);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            gridView.setCheckBar({ useImages: true });
            setContextMenu(gridView);
            setPaging();
            settingGrid2(_val); if (data2 != undefined && $("#MainContent_txtTabCount").val() == '2') document.getElementById('rowCnt').innerHTML = data2.length;
            dP2 = dataProvider;
            gV2 = gridView;
        }

        function settingGrid2(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid3(_val) {
            container = document.getElementById('realgrid3');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column3);
            dataProvider.setFields(field3);
            dataProvider.setRows(data3);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            gridView.setCheckBar({ useImages: true });
            setContextMenu(gridView);
            setPaging();
            settingGrid3(_val); if (data3 != undefined && $("#MainContent_txtTabCount").val() == '3') document.getElementById('rowCnt').innerHTML = data3.length;
            dP3 = dataProvider;
            gV3 = gridView;
        }

        function settingGrid3(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid4(_val) {
            container = document.getElementById('realgrid4');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column4);
            dataProvider.setFields(field4);
            dataProvider.setRows(data4);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid4(_val); if (data4 != undefined && $("#MainContent_txtTabCount").val() == '4') document.getElementById('rowCnt').innerHTML = data4.length;
            dP4 = dataProvider;
            gV4 = gridView;
        }

        function settingGrid4(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid5(_val) {
            //if (!gV5) {
            //    container = document.getElementById('realgrid5');
            //    dP5 = new RealGrid.LocalDataProvider(false);
            //    gV5 = new RealGrid.GridView(container);
            //}

            container = document.getElementById('realgrid5');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column5);
            dataProvider.setFields(field5);
            dataProvider.setRows(data5);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid5(_val); if (data5 != undefined && $("#MainContent_txtTabCount").val() == '5') document.getElementById('rowCnt').innerHTML = data5.length;
            dP5 = dataProvider;
            gV5 = gridView;
        }

        function settingGrid5(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;

            dataProvider.softDeleting = true;
            //gV6.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        /* 그리드 끝 */
    </script>

    <style>
        .grid-primary-column {
            color: blue;
            font-weight: bold;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">MENU_ID</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
                <asp:TextBox id="txtTabCount" runat="server" style="display:none"></asp:TextBox>
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtCondi1" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <!-- 탭 설정 -->
        <div class="grid-tab-container">
            <div class="grid-tabs-wrap">
	            <ul class="tabs">
		            <li class="tab" ><p class="grid-tab-button active" onclick="javascript:openTab(event, 'Tab1');"><asp:Label ID="lb1" runat="server"></asp:Label></p></li>
	                <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab2');"><asp:Label ID="lb2" runat="server"></asp:Label></p></li>
                    <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab3');"><asp:Label ID="lb3" runat="server"></asp:Label></p></li>
	                <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab4');" ><asp:Label ID="lb4" runat="server"></asp:Label></p></li>
	                <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab5');" ><asp:Label ID="lb5" runat="server"></asp:Label></p></li>
	            </ul>
            </div>

            <div class="grid-tab-content active" id="Tab1">
                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid1" class="realgrid_2"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab2">
                <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid2" class="realgrid_2"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab3">
                <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid3" class="realgrid_2"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab4">
                <asp:UpdatePanel runat="server" ID="up4" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid4" class="realgrid_2"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab5">
                <asp:UpdatePanel runat="server" ID="up5" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid5" class="realgrid_2"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:UpdatePanel runat="server" ID="up7" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <div class="toolbar">
                                <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                &nbsp;
                                <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                </select>
                                <input type="button" value="Set" id="btnSet" onclick="getCol_override('MENU_ID'); return false;" style="display:none; margin-top:10px; float:left" />
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

</asp:Content>