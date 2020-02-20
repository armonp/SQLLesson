using System;
using System.Data.SqlClient;

namespace SqlLibrary {
    public class BcConnection {
        public SqlConnection Connection { get; set; } //required to talk to sql

        public void Connect (string server,string database, string auth) {
        //@"server = localhost\sqlexpress; database = EdDb; trusted_connection = true;"; //what server, what database
            var connStr = $"server={server};database={database};{auth}";
            Connection = new SqlConnection(connStr);

            Connection.Open(); 

            if(Connection.State != System.Data.ConnectionState.Open) {
                Console.WriteLine("Could not open the connection -- Check connection string.");
                Connection = null;
                return;
            }
            Console.WriteLine("Connection Opened");
        } 
        public void Disconnect() {
            if (Connection == null) {
                return;
            }
            if(Connection.State == System.Data.ConnectionState.Open) {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}
