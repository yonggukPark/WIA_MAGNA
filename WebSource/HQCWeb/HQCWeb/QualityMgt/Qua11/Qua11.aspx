<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua11.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua11.Qua11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .ellipsis-dropdown {
        width: 240px; /* 고정 너비를 설정합니다 */
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        }
        .ellipsis-dropdown option {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }
    </style>
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlStnCd").val() == "") {
                alert("공정을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlDevCd").val() == "") {
                alert("장비를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlCarType").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() != "") {
                alert("바코드와 시리얼을 동시에 검색할 수 없습니다.");
                return false;
            } else {

                // 날짜를 가져옵니다
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
            return true;
        }

        function fn_Spec() {

            var aData = "";
            aData = "Qua12";

            var jsonData = JSON.stringify({ sMenu: aData });

            $.ajax({
                type: "POST",
                url: "Qua11.aspx/GetMenu",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_Spec_Open(msg.d);
                }
            });
        }

        function fn_Spec_Open(title) {
            if (title == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }
            parent.fn_Add('/QualityMgt/Qua12/Qua12.aspx', title, 'Qua12', true);
        }

        function fn_Upload_Click() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlStnCd").val() == "") {
                alert("공정을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlDevCd").val() == "") {
                alert("장비를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlCarType").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else {

                var aData = "";
                aData = $("#MainContent_ddlShopCd").val() + "/" + $("#MainContent_ddlLineCd").val() + "/" + $("#MainContent_ddlStnCd").val() + "/" + $("#MainContent_ddlDevCd").val() + "/" + $("#MainContent_ddlCarType").val();

                var jsonData = JSON.stringify({ sParams: aData });

                $.ajax({
                    type: "POST",
                    url: "Qua11.aspx/ParamCrypto",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        fn_Upload(msg.d);
                    }
                });
            }
        }

        function fn_Upload(pkCode) {
            if (pkCode == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }

            fn_PostOpenPop(pkCode, '/QualityMgt/Qua11/Qua11_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }


        jQuery(document).ready(function ($) {
            //라벨 설정
            $("#spanTabLabel").text("(최대 조회 기간은 1개월 입니다.)")
        });

        var column, field, data;
        var container, dataProvider, gridView;
        var menuAdd;

        //검색조건에 따라 세팅 변경
        function getCol_override(menuId) {
            if (menuAdd != undefined)
                getCol(menuId + '_' + menuAdd);
            else
                getCol(menuId);
        }

        //숫자검사 정규식
        function isNumber(value) {
            const regex = /^-?\d+(\.\d+)?$/;
            return regex.test(value);
        }

        function compareInt(value) {
            if (Number.isInteger(Number(value)))
                return 'int';
            else
                return 'unum';
        }


        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid_2');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);

            //field 넣기 전 포맷변경(엑셀 문제)
            //if(data != undefined){
            //    field.forEach(item => {
            //        if (item.fieldName != '라인' && item.fieldName != '공정명' && item.fieldName != '공장' && item.fieldName != '생산일자' && item.fieldName != '검사시간') {
            //            data.forEach(row => {
            //                if (isNumber(row[item.fieldName])) {
            //                    item.dataType = 'number';
            //                    item.subType = compareInt(row[item.fieldName]);
            //                }
            //            })
            //        } 
            //    });
            //}

            //숫자 포맷 변경
            //gridView.setFormatOptions({ numberFormat: '#,##0' });

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

            if (gridView.columnByName("KEY_HID") != undefined)
                gridView.columnByName("KEY_HID").visible = false;

            if (data != undefined) {
                gridView.columnByName("라인").styleName = "string-column";
                gridView.columnByName("공정명").styleName = "string-column";

                field.forEach(item => {
                    if (item.fieldName != '라인' && item.fieldName != '공정명' && item.fieldName != '공장' && item.fieldName != '생산일자' && item.fieldName != '검사시간') {
                        data.some(row => {
                            if (isNumber(row[item.fieldName])) {
                                gridView.columnByName(item.fieldName).styleName = "number-column";
                                return true;
                            }
                            return false;
                        })
                    }
                });
            }

            //5차_화면수정요청 : NG인 경우 색 변경
            const f = function (grid, dataCell) {
                var ret = {}
                var rslt = grid.getValue(dataCell.index.itemIndex, "결과")

                if (rslt == 'NG') {
                    ret.style = { background: "#FFD966" }
                }

                return ret;
            }
            if (gridView.columnByName("결과") != undefined)
                gridView.columnByName("결과").styleCallback = f;

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

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="400" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua11</asp:Label> <span id="spanTabLabel"></span> </h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false"/>
                <input type="button" value="Spec" onclick="javascript: fn_Spec();" ID="btnSpec" runat="server" visible="false"/>
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua11'); return false;" />
                <input type="button" value="Upload" onclick="javascript: fn_Upload_Click();" ID="btnUpload" runat="server" visible="false" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:290px;">
                    <col style="width:60px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
                    <col style="width:70px;">
                    <col style="width:350px;">
                    <col style="width:60px;">
                    <col style="width:100px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="270"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlEopFlag" runat="server" Width="70" OnSelectedIndexChanged="ddlEopFlag_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true" Width="200"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStnCd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStnCd_SelectedIndexChanged" CssClass="ellipsis-dropdown"></asp:DropDownList>
                                <asp:DropDownList ID="ddlDevCd" runat="server" CssClass="ellipsis-dropdown"></asp:DropDownList>
                                <asp:DropDownList ID="ddlCarType" runat="server" CssClass="ellipsis-dropdown"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false"/>
                        <input type="button" value="Spec" onclick="javascript: fn_Spec();" ID="btnSpec" runat="server" visible="true"/>--%>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbProdDt" runat="server"></asp:Label></th>
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
                    <th><asp:Label ID="lbBarcode" runat="server"></asp:Label></th>
                    <td colspan="2"><asp:TextBox ID="txtBarcode" runat="server" Width="270" ></asp:TextBox></td>
                    <th><asp:Label ID="lbSerialNo" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtSerialNo" runat="server" Width="270"></asp:TextBox></td>
                    <th><asp:Label ID="lbRslt1" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlRslt1" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <%--<input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua11'); return false;" />
                        <input type="button" value="Upload" onclick="javascript: fn_Upload_Click();" ID="btnUpload" runat="server" visible="true" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid_2" class="realgrid_2"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol_override('Qua11'); return false;" style="display:none; margin-top:10px; float:left" />
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