using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CustomerWebFormDemo
{
    public partial class _Default : System.Web.UI.Page
    {
        private List<string> Customers
        {
            get
            {
                if (ViewState["Customers"] == null)
                    ViewState["Customers"] = new List<string>();
                return (List<string>)ViewState["Customers"];
            }
            set
            {
                ViewState["Customers"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudents();
            }
        }
                
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            int age =int.Parse(txtAge.Text);
            string email = txtEmail.Text;
            string course = ddlCourse.SelectedValue;

            string connStr = @"Server=.;Database=StudentDB;Trusted_Connection=True;";
            using(SqlConnection con = new SqlConnection(connStr))
            {
                string query = "Insert into Students (Name, Age, Email, Course) Values (@Name, @Age, @Email, @Course)";

                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Course", course);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            LoadStudents();
            txtName.Text = "";
            txtAge.Text = "";
            txtEmail.Text = "";
            ddlCourse.SelectedIndex = 0;
            lblMessage.Text = "Student registered Successfully!";
        }

        protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            LoadStudents();
        }

        public void LoadStudents()
        {
            string connStr = @"Server=.;DataBase=StudentDB;Trusted_Connection=True;";

            using(SqlConnection con =  new SqlConnection(connStr))
            {
                string query = "Select * from Students";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();

                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind(); 
            }
        }
    }
}