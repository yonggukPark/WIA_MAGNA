<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Notice003.aspx.cs" Inherits="HQCWeb.SystemMgt.NoticeManagement.Notice003" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
       
    <link rel="stylesheet" href="/toastui-editor/toastui-editor.min.css" />
    <script src="/toastui-editor/toastui-editor-all.min.js"></script>

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00100</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">
            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th style="width:12%;"><asp:Label ID="lbNoticeTitle" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetNoticeTitle" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbNoticeContent" runat="server"></asp:Label></th>
                    <td>
                        <%--<div id="divData" runat="server" style="height:420px; overflow-y:scroll; width:100%;">
                            <asp:Literal ID="ltGetNoticeContent" runat="server"></asp:Literal>    
                        </div>--%>

                        <!-- 에디터를 적용할 요소 (컨테이너) -->
                        <div id="content_Editer" style="width:98%; height:520px;  overflow-y:scroll" runat="server"></div>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lbAttachFile" runat="server"></asp:Label>
                    </th>
                    <td style="width:100%;">
                        <%=strFileInfo %>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
    <script>
        const editor = new toastui.Editor.factory({
            el: document.querySelector('#PopupContent_content_Editer'), // 에디터를 적용할 요소 (컨테이너)
            viewer: true,
            height: '800px',                        // 에디터 영역의 높이 값 (OOOpx || auto)
                            // 내용의 초기 값으로, 반드시 마크다운 문자열 형태여야 함
        });

    </script>
</asp:Content>
