<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="ClientUploadDoc.aspx.cs" Inherits="trainingSchoolv2.demo.ClientUploadDoc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        This demo shows how to upload&nbsp; and save a document to a Folder &amp; to the Database&nbsp; (important) !</p>
    <table class="nav-justified">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblOutput" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="modal-sm" style="width: 134px">Client ID</td>
            <td style="width: 503px">
        <asp:TextBox ID="txtClient" runat="server" OnTextChanged="txtClient_TextChanged"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="modal-sm" style="width: 134px">Group Type </td>
            <td style="width: 503px">
        <asp:DropDownList ID="ddlgroupType" runat="server">
        </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="modal-sm" style="width: 134px">Upload docs </td>
            <td style="width: 503px">
                <asp:FileUpload ID="FileUpload" runat="server" AllowMultiple="true"
                    ToolTip="Please upload your documents" />
       
            </td>
            </tr>
        <tr>
            <td class="modal-sm" style="width: 134px">&nbsp;</td>
            <td style="width: 503px">&nbsp;</td>
            </tr>
        <tr>
            <td colspan="2">


        <asp:Button ID="btnSubmit" runat="server" Text="SubmitSingleFileToFolder" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnSubmitMultiFiles" runat="server" Text="SubmitMultiFilesToFolder" OnClick="btnSubmitMultiFiles_Click" />
                <asp:Button ID="btnSubmitMultiFilesToDb" runat="server" OnClick="btnSubmitMultiFilesToDb_Click" Text="SubmitWithUploadToDb" />
  
                <asp:Button ID="btnShowFiles" runat="server" OnClick="btnShowFiles_Click" Text="ShowFiles" />
                <asp:Button ID="ShowClientfiles" runat="server" OnClick="ShowClientfiles_Click" Text="ShowClientfiles" style="height: 26px" />
  
            </td>
        </tr>
    </table>
    <%--            DataSource='<%# GetUploadList(hiddenClientId.Value) %>'--%>
    <div>
        <p>
            <asp:Label ID="lblFileMsg" runat="server" Text=""></asp:Label></p>
        <asp:GridView
            ID="gvClientFiles"
            SkinID="UploadsView"
            runat="server" 
            AutoGenerateColumns="false" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Name">
                    <ItemStyle Width="100%" />
                    <ItemTemplate>
                        <asp:HyperLink
                            ID="FileLink"
                            NavigateUrl='<%# "~/Uploads/" + Container.DataItem.ToString() %>'
                            Text='<%# Container.DataItem.ToString() %>'
                            runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
<%--                <asp:CommandField ButtonType="Image"
                    DeleteImageUrl="~/Images/delete.png"
                    ShowDeleteButton="True">
                    <ItemStyle Width="1px" />
                </asp:CommandField>--%>
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="hiddenClientId" runat="server" />
    </div>
</asp:Content>
