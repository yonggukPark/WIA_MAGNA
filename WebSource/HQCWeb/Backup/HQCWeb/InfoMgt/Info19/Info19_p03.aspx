<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info19_p03.aspx.cs" Inherits="HQCWeb.InfoMgt.Info19.Info19_p03" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript" src="/Scripts/jquery-ui.js"></script>
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

        $(document).ready(function () {
            enableDrag();
        });

        //"drag" 클래스인 div버튼의 drag & drop을 onload시 지정
        function enableDrag() {
            $(".drag").draggable({
                stop: function (event, ui) {

                    // 각 div의 위치와 id를 hidden input에 저장
                    var id = $(this).attr('id');
                    var position = ui.position; // { top: xx, left: xx }
                    $('#PopupContent_hidPointTop').val(position.top + "px");
                    $('#PopupContent_hidPointLeft').val(position.left + "px");
                    $('#PopupContent_hidPointID').val(id);

                    //숨긴 버튼으로 클릭
                    $('#PopupContent_btn2').click();
                }
            });
        }

        function imagePreview(input) {
            if (input) {
                <%= ClientScript.GetPostBackEventReference(btn1, string.Empty) %>
            }
        }

    </script>    
    
    <asp:HiddenField ID="hidPointTop" runat="server"/>
    <asp:HiddenField ID="hidPointLeft" runat="server"/>
    <asp:HiddenField ID="hidPointID" runat="server"/>
	<!--// POPUP -->
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info19</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
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
                        <asp:DropDownList ID="ddlShopCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStnCd" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbWorkSeq" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lbGetWorkSeq" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbImgMain" runat="server"></asp:Label></th>
                   <td>
                       <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
						<ContentTemplate>
                            <div id="divImg" runat="server" style="position:relative; width:351px; height:408px; left:0px; top:0px; z-index:1; padding-top: 0px; padding-bottom:0px">
                                <asp:Image ID="image1" runat="server" BorderWidth="1px" style="left:0px; top:0px"/>
                            </div>
                            <div style="display:none;">
                                <asp:Button ID="btn1" runat="server" OnClick="btn1_Click"/>
                                <asp:Button ID="btn2" runat="server" OnClick="btn2_Click" />
	                        </div>
                        </ContentTemplate>
						<Triggers>
							<asp:PostBackTrigger ControlID="btn1" />
							<asp:AsyncPostBackTrigger ControlID="btn2" EventName="Click" />
						</Triggers>
					</asp:UpdatePanel> 
                   </td>
	           </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbImgFile" runat="server"></asp:Label></th>
                    <td>
                        <input type="file" ID="fileupload" runat="server" style="width:351px; height:25px" onchange="imagePreview(this);"/>
                       
                    </td>
                </tr>
            </table>
            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Save();" class="btn ml10">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
