<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="AuthGroup004.aspx.cs" Inherits="HQCWeb.SystemMgt.AuthGroupManagement.AuthGroup004" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Add() {
            var aAuthGroupNM    = $("#PopupContent_lbOrgAuthGroupID").html();
            var aUserID         = $("#PopupContent_txtUserID").val();

            if (aUserID== "") {
                alert("권한을 부여할 사용자가 없습니다. 사용자를 선택해주세요.");
                return false;
            }

            var jsonData = JSON.stringify({ sParams: aAuthGroupNM, sParams2: aUserID });

            $.ajax({
                type: "POST",
                url: "AuthGroup004.aspx/GetUserInfo",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_AddRtn(msg.d);
                }
            });
        }

        function fn_AddRtn(_vals) {
            var arrUserInfo = "";
            var arrUserData = "";
            var _ValText = "";

            var _rowCnt = $("#tbList tbody tr").length;

            var _userID = "";
            var iChk = "1";
            
            console.log("_vals=", _vals);

            arrUserInfo = _vals.split(',');

            for (var j = 0; j < arrUserInfo.length; j++) {

                arrUserData = arrUserInfo[j].split('^');

                for (var i = 0; i < _rowCnt; i++) {
                    _userID = $("#tbList tbody tr:eq(" + i + ")>td:eq(3)").html();

                    if (arrUserData[3] != _userID) {
                        iChk = "1";
                    } 
                    else {
                        iChk = "0";
                        break;
                    }
                }

                if (iChk == "1") {
                    _ValText += "<tr style=\"height=30px;\" id='tr" + arrUserData[2] + "'>";
                    _ValText += "   <td style=\"padding:0px;\"><img src=\"../../images/btn/close_on.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_UserDel('" + arrUserData[2] + "');\" /></td>";
                    _ValText += "   <td style=\"padding:0px;\">" + arrUserData[0] + "</td>"; 
                    _ValText += "   <td style=\"padding:0px;\">" + arrUserData[1] + "</td>"; 
                    _ValText += "   <td style=\"padding:0px; display:none;\">" + arrUserData[3] + "</td>";
                    _ValText += "</tr> ";
                }
            }


            $("#tbList > tbody:last").append(_ValText);
        }

        function fn_UserDel(_val) {
            $("#tr" + _val).remove();

            //console.log($("[data-id]").html());
        }

        function fn_Save() {
            var _len = $("#tbList tbody tr").length;
            var _insertInfo = "";

            if (_len == 0) {
                alert("권한을 부여할 사용자가 없습니다. 사용자를 선택해주세요.");
                return false;
            }

            for (var i = 0; i < _len; i++) {
                _userID = $("#tbList tbody tr:eq(" + i + ")>td:eq(3)").html();
                if (i == 0) {
                    _insertInfo = _userID;
                } else {
                    _insertInfo += "," + _userID;
                }
            }

            if (confirm("저장 하시겠습니까?")) {

                var aAuthGroupNM    = $("#PopupContent_lbOrgAuthGroupID").html();
                var aUserID         = _insertInfo;

                var jsonData = JSON.stringify({ sParams: aAuthGroupNM, sParams2: aUserID });

                $.ajax({
                    type: "POST",
                    url: "AuthGroup004.aspx/SetUserInfo",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        fn_AddResult(msg.d);
                    }
                });
                
                return true;
            } else {
                return false;
            }

            //console.log("_insertInfo=", _insertInfo);
        }

        function fn_AddResult(_val) {
            if (_val == "C") {
                fn_AddComplete();
            }
            else {
                fn_AddError();
            }
        }

        function fn_AddComplete() {
            alert("정상등록 되었습니다."); parent.fn_ModalCloseDiv();
        }

        function fn_AddError() {
            alert("등록에 실패하였습니다. 관리자에게 문의바립니다.");
        }

        function fn_AuthUserInfo(_vals) {
            var arrUserInfo = "";
            var arrUserData = "";
            var _ValText = "";

            if (_vals != "") {
                arrUserInfo = _vals.split(',');

                for (var i = 0; i < arrUserInfo.length; i++) {
                
                    arrUserData = arrUserInfo[i].split('^');

                    _ValText += "<tr style=\"height=30px;\" id='tr" + arrUserData[2] + "'>";
                    _ValText += "   <td style=\"padding:0px;\"><img src=\"../../images/btn/close_off.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_AuthUserDel('" + arrUserData[3] + "');\" /></td>";
                    _ValText += "   <td style=\"padding:0px;\">" + arrUserData[0] + "</td>"; 
                    _ValText += "   <td style=\"padding:0px;\">" + arrUserData[1] + "</td>"; 
                    _ValText += "   <td style=\"padding:0px; display:none;\">" + arrUserData[3] + "</td>";
                    _ValText += "</tr> ";
                }

                $("#tbList > tbody:last").append(_ValText);
            }
        }

        function fn_AuthUserDel(_val) {
            if (confirm("삭제 하시겠습니까?")) {

                var aAuthGroupNM    = $("#PopupContent_lbOrgAuthGroupID").html();
                var aUserID         = _val;

                var jsonData = JSON.stringify({ sParams: aAuthGroupNM, sParams2: aUserID });

                $.ajax({
                    type: "POST",
                    url: "AuthGroup004.aspx/DelUserInfo",
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        fn_DelRtn(msg.d);
                    }
                });
                
                return true;
            } else {
                return false;
            }
        }

        function fn_DelRtn(_val) {

            //console.log("_val=", _val);

            var arrRtnInfo = _val.split('^');

            if (arrRtnInfo[0] == "C") {
                fn_DelComplete(arrRtnInfo[1], arrRtnInfo[2]);
            }

            if (_val == "E") {
                fn_DelError();
            }

            fn_loadingEnd();
        }

        function fn_DelComplete(_val, _val2) {
            
            //console.log("_val=", _val);
            //console.log("_val2=", _val2);
            //console.log("strUserJson=",  <%=strUserJson%>);
            //console.log("_val3=", _val3);

            alert("정상삭제 되었습니다.");
            
            $("#tr" + _val).remove();

            $("#PopupContent_txtUserInfo").comboTree({
                source: [],
                comboReload: true
            });

            var comboTree1;

            comboTree1 = $("#PopupContent_txtUserInfo").comboTree({
                source: <%=strUserJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: true,
                hidCon: "PopupContent_txtUserID"
            });

            var tList = new Array();

            var arrVal = _val2.split(',');

            for (var i =0; i < arrVal.length; i++){
                
                var comboData = new Object();

                var arrComboInfo = arrVal[i].split('/');

                comboData.id = arrComboInfo[0];
                comboData.title = arrComboInfo[1];

                tList.push(comboData);
            }

            comboTree1.setSelection(tList);
        }

        function fn_DelError() {
            alert("삭제에 실패하였습니다. 관리자에게 문의바립니다.");
        }

        function fn_testDel() {
            $("#PopupContent_txtUserInfo").comboTree({
                source: [],
                comboReload: true
            });

            var comboTree1;

            comboTree1 = $("#PopupContent_txtUserInfo").comboTree({
                //source: [{ id: "JYJ", title: "[DP] 장용제" }, { id: "TES5", title: "[DP] TES5" }],
                source: <%=strUserJson%>,
                isMultiple: true,
                cascadeSelect: true,
                collapse: false,
                selectAll: true,
                valueChange: true,
                hidCon: "PopupContent_txtUserID"
            });
        }

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00090</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><asp:Label ID="lbAuthGroupNM" runat="server"></asp:Label></th>
                    <td colspan="2">
                        <asp:Label ID="lbGetAuthGroupNM" runat="server"></asp:Label><asp:Label ID="lbOrgAuthGroupID" runat="server" style="display:none;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lbUserNM"  runat="server"></asp:Label>
                    </th>
                    <td>
                        <div class="searchCombo" style="width:170px; font-size:12px;" id="divCombo">
                            <asp:TextBox ID="txtUserInfo" runat="server" ReadOnly="true" style="background-color:white; color:black;" placeholder="Select"></asp:TextBox>
                            <asp:TextBox ID="txtUserID" runat="server" ReadOnly="true" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                         <div class="search_btn_wrap" style="margin-bottom:10px !important; padding-right:15px;">
                             <a href="javascript:fn_Add();" class="btn ml10" style="display:;">Add</a>  <%--style="display:none;"--%>
                             <a href="javascript:fn_testDel();" class="btn ml10" style="display:none;">Del</a>  <%--style="display:none;"--%>
                         </div>
                     </td>
                </tr>

                <tr>
                    <td colspan="3" style="height:490px; vertical-align:top; padding-top:5px;">
                        <div class="contents">
                            <div class="grid_wrap mt14" style="height:480px; overflow-y:scroll; width:100%;">
                                <table cellpadding="0" cellspacing="0" border="1" style="width:96%;" id="tbList">
                                    <colgroup>
                                        <col style="width:10%;" />
                                        <col style="width:45%;" />
                                        <col />
                                    </colgroup>
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th style="text-align:center; padding-right:20px;">부서</th>
                                            <th style="text-align:center; padding-right:20px;">이름</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </td>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->


    <script>

        var comboTree1;

        // 검색어 콤보
        jQuery(document).ready(function ($) {
            comboTree1 = $("#PopupContent_txtUserInfo").comboTree({
                source: <%=strUserJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true, 
                valueChange: true,  
                hidCon: "PopupContent_txtUserID"
            });

            comboTree1.setSelection(<%=strChkUserJson%>);
        });

    </script>

</asp:Content>
