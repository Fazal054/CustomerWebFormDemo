using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace CustomerWebFormDemo.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class MyService : WebService
    {
        [WebMethod]
        public List<Student> GetStudents()
        
        {
            string connStr = @"Server=.;Database=StudentDB;Trusted_Connection=True;";

            List<Student> students = new List<Student>();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT Id, Name, Age, Email, Course FROM Students";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Email = reader["Email"].ToString(),
                        Course = reader["Course"].ToString()
                    });
                }
            }

            return students;
        }
    }

    // DTO class MUST be public for ASMX serialization
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Course { get; set; }
    }
}