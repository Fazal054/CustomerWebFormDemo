<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CustomerWebFormDemo._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<main>

        <div class="row">
            <section>
               
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:Button ID="btnAddStudent" runat="server" Text="Add Student"
                    CssClass="btn btn-primary"
                    OnClientClick="openModal(); return false;" />
                <asp:Button ID="btnSend" runat="server" Text="Send Notification" OnClick="btnSend_Click" />
                <asp:Label ID="lblNotification" runat="server" ForeColor="Green"></asp:Label>
                <asp:GridView ID="GridView1" runat="server"
    DataKeyNames="Id"
    AllowPaging="true"
    PageSize="5"
    OnPageIndexChanging="GridView_PageIndexChanging"
    AutoGenerateColumns="false"
    OnRowEditing="GridView1_RowEditing"
    OnRowCancelingEdit="GridView1_RowCancelingEdit"
    >
    
    <Columns>
        <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" />

        <asp:TemplateField HeaderText="Name">
            <ItemTemplate>
                <%# Eval("Name") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Age">
            <ItemTemplate>
                <%# Eval("Age") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtAge" runat="server" Text='<%# Bind("Age") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Email">
            <ItemTemplate>
                <%# Eval("Email") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Course">
            <ItemTemplate>
                <%# Eval("Course") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtCourse" runat="server" Text='<%# Bind("Course") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
    </Columns>
</asp:GridView>

 
            </section>
        </div>

    <div class="row">
        <section>
              <div id="chatbot" style="position:fixed; bottom:20px; right:20px; width:300px; background:#fff; border:1px solid #ccc; border-radius:10px;">
    
    <div style="background:#007bff; color:white; padding:10px;">
        Campus Bot
    </div>

    <div id="chatBody" style="height:250px; overflow-y:auto; padding:10px;"></div>

    <div style="display:flex;">
        <input type="text" id="userInput" style="flex:1;" placeholder="Ask something..." />
        <button type="button" onclick="sendMessage()">Send</button>
    </div>

</div>
        </section>
    </div>
    </main>
<script>
    function openModal() {
        var myModal = new bootstrap.Modal(document.getElementById('studentModal'));
        myModal.show();
    }

    function sendMessage() {
        
        var message = document.getElementById("userInput").value;

        if (!message) return;

        document.getElementById("chatBody").innerHTML += "<div><b>You:</b> " + message + "</div>";

        fetch("Default.aspx/GetBotResponse", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify({ userMessage: message })
        })
            .then(response => response.json())
            .then(data => {
                document.getElementById("chatBody").innerHTML += "<div><b>Bot:</b> " + data.d + "</div>";
                document.getElementById("userInput").value = "";
            })
            .catch(error => console.log("Error:", error));
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

                <asp:DropDownList ID="ddlCourse" runat="server"></asp:DropDownList><br /><br />

                <asp:Button ID="Button1" runat="server"
                    Text="Save"
                    CssClass="btn btn-success"
                    OnClick="btnSubmit_Click" />

            </div>

        </div>
    </div>
</div>
</asp:Content>