<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Frames.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="HQCWeb.Main" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="contents" id="MainContents"></div>
	<!-- Tab Main의 내용 //-->

    <script>
        function fn_WidgetMainSetting(_val, _val2) {
            const divID = $("#divMainCon" + _val);

            divID.load("<%=strWebIpPort%>" + _val2);
        }

        $(document).ready(function () {
            document.querySelector(".popup_cont").focus();

            $(document).keydown(function (e) {
                if (e.key === 'Escape' || e.keyCode === 27) {
                    divShortCutLayer_close();
                    // ESC 키가 눌렸을 때 원하는 동작을 여기에 추가하세요.
                }
            });
        });
    </script>


	<div class="popup" style="z-index:101; position:absolute; top:50%; left:50%; width:700px; display:none;" id="divShortCutLayer">
		<div class="title">
			<h4>단축키 리스트</h4>
			<button type="button" class="btn_close" id="close" onclick="javascript:divShortCutLayer_close();"></button>
		</div>

		<div class="popup_cont">

            <h3 style="margin-bottom: 10px;">일반화면</h3>
            <table border="1" style="margin-bottom: 10px;">
                <thead>
                    <tr>
                        <th style="border-left: black;">버튼</th>
                        <th>단축키</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="border-left: black;">Search</td>
                        <td>ALT+S 또는 ENTER</td>
                    </tr>
                    <tr>
                        <td style="border-left: black;">Restore</td>
                        <td>ALT+R</td>
                    </tr>
                    <tr>
                        <td style="border-left: black;">New</td>
                        <td>ALT+N</td>
                    </tr>
                    <tr>
                        <td style="border-left: black;">Copy</td>
                        <td>ALT+C</td>
                    </tr>
                    <tr>
                        <td style="border-left: black; border-bottom:black;">Excel</td>
                        <td style="border-bottom:black;">ALT+X</td>
                    </tr>
                </tbody>
            </table>

            <h3 style="margin-bottom: 10px;">팝업화면</h3>
            <table border="1">
                <thead>
                    <tr>
                        <th style="border-left: black;">버튼</th>
                        <th>단축키</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="border-left: black;">Save</td>
                        <td>ALT+S</td>
                    </tr>
                    <tr>
                        <td style="border-left: black;">Modify</td>
                        <td>ALT+M</td>
                    </tr>
                    <tr>
                        <td style="border-left: black;">Delete</td>
                        <td>ALT+Shift+D</td>
                    </tr>
                    <tr>
                        <td style="border-left: black;">Restore</td>
                        <td>ALT+R</td>
                    </tr>
                    <tr>
                        <td style="border-left: black; border-bottom:black;">종료</td>
                        <td style="border-bottom:black;">ESC</td>
                    </tr>
                </tbody>
            </table>

			<%--<!--// POPUP Grid 스타일 -->
			<table cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<th>필드1</th>
						<th>필드2</th>
						<th>필드3</th>
						<th>필드4</th>
						<th>필드5</th>
						<th>필드6</th>
						<th>필드7</th>
					</tr>
				</thead>
				<tbody id="tbodyShortCut">
					<tr>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
					</tr>
					<tr>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
						<td>WEB_61000</td>
					</tr>
				</tbody>
			</table>--%>
			
		</div>
	</div>


</asp:Content>
