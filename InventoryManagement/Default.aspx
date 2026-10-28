
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="InventoryManagement._Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Inventory Management</title>
    <style>
        body {
            font-family: Arial;
            background: #eef2f7;
            margin: 30px;
        }
        .container {
            max-width: 1000px;
            margin: auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
        }
        h1 { color: #24476b; }
        .field { margin: 12px 0; }
        .field label {
            display: inline-block;
            width: 125px;
        }
        .field input {
            width: 250px;
            padding: 8px;
        }
        .btn {
            padding: 8px 15px;
            margin: 5px;
            cursor: pointer;
        }
        .grid { margin-top: 25px; }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="container">

    <h1>Inventory Management System</h1>
    <h3>Add or Update Product</h3>

    <div class="field">
        <label>Product Name:</label>
        <asp:TextBox ID="txtName" runat="server" />
    </div>

    <div class="field">
        <label>Category:</label>
        <asp:TextBox ID="txtCategory" runat="server" />
    </div>

    <div class="field">
        <label>Price:</label>
        <asp:TextBox ID="txtPrice" runat="server"
            TextMode="Number" />
    </div>

    <div class="field">
        <label>Quantity:</label>
        <asp:TextBox ID="txtQuantity" runat="server"
            TextMode="Number" />
    </div>

    <asp:Button ID="btnSave" runat="server"
        Text="Add Product" CssClass="btn"
        OnClick="btnSave_Click" />

    <asp:Button ID="btnClear" runat="server"
        Text="Clear" CssClass="btn"
        CausesValidation="false"
        OnClick="btnClear_Click" />

    <br /><br />
    <asp:Label ID="lblMessage" runat="server" />

    <h3>Available Products</h3>

    <div class="grid">
    <asp:GridView ID="gvProducts" runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="ProductID"
        CellPadding="8"
        GridLines="Both"
        Width="100%"
        OnRowCommand="gvProducts_RowCommand"
        HeaderStyle-BackColor="#24476b"
        HeaderStyle-ForeColor="White"
        AlternatingRowStyle-BackColor="#eef2f7">

        <Columns>
            <asp:BoundField DataField="ProductID"
                HeaderText="ID" />

            <asp:BoundField DataField="ProductName"
                HeaderText="Product Name" />

            <asp:BoundField DataField="Category"
                HeaderText="Category" />

            <asp:BoundField DataField="Price"
                HeaderText="Price" DataFormatString="{0:N2}" />

            <asp:BoundField DataField="Quantity"
                HeaderText="Quantity" />

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server"
                        Text="Edit" CssClass="btn"
                        CausesValidation="false"
                        CommandName="EditProduct"
                        CommandArgument='<%# Eval("ProductID") %>' />

                    <asp:Button ID="btnDelete" runat="server"
                        Text="Delete" CssClass="btn"
                        CausesValidation="false"
                        CommandName="DeleteProduct"
                        CommandArgument='<%# Eval("ProductID") %>'
                        OnClientClick="return confirm('Delete this product?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>

</div>
</form>
</body>
</html>
