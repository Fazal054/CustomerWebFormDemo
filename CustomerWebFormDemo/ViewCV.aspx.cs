using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerWebFormDemo
{
    public partial class ViewCV : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];

            string connStr = @"Server=.;Database=StudentDB;Trusted_Connection=True;";
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT CV, CVFileName FROM Students WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (reader["CV"] != DBNull.Value)
                        {
                            byte[] bytes = (byte[])reader["CV"];
                            string fileName = reader["CVFileName"].ToString();

                            Response.Clear();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);
                            Response.BinaryWrite(bytes);
                            Response.End();
                        }
                    }
                }
            }
        }
    }
}