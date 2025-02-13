<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Mon18.aspx.cs" Inherits="HQCWeb.MonitorMgt.Mon18.Mon18" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            var fromDateStr = $("#MainContent_txtFromDt").val();
            var toDateStr = $("#MainContent_txtToDt").val();
            var wctFlag = $("#MainContent_ddlWctCd").val();

            // 날짜 유효성 검사
            if ((!fromDateStr || !toDateStr) && wctFlag == "H") {
                alert("날짜 데이터가 비어있습니다.");
                return false;
            } else {
                // 문자열 값을 Date 객체로 변환
                var fromDate = new Date(fromDateStr);
                var toDate = new Date(toDateStr);

                // 변환된 날짜 유효성 확인
                if (isNaN(fromDate) || isNaN(toDate)) {
                    alert("날짜 형식이 올바르지 않습니다.");
                    return false;
                }

                // 날짜 차이 계산
                var timeDifference = toDate.getTime() - fromDate.getTime();
                var dayDifference = timeDifference / (1000 * 60 * 60 * 24);

                // 종료일이 시작일보다 이전인지 확인
                if (toDate < fromDate) {
                    alert("시작일보다 종료일이 작습니다.");
                    return false;
                }

                // 1개월 이내인지 확인 (1개월을 30일로 계산)
                if (dayDifference > 30) {
                    alert("최대 조회 기간은 1개월입니다.");
                    return false;
                }
            }

            fn_WatingCall();
            return true;
        }

        function fn_Register() {
            fn_OpenPop('MonitorMgt/Mon18/Mon18_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Lock() {
            var pkString = '';
            var rows = gridView.getCheckedRows();

            if (rows.length > 0) {

                if (rows.length > 100) {
                    alert("100건 이하의 데이터만 일괄수정 가능합니다 : 현재 " + rows.length + "건 ");
                    return false;
                }

                for (var i in rows) {
                    var data = dataProvider.getJsonRow(rows[i]);

                    //if (data.BARCODE_FLAG == "N") {
                    //    alert("단품 조회의 경우에만 일괄삭제 가능합니다.");
                    //    return false;
                    //}

                    pkString += data.KEY_HID;
                    pkString += '|';
                }

                pkString = pkString.substring(0, pkString.length - 1);
                fn_PostOpenPop(pkString, 'MonitorMgt/Mon18/Mon18_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            }

        }

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
            //gridView.checkBar.visible = false;
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

            gridView.columnByName("REWORK_MSG").styleName = "string-column"
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
    </script>

    <style>
        .grid-primary-column {
            color: blue;
            font-weight: bold;
        }

        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="300" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Mon18</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="Lock" onclick="javascript:fn_Lock();" ID="btnLock" runat="server" visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Mon18'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:90px;">
	                <col style="width:290px;">
	                <col style="width:80px;">
	                <col style="width:120px;">
	                <col style="width:80px;">
	                <col style="width:120px;">
                    <col style="width:90px;">
	                <col style="width:290px;">
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbSearchDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFromDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="IME-MODE:disabled; background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox> 
                                <span id="spBetween">~</span>
                                <asp:TextBox ID="txtToDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox>
                                <asp:DropDownList ID="ddlWctCd" runat="server" OnSelectedIndexChanged="ddlWct_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlWctCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbGubunCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlGubunCd" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbBlockFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlBlockFlag" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbBarcode" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtBarcode" runat="server" Width="270" MaxLength="60"></asp:TextBox></td>
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Mon18'); return false;" style="display:none; margin-top:10px; float:left" />
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