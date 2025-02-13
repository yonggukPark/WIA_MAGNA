<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="PublicSplitSample1.aspx.cs" Inherits="HQCWeb.SystemMgt.PublicSplitSample1" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:TextBox ID="hidMainGridHeight" runat="server" style="display:none;" />
    <asp:TextBox ID="hidDetailGriddHeight" runat="server" style="display:none;" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_09000</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtCondi1" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" OnClientClick="fn_WatingCall();"  />
                        <asp:Button ID="btnDetailSearch" runat="server" OnClick="btnDetailSearch_Click" Text="Search" OnClientClick="fn_WatingCall();" style="display:none;" />
                    </td>
                </tr>
            </table>
        </div>

        <br />

        <div class="col2">
            <div class="tbl">
                <p class="sub_tit">SCREEN</p><!--통계수치 테이블의 타이틀-->

                <div id="divMainGrid">
                    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--<uc1:GridControl runat="server" ID="MainGrid" Horizontal="Y" Height="660" />--%>

<div class="grid_wrap"><!--<div class="grid_wrap mt14">-->
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:20px;" />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                </colgroup>
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>ID</th>
                                        <th>한국어</th>
                                        <th>영어</th>
                                        <th>DIC_TXT_LO</th>
                                        <th>REGIST_DATE</th>
                                        <th>REGIST_USER_NM</th>
                                        <th>MODIFY_DATE</th>
                                        <th>MODIFY_USER_NM</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>오류 메시지</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">QCELL_MODULE_TYPE</td>
                                        <td>오류 메시지</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">QCELL_MODULE_TYPE</td>
                                        <td>오류 메시지</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>

            <div class="tbl ml20">
                <p class="sub_tit">LIFETIME</p><!--통계수치 테이블의 타이틀-->

                <div id="divDetailGrid">
                    <asp:HiddenField ID="hidParams" runat="server" />

                    <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--<uc1:GridControl runat="server" ID="DetailGrid" Horizontal="Y" Height="660" />--%>

<div class="grid_wrap"><!--<div class="grid_wrap mt14">-->
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:20px;" />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                    <col />
                                </colgroup>
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>ID</th>
                                        <th>한국어</th>
                                        <th>영어</th>
                                        <th>DIC_TXT_LO</th>
                                        <th>REGIST_DATE</th>
                                        <th>REGIST_USER_NM</th>
                                        <th>MODIFY_DATE</th>
                                        <th>MODIFY_USER_NM</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>오류 메시지</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">QCELL_MODULE_TYPE</td>
                                        <td>오류 메시지</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">QCELL_MODULE_TYPE</td>
                                        <td>오류 메시지</td>
                                        <td>Error Message</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="al-l">DIC_TXT_EN</td>
                                        <td>최종갱신시간</td>
                                        <td>HeartitDate</td>
                                        <td>최종갱신시간</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                        <td>2023-10-13</td>
                                        <td>LKC</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnDetailSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function fn_Detail(_val) {
            $("#MainContent_hidParams").val(_val);
            $("#MainContent_btnDetailSearch").click();
        }
    </script>

</asp:Content>
