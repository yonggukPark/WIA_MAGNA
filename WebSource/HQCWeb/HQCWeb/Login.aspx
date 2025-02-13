<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HQCWeb.Login" %>

<!DOCTYPE html>

<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
<html lang="ko">
<head>
    
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Cache-Control" content="No-Cache" />
    <title></title>

    <link rel="stylesheet" type="text/css" href="/css/New/login.css"/>
    
    <script type="text/javascript" src="/Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="/Scripts/HQC_Common.js?version=20241025"></script>
    <script language="javascript" type="text/javascript">
        function fn_Chk() {
            var _id = "";
            var _password = "";
          
            var _divShow = "Y";


            _id = $("#txtID").val();
            _password = $("#txtPW").val();

            if (_id == "") {
                alert("아이디를 입력하세요.");
                _divShow = "N";
                $("#txtID").focus();
                return false;
            }
            else if (_id == $("#divLoginID").text()) {
                _divShow = "N";
                alert("아이디를 입력하세요.");
                $("#txtID").focus();
                return false;
            }

            if (_password == "") {
                alert("비밀번호를 입력하세요.");
                _divShow = "N";
                $("#txtPW").focus();
                return false;
            }
            else if (_password == "contraseña") {
                _divShow = "N";
                alert("비밀번호를 입력하세요.");
                $("#txtPW").focus();
                return false;
            }

            //fn_LoginClick();

            return true;
        }
         
        function fn_LoginReset() {
            $("#txtID").val("");
            $("#txtPW").val("");
        }

        function fn_divShow() {
            $("#divLoginChk").attr("style", "display:table; width:100%; height:100%; filter:alpha(opacity=65); opacity:0.3; -moz-opacity:0.3; z-index:1; position:absolute; background:#d6d3ce;  top:0.1%; left:0.1%; ");
        }

        function fn_divHide() {
            $("#divLoginChk").hide();
        }


        function fn_hide() {
            fn_ConReset();

            $("#divSendTmpPw").hide();
            $("#divPwChange").hide();
            $("#divBackground").hide();

            return false;
        }

        function fn_show() {
            fn_ConReset();

            $("#divPwChange").show();
            $("#divBackground").show();

            return false;
        }

        function fn_show2() {
            fn_ConReset();

            $("#divSendTmpPw").show();
            $("#divBackground").show();

            return false;
        }

        function fn_ConReset() {
            $("#txtUserID").val("");
            $("#txtUserID_2").val("");
            $("#txtOldPwd").val("");
            $("#txtNewPwd").val("");
            $("#txtNewPwdConfirm").val("");
            $("#txtTel").val("");
        }

        function fn_change() {
            $("#btnChange").click();
        }

        function fn_changeChk() {
            var _id = "";
            var _old_password = "";
            var _new_password = "";
            var _new_Confirm_password = "";

            _id = $("#txtUserID").val();
            _old_password = $("#txtOldPwd").val();
            _new_password = $("#txtNewPwd").val();
            _new_Confirm_password = $("#txtNewPwdConfirm").val();


            if (_id == "") {
                alert("사용자 아이디를 입력하세요.");
                return false;
            }

            if (_old_password == "") {
                alert("기존 비밀번호를 입력하세요.");
                return false;
            }

            if (_new_password == "") {
                alert("신규 비밀번호를 입력하세요.");
                return false;
            }

            if (_new_password.length < 8) {
                alert("비밀번호는 최소 8자리 이상이어야 합니다.");
                return false;
            }

            var chkVal = checkPass(_new_password);
            //alert(chkVal);
            
            if (_new_Confirm_password == _id) {
                alert("아이디와 비밀번호는 동일할수 없습니다.\n다시 입력하세요.");
                return false;
            }

            if (!chkVal) {
                alert("비밀번호는 숫자,영문자,특수문자[!@#$%^&] 조합이어야 합니다.\n다시한번 입력하세요.");
                return false;
            }

            if (_new_Confirm_password == "") {
                alert("신규 비밀번호를 다시한번 입력하세요.");
                return false;
            }

            if (_new_password != _new_Confirm_password) {
                alert("신규 비밀번호 확인이 올바르지 않습니다.\n다시한번 입력하세요.");
                return false;
            }

            return true;
        }

        function fn_refresh() {
            $("#btnRefresh").click();
        }

        function fn_refreshChk() {
            var _id = "";
            var _old_password = "";
            var _new_password = "";
            var _new_Confirm_password = "";

            _id = $("#txtUserID_2").val();
            _tel = $("#txtTel").val();

            if (_id == "") {
                alert("사용자 아이디를 입력하세요.");
                return false;
            }

            if (_tel == "") {
                alert("핸드폰 번호를 입력하세요.");
                return false;
            }

            return true;
        }

        function fn_loadingEnd() {
            $("#divLoginChk").hide();
        }

        function fn_LoginClick() {

            alert('a');
            $("#btnLogin").click();
        }
        window.onload = function () {
            var savedId = getCookie("UserID");
            document.getElementById("txtID").value = savedId;
        };
    </script>
</head>
<body>
    
    <div id="divLoginChk" style="display:table; width:99%; height:99%; filter:alpha(opacity=65); opacity:0.3; -moz-opacity:0.3; z-index:1; position:absolute; background:#d6d3ce; top:0.5%; left:0.5%; display:none;">
        <p class="pClass"><img src="/images/ajax-loader.gif" /><br /></p>
    </div>


    <div id="wrap">

        <div class="column col-left">
            <p>&nbsp;</p>
        </div>
        <form id="form1" runat="server" autocomplete="off">
        <div class="column col-right">
            <div class="contents" tabindex="0">
                <h1></h1>
                <h2>LOGIN<span>MES SYSTEM</span></h2> 
                
                <div class="login_box">
                    <ul class="login">                        
                        <li>
                            <div class="id">
                                <asp:TextBox ID="txtID" runat="server"  type="text" placeholder="아이디를 입력하세요." AutoCompleteType="None" onfocus="if (this.value == 'User ID' || this.value == '아이디' || this.value == 'Usvario') {this.value = '';}"  onBlur="if (this.value == '') {this.value = $('#divLoginID').text();}"></asp:TextBox> 
                            </div>
                        </li>
                        <li class="mt20">
                            <div class="pw">
                                <asp:TextBox ID="txtPW" runat="server" class="pw"  type="password" placeholder="비밀번호를 입력하세요." onfocus="if (this.value == 'contraseña') {this.value = '';}"  onBlur="if (this.value == '') {this.value = 'contraseña';}"></asp:TextBox>  
                            </div>
                        </li>
                        <li class="mt20">
                            <%--<button class="btn_login" onClick="javascript:return fn_Chk();" >로그인</button>--%>
                            <asp:Button CssClass="btn_login" ID="btnLogin" runat="server" Text="로그인" OnClientClick="javascript:return fn_Chk();"  OnClick="btnLogin_Click" style="display:;" />
                        </li>
                    </ul>
                </div>

                <ul class="btn">
                    <li>
                        <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="false">
                            <asp:ListItem Value="">Language</asp:ListItem>
                            <asp:ListItem Value="KO_KR">Korean</asp:ListItem>
                            <asp:ListItem Value="EN_ES">English</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li><button title="비밀번호 변경" onclick="javascript:return fn_show();">Pw Change</button></li>
                    <li><button title="비밀번호 초기화" onclick="javascript:return fn_show2();">Pw Refresh</button></li>
                </ul>                

                <footer>COPYRIGHT 2025 BY <strong>WIA Magna Powertrain co.</strong> ALL RIGHT RESERVED.</footer>
            </div>
        </div>


        <style>
            .popup_wrap {position:relative; padding:0px; width:410px; height:300px; background-color:#ffffff; box-shadow:0px 1px 6px 4px rgba(0, 0, 0, 0.14);} 
            .popup_wrap .title {height:40px; background-color:#053085;}
            .popup_wrap .title h1 {float:left; margin-left:20px; color:#ffffff; font-size:15px; font-weight:400; line-height:38px;}
            .popup_wrap .title a {display:inline-block; float:right; margin-top:2px; margin-right:2px; width:36px; height:36px; background:url("../img/lnb_close_off.png");}
            .popup_wrap .box {margin:20px;}

            .popup_wrap .box table {width:100%; border:1px #8fb4cc solid; border-collapse:collapse;}
            .popup_wrap .box table th {width:160px; height:40px; text-align:left; padding-left:20px; border-right:#cad7e0 1px solid; border-bottom:#cad7e0 1px solid; background-color:#edf5fa; color:#2e6e99; font-size:12px; font-weight:400; line-height:40px;}
            .popup_wrap .box table td {padding-left:20px; border-bottom:#cad7e0 1px solid; text-align:left; font-size:12px; font-weight:300;}
            .popup_wrap .box table td input, .popup_wrap .box table td select {height:28px; border-left:1px #e6e6e6 solid; border-top:1px #e6e6e6 solid; border-right:1px #e6e6e6 solid; border-bottom:1px #999999 solid; display:inline-block; vertical-align:middle;}

            .popup_wrap .box .btn_wrap {position:relative; text-align:center; height:40px;}
            .popup_wrap .box .btn_wrap a {display:inline-block; width:74px; height:30px; color:#ffffff; font-size:12px; font-weight:400; line-height:30px; text-align:center;}
            .popup_wrap .box .btn_wrap a.btn {background-color:#3a8ff0;}
            .popup_wrap .box .btn_wrap a.btn:hover {background-color:#146acc;}
            .popup_wrap .box .btn_wrap a.btn_close {background-color:#525c66;}
            .popup_wrap .box .btn_wrap a.btn_close:hover {background-color:#343943;}
        
            .ml10 {margin-left:10px !important;}
            .mt20 {margin-top:20px !important;}

            .pClass {display:table-cell; text-align:center; vertical-align:middle;}
        </style>



        <div id="divBackground" style="display:none; width:99%; height:99%; filter:alpha(opacity=65); opacity:0.3; -moz-opacity:0.3; z-index:1; position:absolute; font-size:9pt; background:#d6d3ce; top:0.5%; left:0.5%;"></div>

        <div id="divPwChange"  style="display:none; position:absolute; top:33%; left:39%; z-index:10;">
	        <div  class="popup_wrap">
                <div class="title">
                    <h1>Password Change</h1>
                    <a href="javascript:fn_hide();" title="close"></a>
                </div>
                <div class="box">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <th>아이디</th>
                            <td><asp:TextBox ID="txtUserID" runat="server" Width="95%" AutoCompleteType="None"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>이전 비밀번호</th>
                            <td><asp:TextBox ID="txtOldPwd" runat="server" Width="95%" TextMode="Password"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>신규 비밀번호</th>
                            <td><asp:TextBox ID="txtNewPwd" runat="server" Width="95%" TextMode="Password"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>비밀번호 확인</th>
                            <td><asp:TextBox ID="txtNewPwdConfirm" runat="server" Width="95%" TextMode="Password"></asp:TextBox></td>
                        </tr>
                    </table>
       
                    <div class="btn_wrap mt20">
                        <a href="javascript:fn_change();" class="btn ml10">Change</a>
                        <a href="javascript:fn_hide();" class="btn_close ml10">Close</a>
                        <asp:Button ID="btnChange" runat="server" Text="Change" OnClientClick="javascript:return fn_changeChk();" OnClick="btnChange_Click" style="display:none;"  />
                    </div>
                </div>
            </div>
        </div>

        <div id="divSendTmpPw"  style="display:none; position:absolute; top:33%; left:39%; z-index:10;">
	        <div  class="popup_wrap" style="height:220px">
                <div class="title">
                    <h1>Password Refresh</h1>
                    <a href="javascript:fn_hide();" title="close"></a>
                </div>
                <div class="box">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <th>아이디</th>
                            <td><asp:TextBox ID="txtUserID_2" runat="server" Width="95%" AutoCompleteType="None"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>핸드폰 번호</th>
                            <td><asp:TextBox ID="txtTel" runat="server" Width="95%"></asp:TextBox></td>
                        </tr>
                    </table>
       
                    <div class="btn_wrap mt20">
                        <a href="javascript:fn_refresh();" class="btn ml10">Refresh</a>
                        <a href="javascript:fn_hide();" class="btn_close ml10">Close</a>
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClientClick="javascript:return fn_refreshChk();" OnClick="btnRefresh_Click" style="display:none;"  />
                    </div>
                </div>
            </div>
        </div>

        </form>
    </div>


    


    <script>
        setTimeout(function () {
            fn_loadingEnd();
        }, 1000);
    </script>

   
</body>
</html>

