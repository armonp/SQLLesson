using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SqlLibrary {
    public class StudentController {

        public static BcConnection bcConnection { get; set; }

        private static Student LoadStudentProps(SqlDataReader reader) {
            var student = new Student(); //new instances created inside static method are accessible
            student.Id = Convert.ToInt32(reader["Id"]); //data comes from database as Objects.
            student.Firstname = reader["Firstname"].ToString();
            student.Lastname = reader["Lastname"].ToString();
            student.SAT = Convert.ToInt32(reader["SAT"]); //convert each piece to appropriate data type
            student.GPA = Convert.ToDouble(reader["GPA"]);
            student.MajorId = Convert.IsDBNull(reader["MajorId"]) ? (int?)null : Convert.ToInt32(reader["MajorId"]);
            return student;
        }

        public static List<Student> GetAllStudents() { //static method cannot access instance things. Removed static modifier to put LoadStudentProps into method
            var sql = "SELECT * From Student s Left Join Major m on m.Id = s.MajorId"; //pass sql statement to
            var command = new SqlCommand(sql, bcConnection.Connection); //sqlcommand
            var reader = command.ExecuteReader(); //use ExecuteReader() to read data. ExecuteNonQuery() to update data
            if (!reader.HasRows) {
                Console.WriteLine("No rows from GetAllStudents()");
                reader.Close();
                reader = null;
                return new List<Student>();
            }
            var students = new List<Student>();
            while (reader.Read()) {
                var student = LoadStudentProps(reader);
                //var student = new Student(); //new instances created inside static method are accessible
                //student.Id = Convert.ToInt32(reader["Id"]); //data comes from database as Objects.
                //student.Firstname = reader["Firstname"].ToString();
                //student.Lastname = reader["Lastname"].ToString();
                //student.SAT = Convert.ToInt32(reader["SAT"]); //convert each piece to appropriate data type
                //student.GPA = Convert.ToDouble(reader["GPA"]);
                ////student.MajorId = Convert.ToInt32(reader["MajorId"]);
                if(Convert.IsDBNull(reader["Description"])) {
                    student.Major = null;
                } else {
                    var major = new Major {
                        Description = reader["Description"].ToString(),
                        MinSat = Convert.ToInt32(reader["MinSat"])
                    };
                    student.Major = major;
                }
                students.Add(student); //after all data from database is pulled, add to collection Student
            }
            reader.Close();
            reader = null;
            return students; //return the entire collection
        }

        public static Student GetStudentByPk(int id) {
            var sql = $"SELECT * From Student Where Id = {id}";
            var command = new SqlCommand(sql, bcConnection.Connection);
            var reader = command.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                reader = null;
                return null; 
            }
            reader.Read();
            var student = LoadStudentProps(reader);
            //var student = new Student(); //new instances created inside static method are accessible
            //student.Id = Convert.ToInt32(reader["Id"]); //data comes from database as Objects.
            //student.Firstname = reader["Firstname"].ToString();
            //student.Lastname = reader["Lastname"].ToString();
            //student.SAT = Convert.ToInt32(reader["SAT"]); //convert each piece to appropriate data type
            //student.GPA = Convert.ToDouble(reader["GPA"]);
            //student.MajorId = Convert.ToInt32(reader["MajorId"]);
            reader.Close();
            reader = null;
            return student;
           

        }

        public static bool InsertStudent(Student student) { //pass in an instance of Student
                        //should not use string interpolation in sql statements
            //should set @ parameters in sql variable
            //set each parameter to a value from data
            //var sql = $"INSERT into Student (Id, Firstname, Lastname, SAT, GPA, MajorId) VALUES({student.Id}, '{student.Firstname}', '{student.Lastname}', {student.SAT}, {student.GPA}, {majorid});";
            var sql = $"INSERT into Student (Id, Firstname, Lastname, SAT, GPA, MajorId) Values (@Id, @Firstname, @Lastname, @SAT, @GPA, @Majorid);";
            var command = new SqlCommand(sql, bcConnection.Connection);
             command.Parameters.AddWithValue("@Id", student.Id);
             command.Parameters.AddWithValue("@Firstname", student.Firstname);
             command.Parameters.AddWithValue("@Lastname", student.Lastname);
             command.Parameters.AddWithValue("@SAT", student.SAT);
             command.Parameters.AddWithValue("@GPA", student.GPA);
             command.Parameters.AddWithValue("@Majorid", student.MajorId ?? Convert.DBNull); 
             
            var recsAffected = command.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new Exception("Insert Failed"); //exits the method if there is any error inserting
            }
            return true;
        }
        
        public static bool UpdateStudent(Student student) {
            var sql = $"UPDATE Student SET " +
                "Firstname = @Firstname, " +
                "Lastname = @Lastname, " +
                "SAT = @SAT, " +
                "GPA = @GPA, " +
                "MajorId = @MajorId " +
                "WHERE Id = @Id;";

            var command = new SqlCommand(sql, bcConnection.Connection);
            command.Parameters.AddWithValue("@Id", student.Id);
            command.Parameters.AddWithValue("@Firstname", student.Firstname);
            command.Parameters.AddWithValue("@Lastname", student.Lastname);
            command.Parameters.AddWithValue("@SAT", student.SAT);
            command.Parameters.AddWithValue("@GPA", student.GPA);
            command.Parameters.AddWithValue("@Majorid", student.MajorId ?? Convert.DBNull);

            var recsAffected = command.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new Exception("Update Failed"); //exits the method if there is any error inserting
            }
            return true;
        }
        
        public static bool DeleteStudent(Student student) {
            var sql = $"DELETE from Student Where Id = @Id";
            var command = new SqlCommand(sql, bcConnection.Connection);
            command.Parameters.AddWithValue("@Id", student.Id);
            var recsAffected = command.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new Exception("Delete Failed");
            }
            return true;
        }
        
        public bool DeleteStudent(int id) {
            var student = GetStudentByPk(id);
            if(student == null) {
                return false;
            }
            var success = DeleteStudent(student);
            return true;
        }
    }


}
