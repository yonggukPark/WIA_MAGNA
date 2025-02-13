<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua51.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua51.Qua51" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Register() {
            fn_OpenPop('QualityMgt/Qua51/Qua51_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'QualityMgt/Qua51/Qua51_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Upload() {
            fn_OpenPop('QualityMgt/Qua51/Qua51_p03.aspx', $("#MainContent_hidWidth").val(), '180');
        }

        function fn_Register2() {
            if (selectedData != undefined)
                fn_PostOpenPop(selectedData, 'QualityMgt/Qua51/Qua51_p04.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            else {
                alert("먼저 검사할 행을 선택해주세요.");
            }
        }

        function fn_Modify2(pkCode) {
            fn_PostOpenPop(pkCode, 'QualityMgt/Qua51/Qua51_p05.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var column2, field2, data2;
        var container2, dataProvider2, gridView2;
        var selectedData;

        //초기화
        jQuery(document).ready(function ($) {
            initializePage();
        });

        function initializePage() {
            //라벨 설정
            $("#spanTabLabel").text("상세 데이터");
        }

        // 그리드 생성
        function createMainGrid(_val) {
            container = document.getElementById('realMaingrid');
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
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                var current = gridView.getCurrent();
                var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                selectedData = value;

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }

                //행 색상 변경

                fn_Detail(value);

                if (clickData.column == 'MAN_NO') {
                    fn_Modify(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("STATUS").styleName = "string-column"
            gridView.columnByName("MAN_DEPT").styleName = "string-column"
            gridView.columnByName("PART_NAME").styleName = "string-column"
            gridView.columnByName("STANDARD").styleName = "string-column"
            gridView.columnByName("INSP_CYCLE").styleName = "string-column"
            gridView.columnByName("MAN_NO").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            //gridView.setRowGroup({ headerStatement: "${columnHeader}: ${groupValue} - ${rowCount} rows" });
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.displayOptions.useFocusClass = true; // 행 클릭시 강조 활성화
            //리로드인 경우
            if($("#MainContent_hidParams").val() != "")
                fn_Detail($("#MainContent_hidParams").val());
        }

        function createDetailGrid(_val) {
            container2 = document.getElementById('realDetailgrid');
            dataProvider2 = new RealGrid.LocalDataProvider(false);
            gridView2 = new RealGrid.GridView(container2);
            gridView2.setDataSource(dataProvider2);
            gridView2.setColumns(column2);
            dataProvider2.setFields(field2);
            dataProvider2.setRows(data2);
            gridView2.checkBar.visible = false;
            gridView2.stateBar.visible = false;
            setContextMenu(gridView2);
            setPaging2();
            settingDetailGrid(_val); if (data != undefined) document.getElementById('rowCnt2').innerHTML = data.length;
        }

        function settingDetailGrid(_val) {
            //PK 컬럼 클릭시 동작
            gridView2.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }

                if (clickData.column == 'CERT_NO') {
                    var current = gridView2.getCurrent();
                    var value = dataProvider2.getValue(current.dataRow, "KEY_HID");
                    fn_Modify2(value);
                }
            }

            if (_val != undefined) {
                gridView2.setFixedOptions({
                    colCount: _val
                });
            }

            // FILE_ATTACH 컬럼에 renderer 추가
            gridView2.setColumnProperty("FILE_ATTACH", "renderer", {
                type: "html",
                callback: function (grid, cell) {
                    const fileAttachPath = $("#MainContent_hidFileAttachPath").val()//
                    const row = grid.getValues(cell.index.itemIndex); // 행 데이터 가져오기
                    const filesHtml = [];
                    const folderName = "Qua51";

                    // 최대 5개의 파일 경로 처리
                    for (let i = 1; i <= 5; i++) {
                        const filePath = row[`FILE_PATH${i}`];
                        const orgFileName = row[`ORG_FILE_NAME${i}`];
                        const chgFileName = row[`CHG_FILE_NAME${i}`];
                        const icon = row[`FILE_EXT${i}`];

                        if (filePath && orgFileName && icon) {
                            var tIcon = null;

                            if (icon.toLowerCase() == "pdf") {
                                tIcon = "pdf";
                            }

                            filesHtml.push(`
                                <a href="${fileAttachPath}${folderName}/${filePath}/${chgFileName}" download="${orgFileName}">
                                    <img src="/images/btn/${tIcon}_23.png" alt="${orgFileName}" title="${orgFileName}" style="width: 24px; height: 24px;" />
                                </a>
                            `);
                        }
                    }

                    // 여러 파일 링크를 공백으로 구분하여 반환
                    return filesHtml.join(" ");
                }
            });

            gridView2.columnByName("CERT_NO").styleName = "grid-primary-column"
            gridView2.columnByName("RESULT").styleName = "string-column"

            // 파일 경로데이터 숨김
            gridView2.columnByName("ORG_FILE_NAME1").visible = false;
            gridView2.columnByName("CHG_FILE_NAME1").visible = false;
            gridView2.columnByName("FILE_PATH1").visible = false;
            gridView2.columnByName("FILE_EXT1").visible = false;
            gridView2.columnByName("ORG_FILE_NAME2").visible = false;
            gridView2.columnByName("CHG_FILE_NAME2").visible = false;
            gridView2.columnByName("FILE_PATH2").visible = false;
            gridView2.columnByName("FILE_EXT2").visible = false;
            gridView2.columnByName("ORG_FILE_NAME3").visible = false;
            gridView2.columnByName("CHG_FILE_NAME3").visible = false;
            gridView2.columnByName("FILE_PATH3").visible = false;
            gridView2.columnByName("FILE_EXT3").visible = false;
            gridView2.columnByName("ORG_FILE_NAME4").visible = false;
            gridView2.columnByName("CHG_FILE_NAME4").visible = false;
            gridView2.columnByName("FILE_PATH4").visible = false;
            gridView2.columnByName("FILE_EXT4").visible = false;
            gridView2.columnByName("ORG_FILE_NAME5").visible = false;
            gridView2.columnByName("CHG_FILE_NAME5").visible = false;
            gridView2.columnByName("FILE_PATH5").visible = false;
            gridView2.columnByName("FILE_EXT5").visible = false;
            gridView2.columnByName("KEY_HID").visible = false;
            dataProvider2.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView2.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView2.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView2.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView2.pasteOptions.enabled = false;//붙여넣기 금지
        }

    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }
        /* 그리드 포커스 행 강조 : 반드시 이 이름으로 써야 합니다. */
        .rg-focused-row{
          background: #FFF8CE;
        }

    </style>
    
    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="460" />
    <asp:HiddenField ID="hidParams" runat="server" />
    <asp:HiddenField ID="hidFileAttachPath" runat="server" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua51</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua51'); return false;" />
                <input type="button" value="Upload" ID="btnUpload" runat="server" visible="false" onclick="javascript:fn_Upload();" />
                <asp:Button ID="btnDetailSearch" runat="server" OnClick="btnDetailSearch_Click" Text="Search" OnClientClick="fn_WatingCall();" style="display:none;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:80px;">
                    <col style="width:100px;">
                    <col style="width:60px;">
                    <col style="width:180px;">
                    <col style="width:60px;">
                    <col style="width:230px;">
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col>
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbYear" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbManNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtManNo" runat="server" MaxLength="20" Width="150"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbManDept" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlManDept" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbStatus" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>

        <br />

        <div style="height:330px;" id="divMainGrid">
            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realMaingrid" style="width: 100%; height: 280px;"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Qua51'); return false;" style="display:none; margin-top:10px; float:left" />
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

        <br />

        <div class="title">
            <h3><span id="spanTabLabel"></span> </h3>
            <div class="al-r">
                <input type="button" value="New" onclick="javascript:fn_Register2();" ID="btnSubNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel2" runat="server" visible="false" onclick="fn_excelExport2('Qua51_2'); return false;" />
            </div>
        </div>
        
        <br />

        <div class="realgrid_330_1" id="divDetailGrid">

            <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realDetailgrid"  class="realgrid_330_1" ></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage2" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value2" onchange="setPaging2(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet2" onclick="getCol2('Qua51_2'); return false;" style="display:none; margin-top:10px; float:left" />
									<div class="al-r total" ondragstart="return false">Total : <div id="rowCnt2" class="f02" style="float:right"></div></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnDetailSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>


    <script type="text/javascript">
        function fn_Detail(_val) {
            $("#MainContent_hidParams").val(_val);
            $("#MainContent_btnDetailSearch").click();
        }
    </script>



</asp:Content>
