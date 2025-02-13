<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info39_p04.aspx.cs" Inherits="HQCWeb.InfoMgt.Info39.Info39_p04" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script src="/Scripts/realgrid-utils.js"></script>
    <script src="/Scripts/realgrid.2.7.2.min.js"></script>
    <script src="/Scripts/realgrid-lic.js"></script>
    <link href="/css/realgrid-style.css" rel="stylesheet" />
    <script src="/Scripts/jszip.min.js"></script>
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_fudFilePath").val() == "") {
                alert("파일을 선택하세요.");
                return false;
            } else {
                if (confirm("저장 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            return true;
        }

        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }

        function showFileName() {
            var fileUpload = document.getElementById('<%= fudFilePath.ClientID %>');
            var input = document.getElementById('upload_name');
            input.value = fileUpload.files && fileUpload.files[0]
                ? fileUpload.files[0].name
                : '선택된 파일 없음';
        }

        var column, field, data;
        var container, dataProvider, gridView;

        // 포맷용 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);
            dataProvider.setFields(field);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
        }

        //포맷 추출
        function fn_Format(f_name) {
            var excelType = true;
            var showProgress = true;
            var itemType = true;//숨겨진값 
            var pageType = true;//전체페이지 내보내기
            var indicator = 'hidden';
            var header = 'default';
            var footer = 'hidden';//아래 footer

            gridView.exportGrid({
                type: "excel",
                target: "local",
                fileName: f_name + ".xlsx",
                showProgress: showProgress,
                progressMessage: "Exporting....",
                indicator: indicator,
                header: header,
                footer: footer,
                compatibility: excelType,
                allItems: itemType,
                pagingAllItems: pageType,
                done: function () {  //내보내기 완료 후 실행되는 함수
                }
            });
        }

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info39</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <%--<th><b style="color:red;">*</b><asp:Label ID="lbFilePath" runat="server"></asp:Label></th>--%>
                    <td  colspan="2" style="width: 100%;text-align: center;vertical-align: middle; border-bottom:none">
                        <div style="height:28px; display: inline-block; text-align: center;">
                            <input id="upload_name" type="text" style="width:70%;" readonly>
                            <a class="btn_img_upload ml10">
                                <label for="PopupContent_fudFilePath">찾아보기...</label>
                                 <asp:FileUpload ID="fudFilePath" runat="server" onchange="showFileName()" />
                            </a>
                        </div>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Format('Format');" class="btn ml10" id="aFormat" runat="server">Format</a>
                <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
    <div id="realgrid" class="realgrid" style="display:none;"></div>
</asp:Content>
