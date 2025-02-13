<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Pln03.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln03.Pln03" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;

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
            settingGrid(_val);
        }

        function settingGrid(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
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
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
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

            return sevenDays;
        }



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
            <h3><asp:Label ID="lbTitle" runat="server">Pln03</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:120px;" />
                    <col style="width:100px;" />
                    <col style="width:220px;" />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbPlanDt" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox></td>
                    <th><asp:Label ID="lbPlanCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPlanCd" runat="server"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Pln03'); return false;" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" style="width: 100%; height: 440px;"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:2px;" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Pln03'); return false;" style="display:none; margin-top:2px;" />
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