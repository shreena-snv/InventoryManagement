
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class _Default : System.Web.UI.Page
    {
        private readonly string cs =
            ConfigurationManager.ConnectionStrings[
                "InventoryConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        // READ: Display all products
        private void LoadProducts()
        {
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM Products ORDER BY ProductID DESC", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        // CREATE and UPDATE
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string category = txtCategory.Text.Trim();

            decimal price;
            int quantity;

            if (name == "" || category == "")
            {
                ShowMessage("Enter product name and category.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out price) ||
                price < 0)
            {
                ShowMessage("Enter a valid non-negative price.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out quantity) ||
                quantity < 0)
            {
                ShowMessage("Enter a valid non-negative quantity.");
                return;
            }

            bool isUpdate = ViewState["ProductID"] != null;

            string sql = isUpdate
                ? @"UPDATE Products
                    SET ProductName = @Name,
                        Category = @Category,
                        Price = @Price,
                        Quantity = @Quantity
                    WHERE ProductID = @ID"
                : @"INSERT INTO Products
                    (ProductName, Category, Price, Quantity)
                    VALUES (@Name, @Category, @Price, @Quantity)";

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100)
                    .Value = name;

                cmd.Parameters.Add("@Category", SqlDbType.VarChar, 50)
                    .Value = category;

                cmd.Parameters.Add("@Price", SqlDbType.Decimal)
                    .Value = price;
                cmd.Parameters["@Price"].Precision = 10;
                cmd.Parameters["@Price"].Scale = 2;

                cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                    .Value = quantity;

                if (isUpdate)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int)
                        .Value = Convert.ToInt32(ViewState["ProductID"]);
                }

                con.Open();
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    ShowMessage(isUpdate
                        ? "Product updated successfully."
                        : "Product added successfully.");

                    ClearForm();
                    LoadProducts();
                }
                else
                {
                    ShowMessage("No product was changed.");
                }
            }
        }

        // EDIT: Load selected product into the form
        protected void gvProducts_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditProduct")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT ProductID, ProductName, Category,
                             Price, Quantity
                      FROM Products WHERE ProductID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text =
                                reader["ProductName"].ToString();
                            txtCategory.Text =
                                reader["Category"].ToString();
                            txtPrice.Text =
                                reader["Price"].ToString();
                            txtQuantity.Text =
                                reader["Quantity"].ToString();

                            ViewState["ProductID"] = id;
                            btnSave.Text = "Update Product";
                            ShowMessage("Edit the details and click Update.");
                        }
                    }
                }
            }
            else if (e.CommandName == "DeleteProduct")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Products WHERE ProductID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    ShowMessage(result > 0
                        ? "Product deleted successfully."
                        : "Product not found.");

                    ClearForm();
                    LoadProducts();
                }
            }
        }

        // CLEAR: Reset the input fields
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblMessage.Text = "";
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtCategory.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";

            ViewState["ProductID"] = null;
            btnSave.Text = "Add Product";
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
        }
    }
}
