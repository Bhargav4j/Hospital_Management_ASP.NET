using System;
using Npgsql;
using System.Web;

namespace DBProject
{
    public partial class Health : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 200;

            try
            {
                // Check database connectivity
                var dbServer = System.Environment.GetEnvironmentVariable("DB_SERVER");
                var dbName = System.Environment.GetEnvironmentVariable("DB_NAME");
                var dbUser = System.Environment.GetEnvironmentVariable("DB_USER");
                var dbPassword = System.Environment.GetEnvironmentVariable("DB_PASSWORD");

                string connectionString;
                if (!string.IsNullOrEmpty(dbServer) && !string.IsNullOrEmpty(dbName) &&
                    !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
                {
                    connectionString = $"Host={dbServer};Database={dbName};Username={dbUser};Password={dbPassword};Port=5432;Timeout=5;";
                }
                else
                {
                    connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sqlCon1"].ConnectionString;
                }

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT 1", conn))
                    {
                        cmd.ExecuteScalar();
                    }
                }

                Response.Write("{\"status\":\"healthy\",\"database\":\"connected\"}");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 503;
                Response.Write("{\"status\":\"unhealthy\",\"database\":\"disconnected\",\"error\":\"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }
    }
}
