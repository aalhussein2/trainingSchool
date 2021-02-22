<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="registration.aspx.cs"
    Inherits="trainingSchoolv2.school.registration" EnableEventValidation = "false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <table class="nav-justified">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblOutput" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 175px">Member ID</td>
            <td>
                <asp:TextBox ID="txtMemberId" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 175px">First Name</td>
            <td>
                <asp:TextBox ID="txtFName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 175px">Middle Name</td>
            <td>
                <asp:TextBox ID="txtMName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 175px; height: 22px">Last Name</td>
            <td style="height: 22px">
                <asp:TextBox ID="txtLName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 175px">Cell</td>
            <td>
                <asp:TextBox ID="txtCell" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 175px">Email</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 175px">Gender</td>
            <td>
                <asp:DropDownList ID="ddlGender" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 175px">&nbsp;</td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" />
                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
                <asp:Button ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="Select " />
                <br />
            </td>
        </tr>
        <tr>
            <td style="width: 175px">&nbsp;</td>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click" Text="ExportToExcel" />
                <asp:Button ID="btnExportToWord" runat="server" OnClick="btnExportToWord_Click" Text="ExportToWord" />
                <asp:Button ID="btnExportToPdf" runat="server" OnClick="btnExportToPdf_Click" Text="ExportToPdf" />
            </td>
        </tr>
        <tr>
            <td style="width: 175px">&nbsp;</td>
            <td>
                <asp:Button ID="btnExportToExcelCls" runat="server"  Text="ExportToExcelCls" OnClick="btnExportToExcelCls_Click" />
                <asp:Button ID="btnExportToWordCls" runat="server"  Text="ExportToWordCls" OnClick="btnExportToWordCls_Click" />
                <asp:Button ID="btnExportToPdfCls" runat="server"  Text="ExportToPdfCls" OnClick="btnExportToPdfCls_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvMember" runat="server" AutoGenerateColumns="False" DataKeyNames="memberId" >
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center"
                HeaderText="memberId">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkupdate" runat="server"
                        CommandArgument='<%# Bind("memberId") %>'
                        OnClick="populateForm_Click"
                        Text='<%# Eval("memberId")  %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="left"></ItemStyle>
            </asp:TemplateField>
            <asp:BoundField DataField="fName" HeaderText="fName" SortExpression="fName" />
            <asp:BoundField DataField="mName" HeaderText="mName" SortExpression="mName" />
            <asp:BoundField DataField="cell" HeaderText="cell" SortExpression="cell" />
            <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
<%--            <asp:BoundField DataField="genderId" HeaderText="genderId" SortExpression="genderId" />--%>
            <asp:BoundField DataField="gender" HeaderText="gender" SortExpression="gender" />
        </Columns>
    </asp:GridView>
</asp:Content>
