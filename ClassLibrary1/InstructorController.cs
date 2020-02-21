using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SqlLibrary {
    class InstructorController {
        public static BcConnection bcConnection { get; set; }

        private static Instructor SetInstrProps(SqlDataReader reader) {
            var instructor = new Instructor();
            instructor.Id = Convert.ToInt32(reader["Id"]);
            instructor.Firstname = reader["Firstname"].ToString();
            instructor.Lastname = reader["Lastname"].ToString();
            instructor.YearsExp = Convert.ToInt32(reader["YearsExperience"]);
            instructor.isTenured = Convert.ToBoolean(reader["IsTenured"]);
            return instructor;
        }
        //GetAllInstructors
        public static List<Instructor> GetAllInstructors() {
            var sql = "SELECT * FROM Instructor";
            var command = new SqlCommand(sql, bcConnection.Connection);
            var reader = command.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                reader = null;
                Console.WriteLine("No Instructors Found");
                return new List<Instructor>();
            }
            var instructors = new List<Instructor>();
            while (reader.Read()) {
                var instructor = SetInstrProps(reader);
                instructors.Add(instructor);
            }
            reader.Close();
            reader = null;
            return instructors;

        }
        //GetInstructorById
        public static List<Instructor> GetInstructorByPk(int id) {

            var sql = "Select * From Instructors where Id = @Id";
            var command = new SqlCommand(sql, bcConnection.Connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = command.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                reader = null;
                throw new Exception("Instructor Not Found");
            } 
            var instructors = new List<Instructor>();
            while (reader.Read()) {
                SetInstrProps(reader);
            }
                reader.Close();
                reader = null;
            return instructors;
        }
        
        private static int InstructorMaintainence(Instructor instructor, string sql) {
            var command = new SqlCommand(sql, bcConnection.Connection);
            command.Parameters.AddWithValue("@Id", instructor.Id);
            command.Parameters.AddWithValue("@Firstname", instructor.Firstname);
            command.Parameters.AddWithValue("@Lastname", instructor.Lastname);
            command.Parameters.AddWithValue("@YrsExp", instructor.YearsExp);
            command.Parameters.AddWithValue("@isTenured", instructor.isTenured);
            var recsAffected = command.ExecuteNonQuery();
            return recsAffected;
        }
        public static bool InsertInstructor(Instructor instructor) {

            var sql = "Insert into Instructor (Id, Firstname, Lastname, YearsExperience, IsTenured) Values (@Id, @Firstname, @Lastname, @YrsExp, @isTenured) ";
            int recsAffected = InstructorMaintainence(instructor, sql);
            if (recsAffected != 1) {
                throw new Exception("Insert Failed");
            }

            return true;
        }


        public static bool UpateInstructor(Instructor instructor) {
            var sql = "Update Instructor Set Firstname = @Firstname, Lastname = @Lastname, YearsExperience = @YrsExp, IsTenured = @isTenured WHERE Id = @Id; ";
            var cmd = new SqlCommand(sql, bcConnection.Connection);
            int recsAffected = InstructorMaintainence(instructor, sql);
            if(recsAffected !=1 ) {
                throw new Exception("Update Failed");
            }
            return true;
        }

        public static bool DeleteInstructor(Instructor instructor) {
            var sql = "Delete From Instructor where Id = @Id; ";
            var cmd = new SqlCommand(sql, bcConnection.Connection);
            int recsAffected = InstructorMaintainence(instructor, sql);
            if(recsAffected != 1) {
                throw new Exception("Deleted Failed");
            }
            return true;
        }
    }
}
