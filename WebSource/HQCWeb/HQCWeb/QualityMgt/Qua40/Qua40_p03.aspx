<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua40_p03.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua40.Qua40_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 21%;
            position : absolute; 
        }
    </style>
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_txtDate").val() == "") {
                alert("발생일을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlPartNo").val() == "") {
                alert("품번을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtLotNo").val() == "") {
                alert("로트넘버를 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlDRespCd").val() == "") {
                alert("귀책처를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDCode").val() == "") {
                alert("불량 현상을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlDReasonCode").val() == "") {
                alert("불량 원인을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtDefectCnt").val() == "") {
                alert("불량수량을 입력하세요.");
                return false;
            }else if ($("#PopupContent_txtReworkMsg").val() == "") {
                alert("사유를 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlDecomposeType").val() == "") {
                alert("분해유형을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlResultCd").val() == "") {
                alert("결과를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlSStorageCd").val() == "") {
                alert("출고창고를 선택하세요.");
                return false;
            } else {
                if (confirm("등록 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            return true;
        }

        function fn_Validation2() {
            if ($("#PopupContent_ddlPartNo").val() == "") {
                alert("품번을 선택하세요.");
                return false;
            } else if ($("#PopupContent_txtLotNo").val() == "") {
                alert("로트넘버를 입력하세요.");
                return false;
            } else{
                fn_WatingCall();
                return true;
            }
        }

        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }

        function fn_Check() {
            $("#PopupContent_btnCheck").click();
        }

        var comboData;

        // 로딩시 시행
        jQuery(document).ready(function ($) {
            $("#PopupContent_txtReturnDt").datepicker({
                dateFormat: "yy-mm-dd"
            });

            $("#PopupContent_txtSearchCombo").comboTree({
                source: [],
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                hidCon: "PopupContent_txtSearchComboHidden"
            });

            fn_Combo_Refresh();
        });

        /*다중콤보 시작*/

        function fn_SearchSetStyle() {
            if ($("#divSearchSet").css("display") == "none") {
                $("#divSearchSet").show();
            }
            else {
                $("#divSearchSet").hide();
            }
        }

        // 콤보 초기화
        function fn_Combo_Refresh() {
            $("#txtSearchValue").val(comboData);

            fn_Set();
        }

        function fn_Set() {
            $("#PopupContent_txtSearchCombo").comboTree({
                source: [],
                comboReload: true
            });

            var txtSearch = $("#txtSearchValue").val();

            txtSearch = txtSearch.replace(/(?:\r\n|\r|\n)/g, ',');

            const arr = txtSearch.split(',');
            const arrTxtSearch = arr.filter((el, index) => arr.indexOf(el) === index);

            var tList = new Array();

            var iLen = 0;

            for (var i = 0; i < arrTxtSearch.length; i++) {
                if (arrTxtSearch[i] != "") {
                    iLen++;

                    var comboData = new Object();

                    comboData.id = arrTxtSearch[i];
                    comboData.title = arrTxtSearch[i];

                    tList.push(comboData);
                }
            }

            if (iLen > 10) {
                alert("로트 셋팅 가능 숫자는 10개 입니다.");

                $("#PopupContent_txtSearchCombo").comboTree({
                    source: [],
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true,
                    onLoadAllChk: true
                });

                return false;
            } else {
                $("#PopupContent_txtSearchCombo").comboTree({
                    source: tList,
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true,
                    onLoadAllChk: true,
                    hidCon: "PopupContent_txtSearchComboHidden"
                });

                $("#divSearchSet").hide();
            }
        }

        /*다중콤보 끝 */
    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Qua40</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDefectDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPartNo" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLotNo" runat="server"></asp:Label></th>
                    <td>
                        <table>
                            <tr>
                                <td style="padding:0px;">
                                    <%--<asp:TextBox ID="txtLotNo" runat="server"></asp:TextBox> --%>
                                    <div class="searchCombo" style="font-size:12px;">
                                        <asp:TextBox ID="txtSearchCombo" runat="server" ReadOnly="true" style="background-color:white; color:black; width: 270px;"></asp:TextBox>
                                        <asp:TextBox id="txtSearchComboHidden" runat="server" style="display:none;"></asp:TextBox>
                                    </div>
                                    <img src="/img/gnb_on_arrow.png" style="float:left; padding-left:270px;" onclick="javascript:fn_SearchSetStyle();" />
                                    <div id="divSearchSet" class="searchCombo" style="display:none;">
                                        <textarea id="txtSearchValue" rows="5" style="width:270px;"></textarea>
                                        <br />
                                        <input type="button" value="적용" onclick="javascript:fn_Set();" />
                                        <input type="button" value="닫기" onclick="javascript:fn_SearchSetStyle();" />
                                    </div>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click"/>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="padding:0px;">
                                    <div class = "td_wrap">
                                        <a href="javascript:fn_Check();" class="btn ml10" id="aCheck" runat="server" >Check</a>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDRespCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDRespCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDCode" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDReasonCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDReasonCode" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDefectCnt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDefectCnt" type="number" runat="server" Attributes="min=1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReturnDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReturnDt" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbReworkMsg" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtReworkMsg" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbDecomposeType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDecomposeType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbResultCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlResultCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbSStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSStorageCd" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDStorageCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
                <asp:Button ID="btnCheck"     runat="server" OnClientClick="javascript:return fn_Validation2();" OnClick="btnCheck_Click" Text="Check" style="display:none;"/>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
