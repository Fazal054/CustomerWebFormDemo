<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CustomerWebFormDemo._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>

        <div class="row">
            <section>
               
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:Button ID="btnAddStudent" runat="server" Text="Add Student"
                    CssClass="btn btn-primary"
                    OnClientClick="openModal(); return false;" />

                <asp:GridView ID="GridView1" BorderWidth="2px" BorderStyle="Solid" BorderColor="Red" CellPadding="5" GridLines="Both" runat="server" AllowPaging="true" PageSize="5" OnPageIndexChanging="GridView_PageIndexChanging" AutoGenerateColumns="true">
                </asp:GridView>
            </section>
            
        </div>
    </main>



<script>
    function openModal() {
        var myModal = new bootstrap.Modal(document.getElementById('studentModal'));
        myModal.show();
    }
</script>
<div class="modal fade" id="studentModal">
    <div class="modal-dialog">
        <div class="modal-content">

            <div class="modal-header">
                <h4 class="modal-title">Add Student</h4>
            </div>

            <div class="modal-body">

                <asp:TextBox ID="txtName" runat="server" placeholder="Enter Name"></asp:TextBox><br />
                <asp:TextBox ID="txtAge" runat="server" placeholder="Enter Age"></asp:TextBox><br />
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter Email"></asp:TextBox><br />

                <asp:DropDownList ID="ddlCourse" runat="server">
                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    <asp:ListItem Text="C#" Value="C#"></asp:ListItem>
                    <asp:ListItem Text="Java" Value="Java"></asp:ListItem>
                    <asp:ListItem Text="Python" Value="Python"></asp:ListItem>
                    <asp:ListItem Text="JavaScript" Value="JavaScript"></asp:ListItem>  
                    <asp:ListItem Text="Sql" Value="sql"></asp:ListItem>
                </asp:DropDownList><br /><br />

                <asp:Button ID="Button1" runat="server"
                    Text="Save"
                    CssClass="btn btn-success"
                    OnClick="btnSubmit_Click" />

            </div>

        </div>
    </div>
</div>

    </asp:Content>