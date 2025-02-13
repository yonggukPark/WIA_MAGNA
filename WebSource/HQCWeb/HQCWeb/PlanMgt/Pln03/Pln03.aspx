<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Pln03.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln03.Pln03" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var searchDate;

        //그리드 다중 컬럼 설정
        var layout1;

        layout1 = [
          "PLANT_CD","SHOP_CD", "LINE_CD", "CAR_TYPE", "PART_NO", "PART_DESC", "PLAN_NM",

          {
              name: "OPERATION_PLAN",
              direction: "horizontal",
              items: [
                "Q1", "Q2", "Q3", "Q4", "Q5", "Q6", "Q7"
              ],
              header: {
                  text: "운영계획",
              }
          }, "KEY_HID"
        ];

        //function fn_Chk(pkCode, seq) {

        //    var jsonData = JSON.stringify({ sParams: pkCode, sParams2: seq });

        //    $.ajax({
        //        type: "POST",
        //        url: "Pln03.aspx/GetPkCode",
        //        data: jsonData,
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (msg) {
        //            fn_Popup(msg.d);
        //        }
        //    });
        //}

        //function fn_Popup(pkCode) {
        //    fn_PostOpenPop(pkCode, '/ResultMgt/ResComm/ResComm.aspx', "1600", "750");
        //}

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);

            //field 넣기 전 포맷변경(엑셀 문제)
            field.forEach(item => {
                if (
                    item.fieldName == 'Q1' || item.fieldName == 'Q2' || item.fieldName == 'Q3' ||
                    item.fieldName == 'Q4' || item.fieldName == 'Q5' || item.fieldName == 'Q6' || item.fieldName == 'Q7'
                   ) {
                    item.dataType = 'number'; item.subType = 'int'
                }
            });

            //숫자 포맷 변경
            gridView.setFormatOptions({ numberFormat: '#,##0' });

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

            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                //if (clickData.column.substring(0, 1) == 'Q') {
                //    var current = gridView.getCurrent();
                //    if (dataProvider.getValue(current.dataRow, current.column) > 0) {
                //        var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                //        var cnt = clickData.column.substring(1, 2) - 1;
                //        var currentDate = new Date(searchDate);
                //        currentDate.setDate(searchDate.getDate() + cnt);
                        
                //        // 현재 날짜의 연도, 월, 일 부분을 각각 추출
                //        var year = currentDate.getFullYear();
                //        var month = (currentDate.getMonth() + 1).toString().padStart(2, '0'); // 월은 0부터 시작하므로 +1을 해야 함
                //        var day = currentDate.getDate().toString().padStart(2, '0');

                //        // YYYYMMDD 형식으로 문자열 생성
                //        var seq = year.toString() + month + day;
                //        fn_Chk(value, seq);
                //    }
                //    else {
                //        alert("데이터가 없습니다.");
                //    }
                //}
            }

            //헤더 설정
            var weekDate = getSevenDaysArray($("#MainContent_txtDate").val());
            var header;
            for (var i = 0; i < weekDate.length; i++) {
                header = gridView.getColumnProperty("Q" + (i + 1), "header");
                header.text = weekDate[i];
                gridView.setColumnProperty("Q" + (i + 1), "header", header);
            }

            //그리드 다중 컬럼 설정
            gridView.setColumnLayout(layout1);
            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("PART_DESC").styleName = "string-column";

            gridView.columnByName("Q1").styleName = "number-column";
            gridView.columnByName("Q2").styleName = "number-column";
            gridView.columnByName("Q3").styleName = "number-column";
            gridView.columnByName("Q4").styleName = "number-column";
            gridView.columnByName("Q5").styleName = "number-column";
            gridView.columnByName("Q6").styleName = "number-column";
            gridView.columnByName("Q7").styleName = "number-column";

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
        }

        //날짜 추출 함수
        function getSevenDaysArray(inputDate) {
            const startDate = new Date(inputDate);
            const sevenDays = [];

            for (let i = 0; i < 7; i++) {
                // 현재 날짜에 i일 더하고
                const currentDate = new Date(startDate);
                currentDate.setDate(startDate.getDate() + i);

                // MM-dd 형식으로 자르기
                const dateString = currentDate.toISOString().substring(5, 10);
                sevenDays.push(dateString);
            }
            searchDate = startDate;
            return sevenDays;
        }


        $(document).ready(function () {
            fn_loadingEnd();
        });
    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }

        .number-column {
            /*font-weight: bold;
            text-decoration: underline; cursor: pointer;*/
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
            <h3><asp:Label ID="lbTitle" runat="server">Pln03</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Pln03'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;" />
                    <col style="width:120px;" />
                    <col style="width:60px;" />
                    <col style="width:120px;" />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbPlanDt" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black; padding-left:8px; padding-right:8px;width:100px" ></asp:TextBox></td>
                    <th><asp:Label ID="lbPlanCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPlanCd" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Pln03'); return false;" />--%>
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Pln03'); return false;" style="display:none; margin-top:10px; float:left" />
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