using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using System;
using MySql.Data.MySqlClient;

namespace School.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        

        // dependency injection of database context
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>
        /// GET api/Teacher/ListTeachers -> [{"TeacherId":1,"TeacherFname":"Alexander", "TeacherLName":"Bennett","EmployeeNumber":"T378","HireDate":"2016-08-05 00:00:00","salary":"55.30" },{"TeacherId":2,"TeacherFname":"Caitlin", "TeacherLName":"Cummings","EmployeeNumber":"T381","HireDate":"2014-06-10 00:00:00","salary":"62.77" }..]
        /// </example>
        /// <returns>
        /// A list of Teacher objects 
        /// </returns>

        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers()
        {
            // Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from teachers";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["teacherlname"].ToString();
                        DateTime TeacherJoinDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        double Salary = Convert.ToDouble(ResultSet["salary"]);

                        //short form for setting all properties while creating the object
                        Teacher CurrentAuthor = new Teacher()
                        {
                            Id = Id,
                            FName = FirstName,
                            LName = LastName,
                            ENo = EmployeeNumber,
                            JoinDate = TeacherJoinDate,
                            Salary= Salary
                        };

                        Teachers.Add(CurrentAuthor);

                    }
                }
            }


            //Return the final list of Teachers
            return Teachers;
        }
        /// <summary>
        /// Returns an Teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Teacher/FindTeacher/1 -> {"TeacherId":1,"TeacherFname":"Alexander", "TeacherLName":"Bennett","EmployeeNumber":"T378","HireDate":"2016-08-05 00:00:00","salary":"55.30" }
        /// </example>
        /// <returns>
        /// A matching Teacher object by its ID. Empty object if teacher not found
        /// </returns>

        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {

            //Empty Teacher
            Teacher SelectedTeacher = new Teacher();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // @id is replaced with a 'sanitized' id
                Command.CommandText = "select * from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    if (ResultSet.HasRows)
                    {
                        //Loop Through Each Row the Result Set
                        while (ResultSet.Read())
                        {
                            //Access Column information by the DB column name as an index
                            int Id = Convert.ToInt32(ResultSet["teacherid"]);
                            string FirstName = ResultSet["teacherfname"].ToString();
                            string LastName = ResultSet["teacherlname"].ToString();
                            string EmployeeNumber = ResultSet["employeenumber"].ToString();
                            DateTime TeacherJoinDate = Convert.ToDateTime(ResultSet["hiredate"]);
                            double Salary = Convert.ToDouble(ResultSet["salary"]);

                            SelectedTeacher.Id = Id;
                            SelectedTeacher.FName = FirstName;
                            SelectedTeacher.LName = LastName;
                            SelectedTeacher.ENo = EmployeeNumber;
                            SelectedTeacher.JoinDate = TeacherJoinDate;
                            SelectedTeacher.Salary = Salary;

                        }

                    }

                }

                //Return the final list of Teacher names
                return SelectedTeacher;
                

            }
        }
    }
}
