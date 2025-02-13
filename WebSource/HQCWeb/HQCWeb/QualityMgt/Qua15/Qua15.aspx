<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua15.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua15.Qua15" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 56%;
            position : absolute; 
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }

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
    </style>
    <script type="text/javascript">

        let intervalIds = {}; // 요청별 Interval ID 저장 객체

        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtStnCdHidden").val() == "") {
                alert("공정을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtCarTypeHidden").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else {

                fn_WatingCall();

                const requestKey = "btnExcelNew"; // 버튼별 고유 키
                intervalIds[requestKey] = setInterval(() => checkDownloadStatus(requestKey), 2000);

                return true;
            }
            return true;
        }

        function fn_Validation2() {
            if ($("#MainContent_ddlShopCd2").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd2").val() == "") {
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

                fn_WatingCall();

                const requestKey = "btnExcelNew2"; // 버튼별 고유 키
                intervalIds[requestKey] = setInterval(() => checkDownloadStatus(requestKey), 2000);

                return true;
            }
            return true;
        }

        //2초마다 로딩 체크
        function checkDownloadStatus(requestKey) {
            $.ajax({
                type: "POST",
                url: "Qua15.aspx/GetStatus", // 해당 WebMethod URL
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const status = response.d; // 서버에서 반환된 값

                    if (status === "C") {
                        clearInterval(intervalIds[requestKey]); // 요청별 Interval 중지
                        delete intervalIds[requestKey]; // 요청 상태 제거
                        fn_loadingEnd(); // 로딩 종료 처리
                        alert("조회 완료되었습니다."); //안내 팝업 추가 
                    } else {
                        console.log(`Current Status (${requestKey}):`, status);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error occurred:", error);
                }
            });
        }

        <%--function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnExcelNew) %>
        }--%>

        var cStn, cCarType;

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            initializePage();
        });

        function initializePage() {
            //라벨 설정
            $("#spanTabLabel1").text("- 토크 ")
            $("#spanTabLabel1_1").text("주의 : 엑셀 다운로드중 페이지를 이동하면 안됩니다.");
            $("#spanTabLabel2").text("검사기")
            $("#spanTabLabel2_1").text("주의 : 엑셀 다운로드중 페이지를 이동하면 안됩니다.");

            fn_Set_Stn();
            fn_Set_CarType();
        }

        //Shop코드 재설정
        function fn_Set_Stn() {
            if (cStn === undefined)
                cStn = [];

            $("#MainContent_txtStnCd").comboTree({
                source: cStn,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtStnCdHidden",
                functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                functionCallFunc: "fn_Stn_Checked"
            });
        }

        //Shop코드 onchange
        function fn_Stn_Checked() {
            $("#MainContent_btnFunctionCall").click();
        }

        //부품코드 재설정
        function fn_Set_CarType() {
            if (cCarType === undefined)
                cCarType = [];

            $("#MainContent_txtCarType").comboTree({
                source: cCarType,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtCarTypeHidden"
            });
        }

        function fn_SetLabel(str, arg) {
            if (arg == 1)
                $("#spanTabLabel1_1").text(str);
            else if (arg == 2)
                $("#spanTabLabel2_1").text(str);
        }
    </script>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!-- TORQUE -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua15</asp:Label> <span id="spanTabLabel1"></span> <span id="spanTabLabel1_1" style="margin-left:58vw"></span> </h3>
            <div class="al-r">
               <asp:Button ID="btnExcelNew" runat="server" Text="Excel" OnClick="btnExcelNew_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
            </div>
        </div>

        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:320px;">
                    <col style="width:60px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
                    <col style="width:70px;">
                    <col style="width:270px;">
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
                    <th><asp:Label ID="lbRslt1" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlRslt1" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbProdDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:TextBox ID="txtFromDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="IME-MODE:disabled; background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox> 
                            <span id="spBetween2">~</span>
                            <asp:TextBox ID="txtToDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox>
                            <asp:DropDownList ID="ddlWctCd" runat="server" OnSelectedIndexChanged="ddlWct_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlWctCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td colspan="2" style = "border-right: 0px;">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:250px; font-size:12px;">
                                    <input type="text" ID="txtStnCd" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtStnCdHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        </td>
                        <td colspan="2">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:250px; font-size:12px;">
                                    <input type="text" ID="txtCarType" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtCarTypeHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>
        <!-- TORQUE END -->
        <br />

        <!-- INSPECTION -->
        <div class="title">
            <h3><span id="spanTabLabel2"></span> <span id="spanTabLabel2_1" style="margin-left:68vw"></span> </h3>
            <div class="al-r">
               <asp:Button ID="btnExcelNew2" runat="server" Text="Excel" OnClick="btnExcelNew2_Click" OnClientClick="javascript:return fn_Validation2();" Visible="false" />
            </div>
        </div>

        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:320px;">
                    <col style="width:60px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
                    <col style="width:70px;">
                    <col style="width:380px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbShopCd2" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd2" runat="server" OnSelectedIndexChanged="ddlShopCd2_SelectedIndexChanged" AutoPostBack="true" Width="270"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd2" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlEopFlag2" runat="server" Width="70" OnSelectedIndexChanged="ddlEopFlag2_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLineCd2" runat="server" OnSelectedIndexChanged="ddlLineCd2_SelectedIndexChanged" AutoPostBack="true" Width="200"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd2" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag2" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbRslt2" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlRslt2" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false"/>
                        <input type="button" value="Spec" onclick="javascript: fn_Spec();" ID="btnSpec" runat="server" visible="true"/>--%>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbProdDt2" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:TextBox ID="txtFromDt2" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="IME-MODE:disabled; background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox> 
                            <span id="spBetween">~</span>
                            <asp:TextBox ID="txtToDt2" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox>
                            <asp:DropDownList ID="ddlWctCd2" runat="server" OnSelectedIndexChanged="ddlWct2_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlWctCd2" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbStnCd2" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStnCd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStnCd_SelectedIndexChanged" CssClass="ellipsis-dropdown"></asp:DropDownList>
                                <asp:DropDownList ID="ddlDevCd" runat="server" CssClass="ellipsis-dropdown"></asp:DropDownList>
                                <asp:DropDownList ID="ddlCarType" runat="server" CssClass="ellipsis-dropdown"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd2" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd2" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag2" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>

                    <td class="al-r">
                        <%--<input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua11'); return false;" />
                        <input type="button" value="Upload" onclick="javascript: fn_Upload_Click();" ID="btnUpload" runat="server" visible="true" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <!-- INSPECTION END -->
    </div>

</asp:Content>