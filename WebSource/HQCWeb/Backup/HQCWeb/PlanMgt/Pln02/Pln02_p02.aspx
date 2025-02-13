<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Pln02_p02.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln02.Pln02_p02" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlPlanCd").val() != "PC03" && $("#MainContent_ddlPlanCd").val() == "") {
                alert('계획상세를 선택해주세요.');
                $("#MainContent_ddlPlanDetailCd").focus();
                return false;
            } else if ($("#MainContent_lbGetOrderNo").val() >= 900000000 && $("#MainContent_ddlPlanCd").val() != "PC02") {
                alert('이벤트 오더입니다. 구분을 다시 선택해주세요.');
                $("#MainContent_ddlPlanCd").focus();
                return false;
            } else if ($("#MainContent_ddlPlanCd").val() == "PC02") {
                alert('양산입니다. 구분을 다시 선택해주세요.');
                $("#MainContent_ddlPlanCd").focus();
                return false;
            }  else {
                if (confirm("등록 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            return true;
        }

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

        function fn_RefreshConfirm() {

            if (confirm("초기화 하시겠습니까?")) {
                $("#MainContent_aSave").show();
                $("#MainContent_ddlShopCd").prop('disabled', false);
                $("#MainContent_ddlLineCd").prop('disabled', false);
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_Save() {
            $("#MainContent_btnSave").click();
        }

        function fn_Modify() {
            $("#MainContent_btnModify").click();
        }

        function fn_Delete() {
            $("#MainContent_btnDelete").click();
        }

        function fn_Refresh() {
            $("#MainContent_btnRefresh").click();
        }

        //그리드 순번 클릭
        function fn_gridInfoCall(val) {
            $("#MainContent_hidGridValue").val(val);
            $("#MainContent_btnGrid").click();
        }

    </script>    
    
	<asp:HiddenField ID="hidPlanSeq"          runat="server" />
	<asp:HiddenField ID="hidOrderType"        runat="server" />
    <asp:HiddenField ID="hidPopDefaultValue2" runat="server" />  
    <asp:HiddenField ID="hidGridValue"        runat="server" />
    
    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title" style="background-color:#053085; height:40px;">
            <h3 style="float:left; margin-left: 20px; color: #ffffff; font-size: 15px; font-weight: 400; line-height: 38px;"><asp:Label ID="lbTitle" runat="server">Pln02</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h3>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close""></a>
        </div>
        <!-- Title + Win_Btn //-->

        <!--// Grid Search -->
        <br />

        <div class="col2" style="height:630px">
            <div class="tbl" style="width:55%;">

                <div id="divMainGrid">
                    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="realMaingrid" style="width: 100%; height: 440px; display:none;"></div>
                            <div class="grid_wrap" style="height:440px; overflow-y:scroll; overflow-x:hidden;"><!--<div class="grid_wrap mt14">-->
                                <table cellpadding="0" cellspacing="0" id="tbPlanList">
                                    <colgroup>
                                        <col style="width:5px;" />
                                        <col style="width:15px;" />
                                        <col style="width:30px;" />
                                        <col style="width:30px;" />
                                        <col style="width:30px;" />
                                        <col style="width:40px;" />
                                        <col style="width:40px;" />
                                        <col style="width:40px;" />
                                        <col style="width:55px;" />
                                    </colgroup>
                                    <%=strList %>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            
            <div class="popup_wrap" style="width:40%; height:620px">
                    <!--// Box -->
                    <div class="box">

                        <!--// Table -->
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <th><asp:Label ID="lbOrderNo" runat="server"></asp:Label></th>
                                <td>
                                    <asp:Label ID="lbGetOrderNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbPlanDt" runat="server"></asp:Label></th>
                                <td>
                                    <asp:Label ID="lbGetPlanDt" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                                <td>
                                    <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlLineCd" runat="server"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                                <td>
                                    <asp:Label ID="lbGetCarType" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                                <td>
                                    <asp:Label ID="lbGetPartNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbStatusFlg" runat="server"></asp:Label></th>
                                <td>
                                    <asp:DropDownList ID="ddlStatusFlg" runat="server" ></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbPlanQty" runat="server"></asp:Label></th>
                                <td>
                                    <asp:Label ID="lbGetPlanQty" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbPlanDQty" runat="server"></asp:Label></th>
                                <td>
                                    <asp:TextBox ID="txtPlanDQty" runat="server" type="number"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbPlanNQty" runat="server"></asp:Label></th>
                                <td>
                                    <asp:TextBox ID="txtPlanNQty" runat="server" type="number"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbFinishFlg" runat="server"></asp:Label></th>
                                <td>
                                    <asp:DropDownList ID="ddlFinishFlg" runat="server" ></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbPlanCd" runat="server"></asp:Label></th>
                                <td>
                                    <asp:DropDownList ID="ddlPlanCd" runat="server" OnSelectedIndexChanged="ddlPlanCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbPlanDetailCd" runat="server"></asp:Label></th>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPlanDetailCd" runat="server"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlPlanCd" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="lbRemark" runat="server"></asp:Label></th>
                                <td>
                                    <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <!-- Table //-->

                    </div>
                    <!-- Box //-->
            </div>
        </div>

        <div class="btn_wrap mt20" style="position: relative; text-align: center; height: 40px;">
            <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
            <a href="javascript:fn_Modify();" class="btn ml10" id="aModify" runat="server" visible="false">Modify</a>
            <a href="javascript:fn_Delete();" class="btn ml10" id="aDelete" runat="server" visible="false">Delete</a>
            <a href="javascript:fn_Refresh();" class="btn ml10" id="aRefresh" runat="server" visible="true">Refresh</a>
            <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

            <asp:Button ID="btnModify"  runat="server" OnClientClick="javascript:return fn_ModifyConfirm();" OnClick="btnModify_Click" Text="Modify" style="display:none;" />
            <asp:Button ID="btnDelete"  runat="server" OnClientClick="javascript:return fn_DeleteConfirm();" OnClick="btnDelete_Click" Text="Delete" style="display:none;" />
            <asp:Button ID="btnRefresh" runat="server" OnClientClick="javascript:return fn_RefreshConfirm();" OnClick="btnRefresh_Click" Text="Refresh" style="display:none;" />
            <asp:Button ID="btnSave"  OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" runat="server" Text="Save" style="display:none;" />
            <asp:Button ID="btnGrid"  OnClick="btnGrid_Click" runat="server" style="display:none;" />
        </div>
    </div>
    
    <script type="text/javascript">        
        $("div[name=divView]").width("1200px");
    </script>

</asp:Content>
