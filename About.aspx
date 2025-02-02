<%@ Page Title="The Wacky Website" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApp1.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>&nbsp;</h2>
    <p>Here are some cool buttons and text boxes. They do as the project tells you to.</p>
<p>&nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Load Data" />
</p>
<p>&nbsp;<asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Clear Data" />
</p>
<p>&nbsp;First Name: <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
&nbsp;</p>
<p>&nbsp;Last Name: <asp:TextBox ID="TextBox2" runat="server" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
</p>
<p>
    <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Query" />
</p>
<p>
<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay">
            <div id="modalprogress">
                <div id="theprogress">
                    <asp:Image ID="Image1" runat="server" Height="64px" ImageUrl="~/load_gif.gif" Width="64px" />
                    <br />
                    Please wait...
                </div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

    <p dir="ltr">
    <asp:Label ID="Label1" runat="server"></asp:Label>
</p>

</asp:Content>
