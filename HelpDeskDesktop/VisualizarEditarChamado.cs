using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    public partial class VisualizarEditarChamadoForm : Form
    {
        private int chamadoID;
        private Panel panelConteudo;
        private TextBox txtID;
        private TextBox txtTitulo;
        private ComboBox cbCategoria;
        private ComboBox cbStatus;
        private ComboBox cbPrioridade;
        private RichTextBox rtbDescricao;
        private RichTextBox rtbComentarios;
        private RichTextBox rtbNovoComentario;
        private Label lblResponsavel;
        private Label lblDataCriacao;
        private Label lblDataUltAtualizacao;

        private bool modoEdicao;

        public VisualizarEditarChamadoForm(int idChamado, bool modoEdicao = false)
        {
            this.chamadoID = idChamado;
            this.modoEdicao = modoEdicao;
            this.DesignerInitialize();
            this.CarregarDadosChamado();

            if (!modoEdicao)
            {
                this.Text = $"Visualizar Chamado #{this.chamadoID:D3}";
                // Desabilitar campos de edição se for apenas visualização
            }
            else
            {
                this.Text = $"Editar Chamado #{this.chamadoID:D3}";
            }
        }

        private void DesignerInitialize()
        {
            // Configurações da Janela - TELA MENOR PARA EDIÇÃO
            if (modoEdicao)
            {
                this.Text = $"Editar Chamado #{this.chamadoID:D3}";
                this.Size = new System.Drawing.Size(600, 600);
            }
            else
            {
                this.Text = $"Visualizar Chamado #{this.chamadoID:D3}";
                this.Size = new System.Drawing.Size(900, 800);
            }

            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Font = new Font("Segoe UI", 10);
            this.AutoScroll = true;

            // ===== PAINEL PRINCIPAL =====
            panelConteudo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(20),
                AutoScroll = true
            };
            this.Controls.Add(panelConteudo);

            int yPos = 0;

            // ===== TITULO =====
            Label lblTituloPrincipal = new Label
            {
                Text = modoEdicao ? $"Editar Chamado #{this.chamadoID:D3}" : $"Visualizar Chamado #{this.chamadoID:D3}",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panelConteudo.Controls.Add(lblTituloPrincipal);
            yPos += 50;

            // ===== SEÇÃO 1: INFORMAÇÕES GERAIS =====
            CriarLabelSecao(panelConteudo, "Informações do Chamado", yPos);
            yPos += 40;

            // ID (ReadOnly)
            CriarCampoTexto(panelConteudo, "ID do Chamado:", yPos, this.chamadoID.ToString("D3"), true, out txtID);
            yPos += 75;

            // Data de Criação
            lblDataCriacao = new Label
            {
                Text = "Data de Criação:",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DarkGray,
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panelConteudo.Controls.Add(lblDataCriacao);
            yPos += 30;

            // Data Última Atualização
            lblDataUltAtualizacao = new Label
            {
                Text = "Última Atualização:",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DarkGray,
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panelConteudo.Controls.Add(lblDataUltAtualizacao);
            yPos += 40;

            // Título (ReadOnly no modo visualização)
            CriarCampoTexto(panelConteudo, "Título:", yPos, "", !modoEdicao, out txtTitulo);
            yPos += 75;

            Label lblCategoria = new Label
            {
                Text = "Categoria:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panelConteudo.Controls.Add(lblCategoria);

            cbCategoria = new ComboBox
            {
                Location = new Point(0, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = modoEdicao
            };
            cbCategoria.Items.AddRange(new object[] { "Hardware", "Software", "Rede", "Acesso", "Outro" });
            panelConteudo.Controls.Add(cbCategoria);

            if (modoEdicao)
            {
                cbCategoria.SelectedIndexChanged += (s, e) =>
                {
                    string categoria = cbCategoria.SelectedItem?.ToString();
                    string prioridade = CalcularPrioridade(categoria);

                    if (cbPrioridade != null)
                    {
                        // Define a prioridade automaticamente baseada na categoria
                        if (cbPrioridade.Items.Contains(prioridade))
                            cbPrioridade.SelectedItem = prioridade;

                        AplicarCorPrioridade(prioridade);
                    }
                };
            }

            yPos += 75;

            // Status (Sempre ReadOnly para usuário comum)
            Label lblStatus = new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            cbStatus = new ComboBox
            {
                Location = new Point(0, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            cbStatus.Items.AddRange(new object[] { "Aberto", "Em Andamento", "Resolvido" }); 
            panelConteudo.Controls.Add(lblStatus);
            panelConteudo.Controls.Add(cbStatus);
            yPos += 75;

            // Prioridade (Sempre ReadOnly)
            Label lblPrioridade = new Label
            {
                Text = "Prioridade:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            cbPrioridade = new ComboBox
            {
                Location = new Point(0, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            cbPrioridade.Items.AddRange(new object[] { "Baixa", "Média", "Alta", "Urgente" });
            panelConteudo.Controls.Add(lblPrioridade);
            panelConteudo.Controls.Add(cbPrioridade);
            yPos += 75;

            // Responsável
            lblResponsavel = new Label
            {
                Text = "Responsável:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            panelConteudo.Controls.Add(lblResponsavel);
            yPos += 40;

            // ===== SEÇÃO 2: DESCRIÇÃO =====
            CriarLabelSecao(panelConteudo, "Descrição", yPos);
            yPos += 35;

            Label lblDescricao = new Label
            {
                Text = "Descrição do Problema:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            rtbDescricao = new RichTextBox
            {
                Location = new Point(0, yPos + 25),
                Size = new System.Drawing.Size(500, 120),
                Text = "", 
                BackColor = modoEdicao ? Color.White : Color.FromArgb(240, 240, 240),
                ReadOnly = !modoEdicao,
                Font = new Font("Segoe UI", 10)
            };
            panelConteudo.Controls.Add(lblDescricao);
            panelConteudo.Controls.Add(rtbDescricao);
            yPos += 160;

            // ===== SEÇÃO 3: COMENTÁRIOS (SÓ NO MODO VISUALIZAÇÃO) =====
            if (!modoEdicao)
            {
                CriarLabelSecao(panelConteudo, "Histórico de Comentários e Atualizações", yPos);
                yPos += 35;

                rtbComentarios = new RichTextBox
                {
                    Location = new Point(0, yPos),
                    Size = new System.Drawing.Size(800, 150),
                    BackColor = Color.FromArgb(240, 240, 240),
                    ReadOnly = true,
                    Font = new Font("Segoe UI", 9)
                };
                panelConteudo.Controls.Add(rtbComentarios);
                yPos += 170;

                // ===== SEÇÃO 4: ADICIONAR COMENTÁRIO (SÓ NO MODO VISUALIZAÇÃO) =====
                CriarLabelSecao(panelConteudo, "Adicionar Comentário", yPos);
                yPos += 35;

                rtbNovoComentario = new RichTextBox
                {
                    Location = new Point(0, yPos),
                    Size = new System.Drawing.Size(800, 100),
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 10)
                };
                panelConteudo.Controls.Add(rtbNovoComentario);
                yPos += 120;
            }

            // ===== BOTÕES DE AÇÃO =====
            if (!modoEdicao)
            {
                Button btnAnexos = CriarBotao(panelConteudo, "📎 Anexos", 0, yPos, Color.FromArgb(155, 89, 182));
                btnAnexos.Click += (s, e) => AbrirGerenciadorAnexos();

                Button btnAdicionarComentario = CriarBotao(panelConteudo, "💬 Adicionar Comentário", 220, yPos, Color.FromArgb(52, 152, 219));
                btnAdicionarComentario.Click += (s, e) => AdicionarComentario();

                Button btnVoltar = CriarBotao(panelConteudo, "🔙 Voltar", 440, yPos, Color.FromArgb(192, 57, 43));
                btnVoltar.Click += (s, e) => this.Close();
            }
            else
            {
                Button btnSalvar = CriarBotao(panelConteudo, "💾 Salvar Alterações", 0, yPos, Color.FromArgb(46, 204, 113));
                btnSalvar.Click += (s, e) => SalvarEdicoes();

                Button btnCancelar = CriarBotao(panelConteudo, "❌ Cancelar", 220, yPos, Color.FromArgb(192, 57, 43));
                btnCancelar.Click += (s, e) => this.Close();
            }

            CarregarDadosChamado();
        }

        private void ControlarVisibilidadeComentarios()
        {
            try
            {
                if (modoEdicao) return;
                if (rtbNovoComentario == null) return;

                string status = "";
                string responsavel = "";

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = "SELECT Status, Responsavel FROM Chamados WHERE Id = @ChamadoID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ChamadoID", this.chamadoID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    status = reader["Status"].ToString();
                                    responsavel = reader["Responsavel"].ToString();
                                }
                            }
                        }
                    }
                }

                bool bloquearComentarios = status == "Resolvido" || string.IsNullOrEmpty(responsavel);

                foreach (Control control in panelConteudo.Controls)
                {
                    if (control is Label label && label.Text == "Adicionar Comentário")
                    {
                        label.Visible = !bloquearComentarios;
                    }
                    if (control is Button button && button.Text == "💬 Adicionar Comentário")
                    {
                        button.Visible = !bloquearComentarios;
                    }
                }

                if (rtbNovoComentario != null)
                {
                    rtbNovoComentario.Visible = !bloquearComentarios;
                    rtbNovoComentario.Enabled = !bloquearComentarios;

                    if (bloquearComentarios)
                    {
                        if (string.IsNullOrEmpty(responsavel))
                        {
                            rtbNovoComentario.Text = "Chamado aguardando atribuição de responsável";
                        }
                        else
                        {
                            rtbNovoComentario.Text = "Chamado finalizado - não é possível adicionar comentários";
                        }
                        rtbNovoComentario.BackColor = Color.FromArgb(240, 240, 240);
                    }
                    else
                    {
                        if (rtbNovoComentario.Text.Contains("Chamado"))
                            rtbNovoComentario.Text = "";
                        rtbNovoComentario.BackColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao controlar visibilidade: {ex.Message}");
            }
        }

        private string CalcularPrioridade(string categoria)
        {
            if (string.IsNullOrEmpty(categoria))
                return "Média";

            switch (categoria.ToLower())
            {
                case "rede":
                    return "Urgente"; 
                case "acesso":
                    return "Urgente"; 
                case "hardware":
                    return "Alta";   
                case "software":
                    return "Média";  
                case "outro":
                    return "Baixa";   
                default:
                    return "Média";
            }
        }

        private void CriarLabelSecao(Panel painel, string titulo, int yPos)
        {
            Label lblSecao = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(0, yPos),
                AutoSize = true
            };
            painel.Controls.Add(lblSecao);
        }

        private void CriarCampoTexto(Panel painel, string label, int yPos, string valor, bool readOnly, out TextBox textBox)
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, yPos),
                AutoSize = true
            };

            textBox = new TextBox
            {
                Location = new Point(0, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                Text = valor,
                ReadOnly = readOnly,
                Font = new Font("Segoe UI", 10),
                BackColor = readOnly ? Color.FromArgb(240, 240, 240) : Color.White
            };

            painel.Controls.Add(lbl);
            painel.Controls.Add(textBox);
        }

        private Button CriarBotao(Panel painel, string texto, int xPos, int yPos, Color cor)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(xPos, yPos),
                Size = new System.Drawing.Size(200, 45),
                BackColor = cor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = AjustarCor(cor, -20);
            btn.MouseLeave += (s, e) => btn.BackColor = cor;
            painel.Controls.Add(btn);
            return btn;
        }

        private Color AjustarCor(Color cor, int valor)
        {
            int r = Math.Max(0, Math.Min(255, cor.R + valor));
            int g = Math.Max(0, Math.Min(255, cor.G + valor));
            int b = Math.Max(0, Math.Min(255, cor.B + valor));
            return Color.FromArgb(r, g, b);
        }

        private void CarregarDadosChamado()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"SELECT c.Id, c.NumeroChamado, c.Titulo, c.Descricao, c.Categoria, 
                                c.Status, c.Prioridade, c.DataAbertura, c.DataFechamento,
                                c.Responsavel
                         FROM Chamados c
                         WHERE c.Id = @ChamadoID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ChamadoID", this.chamadoID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string numeroChamado = reader["NumeroChamado"].ToString();
                                    if (txtID != null) txtID.Text = numeroChamado;
                                    if (txtTitulo != null) txtTitulo.Text = reader["Titulo"].ToString();
                                    if (rtbDescricao != null) rtbDescricao.Text = reader["Descricao"].ToString();

                                    string categoria = reader["Categoria"].ToString();
                                    if (cbCategoria != null)
                                    {
                                        // Limpar e adicionar items
                                        cbCategoria.Items.Clear();
                                        cbCategoria.Items.AddRange(new object[] { "Hardware", "Software", "Rede", "Acesso", "Outro" });

                                        if (cbCategoria.Items.Contains(categoria))
                                            cbCategoria.SelectedItem = categoria;
                                        else if (cbCategoria.Items.Count > 0)
                                            cbCategoria.SelectedIndex = 0;
                                    }

                                    string status = reader["Status"].ToString();
                                    if (cbStatus != null)
                                    {
                                        // Limpar e adicionar items
                                        cbStatus.Items.Clear();
                                        cbStatus.Items.AddRange(new object[] { "Aberto", "Em Andamento", "Resolvido" });

                                        if (cbStatus.Items.Contains(status))
                                            cbStatus.SelectedItem = status;
                                        else if (cbStatus.Items.Count > 0)
                                            cbStatus.SelectedIndex = 0;
                                    }

                                    string prioridade = reader["Prioridade"].ToString();
                                    if (cbPrioridade != null)
                                    {
                                        // Limpar e adicionar items
                                        cbPrioridade.Items.Clear();
                                        cbPrioridade.Items.AddRange(new object[] { "Baixa", "Média", "Alta", "Urgente" });

                                        if (cbPrioridade.Items.Contains(prioridade))
                                            cbPrioridade.SelectedItem = prioridade;
                                        else if (cbPrioridade.Items.Count > 0)
                                            cbPrioridade.SelectedIndex = 0;

                                        AplicarCorPrioridade(prioridade);
                                    }

                                    DateTime dataAbertura = Convert.ToDateTime(reader["DataAbertura"]);
                                    if (lblDataCriacao != null)
                                        lblDataCriacao.Text = $"Data de Criação: {dataAbertura:dd/MM/yyyy HH:mm}";

                                    if (lblDataUltAtualizacao != null)
                                    {
                                        if (reader["DataFechamento"] != DBNull.Value)
                                        {
                                            DateTime dataFechamento = Convert.ToDateTime(reader["DataFechamento"]);
                                            lblDataUltAtualizacao.Text = $"Data de Fechamento: {dataFechamento:dd/MM/yyyy HH:mm}";
                                        }
                                        else
                                        {
                                            lblDataUltAtualizacao.Text = $"Última Atualização: {dataAbertura:dd/MM/yyyy HH:mm}";
                                        }
                                    }

                                    string responsavel = reader["Responsavel"].ToString();
                                    if (lblResponsavel != null)
                                    {
                                        if (string.IsNullOrEmpty(responsavel))
                                            lblResponsavel.Text = "Responsável: Aguardando Atribuição";
                                        else
                                            lblResponsavel.Text = $"Responsável: {responsavel}";
                                    }

                                    if (!modoEdicao)
                                    {
                                        CarregarComentarios();
                                    }

                                    ControlarVisibilidadeComentarios();
                                }
                                else
                                {
                                    MessageBox.Show("Chamado não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados do chamado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarCorPrioridade(string prioridade)
        {
            if (cbPrioridade == null) return;

            switch (prioridade)
            {
                case "Urgente":
                    cbPrioridade.BackColor = Color.FromArgb(255, 100, 100);
                    cbPrioridade.ForeColor = Color.White;
                    break;
                case "Alta":
                    cbPrioridade.BackColor = Color.FromArgb(255, 150, 100);
                    cbPrioridade.ForeColor = Color.White;
                    break;
                case "Média":
                    cbPrioridade.BackColor = Color.FromArgb(255, 200, 100);
                    cbPrioridade.ForeColor = Color.Black;
                    break;
                case "Baixa":
                    cbPrioridade.BackColor = Color.FromArgb(200, 255, 200);
                    cbPrioridade.ForeColor = Color.Black;
                    break;
                default:
                    cbPrioridade.BackColor = Color.FromArgb(240, 240, 240);
                    cbPrioridade.ForeColor = Color.Black;
                    break;
            }
        }

        private void AdicionarComentario()
        {

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string queryStatus = "SELECT Status, Responsavel FROM Chamados WHERE Id = @ChamadoID";
                        using (SqlCommand cmdStatus = new SqlCommand(queryStatus, conn))
                        {
                            cmdStatus.Parameters.AddWithValue("@ChamadoID", this.chamadoID);

                            using (SqlDataReader reader = cmdStatus.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string status = reader["Status"].ToString();
                                    string responsavel = reader["Responsavel"].ToString();

                                    if (status == "Resolvido" || string.IsNullOrEmpty(responsavel))
                                    {
                                        MessageBox.Show("Não é possível adicionar comentários. O chamado ainda não foi atribuído a um responsável.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao verificar status do chamado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbNovoComentario.Text))
            {
                MessageBox.Show("Por favor, digite um comentário!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"INSERT INTO Comentarios 
                        (ChamadoId, Mensagem, Autor, EhAdministrador, DataCriacao)
                        VALUES 
                        (@ChamadoId, @Mensagem, @Autor, @EhAdministrador, @DataCriacao)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ChamadoId", this.chamadoID);
                            cmd.Parameters.AddWithValue("@Mensagem", rtbNovoComentario.Text.Trim());
                            cmd.Parameters.AddWithValue("@Autor", UserSession.UserName);
                            cmd.Parameters.AddWithValue("@EhAdministrador", UserSession.IsAdmin);
                            cmd.Parameters.AddWithValue("@DataCriacao", DateTime.Now);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                CarregarComentarios();
                                rtbNovoComentario.Clear();

                                MessageBox.Show("Comentário adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar comentário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirGerenciadorAnexos()
        {
            GerenciadorAnexosForm formAnexos = new GerenciadorAnexosForm(this.chamadoID);
            formAnexos.ShowDialog();
        }


        private void CarregarComentarios()
        {
            try
            {
                rtbComentarios.Clear();

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"SELECT c.Mensagem, c.Autor, c.DataCriacao, c.EhAdministrador
                         FROM Comentarios c
                         WHERE c.ChamadoId = @ChamadoID
                         ORDER BY c.DataCriacao ASC"; 

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ChamadoID", this.chamadoID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string mensagem = reader["Mensagem"].ToString();
                                    string autor = reader["Autor"].ToString();
                                    DateTime dataCriacao = Convert.ToDateTime(reader["DataCriacao"]);
                                    bool ehAdmin = Convert.ToBoolean(reader["EhAdministrador"]);

                                    
                                    string prefixo = ehAdmin ? "[ADMIN] " : "[REQUERENTE] ";
                                    string comentario = $"[{dataCriacao:dd/MM/yyyy HH:mm}] {prefixo}{autor}: {mensagem}\n";

                                    rtbComentarios.AppendText(comentario);
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(rtbComentarios.Text))
                {
                    rtbComentarios.Text = "Nenhum comentário ainda.\nSeja o primeiro a comentar!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar comentários: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SalvarEdicoes()
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("O título do chamado é obrigatório!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbDescricao.Text))
            {
                MessageBox.Show("A descrição do chamado é obrigatória!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescricao.Focus();
                return;
            }

            if (cbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma categoria!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategoria.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"UPDATE Chamados 
                        SET Titulo = @Titulo, Descricao = @Descricao, 
                            Categoria = @Categoria, Prioridade = @Prioridade
                        WHERE Id = @ChamadoID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text.Trim());
                            cmd.Parameters.AddWithValue("@Descricao", rtbDescricao.Text.Trim());
                            cmd.Parameters.AddWithValue("@Categoria", cbCategoria.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Prioridade", cbPrioridade.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@ChamadoID", this.chamadoID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Chamado atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Erro ao atualizar chamado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar edições: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}