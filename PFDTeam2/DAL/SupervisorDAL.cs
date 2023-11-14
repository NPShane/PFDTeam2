using PFDTeam2.Models;
using System.Data.SqlClient;

namespace PFDTeam2.DAL
{
    public class SupervisorDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        // Constructor
        public SupervisorDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
                "OnboardingAppConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Supervisor> GetAllSupervisors()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Supervisor ORDER BY SupervisorID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a supervisor list
            List<Supervisor> supervisorList = new List<Supervisor>();
            while (reader.Read())
            {
                supervisorList.Add(
                    new Supervisor
                    {
                        SupervisorID = reader.GetInt32(0),                       
                        Email = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Password = reader.GetString(4),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return supervisorList;
        }

        public bool Login(string email, string password)
        {
            bool authenticated = false;

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OnboardingApp;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Create a SqlCommand object from connection object
                SqlCommand cmd = conn.CreateCommand();

                //Specify the SELECT SQL statement
                cmd.CommandText = @"SELECT * FROM Supervisor";

                //Open a database connection
                conn.Open();

                //Execute the SELECT SQL through a DataReader
                SqlDataReader reader = cmd.ExecuteReader();

                //Read all records until the end
                while (reader.Read())
                {
                    // Convert email address to lowercase for comparison
                    // Password comparison is case-sensitive
                    if ((reader.GetString(1) == email) &&
                        (reader.GetString(4) == password))
                    {
                        authenticated = true;
                        break; // Exit the while loop
                    }
                }
            }

            return authenticated;
        }

        public int GetSupervisorIdByEmail(string email)
        {
            int supervisorId = 0;

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OnboardingApp;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT SupervisorId FROM Supervisor WHERE Email = @email";
                cmd.Parameters.AddWithValue("@email", email);

                supervisorId = (int)cmd.ExecuteScalar();
            }

            return supervisorId;
        }

        public string GetSupervisorAppointment(string loginID)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NPCS;Integrated Security=True;";
            string appointment = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Appointment FROM Supervisor WHERE LoginID = @LoginID";
                    cmd.Parameters.AddWithValue("@LoginID", loginID);

                    appointment = cmd.ExecuteScalar() as string;
                }
            }

            return appointment ?? string.Empty;
        }
    }
}
