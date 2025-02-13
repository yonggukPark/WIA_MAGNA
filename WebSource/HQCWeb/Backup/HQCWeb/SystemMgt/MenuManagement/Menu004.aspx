<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Menu004.aspx.cs" Inherits="HQCWeb.SystemMgt.MenuManagement.Menu004" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            var _len = $("#tbList tbody tr").length;

            var _insertInfo = "";

            var _conID = "";
            var _conNM = "";
            var _conFunction = "";
            var _menuID = $("#PopupContent_lbSelectMenuID").text();

            for (var i = 0; i < _len; i++) {
                _conID = $("#tbList tbody tr:eq(" + i + ")>td:eq(0)").html();
                _conNM = $("#tbList tbody tr:eq(" + i + ")>td:eq(1)").html();
                _conFunction = $("#tbList tbody tr:eq(" + i + ")>td:eq(2)").html();

                if (i == 0) {
                    _insertInfo = _menuID + "," + _conID + "," + _conNM + "," + _conFunction;
                } else {
                    _insertInfo += "/" + _menuID + "," + _conID + "," + _conNM + "," + _conFunction;
                }
            }

            $("#PopupContent_txtInsertInfo").val(_insertInfo);
            
            //console.log("txtInsertInfo=", $("#PopupContent_txtInsertInfo").val());

            if ($("#PopupContent_txtInsertInfo").val() == "") {
                alert("등록할 컨트롤 정보가 없습니다. 컨트롤 정보를 생성후 등록해주세요.");
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
  
        function fn_Add() { 
            var iChk = 1;

            var _cateB = $("#PopupContent_ddl_Category_B");
            var _cateM = $("#PopupContent_ddl_Category_M");
            var _cateS = $("#PopupContent_ddl_Category_S");

            var _selectMenu = $("#PopupContent_lbSelectMenuID");

            if (_cateB.val() == "") {
                iChk = 0;
                alert("대메뉴를 선택하세요.");
            } 

            if (iChk == 1) {
                if (_cateM.attr("disabled") == undefined) {
                    if (_cateM.val() == "") {
                        iChk = 0;
                        alert("중메뉴를 선택하세요.");
                    }
                } else {
                    _selectMenu.text(_cateB.val());
                } 
            }

            if (iChk == 1) {
                if (_cateS.attr("disabled") == undefined) {
                    if (_cateS.val() == "") {
                        iChk = 0;
                        alert("소메뉴를 선택하세요.");
                    }
                }
                else {
                    _selectMenu.text(_cateM.val());
                }
            }

            var _conID = $("#PopupContent_txtConID").val().replace(' ', '');
            var _conName = $("#PopupContent_txtConName").val().replace(' ', '');
            var _conFunction = $("#PopupContent_txtFunction").val().replace(' ', '');

            if (iChk == 1) {
                if (_conID == "") {
                    iChk = 0;
                    alert("버튼 아이디를 입력하세요.");
                }
            }

            if (iChk == 1) {
                if (_conName == "") {
                    iChk = 0;
                    alert("버튼명을 입력하세요.");
                }
            }
            
            if (iChk == 1) {
                if (_conFunction == "") {
                    iChk = 0;
                    alert("이벤트 함수를 입력하세요.");
                }
            }

            var _len = $("#tbList tbody tr").length;

            for (var i = 0; i < _len; i++) {
                var tValue = $("#tbList tbody tr:eq(" + i + ")>td:eq(0)").html();

                if (tValue == _conID) {

                    iChk = 0;

                    alert("이미 사용중인 아이디가 있습니다. 다시 입력하세요.");
                }
            }
            
            if (iChk == 1) {

                var _ValText = "";

                _ValText += "<tr style=\"height=30px;\" id='tr" + _conID + "'> ";
                _ValText += "   <td style=\"text-align:left; padding-left:5px;\">" + _conID + "</td>";
                _ValText += "   <td style=\"text-align:left; padding-left:5px;\">" + _conName + "</td>";
                _ValText += "   <td style=\"text-align:left; padding-left:5px;\">" + _conFunction + "</td> ";
                _ValText += "   <td><img src=\"../../images/btn/close_on.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_del('tr" + _conID + "');\" /></td>";
                _ValText += "</tr> ";

                $("#tbList > tbody:last").append(_ValText);
                
                $("#PopupContent_txtConID").val("");
                $("#PopupContent_txtConName").val("");
                $("#PopupContent_txtFunction").val("");
            }
        }

        function fn_AutoAdd(_id, _nm, _function) {
            if ($('#' + _id).is(':checked')) {
                var iChk = 1;

                var _cateB = $("#PopupContent_ddl_Category_B");
                var _cateM = $("#PopupContent_ddl_Category_M");
                var _cateS = $("#PopupContent_ddl_Category_S");

                var _selectMenu = $("#PopupContent_lbSelectMenuID");

                if (_cateB.val() == "") {
                    iChk = 0;
                    alert("대메뉴를 선택하세요.");

                    if ($('#' + _id).is(':checked')) {
                        $('#' + _id).prop('checked', false);
                    }
                }

                if (iChk == 1) {
                    if (_cateM.attr("disabled") == undefined) {
                        if (_cateM.val() == "") {
                            iChk = 0;
                            alert("중메뉴를 선택하세요.");

                            if ($('#' + _id).is(':checked')) {
                                $('#' + _id).prop('checked', false);
                            }
                        }
                    } else {
                        _selectMenu.text(_cateB.val());
                    }
                }

                if (iChk == 1) {
                    if (_cateS.attr("disabled") == undefined) {
                        if (_cateS.val() == "") {
                            iChk = 0;
                            alert("소메뉴를 선택하세요.");

                            if ($('#' + _id).is(':checked')) {
                                $('#' + _id).prop('checked', false);
                            }
                        }
                    }
                    else {
                        _selectMenu.text(_cateM.val());
                    }
                }

                var _conID = _id.replace(' ', '');
                var _conName = _nm.replace(' ', '');
                var _conFunction = _function.replace(' ', '');

                if (iChk == 1) {
                    if (_conID == "") {
                        iChk = 0;
                        alert("버튼 아이디를 입력하세요.");
                    }
                }

                if (iChk == 1) {
                    if (_conName == "") {
                        iChk = 0;
                        alert("버튼명을 입력하세요.");
                    }
                }

                if (iChk == 1) {
                    if (_conFunction == "") {
                        iChk = 0;
                        alert("이벤트 함수를 입력하세요.");
                    }
                }

                var _len = $("#tbList tbody tr").length;

                for (var i = 0; i < _len; i++) {
                    var tValue = $("#tbList tbody tr:eq(" + i + ")>td:eq(0)").html();

                    if (tValue == _conID) {

                        iChk = 0;

                        alert("이미 사용중인 아이디가 있습니다. 다시 입력하세요.");

                        if ($('#' + _id).is(':checked')) {
                            $('#' + _id).prop('checked', false);
                        }
                    }
                }

                if (iChk == 1) {
                    var _ValText = "";

                    _ValText += "<tr style=\"height=30px;\" id='tr" + _conID + "'> ";
                    _ValText += "   <td style=\"text-align:left; padding-left:5px;\">" + _conID + "</td>";
                    _ValText += "   <td style=\"text-align:left; padding-left:5px;\">" + _conName + "</td>";
                    _ValText += "   <td style=\"text-align:left; padding-left:5px;\">" + _conFunction + "</td> ";
                    _ValText += "   <td><img src=\"../../images/btn/close_on.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_del('tr" + _conID + "');\" /></td>";
                    _ValText += "</tr> ";

                    $("#tbList > tbody:last").append(_ValText);

                    $("#PopupContent_txtConID").val("");
                    $("#PopupContent_txtConName").val("");
                    $("#PopupContent_txtFunction").val("");
                }
            } else {
                $("#tr" + _id).remove();
            }
        }

        function fn_del(_id) {
            $("#" + _id).remove();

            if ($('#' + _id.replace("tr","")).is(':checked')) {
                $('#' + _id.replace("tr", "")).prop('checked', false);
            }
        }

        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }

        function fn_BtnListSet() {
            $("#tbList tbody").html($("#PopupContent_txtBtnList").val());

            $("#PopupContent_txtBtnList").val("");
        }

    </script>    
    <style type="text/css">
        .tb_th {
            text-align:center; 
            padding-right:20px;
        }

        .tb_td {
            text-align:left; 
            padding-left:5px;
        }
    </style>

	<!--// POPUP -->
    <div class="popup_wrap">

        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00010</asp:Label> - 기능 관리</h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">
            <asp:UpdatePanel ID="upCategory" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <!--// Table -->
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <th><b style="color:red;">*</b>&nbsp;<asp:Label ID="lbMenu_B" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddl_Category_B" runat="server" OnSelectedIndexChanged="ddl_Category_B_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><b style="color:red;">*</b>&nbsp;<asp:Label ID="lbMenu_M" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddl_Category_M" runat="server" Enabled="false" OnSelectedIndexChanged="ddl_Category_M_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><b style="color:red;">*</b>&nbsp;<asp:Label ID="lbMenu_S" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddl_Category_S" runat="server" Enabled="false"  OnSelectedIndexChanged="ddl_Category_S_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td colspan="2">
                                <asp:Label ID="lbSelectMenuID" runat="server"></asp:Label>
                                <asp:TextBox ID="txtBtnList" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table> 
                    <!-- Table //--> 
                </ContentTemplate>
            </asp:UpdatePanel>

            <br />

            <table cellpadding="0" cellspacing="0" style="margin-top:10px !important;" border="0">
                <tr>
                    <th style="width:25%;"><b style="color:red;">*</b>&nbsp;<asp:Label ID="lbButtonList" runat="server"></asp:Label></th>
                    <th>
                        <asp:UpdatePanel ID="upButton" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Literal ID="ltButtonList" runat="server"></asp:Literal>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_Category_B" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddl_Category_M" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddl_Category_S" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </th>
                </tr>
                <tr style="display:none;">
                    <th><b style="color:red;">*</b>&nbsp;<asp:Label ID="lbConID" runat="server"></asp:Label></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtConID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display:none;">
                    <th><b style="color:red;">*</b>&nbsp;<asp:Label ID="lbConName" runat="server"></asp:Label></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtConName" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr style="display:none;">
                    <th><b style="color:red;">*</b>&nbsp;<asp:Label ID="lbFunction" runat="server"></asp:Label></th>
                    <td style="width:70%;">
                        <asp:TextBox ID="txtFunction" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td>
                         <div class="search_btn_wrap" style="margin-bottom:10px !important; padding-right:15px;">
                             <a href="javascript:fn_Add();" class="btn ml10">Add</a>
                         </div>
                     </td>
                </tr>
                <tr>
                    <td colspan="2" style="height:250px; vertical-align:top; padding-top:5px;">
                        <div class="contents">
                            <div class="grid_wrap mt14" style="height:240px; overflow-y:scroll; width:100%;">
                                <table cellpadding="0" cellspacing="0" border="1" style="width:96%;" id="tbList">
                                    <colgroup>
                                        <col style="width:30%;" />
                                        <col style="width:35%;" />
                                        <col style="width:35%;" />
                                        <col style="width:10%;" />
                                    </colgroup>
                                    <thead>
                                        <tr>
                                            <th style="text-align:center; padding-right:20px;">아이디</th>
                                            <th style="text-align:center; padding-right:20px;">이름</th>
                                            <th style="text-align:center; padding-right:20px;">함수</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%=strBtnList %>
                                        <%--<tr style="height=30px;" id='tr1'>
                                           <td style="text-align:left; padding-left:5px;">test1</td>
                                           <td style="text-align:left; padding-left:5px;">test2</td>
                                           <td style="text-align:left; padding-left:5px;">test3</td> 
                                           <td><img src="../../images/btn/close_on.gif" style="padding-bottom:2px;" onclick="javascript:fn_del('tr1');" /></td>
                                        </tr> 
                                        <tr style="height=30px;" id='tr2'>
                                           <td style="text-align:left; padding-left:5px;">test2</td>
                                           <td style="text-align:left; padding-left:5px;">test3</td>
                                           <td style="text-align:left; padding-left:5px;">test4</td> 
                                           <td><img src="../../images/btn/close_on.gif" style="padding-bottom:2px;" onclick="javascript:fn_del('tr1');" /></td>
                                        </tr> --%>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>

            <!--// Btn -->
            <div class="btn_wrap mt20">
				<a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:TextBox ID="txtInsertInfo" runat="server" style="display:none;"></asp:TextBox>
                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click"  Text="Save" style="display:none;"/> <%--OnClick="btnSave_Click"--%>
            </div>
            <!-- Btn //-->
        </div>
        <!-- Box //-->
    </div>
    <!-- POPUP //-->
</asp:Content>