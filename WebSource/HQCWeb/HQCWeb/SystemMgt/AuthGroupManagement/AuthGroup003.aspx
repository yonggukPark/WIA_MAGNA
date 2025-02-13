<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="AuthGroup003.aspx.cs" Inherits="HQCWeb.SystemMgt.AuthGroupManagement.AuthGroup003" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_chkAll() {

            var chkIDs = "";

            //console.log($("input:checkbox[name='menuChk']").length);

            if ($('#menuChkAll').is(':checked')) {
                $("input:checkbox[name='menuChk']").prop("checked", true);
            
                $("input:checkbox[name='menuChk']:checked").each(function () {
                    var chk = $(this)[0].id;

                    if (chkIDs == "") {
                        chkIDs = $(this)[0].id;
                    } else {
                        chkIDs += "," + $(this)[0].id;
                    }
                })

                fn_menuInfoAllCall(chkIDs);

            } else {
                $("input:checkbox[name='menuChk']").prop("checked", false);

                $("input:checkbox[name='menuChk']").each(function () {
                    var chk = $(this)[0].id;

                    $("#tr" + chk).remove();
                })
            }
        }

        function fn_menuInfoAllCall(val) {
            var aData = "";

            aData = val;

            var jsonData = JSON.stringify({ sParams: aData });
            
            $.ajax({
                type: "POST",
                url: "AuthGroup003.aspx/SetAllMenuButtonList",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_trAllAdd(msg.d);
                }
            });          
        }
        
        function fn_trAllAdd(_vals) {
            if (_vals == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }

            var iChk = 1;
            var arrInfo = "";
            var arrMenuInfo = "";

            var arrBtnInfo = "";

            arrMenuInfo = _vals.split('^');            

            var _ValText = "";

            for (var j = 0; j < arrMenuInfo.length; j++) {
                
                arrInfo = arrMenuInfo[j].split(',');

                var _menuID = arrInfo[arrInfo.length - 1];

                //console.log("test1=", $("#tr" + _menuID).length);
                //console.log("test2=", $("#MainContent_tr" + _menuID).length);
                                
                if ($("#tr" + _menuID).length > 0 || $("#MainContent_tr" + _menuID).length  > 0) {
                    iChk = 0;
                    //alert("이미 권한부여된 메뉴입니다. 다시 선택하세요.");
                } else {
                    iChk = 1;
                }
                
                var _YNValue = "";

                if (iChk == 1) {
                    _ValText += "<tr style=\"height=30px;\" id='tr" + _menuID + "'>";
                    _ValText += "   <td></td>";
                    _ValText += "   <td style=\"padding:0px;\"><img src=\"/images/btn/close_on.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_rowDel('tr" + _menuID + "');\" /></td>";
                    _ValText += "   <td style=\"padding:0px;text-align:left;\">" + arrInfo[0] + "</td>";

                    for (var i = 1; i < arrInfo.length - 1; i++) {

                        arrBtnInfo = arrInfo[i].split('/');

                        if (arrBtnInfo[1] == "Y") {
                            _ValText += "   <td style=\"padding:0px;\"><input type='checkbox' id='" + arrBtnInfo[0] + "' value='" + arrBtnInfo[0] + "' checked='checked' /></td>";
                        } else {
                            _ValText += "   <td style=\"padding:0px;\"><input type='checkbox' id='" + arrBtnInfo[0] + "' value='" + arrBtnInfo[0] + "' disabled /></td>";
                        }

                        if (arrBtnInfo.length > 1) {
                            if (_YNValue == "") {
                                _YNValue = arrBtnInfo[1];
                            }
                            else {
                                _YNValue += "," + arrBtnInfo[1];
                            }
                        }
                    }
                    _ValText += "   <td style=\"padding:0px;\">" + _YNValue + "</td>";
                    _ValText += "   <td style=\"padding:0px;\">N</td>";

                    _ValText += "</tr> ";
                }
            }
            
            $("#MainContent_tbMenuButtonList > tbody:last").append(_ValText);
        }

        function fn_menuInfoCall(val) {
            if ($("#" + val.id).is(':checked')) {
                var aData = "";

                aData = $(val).val();

                var jsonData = JSON.stringify({ sParams: aData });
                
                $.ajax({
                    type: "POST",
                    url: "AuthGroup003.aspx/SetMenuButtonList",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        fn_trAdd(msg.d);
                    }
                });
                
            } else {
                $("#tr" + val.id).remove();
            }
        }

        function fn_trAdd(_vals) {
            if (_vals == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }

            var iChk = 1;
            var arrInfo = "";
            var arrBtnInfo = "";

            arrInfo = _vals.split(',');

            var _menuID = arrInfo[arrInfo.length - 1];
            
            //console.log("test1=", $("#tr" + _menuID).length);
            //console.log("test2=", $("#MainContent_tr" + _menuID).length);

            if ($("#tr" + _menuID).length > 0 || $("#MainContent_tr" + _menuID).length > 0) {

                iChk = 0;

                alert("이미 권한부여된 메뉴입니다. 다시 선택하세요.");

                $('#' + _menuID).prop('checked', false);
            }

            var _YNValue = "";

            if (iChk == 1) { 
                var _ValText = "";
                _ValText += "<tr style=\"height=30px;\" id='tr" + _menuID + "'>";
                _ValText += "   <td></td>";
                _ValText += "   <td style=\"padding:0px;\"><img src=\"/images/btn/close_on.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_rowDel('tr" + _menuID + "');\" /></td>";
                _ValText += "   <td style=\"padding:0px;\">" + arrInfo[0] + "</td>";

                for (var i = 1; i < arrInfo.length - 1; i++) {

                    arrBtnInfo = arrInfo[i].split('/');
                    
                    if (arrBtnInfo[1] == "Y") {
                        _ValText += "   <td style=\"padding:0px;\"><input type='checkbox' value='" + arrBtnInfo[0] + "' checked='checked' /></td>";
                    } else {
                        _ValText += "   <td style=\"padding:0px;\"><input type='checkbox' value='" + arrBtnInfo[0] + "' disabled /></td>";
                    }
                    
                    if (arrBtnInfo.length > 1) {
                        if (_YNValue == "") {
                            _YNValue = arrBtnInfo[1];
                        }
                        else {
                            _YNValue += "," + arrBtnInfo[1];
                        }
                    }
                }
                _ValText += "   <td style=\"padding:0px;\">" + _YNValue + "</td>";
                _ValText += "   <td style=\"padding:0px;\">N</td>";

                _ValText += "</tr> ";
                $("#MainContent_tbMenuButtonList > tbody:last").append(_ValText);
            }
        }

        function fn_rowDel(_id) {
            $("#" + _id).remove();

            $('#' + _id.replace('tr','')).prop('checked', false);
        }

        function fn_Validation() {

            var _insertInfo = "";
            var _statusInfo = "";
            var _rowCnt = $("#MainContent_tbMenuButtonList tbody tr").length;
            var _cellCnt = $("#MainContent_tbMenuButtonList tbody tr:eq(0) th").length;

            var _prevData = "";
            var _changeData = "";
            var _AuthType = "";

            if (_rowCnt == 1) {
                alert("권한 그룹에 매칭할 메뉴를 선택해주세요.");
                return false;
            }

            for (var i = 1; i < _rowCnt; i++) {
                var _tr = $("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ")");

                _AuthType = $("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") > td:eq(" + (_cellCnt - 1) + ")").html();

                // 신규 생성되는 권한이라면 무조건 등록처리
                if (_AuthType == "N") {

                    if (i > 1) {
                        if (_insertInfo != "") {
                            _insertInfo += "/";
                        }
                    }

                    _insertInfo += _tr[0].id.replace('tr','') + ",";

                    for (var j = 3; j < _cellCnt - 2; j++) {
                        var _YN = "";

                        if ($("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") >td:eq(" + j + ")").find('input[type="checkbox"]').is(':checked')) {
                            _YN = $("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") >td:eq(" + j + ")").find('input[type="checkbox"]').val();
                            _insertInfo += _YN + ",";
                        }

                        if (j == (_cellCnt - 3)) {
                            _insertInfo = _insertInfo.substring(0, (_insertInfo.length) - 1)
                        }
                    }
                }

                // 기존에 등록된 권한이라면 상태값이 변경된것을 확인하여 수정 처리
                if (_AuthType == "A") {
                    // 데이터 변경 이력 조회
                    for (var j = 3; j < _cellCnt - 2; j++) {
                        var _YN = "";

                        if ($("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") >td:eq(" + j + ")").find('input[type="checkbox"]').is(':checked')) {
                            _YN = $("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") >td:eq(" + j + ")").find('input[type="checkbox"]').val();
                            _statusInfo += "Y,";
                        }
                        else {
                            _statusInfo += "N,";
                        }

                        if (j == (_cellCnt - 3)) {
                            _statusInfo = _statusInfo.substring(0, (_statusInfo.length) - 1)
                        }
                    }

                    _prevData = $("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") > td:eq(" + (_cellCnt - 2) + ")").html();
                    _changeData = _statusInfo;
                    
                    // 비교 데이터 변수 초기화
                    _statusInfo = "";

                    // 데이터가 변경된 이력이 있는것만 저장
                    if (_prevData != _changeData) {

                        if (i > 1) {
                            if (_insertInfo != "") {
                                _insertInfo += "/";
                            }
                        }

                        _insertInfo += _tr[0].id.replace('MainContent_tr', '').replace('tr', '') + ",";

                        for (var j = 3; j < _cellCnt - 2; j++) {
                            var _YN = "";

                            if (!$("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") >td:eq(" + j + ")").find('input[type="checkbox"]').is(':disabled')) {

                                if ($("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") >td:eq(" + j + ")").find('input[type="checkbox"]').is(':checked')) {
                                    _YN = $("#MainContent_tbMenuButtonList tbody tr:eq(" + i + ") >td:eq(" + j + ")").find('input[type="checkbox"]').val();
                                    _insertInfo += _YN + ",";
                                }

                            }

                            if (j == (_cellCnt - 3)) {
                                _insertInfo = _insertInfo.substring(0, (_insertInfo.length) - 1)
                            }
                        }
                    }
                }
            }

            //console.log("_insertInfo=", _insertInfo);

            //$("#MainContent_txtInsertInfo").val(_insertInfo);

            if (_insertInfo == "") {
                alert("등록할 권한 정보가 없습니다. 권한 정보를 설정후 등록해주세요.");
                return false;
            } else {
                if (confirm("등록 하시겠습니까?")) {
                    fn_WatingCall();

                    var aAuthInfo = _insertInfo;
                    var aAuthGroupName = $("#MainContent_lbOrgAuthGroup").html();

                    //console.log("aAuthInfo=", aAuthInfo);
                    //console.log("aAuthGroupName=", aAuthGroupName);

                    var jsonData = JSON.stringify({ sParams: aAuthInfo, sParams2: aAuthGroupName });

                    $.ajax({
                        type: "POST",
                        url: "AuthGroup003.aspx/SetAuthGroupButtonInfoAdd",
                        data: jsonData,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            fn_AddRtn(msg.d);
                        }
                    });
                } else {
                    return false;
                }
            }

            return true;
        }

        function fn_Save() {
            fn_Validation();
        }

        function fn_AddRtn(_val) {
            if (_val == "N") {
                fn_AddNoInfo();
            }

            if (_val == "C") {
                fn_AddComplete();
            }

            if (_val == "E") {
                fn_AddError();
            }
            
            fn_loadingEnd();
        }

        function fn_AddNoInfo() { 
            alert("권한정보가 올바르지 않습니다. 관리자에게 문의바랍니다."); parent.fn_ModalCloseDiv();
        }

        function fn_AddComplete() {
            alert("정상등록 되었습니다."); parent.fn_ModalCloseDiv();
        }

        function fn_AddError() {
            alert("등록에 실패하였습니다. 관리자에게 문의바립니다.");
        }


        function fn_MenuChkDisabled(_val) {
            var arrMenus = _val.split(',');

            for (var i = 0; i < arrMenus.length; i++) {

                $("#" + arrMenus[i]).prop("checked", true);

                $("#" + arrMenus[i]).attr('disabled', true);
            }
        }


        function fn_AuthMenuDel(_val) {

            console.log("fn_AuthMenuDel=", _val);

            if (confirm("권한을 삭제 하시겠습니까?")) {
                fn_WatingCall();
                
                var aAuthGroup = $("#MainContent_lbOrgAuthGroup").html();
                var aAuthMenu = _val;

                var jsonData = JSON.stringify({ sParams: aAuthGroup, sParams2: aAuthMenu });

                $.ajax({
                    type: "POST",
                    url: "AuthGroup003.aspx/SetAuthGroupMenuDel",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        fn_DelRtn(msg.d);
                    }
                });

            } else {
                return false;
            }
        }

        function fn_DelRtn(_val) {

            var arrRtnInfo = _val.split(',');

            if (arrRtnInfo[0] == "C") {
                fn_DelComplete(arrRtnInfo[1]);
            }

            if (_val == "E") {
                fn_DelError();
            }

            fn_loadingEnd();
        }


        function fn_DelComplete(_val) {
            alert("정상삭제 되었습니다.");
            
            $("#" + _val).prop("checked", false);
            
            $("#" + _val).removeAttr('disabled');

            $("#tr" + _val).remove();

        }

        function fn_DelError() {
            alert("삭제에 실패하였습니다. 관리자에게 문의바립니다.");
        }

        function fn_Search() {
            var aAuthGroupNM    = $("#MainContent_lbOrgAuthGroup").html();
            var aMenuNM         = $("#MainContent_txtMenuNM").val();

            var jsonData = JSON.stringify({ sParams: aAuthGroupNM, sParams2: aMenuNM });

            $.ajax({
                type: "POST",
                url: "AuthGroup003.aspx/GetMenuSearch",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_SearchRtn(msg.d);
                }
            });
        }

        function fn_SearchRtn(_vals) {
            if (_vals == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }

            // /*  
            var iChk = 1;
            var arrInfo = "";
            var arrMenuInfo = "";

            var arrBtnInfo = "";

            console.log("_vals=", _vals);

            arrMenuInfo = _vals.split('^');

            ///
            var _rowCnt = $("#MainContent_tbMenuButtonList tbody tr").length;
            
            for (var i = _rowCnt; i >= 1; i--) {
                $("#MainContent_tbMenuButtonList tbody tr").eq(i).remove();
            }

            var _ValText = "";

            var _Disables = "";

            if (_vals != "") {
                for (var j = 0; j < arrMenuInfo.length; j++) {

                    arrInfo = arrMenuInfo[j].split(',');

                    var _menuID = arrInfo[arrInfo.length - 1];

                    var _arrMenuInfo = _menuID.split('||');

                    var _YNValue = "";

                    _ValText += "<tr style=\"height=30px;\" id='tr" + _arrMenuInfo[1] + "'>";
                    _ValText += "   <td></td>";
                    _ValText += "   <td style=\"padding:0px;\"><img src=\"/images/btn/close_off.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_AuthMenuDel('" + _arrMenuInfo[0] + "');\" /></td>";
                    _ValText += "   <td style=\"padding:0px;text-align:left;\">" + arrInfo[0] + "</td>";


                    if (_Disables == "") {
                        _Disables = _arrMenuInfo[1];
                    } else {
                        _Disables += "," + _arrMenuInfo[1];
                    }

                    //console.log("arrInfo=", arrInfo);

                    for (var i = 1; i < arrInfo.length - 1; i++) {

                        arrBtnInfo = arrInfo[i].split('/');

                        if (arrBtnInfo[2] == "N") {

                            _ValText += "   <td style=\"padding:0px;\"><input type='checkbox' id='" + arrBtnInfo[0] + "' value='" + arrBtnInfo[0] + "' disabled /></td>";

                        } else {
                            if (arrBtnInfo[1] == "Y") {
                                _ValText += "   <td style=\"padding:0px;\"><input type='checkbox' id='" + arrBtnInfo[0] + "' value='" + arrBtnInfo[0] + "' checked='checked' /></td>";
                            }
                            else {
                                _ValText += "   <td style=\"padding:0px;\"><input type='checkbox' id='" + arrBtnInfo[0] + "' value='" + arrBtnInfo[0] + "' /></td>";
                            }
                        }

                        if (arrBtnInfo.length > 1) {
                            if (_YNValue == "") {
                                _YNValue = arrBtnInfo[1];
                            }
                            else {
                                _YNValue += "," + arrBtnInfo[1];
                            }
                        }
                    }
                    _ValText += "   <td style=\"padding:0px; display:none;\">" + _YNValue + "</td>";
                    _ValText += "   <td style=\"padding:0px; display:none;\">N</td>";

                    _ValText += "</tr> ";

                }

                console.log("_ValText=", _ValText);

                $("#MainContent_tbMenuButtonList > tbody:last").append(_ValText);

                //console.log("_Disables=",_Disables);

                fn_MenuChkDisabled(_Disables);
            }

            // */
        }


        function fn_AuthGroupMenu_Copy() {
            var _rowCnt = $("#MainContent_tbMenuButtonList tbody tr").length;
            var _targetMenu = $("#MainContent_ddlAuthGroup").val();
            
            if (_targetMenu == "") {
                alert("복사할 대상 그룹을 선택하세요.");
                return false;
            }

            if (_rowCnt == 1) {
                alert("복사할 메뉴가 없습니다. 정보를 확인해주세요.");
                return false;
            }

            if (confirm("권한을 복사 하시겠습니까?")) {
                fn_WatingCall();

                var aOrgAuthGroup = $("#MainContent_lbOrgAuthGroup").html();
                var aTargetAuthGroup = $("#MainContent_ddlAuthGroup").val();;

                var jsonData = JSON.stringify({ sParams: aOrgAuthGroup, sParams2: aTargetAuthGroup });

                $.ajax({
                    type: "POST",
                    url: "AuthGroup003.aspx/SetAuthGroupMenuCopy",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        fn_CopyRtn(msg.d);
                    }
                });

            } else {
                return false;
            }
        }

        function fn_CopyRtn(_val) {
            if (_val == "C") {
                fn_CopyComplete();
            }

            if (_val == "E") {
                fn_CopyError();
            }

            fn_loadingEnd();
        }

        function fn_CopyComplete(_val) {
            alert("복사가 완료 되었습니다.");
        }

        function fn_CopyError() {
            alert("복사에 실패하였습니다. 관리자에게 문의바립니다.");
        }

    </script>

    <div class="contents" style = "height: calc(100vh - 75px);">
        <!--// Title + Win_Btn -->
        <div class="title" style="background-color:#053085; height:40px;">
            <h3 style="float:left; margin-left: 20px; color: #ffffff; font-size: 15px; font-weight: 400; line-height: 38px;"><asp:Label ID="lbTitle" runat="server">WEB_00090</asp:Label></h3>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close""></a>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:130px;" />
                    <col style="width:150px;" />
                    <col style="width:140px;" />
                    <col style="width:250px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbAuthGroupNM" runat="server"></asp:Label></th>
                    <td><asp:Label ID="lbGetAuthGroupNM" runat="server"></asp:Label><asp:Label ID="lbOrgAuthGroup" runat="server" style="display:none;"></asp:Label></td>
                    <th><asp:Label ID="lbCopyAuthGroupNM" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlAuthGroup" runat="server"></asp:DropDownList></td>                    
                </tr>
                <tr>
                    <th><asp:Label ID="lbMenuNM" runat="server"></asp:Label></th>
                    <td colspan="3"><asp:TextBox ID="txtMenuNM" runat="server" style="width:100px"></asp:TextBox></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch2" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();"/>
                        <input type="button" value="복사" onclick="javascript: fn_AuthGroupMenu_Copy();" ID="btnCopy" runat="server"/>
                    </td>
                </tr>
            </table>
        </div>

        <br />

        <div class="col2">
            <div class="tbl" style="width:25%;">
                <p class="sub_tit">메뉴 목록</p><!--통계수치 테이블의 타이틀-->

                <div id="divMainGrid">
                    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="realMaingrid" style="width: 100%; height: 440px; display:none;"></div>

                            <div class="grid_wrap" style="height:440px; overflow-y:scroll; overflow-x:hidden;"><!--<div class="grid_wrap mt14">-->
                                <table cellpadding="0" cellspacing="0" id="tbMenuList">
                                    <colgroup>
                                        <col style="width:10px;" />
                                        <col style="width:20px;" />
                                        <col style="width:120px;" />
                                    </colgroup>
                                    <%=strList %>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="tbl ml20" style="width:70%;">
                <p class="sub_tit">권한 목록</p><!--통계수치 테이블의 타이틀-->

                <div id="divDetailGrid"  style="width:calc(830px);">
                    <asp:HiddenField ID="hidParams" runat="server" />
                    <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="grid_wrap" style="height:440px; width:100%; overflow-y:scroll; overflow-x:scroll;"><!--<div class="grid_wrap mt14"> -->
                                <table id="tbMenuButtonList"  cellpadding="0" cellspacing="0" runat="server">
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="btn_wrap mt20" style="position: relative; text-align: center; height: 40px;">
            <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
            <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

            <asp:TextBox ID="txtInsertInfo" runat="server" style="display:none;"></asp:TextBox>
            <asp:Button ID="btnSave"  OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" runat="server" Text="Save" style="display:none;" />
        </div>
    </div>

    <script type="text/javascript">        
        $("div[name=divView]").width("1200px");
    </script>

</asp:Content>
