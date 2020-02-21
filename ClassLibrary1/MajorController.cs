using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SqlLibrary {
    public class MajorController {

        public static BcConnection bcConnection { get; set; } //contains the sql connection

        private static void SetMjrProps(SqlDataReader reader, Major major) {
            major.Id = Convert.ToInt32(reader["Id"]);
            major.Description = reader["Description"].ToString();
            major.MinSat = Convert.ToInt32(reader["MinSAT"]);
        }  

        public static List<Major> GetAllMajors() {
            var sql = "Select * from Major; ";
            var command = new SqlCommand(sql, bcConnection.Connection);
            var reader = command.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                reader = null;
                Console.WriteLine("No Rows for GetAllMajors()");
                return new List<Major>();
            }
            var majors = new List<Major>();
            while (reader.Read()) {
                var major = new Major();
                SetMjrProps(reader, major);
                majors.Add(major);
            }
            reader.Close();
            reader = null;
            return majors;
        }
    
        public static Major GetMajorByPk(int id) {
            var sql = "SELECT * FROM Major where Id = @Id; ";
            var command = new SqlCommand(sql, bcConnection.Connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = command.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                reader = null;
                Console.WriteLine("Major not found");
            }
            reader.Read();
            var major = new Major();
            SetMjrProps(reader, major);
            reader.Close();
            reader = null;
            return major;
        }
        private static int MajorMaintainence(Major major, string sql) {
            var command = new SqlCommand(sql, bcConnection.Connection);
            command.Parameters.AddWithValue("@id", major.Id);
            command.Parameters.AddWithValue("@Description", major.Description);
            command.Parameters.AddWithValue("@MinSAT", major.MinSat);
            var recsAffected = command.ExecuteNonQuery();
            return recsAffected;
        }
        //insert
        public static bool InsertMajor(Major major) {
            var sql = "Insert into Major (Id, Description, MinSAT) Values ( @id, @Description, @MinSAT ); ";
            int recsAffected = MajorMaintainence(major, sql);
            if (recsAffected != 1) {
                throw new Exception("Insert Failed");
            } else {
                return true;
            }
        }

        //update
        public static bool UpdateMajor(Major major) {
            var sql = "Update Major Set Description = @Description, MinSAT = @MinSAT Where Id = @Id";
            var recsAffected = MajorMaintainence(major, sql);
            if(recsAffected != 1) {
                throw new Exception("Update Failed");
            } else 
            return true;
        }

        //delete
        public static bool DeleteMajor(Major major) {
            var sql = "Delete from Major where Id = @Id";
            var recsAffected = MajorMaintainence(major, sql);
            if ( recsAffected != 1) {
                throw new Exception("Delete Failed");
            } else
            return true;
        }
    }
}
