<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info59_p02.aspx.cs" Inherits="HQCWeb.InfoMgt.Info59.Info59_p02" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <style>
        .custom-height .box table td, 
        .custom-height .box table th {
            height: 35px;
            line-height: 35px;
        }
    </style>
    <script type="text/javascript">
        function fn_Validation() {
            if (confirm("등록 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }
       
        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }

        function isNumberKey(evt) { // 실수를 제외한 값을 입력하지 못하게 한다. 
            var charCode = (evt.which) ? evt.which : event.keyCode;
            // Textbox value       
            var _value = event.srcElement.value;

            if (event.keyCode < 48 || event.keyCode > 57) {
                if (!(event.keyCode == 46 || event.keyCode == 45)) { //실수와 . -만 입력가능하도록함
                    return false;
                }
            }

            // 소수점(.)이 두번 이상 나오지 못하게
            var _pattern0 = /^\d*[.]\d*$/; // 현재 value값에 소수점(.) 이 있으면 . 입력불가
            if (_pattern0.test(_value)) {
                if (charCode == 46) {
                    return false;
                }
            }

            // - 이 두번 이상 나오지 못하게
            var _pattern0 = /^\d*[-]\d*$/; // 현재 value값에 소수점(-) 이 있으면 . 입력불가
            if (_pattern0.test(_value)) {
                if (charCode == 45) {
                    return false;
                }
            }


            return true;
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

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap custom-height">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info59</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:60px;" />
                            <col style="width:100px;" />
                            <col style="width:60px;" />
                            <col style="width:100px;" />
                        </colgroup>
                        <tr>
                            <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetShopCd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetLineCd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetCarType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetStnCd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbWorkCd" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetWorkCd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbDevNm" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetDevNm" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbPSet" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetPSet" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbTorqueType" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:Label ID="lbGetTorqueType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItemGroup" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItemGroup" runat="server"></asp:DropDownList>
                            </td>
                            <th><asp:Label ID="lbChkAll" runat="server"></asp:Label></th>
                            <td>
                                <asp:RadioButtonList ID="rbListChkAll" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbListChkAll_SelectedIndexChanged"></asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem02" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem02" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem02Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem02Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem03" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem03" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem03Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem03Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem04" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem04" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem04Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem04Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem05" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem05" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem05Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem05Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem08" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem08" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem08Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem08Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem09" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem09" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem09Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem09Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem10" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem10" runat="server" MaxLength="12" ></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem10Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem10Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem12" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem12" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem12Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem12Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem13" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem13" runat="server" MaxLength="12" onkeypress="return isNumberKey(event)" onkeyup="return delHangle(event)"></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem13Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem13Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbItem14" runat="server"></asp:Label></th>
                            <td>
                                <asp:TextBox ID="txtItem14" runat="server" MaxLength="12" ></asp:TextBox>
                            </td>
                            <th><asp:Label ID="lbItem14Yn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlItem14Yn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlUseYn" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rbListChkAll" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>

            
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
