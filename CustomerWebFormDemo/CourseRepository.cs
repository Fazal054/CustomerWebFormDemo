using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerWebFormDemo
{
    public class CourseRepository
    {
        public static List<Course> GetCourses()
        {
            return new List<Course>
        {
            new Course { Id = "0", Name = "Select" },
            new Course { Id = "C#", Name = "C#" },
            new Course { Id = "Java", Name = "Java" },
            new Course { Id = "Python", Name = "Python" },
            new Course { Id = "JavaScript", Name = "JavaScript" },
            new Course { Id = "SQL", Name = "SQL" }
        };
        }
    }
}