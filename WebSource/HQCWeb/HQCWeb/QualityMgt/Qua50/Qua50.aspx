<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua50.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua50.Qua50" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            fn_WatingCall();
            return true;
        }

        function fn_Register() {
            fn_OpenPop('QualityMgt/Qua50/Qua50_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'QualityMgt/Qua50/Qua50_p02.aspx', $("#MainContent_hidWidth").val(), '800');
        }

        function fn_Copy() {
            alert("아직 기능이 없습니다.");

            //if (currentValue != undefined)
            //    fn_PostOpenPop(currentValue, 'QualityMgt/Qua50/Qua50_p03.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            //else
            //    alert("먼저 복사할 행을 선택해주세요.");
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            fn_Set_Line();
        });

        //부품코드 재설정
        function fn_Set_Line() {
            $("#MainContent_txtLineCd").comboTree({
                source: cLine,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtLineCdHidden"
            });
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var currentValue;//마지막으로 클릭한 그리드 데이터
        var cLine;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid_2');
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
                currentValue = value;

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }

                if (clickData.column == 'NO') {
                    fn_Modify(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            //필터기능 세팅
            gridView.getColumns().forEach(column => {
                gridView.setColumnProperty(column.fieldName, "filterable", true);
                gridView.setColumnFilters(column.fieldName, "");
            });

            gridView.setOptions({
                stateBar: {
                    visible: true
                },
                filtering: {
                    selector: {
                        visible: true
                    }
                }
            });

            // FILE_ATTACH 컬럼에 renderer 추가
            gridView.setColumnProperty("FILE_ATTACH", "renderer", {
                type: "html",
                callback: function (grid, cell) {
                    const fileAttachPath = $("#MainContent_hidFileAttachPath").val()//
                    const row = grid.getValues(cell.index.itemIndex); // 행 데이터 가져오기
                    const filesHtml = [];
                    const folderName = "Qua50";

                    // 최대 5개의 파일 경로 처리
                    for (let i = 1; i <= 5; i++) {
                        const filePath = row[`FILE_PATH${i}`];
                        const orgFileName = row[`ORG_FILE_NAME${i}`];
                        const chgFileName = row[`CHG_FILE_NAME${i}`];
                        const icon = row[`FILE_EXT${i}`];

                        if (filePath && orgFileName && icon) {
                            var tIcon = null;

                            if (icon.toLowerCase() == "png" || icon.toLowerCase() == "gif" || icon.toLowerCase() == "jpg") {
                                tIcon = "image";
                            }

                            if (icon.toLowerCase() == "ppt" || icon.toLowerCase() == "pptx") {
                                tIcon = "powerpoint";
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

            gridView.columnByName("NO").styleName = "grid-primary-column"
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("MOD_REMARK").styleName = "string-column";
            gridView.columnByName("REMARK").styleName = "string-column";

            // 파일 경로데이터 숨김
            gridView.columnByName("ORG_FILE_NAME1").visible = false;
            gridView.columnByName("CHG_FILE_NAME1").visible = false;
            gridView.columnByName("FILE_PATH1").visible = false;
            gridView.columnByName("FILE_EXT1").visible = false;
            gridView.columnByName("ORG_FILE_NAME2").visible = false;
            gridView.columnByName("CHG_FILE_NAME2").visible = false;
            gridView.columnByName("FILE_PATH2").visible = false;
            gridView.columnByName("FILE_EXT2").visible = false;
            gridView.columnByName("ORG_FILE_NAME3").visible = false;
            gridView.columnByName("CHG_FILE_NAME3").visible = false;
            gridView.columnByName("FILE_PATH3").visible = false;
            gridView.columnByName("FILE_EXT3").visible = false;
            gridView.columnByName("ORG_FILE_NAME4").visible = false;
            gridView.columnByName("CHG_FILE_NAME4").visible = false;
            gridView.columnByName("FILE_PATH4").visible = false;
            gridView.columnByName("FILE_EXT4").visible = false;
            gridView.columnByName("ORG_FILE_NAME5").visible = false;
            gridView.columnByName("CHG_FILE_NAME5").visible = false;
            gridView.columnByName("FILE_PATH5").visible = false;
            gridView.columnByName("FILE_EXT5").visible = false;
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

        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 12%;
            position : absolute; 
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="550" />
    <asp:HiddenField ID="hidFileAttachPath" runat="server" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua50</asp:Label></h3>
            <div class="al-r">
               <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Copy" onclick="javascript:fn_Copy();" ID="btnCopy" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua50'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:145px;">
                    <col style="width:70px;">
                    <col style="width:120px;">
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col style="width:60px;">
                    <col style="width:220px;">
                    <col style="width:60px;">
                    <col style="width:250px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbProdMonth" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbEo4mFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEo4mFlag" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" Width="100" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:200px; font-size:12px;">
                                    <input type="text" ID="txtLineCd" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtLineCdHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbEoNo" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtEoNo" runat="server" Width="240" ></asp:TextBox></td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid_2" class="realgrid"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Qua50'); return false;" style="display:none; margin-top:10px; float:left" />
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