<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info04_p02.aspx.cs" Inherits="HQCWeb.InfoMgt.Info04.Info04_p02" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">

        function fn_ModifyConfirm() {

            if (confirm("수정 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_DeleteConfirm() {

            if (confirm("삭제 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_RestoreConfirm() {

            if (confirm("복구 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_Modify() {
            $("#PopupContent_btnModify").click();
        }

        function fn_Delete() {
            $("#PopupContent_btnDelete").click();
        }

        function fn_Restore() {
            $("#PopupContent_btnRestore").click();
        }

        function delHangle(evt) { //한글을 지우는 부분, keyup부분에 넣어준다.
            var objTarget = evt.srcElement || evt.target;
            var _value = event.srcElement.value;
            if (/[ㄱ-ㅎㅏ-ㅡ가-핳]/g.test(_value)) {
                //  objTarget.value = objTarget.value.replace(/[ㄱ-ㅎㅏ-ㅡ가-핳]/g,''); // g가 핵심: 빠르게 타이핑할때 여러 한글문자가 입력되어 버린다.
                objTarget.value = null;
                //return false;
            }
        }

        $(document).ready(function () {
            fn_Datepicker_Reset();
        });

        function fn_Datepicker_Reset() {
            $("#PopupContent_txtDate").datepicker({
                dateFormat: "yy-mm-dd"
            });
        }

        function fn_Check() {
            $("#PopupContent_btnClear").click();
        }

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info04</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetLineCd" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbLineNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtLineNm" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbCycTm" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtCycTm" runat="server" type="number" min="0" step="1" onkeyup="return delHangle(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDispGd" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDispGd" runat="server" type="number" min="0" step="1" onkeyup="return delHangle(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbWctYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlWctYn" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbEopFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEopFlag" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbEopDate" runat="server"></asp:Label></th>
                    <td>
                        <table>
                            <tr>
                                <td style="padding:0px;width:90px;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"/>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="padding:0px;">
                                    <div class = "td_wrap">
                                        <a href="javascript:fn_Check();" class="btn ml10" id="aCheck" runat="server" >Clear</a>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark1" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark1" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbRemark2" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtRemark2" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
                <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
                <a href="javascript:fn_Restore();" class="btn ml10" id="aRestore" runat="server" visible="false">Restore</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
                <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
                <asp:Button ID="btnRestore"  runat="server" OnClientClick="javascript:return fn_RestoreConfirm();" OnClick="btnRestore_Click" Text="Restore" style="display:none;" />
                <asp:Button ID="btnClear"    runat="server" OnClick="btnClear_Click" Text="Clear" style="display:none;"/>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
