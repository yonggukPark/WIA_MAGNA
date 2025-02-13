<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Mon17.aspx.cs" Inherits="HQCWeb.MonitorMgt.Mon17.Mon17" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            fn_WatingCall();
            return true;
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

            //STATUS_FLG에 따라 색상변경
            const f = function (grid, dataCell) {
                var ret = {}
                var StatusFlg = grid.getValue(dataCell.index.itemIndex, "STATE_CD")

                if (StatusFlg.indexOf("0") !== -1) {
                    ret.style = { background: "#99cc99" }
                }
                else if (StatusFlg.indexOf("1") !== -1) {
                    ret.style = { background: "#f8cbad" }
                }
                else if (StatusFlg.indexOf("2") !== -1) {
                    ret.style = { background: "#ffd966" }
                }
                else if (StatusFlg.indexOf("3") !== -1) {
                    ret.style = { background: "#8faadc" }
                }

                return ret;
            }

            gridView.columnByName("STATE_CD").styleCallback = f;
            gridView.columnByName("STN_NM").styleName = "string-column";
            gridView.columnByName("STATE_CD").styleName = "string-column";
            gridView.columnByName("REMARK1").styleName = "string-column";
            gridView.columnByName("REMARK2").styleName = "string-column";

            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
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
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }

        .string-column {
            text-align: left;
        }

        .number-column {
            text-align: right;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="650" />

    <asp:HiddenField ID="hidKeyVal" runat="server" />
    <asp:HiddenField ID="hidupdateJSON" runat="server" />
    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Mon17</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Mon17'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:60px;" />
                            <col style="width:120px;" />
                            <col style="width:60px;" />
	                        <col style="width:80px;">
                            <col style="width:220px;" />
                            <col style="width:60px;" />
                            <col style="width:250px;" />
                            <col style="width:60px;" />
                            <col style="width:120px;" />
                        </colgroup>
                        <tr>
                            <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="100"></asp:DropDownList>
                            </td>
                            <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                            <td style="border-right: none;">
                                <asp:DropDownList ID="ddlEopFlag" runat="server" Width="70" OnSelectedIndexChanged="ddlEopFlag_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true" Width="250"></asp:DropDownList>
                            </td>
                            <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlStnCd" runat="server" Width="250"></asp:DropDownList>
                            </td>
                            <th><asp:Label ID="lbState" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" Width="100"></asp:DropDownList>
                            </td>
                            <td class="al-r">
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
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
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:14px; display:none; margin-top:10px; float:left" runat="server"  >
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Mon17'); return false;" style="display:none; margin-top:10px; float:left" />
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