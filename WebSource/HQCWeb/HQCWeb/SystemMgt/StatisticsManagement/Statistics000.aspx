<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Statistics000.aspx.cs" Inherits="HQCWeb.SystemMgt.StatisticsManagement.Statistics000" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
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
            //setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {
            ////PK 컬럼 클릭시 동작
            //gridView.onCellClicked = function (grid, clickData) {
            //    if (clickData.column == 'COL_1') {
            //        var current = gridView.getCurrent();
            //        var value = dataProvider.getValue(current.dataRow, "KEY_HID");
            //        fn_Modify(value);
            //    }
            //}

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            //gridView.columnByName("MENU_ID").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
        }

        function fn_Search() {
            var bChk = false;
            var vRbCon = "";

            var bRB01 = $("#MainContent_rb01");
            var bRB02 = $("#MainContent_rb02");
            var bRB03 = $("#MainContent_rb03");

            if (bRB01.is(':checked')) {
                bChk = true;
                vRbCon = "RB01";
            }

            if (bRB02.is(':checked')) {
                bChk = true;
                vRbCon = "RB02";
            }

            if (bRB03.is(':checked')) {
                bChk = true;
                vRbCon = "RB03";
            }

            if (!bChk) {
                alert("통계타입을 선택하세요.");
                return false;
            } else {

                if (vRbCon == "RB03") {
                    if ($("#MainContent_ddlGroupCode").val() == "") {
                        alert("그룹을 선택하세요.");
                        $("#MainContent_ddlGroupCode").focus();
                        return false;
                    }
                }
            }

            return;
        }


    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_00120</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:280px;" />
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbStatisticsType" runat="server"></asp:Label></th>
                    <td>
                        <asp:RadioButton ID="rb01" runat="server" Text="메뉴별"     GroupName="rbStatistics" OnCheckedChanged="rb01_CheckedChanged" AutoPostBack="true"  /> &nbsp;&nbsp;
                        <asp:RadioButton ID="rb02" runat="server" Text="사용자별"   GroupName="rbStatistics" OnCheckedChanged="rb02_CheckedChanged" AutoPostBack="true"  /> &nbsp;&nbsp;
                        <asp:RadioButton ID="rb03" runat="server" Text="그룹별"     GroupName="rbStatistics" OnCheckedChanged="rb03_CheckedChanged" AutoPostBack="true"  /> &nbsp;
                    </td>
                    <th><asp:Label ID="lbSearchDate" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                    </td>

                    <th><asp:Label ID="lbSearchText" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchText" runat="server" style="width:150px;"></asp:TextBox>
                                <asp:DropDownList ID="ddlGroupCode" runat="server" style="width:150px;" Visible="false"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rb01" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rb02" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rb03" EventName="CheckedChanged" />

                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="return fn_Search(); fn_WatingCall();" Visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" class="realgrid"></div>
                    <%--<table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('WEB_00120'); return false;" style="display:none; margin-top:10px; float:left" />
									<div class="al-r total" ondragstart="return false">Total : <div id="rowCnt" class="f02" style="float:right"></div></div>
                                </div>
                            </td>
                        </tr>
                    </table>--%>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />

                    <asp:AsyncPostBackTrigger ControlID="rb01" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rb02" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rb03" EventName="CheckedChanged" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>