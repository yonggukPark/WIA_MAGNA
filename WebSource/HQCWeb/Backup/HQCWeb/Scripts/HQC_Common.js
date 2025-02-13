/** Javascript에서 사용할 메시지 정의 */
var calendarConfig = {
		 
	buttonImage: "/application/db/jquery/images/calendar.gif", // 버튼 이미지
		  
    changeMonth: true, // 월을 바꿀수 있는 셀렉트 박스를 표시한다.

	changeYear: true, // 년을 바꿀 수 있는 셀렉트 박스를 표시한다.
			
	yearRange: 'c-5:c+5',
		
	closeText: '닫기',  // 닫기 버튼 패널
		
	nextText: '다음 달', // next 아이콘의 툴팁.

    prevText: '이전 달', // prev 아이콘의 툴팁.

	showButtonPanel: true, // 캘린더 하단에 버튼 패널을 표시한다. 

	currentText: '오늘 날짜' , // 오늘 날짜로 이동하는 버튼 패널  

	dateFormat: "yy-mm-dd", // 텍스트 필드에 입력되는 날짜 형식.

	dayNamesMin: ['월', '화', '수', '목', '금', '토', '일'], // 요일의 한글 형식.

	monthNamesShort: ['1월','2월','3월','4월','5월','6월','7월','8월','9월','10월','11월','12월'] // 월의 한글 형식.
};

/* 레이어 팝업 오픈 */
function showPopup(popId){
var pop = document.getElementById(popId);
	if(document.getElementById(popId).style.display=="none"){
		pop.style.display="block";
	}else{
		pop.style.display="none";		
	}
}

// 숫자에 3자리마다 콤마찍기(현금표시)
function Format_comma(val1){
 var newValue = val1+""; //숫자를 문자열로 변환
 var len = newValue.length;  
 var ch="";
 var j=1;
 var formatValue="";
 
 // 콤마제거  
 newValue = newValue.replace(/\,/gi, ' ');
 
 // comma제거된 문자열 길이
 len = newValue.length;
 
 for(i=len ; i>0 ; i--){
  ch = newValue.substring(i-1,i);
  formatValue = ch + formatValue;
  if ((j%3) == 0 && i>1 ){
   formatValue=","+formatValue;
  }
  j++
 }
 return formatValue;
}

// 콤마제거
function Format_NoComma(val1){
 return (val1+"").replace(/\,/gi, '');
}

//영문/숫자/특수문자 조합 여부 체크 
function checkPass(str){
	//var reg1 = /^[a-zA-Z0-9]{4,20}$/;
	var reg1 = /[a-zA-Z]/g;
	var reg2 = /[0-9]/g;
    var reg3 = /[\{\}\[\]\/?.,;:|\)*~`!^\-_+<>@\#$%&\\\=\(\'\"]/gi;
    var reg4 = /[`~!@#$%^&*|\\\'\";:\/?]/gi; // 특수문자

    var bChk = true;

    if (!reg1.test(str)) {
        //console.log("reg1=", reg1.test(str));
        bChk = false;
    }

    if (!reg2.test(str)) {
        //console.log("reg2=", reg2.test(str));
        bChk = false;
    }

    if (!reg3.test(str)) {
        //console.log("reg3=", reg3.test(str));
        bChk = false;
    }

    if (!reg4.test(str)) {
        //console.log("reg4=", reg4.test(str));
        bChk = false;
    }

    return bChk;
    
    //return (reg1.test(str) && reg2.test(str) && reg3.test(str) && !reg4.test(str) && !(reg5.test(str)));
    //return (reg1.test(str) && reg2.test(str) && reg3.test(str) && reg4.test(str));
}

//영문 또는 숫자 여부 체크 
function checkIdVerify(str){
	var reg1 = /^[a-zA-Z0-9]{4,20}$/;
	var reg2 = /[a-zA-Z]/g;
	var reg3 = /[0-9]/g;
	var reg4 = /[\{\}\[\]\/?.,;:|\)*~`!^\-_+<>@\#$%&\\\=\(\'\"]/gi;

	return ((reg1.test(str) && !reg4.test(str)) || (reg2.test(str) && reg3.test(str) && !reg4.test(str)));
}

// 이메일 주소 체크
function isValidEmail(email_address)  
{  
    // 이메일 주소를 판별하기 위한 정규식  
    var format = /^[_0-9a-zA-Z-]+(\.[_0-9a-zA-Z-]+)*@[0-9a-zA-Z-]+(\.[0-9a-zA-Z-]+)*$/;  
      
    // 인자 email_address를 정규식 format 으로 검색  
    if (email_address.search(format) != -1)  
    {  
        // 정규식과 일치하는 문자가 있으면 true  
        return true;  
    }  
    else  
    {  
        // 없으면 false  
        return false;  
    }  
} 

function strCutLen(str, len) { //문자열 자르기(byte) ..
    var inc = 0;
    var nbytes = 0;
    var msg = "";
    var strLen = str.length;
    for (i = 0; i < strLen ; i++) {
        var ch = str.charAt(i);
        if (escape(ch).length > 4) {
            inc = 2;
        } else if (ch != '\r') {
            inc = 1;
        }
        if ((nbytes + inc) > len) {
            break;
        }
        nbytes += inc;
        msg += ch;
    }
    if (strLen > len) {
   		 msg += msg + '..';
    } 
    return msg;
}



var popUpObj;

// 등록 모달창 호출
function fn_OpenPop(url, width, height) {

    parent.fn_ModalOpenDiv(url, width, height);

 }

 // 수정 삭제 모달창 호출
function fn_PostOpenPop(_val, url, width, height) {

    parent.fn_ModalPostOpenDiv(_val, url, width, height);
   
 }

 // 메인페이지 이동
function fn_home() {
    location.href = "/Main.aspx";
}

// MDI 탭 추가
function fn_Add(_url, _title, _MenuID) {
    var _ShowLi = 0;

    var _height = 0;

    _height = "875px";

    _ShowLi = $("#ul_tabs li").length + 1;

    var _iframe = $("#divIframe").contents().find("iframe").is("#" + _MenuID);

    $("#lnb").find("li").find("a").attr("class", "");

    if (_ShowLi < 9) {

        if (_iframe == false) {

            $("#txtMenuID").val(_MenuID);

            $("#btnAccessLog").click();

            $("#ul_tabs").append("<li class='tab' id='li_tab_" + _MenuID + "'><p onclick=\"javascript:fn_tabChange('" + _MenuID + "');\">" + _title + "</p><button onclick=\"javascript:return fn_Del('" + _MenuID + "');\"></button></li>");

            $("#divIframe").append("<div class='contents_chk' id='div_tab_" + _MenuID + "' ><iframe id='" + _MenuID + "' src='" + _url + "' frameBorder='0' scrolling='no' style='width:100%; height:" + _height + "; overflow:hidden;' ></iframe></div>");

            fn_tabChange(_MenuID);
        }
        else {
            fn_tabChange(_MenuID);
        }
    }
    else {

        if (_iframe == false) {

            $("#txtMenuID").val(_MenuID);

            $("#btnAccessLog").click();

            $("#ul_tabs").find("li").eq(0).remove();

            $("#divIframe").find("div").eq(0).remove();

            $("#ul_tabs").append("<li class='tab' id='li_tab_" + _MenuID + "'><p onclick=\"javascript:fn_tabChange('" + _MenuID + "');\">" + _title + "</p><button onclick=\"javascript:return fn_Del('" + _MenuID + "');\"></button></li>");

            $("#divIframe").append("<div class='contents_chk' id='div_tab_" + _MenuID + "' ><iframe id='" + _MenuID + "' src='" + _url + "' frameBorder='0' scrolling='no' style='width:100%; height:" + _height + "; overflow:hidden;' ></iframe></div>");

            fn_tabChange(_MenuID);
        }
        else {
            fn_tabChange(_MenuID);
        }
    }
}

// MDI 탭 삭제
function fn_Del(_val) {

    // 등록,수정 팝업창을 호출하지 않았을때만 실행
    if ($("#modal").css("display") == "none") {

        $("#li_tab_" + _val).remove();

        $("#div_tab_" + _val).remove();

        $("#a" + _val).attr("class", "");

        $("#ul_tabs").find("li").attr("class", "tab");

        $("#ul_tabs").find("li").eq(0).attr("class", "tab active");

        $("#divIframe").find(".contents_chk").eq(0).attr("style", "display:block;");

        return true;

    } else {
        
        return false;
    }

    return;
}

// MDI 탭 변경
function fn_tabChange(_val) {
    // 등록,수정 팝업창을 호출하지 않았을때만 실행
    if ($("#modal").css("display") == "none") {

        $("#ul_tabs").find("li").attr("class", "tab");

        $("#li_tab_" + _val).attr("class", "tab active");

        $("#a" + _val).attr("class", "active");

        $("#divIframe").find(".contents_chk").attr("style", "display:none;");

        $("#div_tab_" + _val).attr("style", "display:block;");
    }     
}


// 모달창 호출
function fn_ModalOpenDiv(_url, width, height) {
    $("#modal").show();

    $("#ifrPopup").attr("style", "width:" + width + "px;height:" + height + "px;");

    $("#modal").attr("style", "width:" + width + "px;height:" + height + "px;");

    $("#modal_bg").show();

    $('#ifrPopup').attr('src', _url);
}

// 모달창 POST값 전달
function fn_ModalPostOpenDiv(val, _url, width, height) {
    //$("#divTabs").off('click');

    $("#modal").show();

    $("#modal_bg").show();

    $("#ifrPopup").attr("style", "width:" + width + "px;height:" + height + "px;");

    $("#modal").attr("style", "width:" + width + "px;height:" + height + "px;");

    $("#hidValue").val(val);

    $('#ifrPopup').attr('src', _url);

    document.form2.action = _url;
    document.form2.target = "ifrPopup";
    document.form2.method = "post";
    document.form2.submit();
}


function fn_PopupPostOpenPop(_val, url, width, height) {
    //값 셋팅
    $("#hidPopupValue").val(_val);

    //console.log("hidValue=", _val);
    //console.log("url=", url);
    //console.log("hidPopupValue=", $("#hidPopupValue").val());

    var width = width;
    var height = height;
    var top = (window.screen.height - height) / 2;
    var left = (window.screen.width - width) / 2;
    var status = "toolbar=no,directories=no,scrollbars=no,resizable=no,status=no,menubar=no, width=" + width + ",height=" + height + ",top=" + top + ",left=" + left;

    window.open("", "pop2", status);

    document.form2.action = url;
    document.form2.target = "pop2";
    document.form2.method = "post";
    document.form2.submit();
}


// 모달창 닫기
function fn_ModalCloseDiv() {
    $("#modal").hide();

    $("#modal_bg").hide();
}

// 특정 iframe Reload 호출
function fn_ModalReloadCall(ID) {
    $("#" + ID).get(0).contentWindow.fn_parentReload();
}


function onlyNumber(event) {
    event = event || window.event;

    var keyID = (event.which) ? event.which : event.keyCode;

    if ((keyID >= 48 && keyID <= 57) || (keyID >= 96 && keyID <= 105) || keyID == 8 || keyID == 46 || keyID == 37 || keyID == 39)
        return;
    else
        return false;
}

function removeChar(event) {
    event = event || window.event;

    var keyID = (event.which) ? event.which : event.keyCode;

    if (keyID == 8 || keyID == 46 || keyID == 37 || keyID == 39)
        return;
    else
        //event.target.value = event.target.value.replace(/[^0-9]/g, "");
        event.target.value = event.target.value.replace(/[\ㄱ-ㅎㅏ-ㅣ가-힣]/g, '');
}

function fn_EventKeyChk(event) {
    if ((event.keyCode >= 48 && event.keyCode <= 57)
        || event.keyCode == 8
        || event.keyCode == 37 || event.keyCode == 39
        || event.keyCode == 46
        || event.keyCode == 39) {

        return false;
    }
}