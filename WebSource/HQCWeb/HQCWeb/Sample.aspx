<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Sample.aspx.cs" Inherits="HQCWeb.Sample" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contents" tabindex="0">
        <script type="text/javascript">
            function fn_Register() {
                fn_OpenPop('Sample001.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            }
        </script>

        <style>
            .searchCombo{
                text-align: center;
                z-index : 1;
                top : 57%;
                position : absolute; 
            }

            .searchCombo2{
                text-align: center;
                z-index : 10;
                top : 72%;
                position : absolute; 
            }
        </style>   

        <link rel="stylesheet" href="/toastui-editor/toastui-editor.min.css" />
        <script src="/toastui-editor/toastui-editor-all.min.js"></script>


        <asp:HiddenField ID="hidWidth" runat="server" Value="900" />
        <asp:HiddenField ID="hidHeight" runat="server" Value="330" />

        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_09999</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:140px;" />
                    <col style="width:300px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server">조회일자</asp:Label></th>
                    <td>
                        <select id="selSearchDate" onchange="javascript:fn_dateChange();">
                            <option value="">선택하세요</option>
                            <option value="T">금일</option>
                            <option value="D">전일</option>
                            <option value="W">전주</option>
                            <option value="M">1개월</option>
                            <option value="Y">1년</option>
                        </select>
                        <asp:TextBox ID="txtFromDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="IME-MODE:disabled; background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox> <span id="spBetween" style="display:none;">~</span>
                        <asp:TextBox ID="txtToDt" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="display:none; background-color:white; color:black;"></asp:TextBox>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch"      runat="server" Text="Search"  OnClientClick="fn_WatingCall();" /> <!-- OnClick="btnSearch_Click" -->
                        <asp:Button id="btnExcelNew"    runat="server" Text="Excel(Num)"   OnClick="btnExcelNew_Click"  />
                        
                        <input type="button" value="이미지 업로드" onclick="fn_Register();" />

                    </td>
                </tr>
                <tr>
                    <th>
                        다중검색 콤보박스
                    </th>
                    <td>
                        <div class="searchCombo" style="width:200px; font-size:12px;">
                            <asp:TextBox ID="txtSearchCombo" runat="server" ReadOnly="true" style="background-color:white; color:black;"></asp:TextBox>
                        </div>
                        <img src="img/gnb_on_arrow.png" style="float:left; padding-left:200px;" onclick="javascript:fn_SearchSetStyle();" />
                        <div id="divSearchSet" class="searchCombo" style="display:none;">
                            <textarea id="txtSearchValue" rows="5" style="width:250px;"></textarea>
                            <br />
                            <input type="button" value="적용" onclick="javascript:fn_Set();" />
                            <input type="button" value="닫기" onclick="javascript:fn_SearchSetStyle();" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>
                        필터링 콤보박스
                    </th>
                    <asp:DropDownList ID="ddlItems" runat="server" CssClass="filterable-dropdown"></asp:DropDownList>
                </tr>

               <%-- <tr>
                    <th>
                        다중검색 콤보박스
                    </th>
                    <td>
                        <div class="searchCombo2" style="width:170px; font-size:12px;">
                            <asp:TextBox ID="txtSearchCombo2" runat="server"></asp:TextBox>
                        </div>
                        <img src="img/gnb_on_arrow.png" style="float:left; padding-left:175px;" onclick="javascript:fn_SearchSetStyle2();" />
                        <div id="divSearchSet2" class="searchCombo" style="display:none;">
                            <textarea id="txtSearchValue2" rows="5" style="width:250px;"></textarea>
                            <br />
                            <input type="button" value="적용" onclick="javascript:fn_Set2();" />
                            <input type="button" value="닫기" onclick="javascript:fn_SearchSetStyle2();" />
                        </div>
                    </td>
                </tr>--%>

            </table>

            


        </div>
    </div>

    <br />
    <br />
            
    <!-- 에디터를 적용할 요소 (컨테이너) -->
    <div id="content_Editer" style="width:98%; height:800px; padding-left:20px;">

    </div>
    <!--필터링 콤보박스 스크립트 -->
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var ddl = document.getElementById('<%= ddlItems.ClientID %>');
        
            // Add event listener to the dropdown list
            ddl.addEventListener('click', function () {
                var inputBox = document.createElement('input');
                inputBox.setAttribute('type', 'text');
                inputBox.setAttribute('placeholder', 'Type to filter...');
                inputBox.classList.add('dropdown-filter-input');
                inputBox.addEventListener('keyup', function () {
                    var filterText = inputBox.value.toLowerCase();
                    for (var i = 1; i < ddl.options.length; i++) { // Skip the first item (placeholder)
                        var optionText = ddl.options[i].text.toLowerCase();
                        ddl.options[i].style.display = optionText.includes(filterText) ? "" : "none";
                    }
                });

                if (!ddl.classList.contains('filterable-dropdown')) {
                    ddl.classList.add('filterable-dropdown');
                    ddl.insertBefore(inputBox, ddl.firstChild);
                }
            });
        });
    </script>
    
    <!--필터링 콤보박스 스타일 -->
    <style>
        .filterable-dropdown {
            position: relative;
        }

        .dropdown-filter-input {
            width: 100%;
            box-sizing: border-box;
            padding: 8px;
            margin: 0;
            border: none;
            border-bottom: 1px solid #ccc;
        }

        .filterable-dropdown option {
            padding: 8px;
        }
    </style>

    <script>

        // 검색어 콤보
        jQuery(document).ready(function ($) {
            $("#MainContent_txtSearchCombo").comboTree({
                source: [ ],
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true
            });

            //$("#MainContent_txtSearchCombo2").comboTree({
            //    //source: [{ id: 1, title: 1 }, { id: 2, title: 2 }],
            //    source: [ ],
            //    isMultiple: true,
            //    cascadeSelect: false,
            //    collapse: false,
            //    selectAll: true
            //});
        });

        function fn_Set() { 
            $("#MainContent_txtSearchCombo").comboTree({
                source: [],
                comboReload : true
            });

            var txtSearch = $("#txtSearchValue").val();

            txtSearch = txtSearch.replace(/(?:\r\n|\r|\n)/g, ',');

            const arr = txtSearch.split(',');
            const arrTxtSearch = arr.filter((el, index) => arr.indexOf(el) === index);
            
            var tList = new Array();

            var iLen = 0;
            
            for (var i =0; i < arrTxtSearch.length; i++){
                if (arrTxtSearch[i] != "") {
                    iLen++;

                    var comboData = new Object();

                    comboData.id = arrTxtSearch[i];
                    comboData.title = arrTxtSearch[i];

                    tList.push(comboData);
                }
            }

            if (iLen > 50) {
                alert("검색어 셋팅 가능 숫자는 50개 입니다.");
                
                $("#MainContent_txtSearchCombo").comboTree({
                    source: [ ],
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true, 
                    onLoadAllChk: true
                });
                
                return false;
            } else {
                $("#MainContent_txtSearchCombo").comboTree({
                    source: tList,
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true,
                    onLoadAllChk: true
                });

                $("#divSearchSet").hide();
            }
        }

        function fn_dateChange() {
            var sValue = $("#selSearchDate").val();

            if (sValue == "W" || sValue == "M" || sValue == "Y") {
                $("#MainContent_txtToDt").show();
            }
            else { 
                $("#MainContent_txtToDt").hide();
            }

            const today = new Date($("#MainContent_txtToDt").val());

            if (sValue == "T") {
                today.setDate(today.getDate());
            }

            if (sValue == "D") {
                today.setDate(today.getDate() - 1);
            }

            if (sValue == "W") {
                today.setDate(today.getDate() - 6);
            }

            if (sValue == "M") {
                today.setDate(today.getDate() - 30);
            }

            if (sValue == "Y") {
                today.setDate(today.getDate() - 365);
            }

            $("#MainContent_txtFromDt").val(today.toISOString().substring(0, 10));
        }

        function fn_SearchSetStyle() {
            if ($("#divSearchSet").css("display") == "none") {
                $("#divSearchSet").show();
            }
            else {
                $("#divSearchSet").hide();
            }
        }

        function fn_SearchSetStyle2() {
            if ($("#divSearchSet2").css("display") == "none") {
                $("#divSearchSet2").show();
            }
            else {
                $("#divSearchSet2").hide();
            }
        }



        const editor = new toastui.Editor({
            el: document.querySelector('#content_Editer'), // 에디터를 적용할 요소 (컨테이너)
            height: '600px',                        // 에디터 영역의 높이 값 (OOOpx || auto)
            initialEditType: 'wysiwyg',             // 최초로 보여줄 에디터 타입 (markdown || wysiwyg)
            initialValue: '',                       // 내용의 초기 값으로, 반드시 마크다운 문자열 형태여야 함
            previewStyle: 'vertical',                // 마크다운 프리뷰 스타일 (tab || vertical)
            hideModeSwitch: true,
            exts: ['link', 'italic', 'heading' ],
            
        });


        function fn_EventKeyChk(event) {
            if ((event.keyCode >= 48 && event.keyCode <= 57)
                || event.keyCode == 8
                || event.keyCode == 37 || event.keyCode == 39
                || event.keyCode == 46
                || event.keyCode == 39) {

                return false;
            }
        }


        function removeChar(event) {
            event = event || window.event;

            //var dateVal = event.target.value;

            var keyID = (event.which) ? event.which : event.keyCode;

            if (keyID == 8 || keyID == 46 || keyID == 37 || keyID == 39)
                return;
            else
                event.target.value = event.target.value.replace(/[\ㄱ-ㅎㅏ-ㅣ가-힣]/g, '');
        }


    </script>
</asp:Content>
