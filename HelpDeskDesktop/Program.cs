using System;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Testar conexão primeiro
            if (DatabaseConnection.TestConnection())
            {
                Application.Run(new LoginForm());
            }
            else
            {
                MessageBox.Show("Não foi possível conectar ao banco de dados. Verifique a conexão.",
                              "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}