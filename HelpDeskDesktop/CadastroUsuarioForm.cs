using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    public partial class CadastroUsuarioForm : Form
    {
        public CadastroUsuarioForm()
        {
            InitializeComponent();
            ConfigurarPlaceholders();
        }

        private void ConfigurarPlaceholders()
        {
            // Configura placeholders iniciais
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                txtNome.Text = "Digite seu nome completo";
                txtNome.ForeColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Digite seu e-mail";
                txtEmail.ForeColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                txtSenha.Text = "Digite sua senha";
                txtSenha.ForeColor = Color.Gray;
                txtSenha.UseSystemPasswordChar = false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmarSenha.Text))
            {
                txtConfirmarSenha.Text = "Confirme sua senha";
                txtConfirmarSenha.ForeColor = Color.Gray;
                txtConfirmarSenha.UseSystemPasswordChar = false;
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                CadastrarUsuario();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Fecha IMEDIATAMENTE sem verificar nada
            this.Close();
        }

        private bool ValidarCampos()
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;
            string confirmarSenha = txtConfirmarSenha.Text;

            // Ignorar placeholders
            if (nome == "Digite seu nome completo") nome = "";
            if (email == "Digite seu e-mail") email = "";
            if (senha == "Digite sua senha") senha = "";
            if (confirmarSenha == "Confirme sua senha") confirmarSenha = "";

            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Por favor, digite seu nome completo!", "Atenção");
                txtNome.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                MessageBox.Show("Por favor, digite um e-mail válido!", "Atenção");
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(senha) || senha.Length < 6)
            {
                MessageBox.Show("A senha deve ter pelo menos 6 caracteres!", "Atenção");
                txtSenha.Focus();
                return false;
            }

            if (senha != confirmarSenha)
            {
                MessageBox.Show("As senhas não coincidem!", "Atenção");
                txtConfirmarSenha.Focus();
                return false;
            }

            return true;
        }

        private void CadastrarUsuario()
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // Remover placeholders
            if (nome == "Digite seu nome completo") nome = "";
            if (email == "Digite seu e-mail") email = "";
            if (senha == "Digite sua senha") senha = "";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        // Verificar se email já existe
                        string verificaQuery = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email";
                        using (SqlCommand verificaCmd = new SqlCommand(verificaQuery, conn))
                        {
                            verificaCmd.Parameters.AddWithValue("@Email", email);
                            int count = (int)verificaCmd.ExecuteScalar();

                            if (count > 0)
                            {
                                MessageBox.Show("Este e-mail já está cadastrado!", "Erro");
                                return;
                            }
                        }

                        // Inserir novo usuário
                        string insertQuery = @"INSERT INTO Usuarios (Nome, Email, Senha, DataCadastro, IsAdministrador) 
                                             VALUES (@Nome, @Email, @Senha, @DataCadastro, @IsAdministrador)";

                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Nome", nome);
                            insertCmd.Parameters.AddWithValue("@Email", email);
                            insertCmd.Parameters.AddWithValue("@Senha", senha);
                            insertCmd.Parameters.AddWithValue("@DataCadastro", DateTime.Now);
                            insertCmd.Parameters.AddWithValue("@IsAdministrador", false);

                            int rowsAffected = insertCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso");
                                this.Close(); // Fecha após cadastro bem-sucedido
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar: {ex.Message}", "Erro");
            }
        }

        // Eventos dos placeholders
        private void txtNome_Enter(object sender, EventArgs e)
        {
            if (txtNome.Text == "Digite seu nome completo")
            {
                txtNome.Text = "";
                txtNome.ForeColor = Color.Black;
            }
        }

        private void txtNome_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                txtNome.Text = "Digite seu nome completo";
                txtNome.ForeColor = Color.Gray;
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

        private void txtConfirmarSenha_Enter(object sender, EventArgs e)
        {
            if (txtConfirmarSenha.Text == "Confirme sua senha")
            {
                txtConfirmarSenha.Text = "";
                txtConfirmarSenha.ForeColor = Color.Black;
                txtConfirmarSenha.UseSystemPasswordChar = true;
            }
        }

        private void txtConfirmarSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirmarSenha.Text))
            {
                txtConfirmarSenha.Text = "Confirme sua senha";
                txtConfirmarSenha.ForeColor = Color.Gray;
                txtConfirmarSenha.UseSystemPasswordChar = false;
            }
        }

        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            if (txtSenha.Text != "Digite sua senha")
            {
                txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
            }
            if (txtConfirmarSenha.Text != "Confirme sua senha")
            {
                txtConfirmarSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
            }
        }
    }
}