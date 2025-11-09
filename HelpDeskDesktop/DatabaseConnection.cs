using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    public class DatabaseConnection
    {
        // SUA STRING DE CONEXÃO CORRIGIDA
        private static string connectionString = "workstation id=HelpDeskDB.mssql.somee.com;packet size=4096;user id=Dopingzin_SQLLogin_1;pwd=bgbeptf1bu;data source=HelpDeskDB.mssql.somee.com;persist security info=False;initial catalog=HelpDeskDB;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar com o banco: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool TestConnection()
        {
            using (SqlConnection conn = GetConnection())
            {
                return conn != null && conn.State == System.Data.ConnectionState.Open;
            }
        }
    }
}