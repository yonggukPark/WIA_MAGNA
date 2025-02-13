<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="OM_002_08.aspx.cs" Inherits="HQCWeb.Cell.Product.OM_002_08" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo_2{
            text-align: center;
            z-index : 10;
            top : 40%;
            position : absolute; 
        }
    </style>

    <style>
        .searchCombo_3{
            text-align: center;
            z-index : 1;
            top : 72%;
            position : absolute; 
        }
    </style>

    <script>
        function fn_hide() {
            if ($("#div_Search").css("display") == "none") {
                $("#div_Search").show();

                if (screen.width == 1280) {
                    $(".dxgvCSD").attr("style", "height:555px; overflow: scroll;");
                } else {
                    $(".dxgvCSD").attr("style", "height:625px; overflow: scroll;");
                }
            } else {
                $("#div_Search").hide();

                if (screen.width == 1280) {
                    $(".dxgvCSD").attr("style", "height:691px; overflow: scroll;");
                } else {
                    $(".dxgvCSD").attr("style", "height:761px; overflow: scroll;");
                }
            }
        }
    </script>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_11100</asp:Label></h3>
            <ul class="win_btn">
                <li><a href="javascript:fn_hide();" class="reduce">축소</a></li>
            </ul>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13" id="div_Search">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:160px;" />
                    <col style="width:100px;" />
                    <col style="width:150px;" />
                    <col style="width:100px;" />
                    <col style="width:370px;" />
                    <col/>
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbWorkDate" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtDate" runat="server" style="width:80px; z-index:200;"></asp:TextBox></td>

                    <th><asp:Label ID="lbLine" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLine" runat="server"></asp:DropDownList>

                    </td>

                    <th><asp:Label ID="lbOperation" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlOperation" runat="server"></asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPOClass" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPOClass" runat="server"></asp:DropDownList>
                    </td>

                    <th><asp:Label ID="lbPOType" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:125px;">
                            <asp:TextBox id="txtPOType" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbPO" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:350px;">
                            <asp:TextBox id="txtPO" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                            <asp:TextBox id="txtPOHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>

                </tr>

                <tr>
                    <th><asp:Label ID="lbSize" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_3" style="width:140px;">
                            <asp:TextBox id="txtSize" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbBusBar" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_3" style="width:125px;">
                            <asp:TextBox id="txtBusBar" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbSpecial" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_3" style="width:125px;">
                            <asp:TextBox id="txtSpecial" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();"  Visible="false"  />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div id="div_grid" style="height:555px;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:GridControl runat="server" ID="grid" Horizontal="Y" RowColorUsed= "Y" RowColorUsedColumn="PROCESS_SEGMENT_ID" RowColorCheckWord="SubTotal"  />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <script>

        if (screen.width == 1280) {
            $("#MainContent_hidGridHeight").val("555");
        }
        else {
            $("#MainContent_hidGridHeight").val("625");
        }

        jQuery(document).ready(function ($) {
            if (screen.width == 1280) {
                $(".dxgvCSD").attr("style", "height:555px; overflow: scroll;");
            } else {
                $(".dxgvCSD").attr("style", "height:625px;  overflow: scroll;");
            }

            $("#MainContent_txtPOType").comboTree({
                source: <%=strPOTypeJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: false
            });

            $("#MainContent_txtPO").comboTree({
                source:<%=strPOJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true, 
                valueChange: true,
                hidCon: "MainContent_txtPOHidden"
            });

            $("#MainContent_txtSize").comboTree({
                source: <%=strSizeJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: false
            });

            $("#MainContent_txtBusBar").comboTree({
                source: <%=strBusBarJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true, 
                valueChange: false
            });

            $("#MainContent_txtSpecial").comboTree({
                source: <%=strSpecialJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: false
            });

        });
    </script>
</asp:Content>