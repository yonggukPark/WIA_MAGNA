<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua51_p04.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua51.Qua51_p04" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    
    <asp:TextBox ID="txtFileName"   runat="server" style="display:none; width:100%;"></asp:TextBox>

    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_txtCertNo").val() == "") {
                alert("성적서 번호를 입력하세요.");
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

        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Qua51</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCertNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCertNo" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbIssueDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbAttachFile" runat="server"></asp:Label></th>
                    <td style="padding-left:10px;">
                        <asp:FileUpload ID="fuUpload" runat="server" AllowMultiple="true" accept=".pdf" style="display:none;"  onchange="onFileSelected();" />
                        <div class="search_btn_wrap" style="margin-bottom:10px !important; float:left; padding:0px;">
                            <a href="javascript:fn_FindFile();" class="btn ml10">파일찾기</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding:0px;">
                        <div class="contents" tabindex="0">
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
                <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
            </div>
            <div>
                첨부파일은 PDF 파일당 용량 한도 20MB까지만 가능합니다. 
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
    
    <!-- 파일처리 관련 -->
    <script>

        function fn_fileDel(_val, _val2, _val3, _val4, _val5) {
            if (confirm("선택한 파일을 삭제하시겠습니까?")) {
                //jquery 동작하지 않음
                //$("#tr" + _val).remove(); 
                document.getElementById("tr" + _val).remove();

                var _vSaveFileName = $("#PopupContent_txtFileName").val();

                _vSaveFileName = _vSaveFileName.replace(_val + "." + _val2 + ",", "");

                $("#PopupContent_txtFileName").val(_vSaveFileName);
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
            var _vSaveFileName = $('#PopupContent_txtFileName').val();

            var _vMB = 1048576 * 20;

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

                    // 마지막 점의 위치 찾기
                    const lastDotIndex = _vFileName.lastIndexOf(".");

                    // 파일명과 확장자 분리
                    const _vFname = _vFileName.substring(0, lastDotIndex).replace(/[^a-zA-Z0-9가-힣-_:.]/g, '_'); // 마지막 점 이전 부분(허용되지 않는 문자 _로 변경)
                    const extension = _vFileName.substring(lastDotIndex + 1).toLowerCase(); // 마지막 점 이후 부분

                    if (extension == "pdf") {
                        _vFileIcon = "pdf";
                    }
                    else {
                        alert("PDF 파일만 등록 가능합니다.");

                        fn_AllFileDel();

                        break;
                    }

                    var _ValText = "";

                    _ValText += "<tr style=\"height=30px;\"  id='tr" + _vFname + "'>";
                    _ValText += "   <td><img src=\"/images/btn/delete.png\" style=\"width:15px;height:15px;padding-bottom:2px;\" onclick=\"javascript:fn_fileDel('" + _vFname + "', '" + extension + "', 'N', '');\" /></td>";
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
