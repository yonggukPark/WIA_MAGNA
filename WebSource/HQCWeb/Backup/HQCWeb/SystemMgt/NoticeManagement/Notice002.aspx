<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Notice002.aspx.cs" Inherits="HQCWeb.SystemMgt.NoticeManagement.Notice002" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">

    <asp:TextBox ID="txtFileName"   runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtFilePath"   runat="server" style="display:none;"></asp:TextBox>

    <link rel="stylesheet" href="/toastui-editor/toastui-editor.min.css" />
    <script src="/toastui-editor/toastui-editor-all.min.js"></script>


    <script type="text/javascript">
        function fn_DeleteConfirm() {

            if (confirm("삭제 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_Delete() {
            $("#PopupContent_btnDelete").click();
        }
    </script>    

	<!--// POPUP -->
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">WEB_00100</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">
            <asp:Label ID="lbNoticeNum" runat="server" style="display:none;"></asp:Label>
            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbNoticeTitle" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtNoticeTitle" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbNoticeContent" runat="server"></asp:Label></th>
                    <td>
                        <!-- 에디터를 적용할 요소 (컨테이너) -->
                        <div id="content_Editer" style="width:98%;" runat="server"></div>
                    </td>
                </tr>
                
                <tr>
                    <th><asp:Label ID="lbUseYN" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                    </td>  
                </tr>

                <tr>
                    <th><asp:Label ID="lbAttachFile" runat="server"></asp:Label></th>
                    <td style="padding-left:10px;">
                        <asp:FileUpload ID="fuUpload" runat="server" AllowMultiple="true" accept=".gif,.png,.jpg,.xls,.xlsx,.ppt,.pptx" style="display:none;"  onchange="onFileSelected();" />
                        <div class="search_btn_wrap" style="margin-bottom:10px !important; float:left; padding:0px;">
                            <a href="javascript:fn_FindFile();" class="btn ml10">파일찾기</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding:0px;">
                        <div class="contents">
                            <div class="grid_wrap" style="height:140px; overflow-y:scroll; overflow-x:hidden; width:100%;"><!--<div class="grid_wrap mt14">-->
					            <table id="tbFileList"  cellpadding="0" cellspacing="0">
						            <thead>
							            <tr>
								            <th style="text-align:center; padding:0px; width:20px;"></th>
								            <th style="text-align:center; padding:0px; width:20px;">파일타입</th>
								            <th style="text-align:center; padding:0px;">파일명</th>
                                            <th style="text-align:center; padding:0px; display:none;"></th>
                                            <th style="text-align:center; padding:0px; display:none;">파일사이즈</th>
							            </tr>
						            </thead>
						            <tbody>
                                        <%=strFileInfo %>
						            </tbody>
					            </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
                <asp:Button ID="btnUpload" runat="server"  OnClick="btnUpload_Click"  Text="Save" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->


    <script>
        const editor = new toastui.Editor({
            el: document.querySelector('#PopupContent_content_Editer'), // 에디터를 적용할 요소 (컨테이너)
            height: '300px',                        // 에디터 영역의 높이 값 (OOOpx || auto)
            initialEditType: 'wysiwyg',             // 최초로 보여줄 에디터 타입 (markdown || wysiwyg)
            initialValue: '',                       // 내용의 초기 값으로, 반드시 마크다운 문자열 형태여야 함
            previewStyle: 'vertical',                // 마크다운 프리뷰 스타일 (tab || vertical)
            hideModeSwitch: true,
            placeholder: '내용을 입력해 주세요.',
            /* start of hooks */
            hooks: {
                addImageBlobHook(blob, callback) {  // 이미지 업로드 로직 커스텀
                    var reader = new FileReader();
                    // /*
                    reader.readAsDataURL(blob);
                    reader.onloadend = function () {
                        var base64data = reader.result;

                        var _arrType = blob.type.split('/');

                        if (_arrType[0] == "image") {
                            var jsonData = JSON.stringify({ ImageData: base64data, strFileName: blob.name, strFileSize: blob.size, strFielType: blob.type });

                            $.ajax({
                                type: "POST",
                                url: "Notice002.aspx/SetImageUpload",
                                data: jsonData,
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",

                                success: function (msg) {
                                    var _rtn = msg.d.split('|');

                                    if (_rtn[0] == "C") {
                                        console.log("img=", _rtn[1]);
                                        callback(_rtn[1], 'image alt attribute');
                                    }

                                    if (_rtn[0] == "E") {
                                        alert("이미지 업로드에 실패하였습니다. 관리자에게 문의바랍니다.");
                                    }

                                    if (_rtn[0] == "N") {
                                        alert("이미지 형식 파일만 에디터에 추가할수 있습니다. 파일을 다시 선택하세요.");
                                    }
                                },
                                error: function (er) {
                                    console.log("er=", er);
                                }
                            });
                        }
                        else {
                            alert("이미지 형식 파일만 에디터에 추가할수 있습니다. 파일을 다시 선택하세요.");
                        }
                    }
                }
            }
        });

        function fn_Modify() {
            if ($("#PopupContent_txtNoticeTitle").val() == "") {
                alert("제목을 입력하세요.");
                return false;
            }
            else if (editor.getMarkdown().length < 1) {
                alert("내용을 입력하세요.");
                return false;
            }
            else {
                if (confirm("수정 하시겠습니까?")) {

                    fn_WatingCall();

                    // /*
                    var vNum = $("#PopupContent_lbNoticeNum").text();
                    var vTitle = $("#PopupContent_txtNoticeTitle").val();
                    var vContent = editor.getHTML();
                    var vUseYN = $("#PopupContent_ddlUseYN").val();
                    var vPrevData = $("#hidPopDefaultValue").val();
                                        
                    var _vSaveFileName = $("#PopupContent_txtFileName").val();

                    _vSaveFileName = _vSaveFileName.substr(0, _vSaveFileName.length - 1);

                    $("#PopupContent_txtFileName").val(_vSaveFileName);

                    var jsonData = JSON.stringify({ sParams: vTitle, sParams2: vContent, sParams3: vUseYN, sParams4: vNum, sParams5: vPrevData });

                    $.ajax({
                        type: "POST",
                        url: "Notice002.aspx/SetNoticeInfoAdd",
                        data: jsonData,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            fn_AddRtn(msg.d);
                        }
                    });
                    // */

                    return true;
                } else {
                    return false;
                }
            }
        }

        function fn_AddRtn(_val) {
            if (_val == "N") {
                fn_AddNoInfo();
            }

            if (_val == "C") {
                // fn_AddComplete();

                $("#PopupContent_btnUpload").click();
            }

            if (_val == "E") {
                fn_AddError();
            }

            fn_loadingEnd();
        }

        function fn_AddNoInfo() {
            alert("공지사항 정보가 올바르지 않습니다. 관리자에게 문의바랍니다."); parent.fn_ModalCloseDiv();
        }

        function fn_AddComplete() {
            alert("정상수정 되었습니다."); parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv();
        }

        function fn_AddError() {
            alert("수정에 실패하였습니다. 관리자에게 문의바립니다.");
        }        

        function fn_fileDel(_val, _val2, _val3, _val4, _val5) {
            if (_val3 == "P") {
                if (confirm("서버에 업로드된 파일을 삭제합니다. 삭제처리된 파일은 되돌릴수 없습니다. 삭제 하시겠습니까?")) {
                    $("#tr" + _val).remove();

                    var vNum = $("#PopupContent_lbNoticeNum").text();
                    var vPath = $("#PopupContent_txtFilePath").val();

                    var jsonData = JSON.stringify({ sParams: vNum, sParams2: _val4, sParams3: _val5, sParams4: vPath });

                    $.ajax({
                        type: "POST",
                        url: "Notice002.aspx/SetNoticeFileInfoDel",
                        data: jsonData,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            fn_FileDelRtn(msg.d);
                        }
                    });


                }
            } else {
                if (confirm("선택한 파일을 삭제하시겠습니까?")) {
                    $("#tr" + _val).remove();

                    var _vSaveFileName = $("#PopupContent_txtFileName").val();

                    _vSaveFileName = _vSaveFileName.replace(_val + "." + _val2, "");

                    $("#PopupContent_txtFileName").val(_vSaveFileName);
                }
            }
        }

        function fn_FileDelRtn(_val) {
            if (_val == "D") {
                alert("파일이 삭제되었습니다.");
            }

            if (_val == "E") {
                alert("파일 삭제에 실패하였습니다. 관리자에게 문의 바랍니다."); parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv();
            }
        }


        function fn_FindFile() {
            const fileDialog = $('#PopupContent_fuUpload');
            fileDialog.click();
        }

        var onFileSelected = function (e) {
            var _vSaveFileName = "";

            var _vMB = 1048576 * 2;

            var len = $('#PopupContent_fuUpload').get(0).files.length;

            var _rowCnt = $("#tbFileList tbody tr").length;

            var _pFileName = "";
            var _pFileSize = "";

            var _vFileChk = 1;

            for (var j = 0; j < len; j++) {
                var _vFileInfo = $('#PopupContent_fuUpload').get(0).files[j];

                var _vFileName = _vFileInfo.name;
                var _vFileType = _vFileInfo.type;
                var _vFileSize = _vFileInfo.size;

                for (var i = 0; i < _rowCnt; i++) {
                    _pFileName = $("#tbFileList tbody tr:eq(" + i + ") > td:eq(2)").html();
                    _pFileSize = $("#tbFileList tbody tr:eq(" + i + ") > td:eq(4)").html();

                    if ((_pFileName == _vFileName) && (_pFileSize == _vFileSize)) {

                        alert("이미 첨부된 파일이 있습니다. 파일을 다시 선택하세요.");

                        _vFileChk = 0;

                        break;
                    }
                }
            }

            if (_vFileChk == 1) {
                var _tCnt = _rowCnt + len;

                if (_tCnt > 5) {
                    alert("첨부파일은 최대 5개까지만 가능합니다. 파일을 다시 선택해주세요.");
                    return false;
                }

                for (var i = 0; i < len; i++) {

                    var _vFileInfo = $('#PopupContent_fuUpload').get(0).files[i];

                    var _vFileName = _vFileInfo.name;
                    var _vFileType = _vFileInfo.type;
                    var _vFileSize = _vFileInfo.size;

                 
                    if (_vMB < _vFileSize) {

                        alert("첨부한 파일의 용량이 큽니다. 다른 파일을 선택하시기 바랍니다.");

                        fn_AllFileDel();

                        break;
                    }

                    var _vFileIcon = "";

                    var _vFileInfo = _vFileName.split('.');


                    if (_vFileInfo[1] == "png" || _vFileInfo[1] == "gif" || _vFileInfo[1] == "jpg") {
                        _vFileIcon = "image";
                    }

                    if (_vFileInfo[1] == "ppt" || _vFileInfo[1] == "pptx") {
                        _vFileIcon = "powerpoint";
                    }

                    if (_vFileInfo[1] == "xls" || _vFileInfo[1] == "xlsx") {
                        _vFileIcon = "excel";
                    }

                    if (_vFileInfo[1] == "doc" || _vFileInfo[1] == "docx") {
                        _vFileIcon = "word";
                    }

                    var _ValText = "";

                    _ValText += "<tr style=\"height=30px;\"  id='tr" + _vFileInfo[0] + "'>";
                    _ValText += "   <td><img src=\"/images/btn/delete.png\" style=\"width:15px;height:15px;padding-bottom:2px;\" onclick=\"javascript:fn_fileDel('" + _vFileInfo[0] + "', '" + _vFileInfo[1] + "', 'N', '');\" /></td>";
                    _ValText += "   <td><img src=\"/images/btn/" + _vFileIcon + "_23.png\" /></td>";
                    _ValText += "   <td style=\"text-align:left; padding-left:10px;\">" + _vFileName + "</td>";
                    _ValText += "   <td style=\"display:none;\">N</td>";
                    _ValText += "   <td style=\"display:none;\">" + _vFileSize + "</td>";
                    _ValText += "</tr> ";

                    $("#tbFileList > tbody:last").append(_ValText);

                    _vSaveFileName += _vFileName + ",";                    
                }

                $("#PopupContent_txtFileName").val(_vSaveFileName);
            }
        }

        function fn_AllFileDel() {
            var _rowCnt = $("#tbFileList tbody tr").length;

            for (var i = _rowCnt; i >= 1; i--) {
                $("#tbFileList tbody tr").eq(i).remove();
            }
        }
    </script>

</asp:Content>
