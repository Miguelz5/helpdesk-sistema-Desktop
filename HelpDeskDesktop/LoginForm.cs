using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            ConfigurarPlaceholders();
            ConectarEventosLogin(); 
        }

        private void ConfigurarPlaceholders()
        {
            // Placeholder no txtEmail
            txtEmail.Text = "Digite seu e-mail";
            txtEmail.ForeColor = Color.Gray;

            // Placeholder no txtSenha
            txtSenha.Text = "Digite sua senha";
            txtSenha.ForeColor = Color.Gray;
            txtSenha.UseSystemPasswordChar = false;
        }

        private void ConectarEventosLogin()
        {
            // Conecta os eventos para placeholders funcionarem
            txtEmail.Enter += txtEmail_Enter;
            txtEmail.Leave += txtEmail_Leave;
            txtSenha.Enter += txtSenha_Enter;
            txtSenha.Leave += txtSenha_Leave;

            // Conecta o evento do link de cadastro
            lnkCadastro.LinkClicked += lnkCadastro_LinkClicked;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // Ignora o placeholder na validação
            if (email == "Digite seu e-mail") email = "";
            if (senha == "Digite sua senha") senha = "";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidarLogin(email, senha))
            {
                MessageBox.Show("Login realizado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email ou senha incorretos!", "Erro de Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarLogin(string email, string senha)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                if (conn != null)
                {
                    // CORREÇÃO: IsAdministrador (com D)
                    string query = "SELECT Id, Nome, Email, IsAdministrador FROM Usuarios WHERE Email = @Email AND Senha = @Senha";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Senha", senha);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserSession.UserId = reader.GetInt32(0);
                                UserSession.UserName = reader.GetString(1);
                                UserSession.UserEmail = reader.GetString(2);
                                UserSession.IsAdmin = reader.GetBoolean(3);

                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "Digite seu e-mail")
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.Black;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Digite seu e-mail";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "Digite sua senha")
            {
                txtSenha.Text = "";
                txtSenha.ForeColor = Color.Black;
                txtSenha.UseSystemPasswordChar = true;
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                txtSenha.Text = "Digite sua senha";
                txtSenha.ForeColor = Color.Gray;
                txtSenha.UseSystemPasswordChar = false;
            }
        }

        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            // Só altera se não for placeholder
            if (txtSenha.Text != "Digite sua senha")
            {
                txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
            }
        }

        private bool eventoJaExecutado = false;

        
        private void lnkCadastro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Método SIMPLES e FUNCIONAL
            CadastroUsuarioForm cadastroForm = new CadastroUsuarioForm();
            cadastroForm.ShowDialog(); // Modal - bloqueia o login

            // Quando o cadastro fechar, o login continua normalmente
            // Não precisa fazer nada especial
        }

    }
}
