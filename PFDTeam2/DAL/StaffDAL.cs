using PFDTeam2.Models;
using System.Data.SqlClient;

namespace PFDTeam2.DAL
{
    public class StaffDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public StaffDAL()
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

        public List<Staff> GetAllStaff()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Staff> StaffList = new List<Staff>();
            while (reader.Read())
            {
                StaffList.Add(
                new Staff
                {
                    StaffID = reader.GetInt32(0), //0: 1st column
                    FirstName = reader.GetString(1), //1: 2nd column
                    LastName = reader.GetString(2), //2: 3rd column
                    Email = reader.GetString(3), //3: 4th column
                    Password = reader.GetString(4), //5: 6th column
                    AllocatedLaptop = reader.GetString(5), //6: 7th column
                    DepartmentID = reader.GetString(6), //9: 10th column
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return StaffList;
        }

        public bool Login(string email, string password)
        {
            bool authenticated = false;

            // Replace "YourConnectionString" with the actual connection string for your database
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OnboardingApp;Integrated Security=True;";

            // Use a using statement to ensure the connection is properly closed when done
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Create a SqlCommand object from connection object
                SqlCommand cmd = conn.CreateCommand();

                //Specify the SELECT SQL statement
                cmd.CommandText = @"SELECT * FROM Staff";

                //Open a database connection
                conn.Open();

                //Execute the SELECT SQL through a DataReader
                SqlDataReader reader = cmd.ExecuteReader();

                //Read all records until the end
                while (reader.Read())
                {
                    // Convert email address to lowercase for comparison
                    // Password comparison is case-sensitive
                    if ((reader.GetString(3) == email) &&
                        (reader.GetString(4) == password))
                    {
                        authenticated = true;
                        break; // Exit the while loop
                    }
                }
            }

            return authenticated;
        }

        public int GetStaffIdByEmail(string email)
        {
            int memberId = 0;

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OnboardingApp;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT StaffId FROM Staff WHERE Email = @email";
                cmd.Parameters.AddWithValue("@email", email);

                memberId = (int)cmd.ExecuteScalar();
            }

            return memberId;
        }

        public string GetStaffAppointment(string loginID)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NPCS;Integrated Security=True;";
            string appointment = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Appointment FROM Staff WHERE LoginID = @LoginID";
                    cmd.Parameters.AddWithValue("@LoginID", loginID);

                    appointment = cmd.ExecuteScalar() as string;
                }
            }

            return appointment ?? string.Empty;
        }
    }
}
