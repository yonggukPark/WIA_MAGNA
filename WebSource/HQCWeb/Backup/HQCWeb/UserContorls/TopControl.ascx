<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopControl.ascx.cs" Inherits="HQCWeb.UserContorls.TopControl" %>

<header>
    <style type="text/css">
        .top_button {
            position: relative;
            display: inline-block;
            width: 80px;
            height: 28px;
            line-height: 2px;
            font-size: 12px;
            color: #ffffff;
            font-weight: 500;
            margin-left: 5px;
            background-color: #8a877c;
        }
    </style>

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
            alert("A");
        }
    </script>

    <!--// Top Util -->
    <div class="toputil">
        <!-- Centered link -->
        <div class="toputil-centered">
            <h1>MES Application Client</h1>
        </div>

        <!-- Left-aligned links (default) -->
        <div class="toputil-left">
            <a href="javascript:fn_Display();" class="system system-active">MENU</a><%--<a href="#" class="window" style="display:none;">Window</a>--%>
            <asp:Label ID="lbUserID" runat="server" style="color:white;"></asp:Label> &nbsp; <asp:Label ID="lbPlant" runat="server" style="color:white;"></asp:Label>
        </div>

        <div class="al-r" style="position:relative; float:right; width:351px; padding-right:25px; padding-top:5px;">
            <%--<input type="button" value="전체화면" onclick="openFullScreenMode()" class="top_button" />
            <input type="button" value="창모드" onclick="closeFullScreenMode()" class="top_button" />--%>
            <asp:Button ID="btnSessionClear" Text="Session Clear" runat="server" class="top_button" OnClick="btnSessionClear_Click" style="display:none;"  />
            <asp:Button ID="btnNFC" Text="Tag Read" runat="server" class="top_button" OnClick="btnNFC_Click" style="display:none;" />
            <input type="button" value="Widget관리" onclick="javascript: return fn_WidgetMgt();" class="top_button" />
            <asp:Button ID="btnLogOut" Text="LogOut" runat="server" class="top_button" OnClientClick="return fn_logout();" OnClick="btnLogOut_Click"  />
        </div>
    </div>
    <!-- Top Util //-->
    <!--// Top navigation -->
    <div class="top-nav" style="display:none;">
        <ul  class="row top_menu">
            <%=strMenu %>          
            <%--<li class="GPDB">
                <a href="#" class="menu01">시스템 관리</a>
                <ul>
                    <li><a href="javascript:fn_Add('/SystemMgt/UserManagement/User000.aspx','사용자 관리','1');">사용자 관리</a></li>
                    <li><a href="javascript:fn_Add('/SystemMgt/MessageManagement/Message000.aspx','메세지 관리','SYS0003');">메세지 관리</a></li>
                    <li><a href="javascript:fn_Add('/SystemMgt/DictionaryManagement/Dictionary000.aspx','사전 관리','SYS0002');">사전 관리</a></li>
                    <li><a href="javascript:fn_Add('/SystemMgt/MenuManagement/Menu000.aspx','메뉴 관리','SYS0001');">메뉴 관리</a></li>
                </ul>
            </li>--%>
            <li  class="cell ml28" style="float:right; padding-right:20px; display:none;">
                <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged" style="line-height:23px;">
                    <asp:ListItem Value="KO_KR" Text="Korea"></asp:ListItem>
                    <asp:ListItem Value="EN_US" Text="English"></asp:ListItem>
                    <asp:ListItem Value="LO_LN" Text="Local"></asp:ListItem>
                </asp:DropDownList>
            </li>
        </ul>
    </div>
    
    <!-- Top navigation //-->
</header>
