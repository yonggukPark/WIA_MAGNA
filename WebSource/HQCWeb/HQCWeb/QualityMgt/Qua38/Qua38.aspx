<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua38.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua38.Qua38" %>

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
            fn_OpenPop('QualityMgt/Qua38/Qua38_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'QualityMgt/Qua38/Qua38_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        // 복사 기능 사용?
        //function fn_Copy() {
        //    if (currentValue != undefined)
        //        fn_PostOpenPop(currentValue, 'QualityMgt/Qua38/Qua38_p03.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        //    else
        //        alert("먼저 복사할 행을 선택해주세요.");
        //}

        function fn_Confirm() {
            var rows = gridView.getCheckedRows(true);
            var pkString = '';

            rows.forEach((item) => {
                pkString += dataProvider.getValue(item, "KEY_HID");
                pkString += '|';
            });

            pkString = pkString.substring(0, pkString.length - 1);

            var jsonData = JSON.stringify({ sParams: pkString });

            $.ajax({
                type: "POST",
                url: "Qua38.aspx/SetConfirm",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_Rtn(msg.d);
                }
            });
        }

        function fn_Rtn(msg) {
            if (msg != "OK"){
                alert("다음 데이터의 승인이 실패했습니다." + msg);
            }
            else {
                alert("승인이 완료되었습니다.");
            }
            $("#MainContent_btnSearch").click();
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var currentValue;//마지막으로 클릭한 그리드 데이터
        var cPart;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);
            //field 넣기 전 포맷변경(엑셀 문제)
            field.forEach(item => {
                if (item.fieldName == 'DEFECT_CNT') {
                    item.dataType = 'number'; item.subType = 'int'
                }
            });

            //숫자 포맷 변경
            gridView.setFormatOptions({ numberFormat: '#,##0' });

            dataProvider.setFields(field);
            dataProvider.setRows(data);
            gridView.checkBar.visible = true;
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
                var chk = dataProvider.getValue(current.dataRow, "ERP_RESULT");
                currentValue = value;

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }

                if (clickData.column == 'LOG_SEQ' && chk != "Y") {
                    fn_Modify(value);
                }
                else if (chk == "Y") {
                    alert("이미 인터페이스 처리된 데이터입니다.")
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            if (data != undefined && data != '') {
                for (var i = 0; i < data.length ; i++){
                    if (data[i].CONFIRM_USER.length != 0) 
                        gridView.setCheckable(i, false);//이미 컨펌된 데이터는 컨펌불가
                }
            }
            

            gridView.columnByName("LOG_SEQ").styleName = "grid-primary-column";
            gridView.columnByName("PART_DESC").styleName = "string-column";
            gridView.columnByName("DEFECT_REASON").styleName = "string-column";
            gridView.columnByName("DEFECT_COMPANY").styleName = "string-column";
            gridView.columnByName("DECOMPOSE_TYPE").styleName = "string-column";
            gridView.columnByName("F_STORAGE_NM").styleName = "string-column";
            gridView.columnByName("D_STORAGE_NM").styleName = "string-column";
            gridView.columnByName("DELETE_DESC").styleName = "string-column";

            gridView.columnByName("DEFECT_CNT").styleName = "number-column";
            
            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("DEFECT_DT_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
        }

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            $("#MainContent_txtPartNo").comboTree({
                source: cPart,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtPartNoHidden"
            });
        });

        //부품코드 재설정
        function fn_Set_Part() {

            $("#MainContent_txtPartNo").comboTree({
                source: cPart,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtPartNoHidden"
            });
        }

    </script>

    <style>
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 12%;
            position : absolute; 
        }

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
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua38</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Copy" onclick="javascript:fn_Copy();" ID="btnCopy" runat="server" visible="false" />
                <input type="button" value="Confirm"   OnClick="javascript:fn_Confirm();" ID="btnConfirm" runat="server" visible="false"   />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua38'); return false;" />
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
                    <col style="width:450px;">
                    <col style="width:100px;">
                    <col style="width:270px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbDefectDt" runat="server"></asp:Label></th>
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
                    <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo" style="width:430px; font-size:12px;">
                            <input type="text" ID="txtPartNo" runat="server" style="background-color:white; color:black;" readonly/>
                            <asp:TextBox id="txtPartNoHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbDefectCompany" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDefectCompany" runat="server" Width="250"></asp:DropDownList>
                    </td>
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Qua38'); return false;" style="display:none; margin-top:10px; float:left" />
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