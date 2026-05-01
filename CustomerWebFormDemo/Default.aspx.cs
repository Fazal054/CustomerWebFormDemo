using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerWebFormDemo
{
    public partial class _Default : System.Web.UI.Page
    {

        [WebMethod]
        public static string GetBotResponse(string userMessage)
        {
            return CallOpenAI(userMessage);
        }

        public void StartRabbitMqListener()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "demo-queue-response",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                NotificationStore.LastNotification = message;
            };
            channel.BasicConsume(
                queue: "demo-queue-response",
                autoAck: true,
                consumer: consumer);
        }

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
                BindCourses();
                WelcomeBox1.UserName = "John";
            }
            if (!string.IsNullOrEmpty(NotificationStore.LastNotification))
            {
                lblNotification.Text = NotificationStore.LastNotification;
            }
        }
                
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            int age =int.Parse(txtAge.Text);
            string email = txtEmail.Text;
            string course = ddlCourse.SelectedValue;

            byte[] cvData = null;
            string fileName = null;

            if(fileUploadCV.HasFile)
            {
                string extension = System.IO.Path.GetExtension(fileUploadCV.FileName).ToLower();

                if(extension != ".pdf")
                {
                    lblFileError.Text = "Only PDF files are allowed.";

                    ScriptManager.RegisterStartupScript(this, this.GetType(),
    "Popup",
    "setTimeout(function(){ var myModal = new bootstrap.Modal(document.getElementById('studentModal')); myModal.show(); }, 200);",
    true);
                    return;
                }
                fileName = fileUploadCV.FileName;
                cvData = fileUploadCV.FileBytes;
            }

            string connStr = @"Server=.;Database=StudentDB;Trusted_Connection=True;";
            using(SqlConnection con = new SqlConnection(connStr))
            {
                string query = "Insert into Students (Name, Age, Email, Course, CV, CVFileName) Values (@Name, @Age, @Email, @Course, @CV, @FileName)";

                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Course", course);
                    cmd.Parameters.AddWithValue("@CV", (object)cvData ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FileName", (object)fileName ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            lblFileError.Text = "";
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            LoadStudents();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewCV")
            {
                string id = e.CommandArgument.ToString();
                Response.Redirect("ViewCV.aspx?id=" + id);
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
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

        public static string CallOpenAI(string message)
        {
            string apiKey = "YOUR_API_KEY"; // 🔴 replace this

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                new { role = "system", content = "You are a helpful assistant for a University Student Management System. Guide users on how to use the system." },
                new { role = "user", content = message }
            }
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    content).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                dynamic json = JsonConvert.DeserializeObject(result);

                return json.choices[0].message.content.ToString();
            }
        }

        private void BindCourses()
        {
            ddlCourse.DataSource = CourseRepository.GetCourses();
            ddlCourse.DataTextField = "Name";
            ddlCourse.DataValueField = "Id";
            ddlCourse.DataBind();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Create queue if not exists
            channel.QueueDeclare(
                queue: "demo-queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            string message = "Hello from WebForms!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: "demo-queue",
                basicProperties: null,
                body: body);

            Response.Write("Message Sent!");
        }
    }
    }
}
public static class NotificationStore
{
    public static string LastNotification { get; set; }
}