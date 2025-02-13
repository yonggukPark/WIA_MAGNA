<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Info30_p01.aspx.cs" Inherits="HQCWeb.InfoMgt.Info30.Info30_p01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("SHOP CODE를 선택해주세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인을 선택해주세요.");
                return false;
            } else if ($("#PopupContent_ddlDevCd").val() == "") {
                alert("장비를 선택해주세요.");
                return false;
            } else if ($("#PopupContent_ddlCarType").val() == "") {
                alert("차종을 선택해주세요.");
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

        //스크롤 초기화
        jQuery(document).ready(function ($) {
            initialize();
        });

        function initialize() {
            $(".popup_wrap").css("overflow-y", "scroll");
        }

    </script>    

	<!--// POPUP -->
    <div class="popup_wrap" tabindex="0">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Info30</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                <!--// Table -->
                    <table cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:60px;" />
                            <col style="width:250px;" />
                            <col style="width:60px;" />
                            <col style="width:250px;" />
                            <col style="width:60px;" />
                            <col style="width:250px;" />
                            <col style="width:60px;" />
                            <col style="width:250px;" />
                        </colgroup>
                        <tr>
                            <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <th><b style="color:red;">*</b><asp:Label ID="lbDevId" runat="server"></asp:Label></th>
                            <td>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlDevCd" runat="server" OnSelectedIndexChanged="ddlDevId_ddlCarType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                            <td>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCarType" runat="server" OnSelectedIndexChanged="ddlDevId_ddlCarType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr> <!-- ITEM 시작 -->
                        <tr>
                            <th><asp:Label ID="lbItem001" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem001" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem002" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem002" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem003" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem003" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem004" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem004" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem005" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem005" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem006" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem006" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem007" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem007" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem008" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem008" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem009" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem009" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem010" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem010" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem011" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem011" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem012" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem012" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem013" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem013" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem014" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem014" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem015" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem015" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem016" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem016" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem017" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem017" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem018" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem018" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem019" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem019" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem020" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem020" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem021" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem021" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem022" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem022" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem023" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem023" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem024" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem024" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem025" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem025" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem026" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem026" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem027" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem027" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem028" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem028" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem029" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem029" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem030" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem030" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem031" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem031" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem032" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem032" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem033" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem033" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem034" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem034" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem035" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem035" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem036" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem036" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem037" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem037" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem038" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem038" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem039" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem039" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem040" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem040" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem041" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem041" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem042" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem042" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem043" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem043" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem044" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem044" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem045" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem045" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem046" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem046" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem047" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem047" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem048" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem048" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem049" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem049" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem050" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem050" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem051" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem051" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem052" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem052" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem053" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem053" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem054" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem054" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem055" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem055" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem056" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem056" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem057" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem057" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem058" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem058" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem059" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem059" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem060" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem060" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem061" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem061" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem062" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem062" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem063" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem063" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem064" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem064" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem065" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem065" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem066" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem066" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem067" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem067" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem068" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem068" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem069" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem069" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem070" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem070" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem071" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem071" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem072" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem072" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem073" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem073" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem074" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem074" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem075" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem075" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem076" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem076" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem077" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem077" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem078" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem078" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem079" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem079" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem080" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem080" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem081" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem081" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem082" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem082" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem083" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem083" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem084" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem084" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem085" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem085" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem086" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem086" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem087" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem087" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem088" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem088" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem089" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem089" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem090" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem090" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem091" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem091" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem092" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem092" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem093" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem093" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem094" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem094" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem095" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem095" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem096" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem096" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem097" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem097" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem098" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem098" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem099" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem099" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem100" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem100" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem101" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem101" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem102" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem102" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem103" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem103" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem104" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem104" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem105" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem105" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem106" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem106" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem107" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem107" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem108" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem108" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem109" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem109" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem110" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem110" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem111" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem111" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem112" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem112" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem113" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem113" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem114" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem114" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem115" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem115" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem116" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem116" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem117" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem117" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem118" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem118" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem119" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem119" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem120" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem120" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem121" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem121" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem122" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem122" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem123" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem123" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem124" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem124" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem125" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem125" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem126" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem126" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem127" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem127" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem128" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem128" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem129" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem129" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem130" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem130" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem131" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem131" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem132" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem132" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem133" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem133" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem134" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem134" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem135" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem135" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem136" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem136" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem137" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem137" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem138" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem138" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem139" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem139" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem140" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem140" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem141" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem141" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem142" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem142" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem143" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem143" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem144" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem144" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem145" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem145" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem146" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem146" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem147" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem147" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem148" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem148" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem149" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem149" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem150" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem150" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem151" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem151" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem152" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem152" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem153" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem153" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem154" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem154" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem155" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem155" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem156" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem156" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem157" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem157" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem158" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem158" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem159" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem159" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem160" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem160" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem161" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem161" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem162" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem162" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem163" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem163" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem164" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem164" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem165" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem165" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem166" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem166" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem167" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem167" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem168" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem168" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem169" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem169" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem170" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem170" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem171" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem171" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem172" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem172" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem173" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem173" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem174" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem174" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem175" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem175" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem176" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem176" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem177" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem177" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem178" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem178" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem179" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem179" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem180" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem180" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem181" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem181" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem182" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem182" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem183" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem183" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem184" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem184" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem185" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem185" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem186" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem186" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem187" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem187" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem188" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem188" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem189" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem189" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem190" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem190" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem191" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem191" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem192" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem192" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem193" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem193" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem194" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem194" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem195" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem195" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem196" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem196" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem197" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem197" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem198" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem198" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem199" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem199" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem200" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem200" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem201" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem201" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem202" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem202" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem203" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem203" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem204" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem204" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem205" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem205" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem206" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem206" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem207" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem207" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem208" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem208" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem209" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem209" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem210" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem210" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem211" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem211" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem212" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem212" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem213" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem213" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem214" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem214" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem215" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem215" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem216" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem216" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem217" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem217" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem218" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem218" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem219" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem219" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem220" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem220" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem221" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem221" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem222" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem222" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem223" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem223" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem224" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem224" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem225" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem225" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem226" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem226" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem227" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem227" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem228" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem228" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem229" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem229" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem230" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem230" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem231" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem231" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem232" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem232" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem233" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem233" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem234" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem234" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem235" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem235" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem236" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem236" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem237" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem237" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem238" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem238" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem239" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem239" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem240" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem240" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem241" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem241" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem242" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem242" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem243" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem243" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem244" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem244" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem245" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem245" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem246" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem246" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem247" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem247" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem248" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem248" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem249" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem249" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem250" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem250" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem251" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem251" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem252" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem252" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem253" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem253" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem254" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem254" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem255" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem255" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem256" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem256" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem257" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem257" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem258" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem258" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem259" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem259" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem260" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem260" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem261" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem261" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem262" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem262" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem263" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem263" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem264" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem264" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem265" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem265" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem266" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem266" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem267" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem267" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem268" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem268" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem269" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem269" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem270" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem270" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem271" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem271" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem272" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem272" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem273" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem273" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem274" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem274" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem275" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem275" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem276" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem276" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem277" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem277" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem278" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem278" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem279" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem279" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem280" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem280" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem281" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem281" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem282" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem282" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem283" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem283" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem284" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem284" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem285" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem285" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem286" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem286" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem287" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem287" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem288" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem288" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem289" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem289" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem290" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem290" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem291" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem291" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem292" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem292" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem293" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem293" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem294" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem294" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem295" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem295" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem296" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem296" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>

                        <tr>
                            <th><asp:Label ID="lbItem297" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem297" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem298" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem298" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem299" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem299" runat="server" MaxLength="25"></asp:TextBox></td>
                            <th><asp:Label ID="lbItem300" runat="server"></asp:Label></th>
                            <td><asp:TextBox ID="txtItem300" runat="server" MaxLength="25"></asp:TextBox></td>
                        </tr>
                        <tr> <!-- ITEM 종료 -->
                            <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlUseYN" runat="server"></asp:DropDownList>
                            </td>
                            <td colspan="6"></td>
                        </tr>
                    </table>
                <!-- Table //-->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlDevCd" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>

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
