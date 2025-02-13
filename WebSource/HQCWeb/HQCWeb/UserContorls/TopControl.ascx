<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopControl.ascx.cs" Inherits="HQCWeb.UserContorls.TopControl" %>

<header>


    <script>
        function fn_logout() {
            if (confirm('로그아웃 하시겠습니까?')) {
                return true;
            }
            else {
                return false;
            }
        }

        function fn_WidgetMgt() {

            fn_Add('/SystemMgt/WidgetManagement/WidgetMgt.aspx', '사용자 위젯관리', 'WEB_00130');
        }

        
        function openPopup(url) {
            window.open(url, '_blank');
        }

        function shortCutPopup() {
            fn_PopupLayerCall();
            $("#divShortCutLayer").show();

            $("#divShortCutLayer").attr("style", "z-index:101; position:absolute; top:50%; left:50%; width:calc(100vw - 1000px); height:calc(100vh - 300px)");

            //$("#tbodyShortCut").html("");
            //$("#tbodyShortCut").html("<tr><td>Graph01</td><td>Graph01</td><td>Graph01</td><td>Graph01</td><td>Graph01</td><td>Graph01</td><td>Graph01</td></tr>");
        }
    </script>


    <ul class="top_rnb">
	    <li><asp:Label ID="lbUserID" runat="server"></asp:Label><span>님</span></li>
	    <li>
            <%--<button class="widget">위젯관리</button>--%>
            <input type="button" value="단축키" onclick="javascript: return shortCutPopup();" class="widget" />
	    </li>
	    <%--<li>--%>
            <%--<button class="logout" ">logout</button>--%>
            <%--<asp:Button ID="btnWct" Text="WCT" runat="server" class="wct" OnClientClick="javascript:return true;" OnClick="btnWCT_Click"  style="display:;" />--%>
	    <%--</li>--%>
	    <li>
            <%--<button class="widget">위젯관리</button>--%>
            <input type="button" value="사용자 위젯관리" onclick="javascript: return fn_WidgetMgt();" class="widget" style="width:120px;"/>
	    </li>
	    <li>
            <%--<button class="logout" ">logout</button>--%>
            <asp:Button ID="btnLogOut" Text="LogOut" runat="server" class="logout" OnClientClick="return fn_logout();" OnClick="btnLogOut_Click"  style="display:;" />
	    </li>
    </ul>


    
    <%--<div class="toputil">
        <div class="toputil-centered">
            <h1>MES Application Client</h1>
        </div>

        <div class="toputil-left">
            <a href="javascript:fn_Display();" class="system system-active">MENU</a>
            <asp:Label ID="lbUserID" runat="server" style="color:white;"></asp:Label> &nbsp; <asp:Label ID="lbPlant" runat="server" style="color:white;"></asp:Label>
        </div>

        <div class="al-r" style="position:relative; float:right; width:351px; padding-right:25px; padding-top:5px;">
            <asp:Button ID="btnSessionClear" Text="Session Clear" runat="server" class="top_button" OnClick="btnSessionClear_Click" style="display:none;"  />
            <asp:Button ID="btnNFC" Text="Tag Read" runat="server" class="top_button" OnClick="btnNFC_Click" style="display:none;" />
            <input type="button" value="사용자 위젯관리" onclick="javascript: return fn_WidgetMgt();" class="top_button" style="width:120px;"/>
            
        </div>
    </div>--%>
    <!-- Top Util //-->
    <!--// Top navigation 
    <div class="top-nav" style="display:none;">
        <ul  class="row top_menu">
            <li  class="cell ml28" style="float:right; padding-right:20px; display:none;">
                <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged" style="line-height:23px;">
                    <asp:ListItem Value="KO_KR" Text="Korea"></asp:ListItem>
                    <asp:ListItem Value="EN_US" Text="English"></asp:ListItem>
                    <asp:ListItem Value="LO_LN" Text="Local"></asp:ListItem>
                </asp:DropDownList>
            </li>
        </ul>
    </div>
    -->
    <!-- Top navigation //-->
</header>
