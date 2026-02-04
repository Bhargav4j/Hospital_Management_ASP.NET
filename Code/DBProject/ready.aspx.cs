using System;
using System.Data.SqlClient;
using System.Web;

namespace DBProject
{
    public partial class Ready : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 200;

            try
            {
                // Check if application is fully initialized and ready
                // Check database connectivity
                var dbServer = System.Environment.GetEnvironmentVariable("DB_SERVER");
                var dbName = System.Environment.GetEnvironmentVariable("DB_NAME");
                var dbUser = System.Environment.GetEnvironmentVariable("DB_USER");
                var dbPassword = System.Environment.GetEnvironmentVariable("DB_PASSWORD");

                string connectionString;
                if (!string.IsNullOrEmpty(dbServer) && !string.IsNullOrEmpty(dbName) &&
                    !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
                {
                    connectionString = $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};Connection Timeout=5;";
                }
                else
                {
                    connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sqlCon1"].ConnectionString;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT 1", conn))
                    {
                        cmd.ExecuteScalar();
                    }
                }

                Response.Write("{\"status\":\"ready\",\"database\":\"connected\"}");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 503;
                Response.Write("{\"status\":\"not ready\",\"database\":\"disconnected\",\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }
    }
}
