<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Pln11.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln11.Pln11" %>

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
            fn_OpenPop('팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_ModifyConfirm(pkCode) {
            fn_PostOpenPop(pkCode, '/PlanMgt/Pln11/Pln11_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch2) %>
        }

        $(document).ready(function () {
            //달력 초기화 호출
            MakeCalendarInit();
        });

        var column, field, data;
        var container, dataProvider, gridView;
        var monthArr, selDay = "";

        // 그리드 생성
        function createGrid(_val) {

        }

        //달력 그리기 호출(현재날짜)
        function MakeCalendarInit() {

            monthArr = new Array(7 * 6); // 7일 6주 날짜데이터 설정
            for (var i = 0; i < monthArr.length; i++) //초기화
            {
                monthArr[i] = new Array(2);
                monthArr[i][1] = -1; //click count
            }

            var cuDay = new Date();
            MakeCalendar(cuDay.getFullYear(), cuDay.getMonth());
        }

        function fn_Prev_onclick() { // 이전달 클릭
            var NextDate = new Date();

            NextDate.setFullYear(intCuYear);
            NextDate.setMonth(intCuMonth - 1, 1);

            MakeCalendar(NextDate.getFullYear(), NextDate.getMonth());
        }

        function fn_Next_onclick() { //이후달 클릭
            var NextDate = new Date();

            NextDate.setFullYear(intCuYear);
            NextDate.setMonth(intCuMonth + 1, 1);

            MakeCalendar(NextDate.getFullYear(), NextDate.getMonth());
        }

        //달력 그리기
        function MakeCalendar(intSelectYear, intSelectMonth) {

            var MainList = "";

            var lastDay = new Date();
            lastDay.setFullYear(intSelectYear, intSelectMonth, 1);

            var NowDate = new Date();
            var iYear = NowDate.getYear();
            var iMonth = NowDate.getMonth();
            var iDay = NowDate.getDate();

            //--------------------------
            if (iMonth == 12) {
                lastDay.setFullYear(intSelectYear + 1);
                lastDay.setMonth(1, 1);

                lastDay.setDate(lastDay.getDate() - 1);         //d일을 더함
            }
            else {
                lastDay.setFullYear(intSelectYear);
                lastDay.setMonth(intSelectMonth + 1, 1);

                lastDay.setDate(lastDay.getDate() - 1);
            }

            var firstDay = new Date();

            firstDay.setFullYear(intSelectYear);
            firstDay.setMonth(intSelectMonth, 1);


            if (iDay > lastDay.getDate()) iDay = lastDay.getDate();

            var today = new Date();
            today.setFullYear(iYear);
            today.setMonth(iMonth, iDay);

            var currentDay = new Date();
            var i;
            var iFdow = 1;
            var iLdow = 1;
            var strName = "";
            var strDesc = "";

            iFdow = firstDay.getDay();// GetDayOfWeekInt(firstDay.DayOfWeek);
            iLdow = lastDay.getDay(); //GetDayOfWeekInt(lastDay.DayOfWeek);

            firstDay.setDate(firstDay.getDate() - iFdow);

            var startDay = firstDay;
            var endDay = new Date();
            lastDay.setDate(lastDay.getDate() + 7 - iLdow - 1);
            endDay = lastDay;
            var curDay = new Date();
            curDay = startDay;

            var strMMdd = "";
            var varId = "";

            curDay = startDay;

            endDay.setDate(endDay.getDate() + 1);

            MainList += " <tr><td colspan='7' style='height:1px; background-color:#ffffff;'></td></tr> \n";

            for (i = 0; !(endDay.getMonth() == curDay.getMonth() && endDay.getDate() == curDay.getDate()) ; i++) {

                if (curDay.getDay() == 0) {//일요일이면 tr
                    MainList += " <tr> \n ";
                }

                var vMonth;
                var vDay;

                if (curDay.getMonth() < 9)
                    vMonth = "0" + (curDay.getMonth() + 1);
                else vMonth = curDay.getMonth() + 1;

                if (curDay.getDate() < 10)
                    vDay = "0" + curDay.getDate();
                else vDay = curDay.getDate();

                varId = curDay.getFullYear().toString() + vMonth.toString() + vDay;

                monthArr[i][0] = varId; //날짜 저장
                //추가
                if (i == 0)//시작날짜
                    $("#MainContent_hidMonthStartDt").val(varId);

                //날짜 표시
                if (currentDay.getMonth() == curDay.getMonth() && currentDay.getDate() == curDay.getDate()) { //오늘
                    MainList += "<td id = '" + varId + "_TD' class='bg01' onclick=\"CalendarClick('" + varId + "')\"><p>" + curDay.getDate() + "</p> \n <ul class='text'> \n";
                }
                else {
                    MainList += "<td id = '" + varId + "_TD' onclick=\"CalendarClick('" + varId + "')\"><p>" + curDay.getDate() + "</p> \n <ul class='text'> \n";
                }

                // 데이터 표시
                MainList += "<li id = '" + varId + "_T'></li> \n";
                MainList += "</ul></td>";

                if (curDay.getDay() == 6) {//토요일이면 tr 종료
                    MainList += " </tr> \n ";
                }

                curDay.setDate(curDay.getDate() + 1);
            }

            MainList += "    </tr> \n";

            //끝날짜
            $("#MainContent_hidMonthLastDt").val(varId);

            var obj = document.getElementById("Calendar");
            obj.innerHTML = MainList;

            obj = document.getElementById("lblYearMonth");
            obj.innerHTML = intSelectYear.toString() + " . " + (intSelectMonth + 1).toString().padStart(2, '0');

            intCuYear = intSelectYear;
            intCuMonth = intSelectMonth;

            fn_WatingCall();

            //조회
            $("#MainContent_btnSearch2").click();
        }

        function CalendarClick(sDay) //달력 클릭
        {
            selDay = sDay;
            if (event.button != 0 && event.button != 1) {
                return;
            }
            chk_hour = "N";
            SelectCellCalendar(sDay);
        }

        function SelectCellCalendar(sDay) // 달력 Cell 클릭
        {
            // 달력 표시
            var date = new Date();
            var vMonth;
            var vDay;
            var today;

            if (date.getMonth() < 9)
                vMonth = "0" + (date.getMonth() + 1);
            else
                vMonth = date.getMonth() + 1;

            if (date.getDate() < 10)
                vDay = "0" + date.getDate();
            else
                vDay = date.getDate();

            today = date.getFullYear().toString() + vMonth.toString() + vDay;

            $("#" + sDay + "_TD").attr("class", "bg02");

            //클릭내용 기록 및 달력 표시 해제
            for (var i = 0; i < monthArr.length; i++) //cnt 넣기
            {
                if (monthArr[i][0] == sDay) {
                    monthArr[i][1] = 1;
                }
                else {
                    monthArr[i][1] = 0;
                    if (monthArr[i][0] == today)
                        $("#" + monthArr[i][0] + "_TD").attr("class", "bg01");
                    else
                        $("#" + monthArr[i][0] + "_TD").attr("class", "");
                }
            }
            
            $("#MainContent_hiddenSearchDt").val(sDay);
            $("#MainContent_btnModify").click();
            // 데이터 검색 시작
            //$("#MainContent_txtDate").val(sDay.substr(0, 4) + '-' + sDay.substr(4, 2) + '-' + sDay.substr(6, 2)) // 원본일 설정
            //$("#MainContent_hidSeldate").val(sDay); //생산일자 설정
            //$("#MainContent_btnSchInquiry").click();
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
    <asp:HiddenField ID="hidMonthLastDt" runat="server" />
    <asp:HiddenField ID="hidMonthStartDt" runat="server"  />
    <asp:HiddenField ID="hiddenSearchDt" runat="server"  />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Pln11</asp:Label></h3>
            <div class="al-r">
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
                <asp:Button ID="btnSearch2" runat="server" Text="FunctionCall" OnClick="btnSearch_Click"  style="display:none;"  />
                <asp:Button ID="btnModify" runat="server" Text="FunctionCall" OnClick="btnModify_Click"  style="display:none;"  />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtCondi1" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch2" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <div class="schedule_wrap" style="width:100%">
            
            <!--// Calendar -->
            <div class="contents_l" style="width:100%">
                <div class="calendar">
                    <ul class="top">
                        <li><button type="button" class="prev" onclick="fn_Prev_onclick();return false"></button></li>
                        <li id="lblYearMonth"></li>
                        <li><button type="button" class="next" onclick="fn_Next_onclick();return false"></button></li>
                    </ul>
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 100px;">
                            <col style="width: 100px;">
                            <col style="width: 100px;">
                            <col style="width: 100px;">
                            <col style="width: 100px;">
                            <col style="width: 100px;">
                            <col style="width: 100px;">
                        </colgroup>
                        <thead>
                            <tr>
                                <th>Sun</th>
                                <th>Mon</th>
                                <th>Tue</th>
                                <th>Wed</th>
                                <th>Thu</th>
                                <th>Fri</th>
                                <th>Sat</th>
                            </tr>
                        </thead>
                        <tbody id="Calendar">

                        </tbody>
                    </table>
                </div>
            </div>
    </div>

</asp:Content>