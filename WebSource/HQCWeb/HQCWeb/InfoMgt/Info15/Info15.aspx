<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Info15.aspx.cs" Inherits="HQCWeb.InfoMgt.Info15.Info15" %>

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

        function fn_Copy_Validation() {
            var date = new Date();
            var vMonth;
            var vDay;
            var NT;

            if (date.getMonth() < 9)
                vMonth = "0" + (date.getMonth() + 1);
            else
                vMonth = date.getMonth() + 1;

            if (date.getDate() < 10)
                vDay = "0" + date.getDate();
            else
                vDay = date.getDate();

            NT = date.getFullYear().toString() + vMonth.toString() + vDay;

            var DT = $("#MainContent_txtDate").val();
            var FM = $("#MainContent_txtFromDt").val();
            var TO = $("#MainContent_txtToDt").val();

            DT = DT.replace('-', '').replace('-', '');
            FM = FM.replace('-', '').replace('-', '');
            TO = TO.replace('-', '').replace('-', '');

            if (FM > TO) {
                alert('대상일 입력이 잘못되었습니다. 확인하세요.');
                $("#MainContent_txtFromDt").focus();
                return false;
            }

            if (NT > FM) {
                alert('과거 스케쥴은 변경할 수 없습니다.');
                $("#MainContent_txtFromDt").focus();
                return false;
            }

            if ($("#MainContent_txtShopCdHidden").val() == "") {
                alert("Shop Code를 선택하세요.");
                $("#MainContent_txtShopCd").focus();
                return false;
            } else if ($("#MainContent_txtLineCdHidden").val() == "") {
                alert("라인을 선택하세요.");
                $("#MainContent_txtLineCd").focus();
                return false;
            }

            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                $("#MainContent_ddlShopCd").focus();
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                $("#MainContent_ddlLineCd").focus();
                return false;
            }

            fn_WatingCall();
            return true;
        }

        function fn_Insert_Validation() {

            var headers = document.querySelectorAll('.hr h4');
            var checkCnt = 0;

            var pYear, pMonth, pDay;

            for (var i = 0; i < monthArr.length ; i++) {
                if (monthArr[i][1] == "1") {
                    pYear = monthArr[i][0].substr(0, 4);
                    pMonth = monthArr[i][0].substr(4, 2);
                    pDay = monthArr[i][0].substr(6, 2);
                }
            }

            //가동시간 확인(제외시간 미포함)
            headers.forEach(header => {
                if (header.className = 'use') {
                    checkCnt++;
                }
            });

            if (checkCnt < 1) {
                alert("스케줄에 선택된 가동시간이 없습니다.");
                return false;
            }

            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                $("#MainContent_ddlShopCd").focus();
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                $("#MainContent_ddlLineCd").focus();
                return false;
            } else {
                if (confirm(pYear + "년 " + pMonth + "월 " + pDay + "일 " + $("#MainContent_ddlShopCd").val() + " " + $("#MainContent_ddlLineCd").val() + " 스케쥴을 등록하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            fn_WatingCall();
            return true;
        }

        function fn_Delete_Validation() {
            
            var pYear, pMonth, pDay;

            for (var i = 0; i < monthArr.length ; i++) {
                if (monthArr[i][1] == "1") {
                    pYear = monthArr[i][0].substr(0, 4);
                    pMonth = monthArr[i][0].substr(4, 2);
                    pDay = monthArr[i][0].substr(6, 2);
                 }
            }

            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                $("#MainContent_ddlShopCd").focus();
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                $("#MainContent_ddlLineCd").focus();
                return false;
            } else {
                if (confirm(pYear + "년 " + pMonth + "월 " + pDay + "일 " + $("#MainContent_ddlShopCd").val() + " " + $("#MainContent_ddlLineCd").val() + " 스케쥴을 지우시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            fn_WatingCall();
            return true;
        }

        function fn_Register() {
            fn_OpenPop('팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, '팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var ptnData;
        var cShop, cLine;
        var colorData;
        var monthArr, selDay = "";
        var chk_hour = "N";
        var m_dSche = "";
        var intCuYear = 2024;
        var intCuMonth = 6;
        
        $(document).ready(function () {
            //패턴 관련
            settingGrid();

            //콤보박스 초기화
            fn_Set_Shop();
            fn_Set_Line();

            //달력 초기화 호출
            MakeCalendarInit();

            //패턴표 초기화 호출
            MakePtnColorInit();
        });

        //패턴그리드 출력
        function createGrid() {
            var hour, min;

            //selectedhour reset
            lbSelHour.innerHTML = '';
            cb1.checked = false;

            ptnData.forEach((row, i) => {
                row.forEach((value, j) => {
                    hour = (i > 0) ? i.toString().padStart(2, '0') : '24';
                    min = ((j + 1) * 5).toString().padStart(2, '0');

                    if (value === "0") {
                        document.getElementById('h' + hour + 'm' + min).className = '';
                    } else if (value === "1") {
                        document.getElementById('h' + hour + 'm' + min).className = 'use';
                    } else {
                        document.getElementById('h' + hour + 'm' + min).className = 'notime';
                        document.getElementById('h' + hour + 'm' + min).style = 'background:'+value;
                    }
                });
            });

            settingGrid();
        }

        //hidValue
        //분단위 체크
        function fn_Minute_Checked(str) {

            $.ajax({
                type: "POST",
                url: "Info15.aspx/GetPattern",
                data: JSON.stringify({ clicked: str }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                },
                error: function (response) {
                    alert("에러가 발생했습니다. 관리자에게 문의바립니다.");
                }
            });
        }


        //패턴로직 설정
        function settingGrid() {
            const items = document.querySelectorAll('.no-wrap li');
            const headers = document.querySelectorAll('.hr h4');
            const checkbox = document.getElementById('cb1');

            //분
            items.forEach(item => {
                // 기존 이벤트 제거
                item.removeEventListener('click', fn_item_Click);

                // 새로운 이벤트 추가
                item.addEventListener('click', fn_item_Click);
            });

            //시간
            headers.forEach(header => {
                header.addEventListener('click', function () {
                    headers.forEach(header => {
                        header.className = '';
                    });

                    this.className = 'chk';
                    var i = 0;
                    lbSelHour.innerHTML = this.innerHTML;
                    checkbox.checked = false;
                    items.forEach(item => {
                        if (document.getElementById('h' + this.innerHTML + item.id).className === 'use') {
                            document.getElementById(item.id).className = 'use2';
                            fn_Minute_Checked(this.innerHTML + item.id + '/1');
                            i++;
                        }
                        else if (document.getElementById('h' + this.innerHTML + item.id).className === '') {
                            document.getElementById(item.id).className = '';
                            fn_Minute_Checked(this.innerHTML + item.id + '/0');
                        }
                        else {
                            document.getElementById(item.id).className = 'notime2';
                            fn_Minute_Checked(this.innerHTML + item.id + '/0');
                        }
                    });

                    if (i == items.length)
                        checkbox.checked = true;

                });
            });

            // 기존 이벤트 제거
            checkbox.removeEventListener('click', fn_check_Click);

            // 새로운 이벤트 추가
            checkbox.addEventListener('click', fn_check_Click);
        }

        //아이템 클릭이벤트 함수
        function fn_item_Click() {
            const currentItem = document.getElementById(this.id);

            if (lbSelHour.innerHTML != '') {
                headers.forEach(header => {
                    if (header.className == 'chk') {
                        if (currentItem.className === '') {
                            currentItem.className = 'use2';
                            document.getElementById('h' + header.innerHTML + this.id).className = 'use';
                            fn_Minute_Checked(header.innerHTML + this.id + '/1');
                        }
                        else if (currentItem.className === 'use2') {
                            currentItem.className = '';
                            document.getElementById('h' + header.innerHTML + this.id).className = '';
                            fn_Minute_Checked(header.innerHTML + this.id + '/0');
                        }
                        else {
                            alert("제외시간은 패턴수정이 불가능합니다. 제외시간 관리에서 수정하세요.");
                        }
                    }
                });
            }
            else {
                alert("시간을 선택하세요.");
            }
        }

        //체크박스 클릭이벤트 함수
        function fn_check_Click() {
            const items = document.querySelectorAll('.no-wrap li');
            if (lbSelHour.innerHTML == '') {
                this.checked = !this.checked;
                alert("시간을 선택하세요.");
            }
            else if (this.checked) {
                items.forEach(item => {
                    if (document.getElementById(item.id).className === '') {
                        document.getElementById(item.id).className = 'use2';
                        document.getElementById('h' + lbSelHour.innerHTML + item.id).className = 'use';
                        //fn_Minute_Checked(lbSelHour.innerHTML + item.id + '/1');
                    }
                });
                fn_Minute_Checked(lbSelHour.innerHTML + 'm65/1');
            }
            else {
                items.forEach(item => {
                    if (document.getElementById(item.id).className === 'use2') {
                        document.getElementById(item.id).className = '';
                        document.getElementById('h' + lbSelHour.innerHTML + item.id).className = '';
                        //fn_Minute_Checked(lbSelHour.innerHTML + item.id + '/0');
                    }
                });
                fn_Minute_Checked(lbSelHour.innerHTML + 'm65/0');
            }
        }


        //Shop코드 재설정
        function fn_Set_Shop() {
            $("#MainContent_txtShopCd").comboTree({
                source: cShop,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtShopCdHidden",
                functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                functionCallFunc: "fn_Shop_Checked"
            });
        }

        //Shop코드 onchange
        function fn_Shop_Checked() {
            $("#MainContent_btnFunctionCall").click();
        }

        //부품코드 재설정
        function fn_Set_Line() {
            $("#MainContent_txtLineCd").comboTree({
                source: cLine,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtLineCdHidden"
            });
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
               else{
                   MainList += "<td id = '" + varId + "_TD' onclick=\"CalendarClick('" + varId + "')\"><p>" + curDay.getDate() + "</p> \n <ul class='text'> \n";
               }

                // 데이터 표시
               MainList += "<li id = '" + varId + "_BMA_T'></li> \n";
               MainList += "<li id = '" + varId + "_BMA_D'></li> \n";
               MainList += "<li id = '" + varId + "_BPA_T'></li> \n";
               MainList += "<li id = '" + varId + "_BPA_D'></li> \n";
               MainList += "<li id = '" + varId + "_BSA_T'></li> \n";
               MainList += "<li id = '" + varId + "_BSA_D'></li> \n";
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
            obj.innerHTML = intSelectYear.toString() + " . " + (intSelectMonth+1).toString().padStart(2, '0');

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
            
           //색상표 그리기
           function MakePtnColorInit() {

               var MainList = "<table> <tr> <td class=\"square_td\"> <div class=\"square\"></div> </td> <td class=\"name_td\"> 가동 </td>";
               var i = 1;


               colorData.forEach(item => {

                   if(i === 0)
                       MainList +=" <tr> ";

                   MainList += " <td class=\"square_td\"> <div class=\"square\" style =\"background-color:" + item.COLOR + "\"></div> </td> <td class=\"name_td\"> " + item.NOWORK_NM + " </td> ";

                   i++;
                   if(i === 5){
                       i = 0;
                       MainList += " </tr> ";
                   }
               });

               MainList += "</table> "; 

               var obj = document.getElementById("ColorDiv");
               obj.innerHTML = MainList;
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

            $("#"+sDay + "_TD").attr("class", "bg02");

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

            // 데이터 검색 시작
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            }
            else {
                $("#MainContent_txtDate").val(sDay.substr(0, 4) + '-' + sDay.substr(4, 2) + '-' + sDay.substr(6, 2)) // 원본일 설정
                $("#MainContent_hidSeldate").val(sDay); //생산일자 설정
                $("#MainContent_btnSchInquiry").click();
            }
        }

        function fn_Stop() {

            var aData = "";
            aData = "Info16";

            var jsonData = JSON.stringify({ sMenu: aData });

            $.ajax({
                type: "POST",
                url: "Info15.aspx/GetMenu",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_Stop_Open(msg.d);
                }
            });
        }

        function fn_Stop_Open(title) {
            if (title == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }
            parent.fn_Add('/InfoMgt/Info16/Info16.aspx', title, 'Info16', true);
        }

    </script>

    <style>
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 6%;
            position : absolute; 
        }

        .searchCombo2{
            text-align: center;
            z-index : 9998;
            top : 60%;
            position : absolute; 
        }
        /* 하단 제외시간 표시용 사각표(div) 초기값은 가동 */
        .square {
          height: 10px;
          width: 10px;
          background-color: #59a7b3;
        }

        .square_td {
            width: 15px;
        }
        
        .name_td {
            width: 100px;
        }

    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />
    <asp:HiddenField ID="hidMonthLastDt" runat="server" />
    <asp:HiddenField ID="hidMonthStartDt" runat="server"  />
    <asp:HiddenField ID="hidSeldate" runat="server"  />
    <asp:HiddenField ID="hidSchFlag" runat="server"  />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Info15</asp:Label></h3>
            <div class="al-r">
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
                <asp:Button ID="btnSearch2" runat="server" Text="FunctionCall" OnClick="btnSearch_Click"  style="display:none;"  />
                <asp:Button ID="btnSchInquiry" runat="server" Text="FunctionCall" OnClick="btnSchedSearch_Click"  style="display:none;"  />
            </div>
        </div>
        <!-- Title + Win_Btn //-->

        <div class="schedule_wrap">
            
            <!--// Calendar -->
            <div class="contents_l">
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
                            <%--<tr><td colspan="7" style="height:1px; background-color:#ffffff;"></td></tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td><p>1</p>
                                    <ul class="text">
                                        <li id="20240301_BMA_T">BMA</li>
                                        <li>(01,04,05,06,07,08)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07,08)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07,08)</li>
                                    </ul>
                                </td>
                                <td><p>2</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <td><p>3</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>4</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>5</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>6</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>7</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>8</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>9</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <td><p>10</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>11</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>12</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>13</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>14</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>15</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>16</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <td><p>17</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>18</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>19</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td class="bg01"><p>20</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td class="bg02"><p>21</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>22</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>23</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <td><p>24</p>
                                    <!--<ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>-->
                                </td>
                                <td><p>25</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>26</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>27</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>28</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>29</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                                <td><p>30</p>
                                    <ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <td><p>31</p>
                                    <!--<ul class="text">
                                        <li>BMA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BPA</li>
                                        <li>(01,04,05,06,07)</li>
                                        <li>BSA</li>
                                        <li>(01,04,05,06,07)</li>
                                    </ul>-->
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>--%>
                        </tbody>
                    </table>
                </div>
            </div>
            <!-- Calendar //-->

            <div class="contents_r ml30">
                
                <h1>스케줄 복사</h1>
                <div class="search_grid">
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 60px;">
                            <col style="width: 200px;">
                            <col style="width: 60px;">
                            <col style="width: 200px;">
                        </colgroup>
                        <tr>
                            <th><asp:Label ID="lbSourceDt" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox>
                                <asp:Button ID="btnGCopy" runat="server" Text="Copy" OnClick="btnCopy_Click" OnClientClick="javascript:return fn_Copy_Validation();" Visible="false" style="padding:0px" />
                            </td>
                            <th class="bdr"><asp:Label ID="lbTxtShopCd" runat="server"></asp:Label></th>
                            <td>
                                <div class="searchCombo" style="width:115px; font-size:12px;">
                                    <input type="text" ID="txtShopCd" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtShopCdHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbTargetDt" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtFromDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="IME-MODE:disabled; background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox> 
                                <span id="spBetween">~</span>
                                <asp:TextBox ID="txtToDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox>
                            </td>
                            <th class="bdr"><asp:Label ID="lbTxtLineCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="searchCombo2" style="width:250px; font-size:12px;">
                                            <input type="text" ID="txtLineCd" runat="server" style="background-color:white; color:black;" readonly/>
                                            <asp:TextBox id="txtLineCdHidden" runat="server" style="display:none;"></asp:TextBox>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <h1>스케줄 패턴 확인, 검색, 등록</h1>

                <div class="search_grid mt30">
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 60px;">
                            <col style="width: 100px;">
                            <col style="width: 60px;">
                            <col style="width: 200px;">
                            <col style="width: 60px;">
                            <col style="width: 200px;">
                        </colgroup>
                        <tr>
                            <th class="bdr"><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="100"></asp:DropDownList>
                            </td>
                            <th class="bdr"><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true" Width="200"></asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <th class="bdr"><asp:Label ID="lbPtnCd" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlPtnCd" runat="server"></asp:DropDownList>
                                        <asp:Button ID="btnSearch" runat="server" Text="Pattern Search" OnClick="btnPtnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" style="padding:0px" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="mt30 time-box">
                            <div class="hour">
                                <div class="hr">
                                        <h4>08</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h08m05" ></li>
                                                <li id="h08m10" ></li>
                                                <li id="h08m15" ></li>
                                                <li id="h08m20" ></li>
                                                <li id="h08m25" ></li>
                                                <li id="h08m30" ></li>
                                                <li id="h08m35" ></li>
                                                <li id="h08m40" ></li>
                                                <li id="h08m45" ></li>
                                                <li id="h08m50" ></li>
                                                <li id="h08m55" ></li>
                                                <li id="h08m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>09</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h09m05" ></li>
                                                <li id="h09m10" ></li>
                                                <li id="h09m15" ></li>
                                                <li id="h09m20" ></li>
                                                <li id="h09m25" ></li>
                                                <li id="h09m30" ></li>
                                                <li id="h09m35" ></li>
                                                <li id="h09m40" ></li>
                                                <li id="h09m45" ></li>
                                                <li id="h09m50" ></li>
                                                <li id="h09m55" ></li>
                                                <li id="h09m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>10</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h10m05" ></li>
                                                <li id="h10m10" ></li>
                                                <li id="h10m15" ></li>
                                                <li id="h10m20" ></li>
                                                <li id="h10m25" ></li>
                                                <li id="h10m30" ></li>
                                                <li id="h10m35" ></li>
                                                <li id="h10m40" ></li>
                                                <li id="h10m45" ></li>
                                                <li id="h10m50" ></li>
                                                <li id="h10m55" ></li>
                                                <li id="h10m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>11</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h11m05" ></li>
                                                <li id="h11m10" ></li>
                                                <li id="h11m15" ></li>
                                                <li id="h11m20" ></li>
                                                <li id="h11m25" ></li>
                                                <li id="h11m30" ></li>
                                                <li id="h11m35" ></li>
                                                <li id="h11m40" ></li>
                                                <li id="h11m45" ></li>
                                                <li id="h11m50" ></li>
                                                <li id="h11m55" ></li>
                                                <li id="h11m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>12</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h12m05" ></li>
                                                <li id="h12m10" ></li>
                                                <li id="h12m15" ></li>
                                                <li id="h12m20" ></li>
                                                <li id="h12m25" ></li>
                                                <li id="h12m30" ></li>
                                                <li id="h12m35" ></li>
                                                <li id="h12m40" ></li>
                                                <li id="h12m45" ></li>
                                                <li id="h12m50" ></li>
                                                <li id="h12m55" ></li>
                                                <li id="h12m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>13</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h13m05" ></li>
                                                <li id="h13m10" ></li>
                                                <li id="h13m15" ></li>
                                                <li id="h13m20" ></li>
                                                <li id="h13m25" ></li>
                                                <li id="h13m30" ></li>
                                                <li id="h13m35" ></li>
                                                <li id="h13m40" ></li>
                                                <li id="h13m45" ></li>
                                                <li id="h13m50" ></li>
                                                <li id="h13m55" ></li>
                                                <li id="h13m60" ></li>
                                            </ul>
                                        </div>
                                    </div>

                                    <div class="hr">
                                        <h4>14</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h14m05" ></li>
                                                <li id="h14m10" ></li>
                                                <li id="h14m15" ></li>
                                                <li id="h14m20" ></li>
                                                <li id="h14m25" ></li>
                                                <li id="h14m30" ></li>
                                                <li id="h14m35" ></li>
                                                <li id="h14m40" ></li>
                                                <li id="h14m45" ></li>
                                                <li id="h14m50" ></li>
                                                <li id="h14m55" ></li>
                                                <li id="h14m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>15</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h15m05" ></li>
                                                <li id="h15m10" ></li>
                                                <li id="h15m15" ></li>
                                                <li id="h15m20" ></li>
                                                <li id="h15m25" ></li>
                                                <li id="h15m30" ></li>
                                                <li id="h15m35" ></li>
                                                <li id="h15m40" ></li>
                                                <li id="h15m45" ></li>
                                                <li id="h15m50" ></li>
                                                <li id="h15m55" ></li>
                                                <li id="h15m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>16</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h16m05" ></li>
                                                <li id="h16m10" ></li>
                                                <li id="h16m15" ></li>
                                                <li id="h16m20" ></li>
                                                <li id="h16m25" ></li>
                                                <li id="h16m30" ></li>
                                                <li id="h16m35" ></li>
                                                <li id="h16m40" ></li>
                                                <li id="h16m45" ></li>
                                                <li id="h16m50" ></li>
                                                <li id="h16m55" ></li>
                                                <li id="h16m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>17</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h17m05" ></li>
                                                <li id="h17m10" ></li>
                                                <li id="h17m15" ></li>
                                                <li id="h17m20" ></li>
                                                <li id="h17m25" ></li>
                                                <li id="h17m30" ></li>
                                                <li id="h17m35" ></li>
                                                <li id="h17m40" ></li>
                                                <li id="h17m45" ></li>
                                                <li id="h17m50" ></li>
                                                <li id="h17m55" ></li>
                                                <li id="h17m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>18</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h18m05" ></li>
                                                <li id="h18m10" ></li>
                                                <li id="h18m15" ></li>
                                                <li id="h18m20" ></li>
                                                <li id="h18m25" ></li>
                                                <li id="h18m30" ></li>
                                                <li id="h18m35" ></li>
                                                <li id="h18m40" ></li>
                                                <li id="h18m45" ></li>
                                                <li id="h18m50" ></li>
                                                <li id="h18m55" ></li>
                                                <li id="h18m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>19</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h19m05" ></li>
                                                <li id="h19m10" ></li>
                                                <li id="h19m15" ></li>
                                                <li id="h19m20" ></li>
                                                <li id="h19m25" ></li>
                                                <li id="h19m30" ></li>
                                                <li id="h19m35" ></li>
                                                <li id="h19m40" ></li>
                                                <li id="h19m45" ></li>
                                                <li id="h19m50" ></li>
                                                <li id="h19m55" ></li>
                                                <li id="h19m60" ></li>
                                            </ul>
                                        </div>
                                    </div>

                                    <div class="hr">
                                        <h4>20</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h20m05" ></li>
                                                <li id="h20m10" ></li>
                                                <li id="h20m15" ></li>
                                                <li id="h20m20" ></li>
                                                <li id="h20m25" ></li>
                                                <li id="h20m30" ></li>
                                                <li id="h20m35" ></li>
                                                <li id="h20m40" ></li>
                                                <li id="h20m45" ></li>
                                                <li id="h20m50" ></li>
                                                <li id="h20m55" ></li>
                                                <li id="h20m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>21</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h21m05" ></li>
                                                <li id="h21m10" ></li>
                                                <li id="h21m15" ></li>
                                                <li id="h21m20" ></li>
                                                <li id="h21m25" ></li>
                                                <li id="h21m30" ></li>
                                                <li id="h21m35" ></li>
                                                <li id="h21m40" ></li>
                                                <li id="h21m45" ></li>
                                                <li id="h21m50" ></li>
                                                <li id="h21m55" ></li>
                                                <li id="h21m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>22</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h22m05" ></li>
                                                <li id="h22m10" ></li>
                                                <li id="h22m15" ></li>
                                                <li id="h22m20" ></li>
                                                <li id="h22m25" ></li>
                                                <li id="h22m30" ></li>
                                                <li id="h22m35" ></li>
                                                <li id="h22m40" ></li>
                                                <li id="h22m45" ></li>
                                                <li id="h22m50" ></li>
                                                <li id="h22m55" ></li>
                                                <li id="h22m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>23</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h23m05" ></li>
                                                <li id="h23m10" ></li>
                                                <li id="h23m15" ></li>
                                                <li id="h23m20" ></li>
                                                <li id="h23m25" ></li>
                                                <li id="h23m30" ></li>
                                                <li id="h23m35" ></li>
                                                <li id="h23m40" ></li>
                                                <li id="h23m45" ></li>
                                                <li id="h23m50" ></li>
                                                <li id="h23m55" ></li>
                                                <li id="h23m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>24</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h24m05" ></li>
                                                <li id="h24m10" ></li>
                                                <li id="h24m15" ></li>
                                                <li id="h24m20" ></li>
                                                <li id="h24m25" ></li>
                                                <li id="h24m30" ></li>
                                                <li id="h24m35" ></li>
                                                <li id="h24m40" ></li>
                                                <li id="h24m45" ></li>
                                                <li id="h24m50" ></li>
                                                <li id="h24m55" ></li>
                                                <li id="h24m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>01</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h01m05" ></li>
                                                <li id="h01m10" ></li>
                                                <li id="h01m15" ></li>
                                                <li id="h01m20" ></li>
                                                <li id="h01m25" ></li>
                                                <li id="h01m30" ></li>
                                                <li id="h01m35" ></li>
                                                <li id="h01m40" ></li>
                                                <li id="h01m45" ></li>
                                                <li id="h01m50" ></li>
                                                <li id="h01m55" ></li>
                                                <li id="h01m60" ></li>
                                            </ul>
                                        </div>
                                    </div>

                                    <div class="hr">
                                        <h4>02</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h02m05" ></li>
                                                <li id="h02m10" ></li>
                                                <li id="h02m15" ></li>
                                                <li id="h02m20" ></li>
                                                <li id="h02m25" ></li>
                                                <li id="h02m30" ></li>
                                                <li id="h02m35" ></li>
                                                <li id="h02m40" ></li>
                                                <li id="h02m45" ></li>
                                                <li id="h02m50" ></li>
                                                <li id="h02m55" ></li>
                                                <li id="h02m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>03</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h03m05" ></li>
                                                <li id="h03m10" ></li>
                                                <li id="h03m15" ></li>
                                                <li id="h03m20" ></li>
                                                <li id="h03m25" ></li>
                                                <li id="h03m30" ></li>
                                                <li id="h03m35" ></li>
                                                <li id="h03m40" ></li>
                                                <li id="h03m45" ></li>
                                                <li id="h03m50" ></li>
                                                <li id="h03m55" ></li>
                                                <li id="h03m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>04</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h04m05" ></li>
                                                <li id="h04m10" ></li>
                                                <li id="h04m15" ></li>
                                                <li id="h04m20" ></li>
                                                <li id="h04m25" ></li>
                                                <li id="h04m30" ></li>
                                                <li id="h04m35" ></li>
                                                <li id="h04m40" ></li>
                                                <li id="h04m45" ></li>
                                                <li id="h04m50" ></li>
                                                <li id="h04m55" ></li>
                                                <li id="h04m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>05</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h05m05" ></li>
                                                <li id="h05m10" ></li>
                                                <li id="h05m15" ></li>
                                                <li id="h05m20" ></li>
                                                <li id="h05m25" ></li>
                                                <li id="h05m30" ></li>
                                                <li id="h05m35" ></li>
                                                <li id="h05m40" ></li>
                                                <li id="h05m45" ></li>
                                                <li id="h05m50" ></li>
                                                <li id="h05m55" ></li>
                                                <li id="h05m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>06</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h06m05" ></li>
                                                <li id="h06m10" ></li>
                                                <li id="h06m15" ></li>
                                                <li id="h06m20" ></li>
                                                <li id="h06m25" ></li>
                                                <li id="h06m30" ></li>
                                                <li id="h06m35" ></li>
                                                <li id="h06m40" ></li>
                                                <li id="h06m45" ></li>
                                                <li id="h06m50" ></li>
                                                <li id="h06m55" ></li>
                                                <li id="h06m60" ></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="hr">
                                        <h4>07</h4>
                                        <div class="hr-wrap">
                                            <ul>
                                                <li id="h07m05" ></li>
                                                <li id="h07m10" ></li>
                                                <li id="h07m15" ></li>
                                                <li id="h07m20" ></li>
                                                <li id="h07m25" ></li>
                                                <li id="h07m30" ></li>
                                                <li id="h07m35" ></li>
                                                <li id="h07m40" ></li>
                                                <li id="h07m45" ></li>
                                                <li id="h07m50" ></li>
                                                <li id="h07m55" ></li>
                                                <li id="h07m60" ></li>
                                            </ul>
                                        </div>
                                    </div>

                                <div class="no">
                                    <h4>00</h4>
                                    <div class="no-wrap">
                                        <ul>
                                            <li id="m05" ></li>
                                            <li id="m10" ></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="no">
                                    <h4>10</h4>
                                    <div class="no-wrap">
                                        <ul>
                                            <li id="m15" ></li>
                                            <li id="m20" ></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="no">
                                    <h4>20</h4>
                                    <div class="no-wrap">
                                        <ul>
                                            <li id="m25" ></li>
                                            <li id="m30" ></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="no">
                                    <h4>30</h4>
                                    <div class="no-wrap">
                                        <ul>
                                            <li id="m35" ></li>
                                            <li id="m40" ></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="no">
                                    <h4>40</h4>
                                    <div class="no-wrap">
                                        <ul>
                                            <li id="m45" ></li>
                                            <li id="m50" ></li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="no">
                                    <h4>50</h4>
                                    <div class="no-wrap">
                                        <ul>
                                            <li id="m55" ></li>
                                            <li id="m60" ></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch2" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSchInquiry" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGCopy" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <div class="bottom mt20">
                    <p class="float-l">Selected Hour :<label id="lbSelHour"></label> </p>
                    <p class="float-r">전체선택<input type="checkbox" id="cb1" class="ml10"><label for="cb1"></label>
                    <input type="button" value="Set Stop" onclick="javascript: fn_Stop();" ID="btnStop" runat="server" visible="true"/>
                    <asp:Button ID="btnSave"   runat="server" Text="Save"   OnClick="btnSave_Click"   OnClientClick="javascript:return fn_Insert_Validation();" Visible="false" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" OnClientClick="javascript:return fn_Delete_Validation();" Visible="false" />
                </div>
                <div class="bottom mt20" id="ColorDiv" style="padding-left:10px">
                    <%--<table>
                        <tr>
                            <td class="square_td">
                                <div class="square"></div> 
                            </td>
                            <td>
                                가동
                            </td>
                        </tr>
                        <tr>
                            <td>

                            </td>
                        </tr>

                    </table>--%>
                    <%--<img src="../../img/New/wct_Nowork_Color.png"  style="height: 60px; width:400px">--%>
                </div>
            </div>
        </div>
    </div>

</asp:Content>