using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpDeskDesktop  
{
    public partial class MainForm : Form
    {
        private Panel panelSideBar;
        private Panel panelContent;
        private Button btnDashboard;
        private Button btnCriarChamado;
        private Button btnMeusChamados;
        private Button btnFaq; 
        private Button btnPerfil;
        private Button btnSair;
        private Label lblUsuario;
        private PictureBox pictureBoxLogo;
        private Button btnChatIA;

        public MainForm()
        {
            this.DesignerInitialize();
            this.CentralizarJanela();

            this.WindowState = FormWindowState.Maximized;


            lblUsuario.Text = $"Usuário: {UserSession.UserName}";

            CarregarDashboard();
        }

        private void DesignerInitialize()
        {
            // Configurações da Janela Principal
            this.Text = "HelpDesk - Desktop";
            this.Size = new System.Drawing.Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Font = new Font("Segoe UI", 10);

            // ===== PAINEL LATERAL (SIDEBAR) =====
            panelSideBar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(31, 78, 121),
                Padding = new Padding(0, 20, 0, 0)
            };
            this.Controls.Add(panelSideBar);


            pictureBoxLogo = new PictureBox
            {
                Size = new System.Drawing.Size(40, 40),
                Location = new Point(15, 15),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            panelSideBar.Controls.Add(pictureBoxLogo);

            Label lblTitle = new Label
            {
                Text = "HELPDESK",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White, 
                Location = new Point(65, 20),
                AutoSize = true,
            };
            panelSideBar.Controls.Add(lblTitle);

            // Informação do Usuário
            lblUsuario = new Label
            {
                Text = $"Usuário: {UserSession.UserName}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray,
                Location = new Point(15, 70),
                AutoSize = true,
                BackColor = Color.FromArgb(31, 78, 121)
            };
            panelSideBar.Controls.Add(lblUsuario);

            // Botões do Menu
            int yPosition = 130;
            int buttonHeight = 50;
            int buttonMargin = 10;

            btnDashboard = CriarBotaoMenu("📊 Dashboard", yPosition, panelSideBar);
            btnDashboard.Click += (s, e) => CarregarDashboard();
            yPosition += buttonHeight + buttonMargin;

            btnCriarChamado = CriarBotaoMenu("➕ Criar Chamado", yPosition, panelSideBar);
            btnCriarChamado.Click += (s, e) => CarregarCriarChamado();
            yPosition += buttonHeight + buttonMargin;

            btnMeusChamados = CriarBotaoMenu("📋 Meus Chamados", yPosition, panelSideBar);
            btnMeusChamados.Click += (s, e) => CarregarMeusChamados();
            yPosition += buttonHeight + buttonMargin;

            btnFaq = CriarBotaoMenu("❓ FAQ", yPosition, panelSideBar);
            btnFaq.Click += (s, e) => CarregarFaq();
            yPosition += buttonHeight + buttonMargin;

            btnChatIA = CriarBotaoMenu("🤖 Chat IA", yPosition, panelSideBar);
            btnChatIA.Click += (s, e) => CarregarChatIA();
            yPosition += buttonHeight + buttonMargin;

            btnPerfil = CriarBotaoMenu("👤 Meu Perfil", yPosition, panelSideBar);
            btnPerfil.Click += (s, e) => CarregarPerfil(); 
            yPosition += buttonHeight + buttonMargin;

            btnSair = CriarBotaoMenu("🚪 Sair", yPosition, panelSideBar);
            btnSair.BackColor = Color.FromArgb(192, 57, 43);
            btnSair.Click += (s, e) => SairSistema(); 

            // ===== PAINEL DE CONTEÚDO =====
            panelContent = new Panel
            {
                Location = new Point(250, 0),
                Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width - 250, Screen.PrimaryScreen.Bounds.Height),
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(20),
                AutoScroll = true
            };
            this.Controls.Add(panelContent);

            // Carregar Dashboard por padrão
            CarregarDashboard();
        }

        private void CarregarChatIA()
        {
            panelContent.Controls.Clear();

            Label lblTitulo = new Label
            {
                Text = "🤖 Assistente Virtual - HelpDesk",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitulo);

            Label lblDescricao = new Label
            {
                Text = "Estou aqui para ajudar com dúvidas sobre o sistema, problemas técnicos e orientações.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(30, 80),
                AutoSize = true
            };
            panelContent.Controls.Add(lblDescricao);

            Panel panelChat = new Panel
            {
                Location = new Point(30, 120),
                Size = new System.Drawing.Size(panelContent.Width - 100, 400),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };
            panelContent.Controls.Add(panelChat);

            TextBox txtMensagem = new TextBox
            {
                Location = new Point(30, 540),
                Size = new System.Drawing.Size(panelContent.Width - 160, 40),
                Font = new Font("Segoe UI", 10),
               
            };
            panelContent.Controls.Add(txtMensagem);

            Label lblPlaceholder = new Label
            {
                Text = "Digite sua mensagem...",
                Location = new Point(35, 550),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.White,
                Cursor = Cursors.IBeam
            };
            panelContent.Controls.Add(lblPlaceholder);

            txtMensagem.Enter += (s, e) => lblPlaceholder.Visible = false;
            txtMensagem.Leave += (s, e) => lblPlaceholder.Visible = string.IsNullOrEmpty(txtMensagem.Text);
            txtMensagem.TextChanged += (s, e) => lblPlaceholder.Visible = string.IsNullOrEmpty(txtMensagem.Text);
            lblPlaceholder.Click += (s, e) => txtMensagem.Focus();

            Button btnEnviar = new Button
            {
                Text = "📤 Enviar",
                Location = new Point(panelContent.Width - 120, 540),
                Size = new System.Drawing.Size(80, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnEnviar);

            AdicionarMensagemChat(panelChat, "IA", "Olá! Sou seu assistente virtual do HelpDesk. Como posso ajudar você hoje? Posso auxiliar com:\n• Dúvidas sobre o sistema\n• Problemas técnicos\n• Orientações de uso\n• E muito mais!");

            btnEnviar.Click += (s, e) => EnviarMensagemIA(txtMensagem, panelChat);
            txtMensagem.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    EnviarMensagemIA(txtMensagem, panelChat);
                    e.Handled = true;
                }
            };
        }

        private void AdicionarMensagemChat(Panel panelChat, string autor, string mensagem)
        {
            int yPos = panelChat.Controls.Count > 0 ?
                        panelChat.Controls[panelChat.Controls.Count - 1].Bottom + 10 : 10;

            Panel mensagemPanel = new Panel
            {
                Location = new Point(10, yPos),
                Size = new System.Drawing.Size(panelChat.Width - 30, 0),
                BackColor = autor == "IA" ? Color.FromArgb(240, 248, 255) : Color.FromArgb(230, 247, 255),
                Padding = new Padding(15),
                AutoSize = true
            };

            Label lblMensagem = new Label
            {
                Text = mensagem,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                AutoSize = true,
                MaximumSize = new System.Drawing.Size(panelChat.Width - 60, 0)
            };

            Label lblAutor = new Label
            {
                Text = autor == "IA" ? "🤖 Assistente" : "👤 Você",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(0, 5),
                AutoSize = true
            };

            mensagemPanel.Controls.Add(lblAutor);
            lblMensagem.Location = new Point(0, 25);
            mensagemPanel.Controls.Add(lblMensagem);

            panelChat.Controls.Add(mensagemPanel);
            panelChat.ScrollControlIntoView(mensagemPanel);
        }

        private async void EnviarMensagemIA(TextBox txtMensagem, Panel panelChat)
        {
            string mensagemUsuario = txtMensagem.Text.Trim();
            if (string.IsNullOrWhiteSpace(mensagemUsuario))
                return;

            AdicionarMensagemChat(panelChat, "Usuario", mensagemUsuario);
            txtMensagem.Clear();

            Panel digitandoPanel = new Panel
            {
                Location = new Point(10, panelChat.Controls.Count > 0 ?
                                    panelChat.Controls[panelChat.Controls.Count - 1].Bottom + 10 : 10),
                Size = new System.Drawing.Size(panelChat.Width - 30, 40),
                BackColor = Color.FromArgb(240, 248, 255)
            };

            Label lblDigitando = new Label
            {
                Text = "🤖 Assistente está digitando...",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(15, 10),
                AutoSize = true
            };
            digitandoPanel.Controls.Add(lblDigitando);
            panelChat.Controls.Add(digitandoPanel);
            panelChat.ScrollControlIntoView(digitandoPanel);

            await Task.Run(() =>
            {
                string respostaIA = ObterRespostaGemini(mensagemUsuario);

                this.Invoke(new Action(() =>
                {
                    panelChat.Controls.Remove(digitandoPanel);
                    AdicionarMensagemChat(panelChat, "IA", respostaIA);
                }));
            });
        }

        private string ObterRespostaGemini(string pergunta)
        {
            return GeminiService.ObterResposta(pergunta);
        }


        private Button CriarBotaoMenu(string texto, int yPos, Panel painel)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(15, yPos),
                Size = new System.Drawing.Size(220, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;

            painel.Controls.Add(btn);
            return btn;
        }

        private void CarregarFaq()
        {
            panelContent.Controls.Clear();

            Label lblTitulo = new Label
            {
                Text = "FAQ - Perguntas Frequentes",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitulo);

            int yPos = 80;

            Label lblFiltrar = new Label
            {
                Text = "Filtrar por Categoria:",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(lblFiltrar);

            ComboBox cbFiltroCategoria = new ComboBox
            {
                Location = new Point(180, yPos - 3), 
                Size = new System.Drawing.Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbFiltroCategoria.Items.AddRange(new object[] { "Todas", "Geral", "Acesso e Login", "Problemas Técnicos", "Software", "Hardware", "Rede", "Outros"});
            cbFiltroCategoria.SelectedIndex = 0;
            panelContent.Controls.Add(cbFiltroCategoria);

            yPos += 50;

            Panel panelFaqs = new Panel
            {
                Location = new Point(30, yPos),
                Size = new System.Drawing.Size(panelContent.Width - 80, panelContent.Height - yPos - 30),
                AutoScroll = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(10)
            };

            CarregarFaqsDoBanco(panelFaqs, "Todas");

            cbFiltroCategoria.SelectedIndexChanged += (s, e) =>
            {
                string categoriaFiltro = cbFiltroCategoria.SelectedItem.ToString();
                CarregarFaqsDoBanco(panelFaqs, categoriaFiltro);
            };

            panelContent.Controls.Add(panelFaqs);
        }

        private void CarregarFaqsDoBanco(Panel panelFaqs, string categoriaFiltro = "Todas")
        {
            try
            {
                panelFaqs.Controls.Clear();

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"SELECT Pergunta, Resposta, Categoria, Ordem 
                         FROM Faqs 
                         WHERE Ativo = 1";

                        if (categoriaFiltro != "Todas")
                        {
                            query += " AND Categoria = @Categoria";
                        }

                        query += " ORDER BY Ordem ASC, Id ASC";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            if (categoriaFiltro != "Todas")
                            {
                                cmd.Parameters.AddWithValue("@Categoria", categoriaFiltro);
                            }

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                int yPos = 10;
                                int faqCount = 0;

                                while (reader.Read())
                                {
                                    string pergunta = reader["Pergunta"].ToString();
                                    string resposta = reader["Resposta"].ToString();
                                    string categoria = reader["Categoria"].ToString();
                                    int ordem = Convert.ToInt32(reader["Ordem"]);

                                    Panel cardFaq = CriarCardFaq(pergunta, resposta, categoria, yPos);
                                    panelFaqs.Controls.Add(cardFaq);

                                    yPos += cardFaq.Height + 15;
                                    faqCount++;
                                }

                                if (faqCount == 0)
                                {
                                    Label lblSemFaqs = new Label
                                    {
                                        Text = "Nenhuma FAQ encontrada para esta categoria.",
                                        Location = new Point(20, 20),
                                        AutoSize = true,
                                        Font = new Font("Segoe UI", 11, FontStyle.Italic),
                                        ForeColor = Color.Gray
                                    };
                                    panelFaqs.Controls.Add(lblSemFaqs);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Label lblErro = new Label
                {
                    Text = $"Erro ao carregar FAQs: {ex.Message}",
                    Location = new Point(20, 20),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.Red
                };
                panelFaqs.Controls.Add(lblErro);
            }
        }

        private Panel CriarCardFaq(string pergunta, string resposta, string categoria, int yPos)
        {
            Panel card = new Panel
            {
                Location = new Point(10, yPos),
                Size = new System.Drawing.Size(panelContent.Width - 120, 150),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle, 
                Cursor = Cursors.Hand
            };

            Label lblCategoria = new Label
            {
                Text = categoria,
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                BackColor = Color.FromArgb(220, 237, 255),
                Padding = new Padding(5, 2, 5, 2)
            };
            card.Controls.Add(lblCategoria);

            Label lblPergunta = new Label
            {
                Text = pergunta,
                Location = new Point(15, 40),
                Size = new System.Drawing.Size(card.Width - 30, 30),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121)
            };
            card.Controls.Add(lblPergunta);

            Label lblResposta = new Label
            {
                Text = resposta,
                Location = new Point(15, 75),
                Size = new System.Drawing.Size(card.Width - 30, 60),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                Visible = false
            };
            card.Controls.Add(lblResposta);

            Label lblExpandir = new Label
            {
                Text = "🔽",
                Location = new Point(card.Width - 35, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 12),
                Cursor = Cursors.Hand
            };
            card.Controls.Add(lblExpandir);

            bool expandido = false;
            EventHandler toggleResposta = (s, e) =>
            {
                expandido = !expandido;
                lblResposta.Visible = expandido;
                lblExpandir.Text = expandido ? "🔼" : "🔽";

                if (expandido)
                {
                    card.Height = 150 + lblResposta.Height;
                }
                else
                {
                    card.Height = 150;
                }
            };

            card.Click += toggleResposta;
            lblExpandir.Click += toggleResposta;

            return card;
        }

        private void CarregarDashboard()
        {
            panelContent.Controls.Clear();

            Label lblTitulo = new Label
            {
                Text = "Dashboard - Todos os Chamados",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitulo);

            int xPos = 30;
            int yPos = 80;

            int totalAbertos = ObterQuantidadeChamados("Aberto");
            int totalAndamento = ObterQuantidadeChamados("Em Andamento");
            int totalResolvidos = ObterQuantidadeChamados("Resolvido");

            CriarCard(panelContent, "Chamados Abertos", totalAbertos.ToString(), Color.FromArgb(230, 146, 0), xPos, yPos);     // #e69200
            CriarCard(panelContent, "Em Andamento", totalAndamento.ToString(), Color.FromArgb(19, 110, 255), xPos + 250, yPos); // #136eff
            CriarCard(panelContent, "Resolvidos", totalResolvidos.ToString(), Color.FromArgb(0, 181, 121), xPos + 500, yPos);   // #00b579

            yPos += 150;

            Label lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(lblBuscar);

            TextBox txtBuscar = new TextBox
            {
                Location = new Point(90, yPos - 3),
                Size = new System.Drawing.Size(200, 25),
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(txtBuscar);

            Label lblPlaceholder = new Label
            {
                Text = "Nº chamado ou título...",
                Location = new Point(95, yPos),
                Size = new System.Drawing.Size(190, 20),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8),
                BackColor = Color.White,
                Cursor = Cursors.IBeam
            };
            panelContent.Controls.Add(lblPlaceholder);

            txtBuscar.Enter += (s, e) => lblPlaceholder.Visible = false;
            txtBuscar.Leave += (s, e) => lblPlaceholder.Visible = string.IsNullOrEmpty(txtBuscar.Text);
            txtBuscar.TextChanged += (s, e) => lblPlaceholder.Visible = string.IsNullOrEmpty(txtBuscar.Text);
            lblPlaceholder.Visible = string.IsNullOrEmpty(txtBuscar.Text);

            Label lblFiltrarStatus = new Label
            {
                Text = "Status:",
                Location = new Point(310, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(lblFiltrarStatus);

            ComboBox cbFiltroStatus = new ComboBox
            {
                Location = new Point(360, yPos - 3),
                Size = new System.Drawing.Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbFiltroStatus.Items.AddRange(new object[] { "Todos", "Aberto", "Em Andamento", "Resolvido" });
            cbFiltroStatus.SelectedIndex = 0;
            panelContent.Controls.Add(cbFiltroStatus);

            Button btnBuscar = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(530, yPos - 3),
                Size = new System.Drawing.Size(100, 25),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnBuscar);

            Button btnLimpar = new Button
            {
                Text = "🔄 Limpar",
                Location = new Point(640, yPos - 3),
                Size = new System.Drawing.Size(100, 25),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnLimpar);

            yPos += 50;

            Label lblTodosChamados = new Label
            {
                Text = "Todos os Chamados",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(30, yPos),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTodosChamados);
            yPos += 40;

            DataGridView dgvTodosChamados = new DataGridView
            {
                Location = new Point(30, yPos),
                Size = new System.Drawing.Size(panelContent.Width - 80, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 9),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvTodosChamados.Columns.Add("NumeroChamado", "Nº Chamado");
            dgvTodosChamados.Columns.Add("Titulo", "Título");
            dgvTodosChamados.Columns.Add("Categoria", "Categoria");
            dgvTodosChamados.Columns.Add("Status", "Status");
            dgvTodosChamados.Columns.Add("Prioridade", "Prioridade");
            dgvTodosChamados.Columns.Add("DataAbertura", "Data Abertura");
            dgvTodosChamados.Columns.Add("Responsavel", "Responsável");

            CarregarTodosChamadosDoBanco(dgvTodosChamados, "", "Todos");

            btnBuscar.Click += (s, e) =>
            {
                string termoBusca = txtBuscar.Text.Trim();
                string filtroStatus = cbFiltroStatus.SelectedItem.ToString();
                CarregarTodosChamadosDoBanco(dgvTodosChamados, termoBusca, filtroStatus);
            };

            btnLimpar.Click += (s, e) =>
            {
                txtBuscar.Clear();
                cbFiltroStatus.SelectedIndex = 0;
                CarregarTodosChamadosDoBanco(dgvTodosChamados, "", "Todos");
            };

            txtBuscar.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    string termoBusca = txtBuscar.Text.Trim();
                    string filtroStatus = cbFiltroStatus.SelectedItem.ToString();
                    CarregarTodosChamadosDoBanco(dgvTodosChamados, termoBusca, filtroStatus);
                    e.Handled = true;
                }
            };

            cbFiltroStatus.SelectedIndexChanged += (s, e) =>
            {
                string termoBusca = txtBuscar.Text.Trim();
                string filtroStatus = cbFiltroStatus.SelectedItem.ToString();
                CarregarTodosChamadosDoBanco(dgvTodosChamados, termoBusca, filtroStatus);
            };

            panelContent.Controls.Add(dgvTodosChamados);
        }


        private int ObterQuantidadeChamados(string status)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = "SELECT COUNT(*) FROM Chamados WHERE Status = @Status";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter quantidade de {status}: {ex.Message}");
                return 0;
            }
            return 0;
        }

        private void CarregarTodosChamadosDoBanco(DataGridView dgv, string termoBusca = "", string filtroStatus = "Todos")
        {
            try
            {
                dgv.Rows.Clear();

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"SELECT NumeroChamado, Titulo, Categoria, Status, Prioridade, 
                                        DataAbertura, Responsavel
                                 FROM Chamados 
                                 WHERE 1=1";

                        if (filtroStatus != "Todos")
                        {
                            query += " AND Status = @Status";
                        }

                        if (!string.IsNullOrWhiteSpace(termoBusca))
                        {
                            query += " AND (Titulo LIKE @TermoBusca OR NumeroChamado LIKE @TermoBusca)";
                        }

                        query += " ORDER BY DataAbertura DESC";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            if (filtroStatus != "Todos")
                            {
                                cmd.Parameters.AddWithValue("@Status", filtroStatus);
                            }

                            if (!string.IsNullOrWhiteSpace(termoBusca))
                            {
                                cmd.Parameters.AddWithValue("@TermoBusca", $"%{termoBusca}%");
                            }

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string numeroChamado = reader["NumeroChamado"].ToString();
                                    string titulo = reader["Titulo"].ToString();
                                    string categoria = reader["Categoria"].ToString();
                                    string status = reader["Status"].ToString();
                                    string prioridade = reader["Prioridade"].ToString();
                                    DateTime dataAbertura = Convert.ToDateTime(reader["DataAbertura"]);
                                    string responsavel = reader["Responsavel"].ToString();

                                    dgv.Rows.Add(numeroChamado, titulo, categoria, status, prioridade,
                                                dataAbertura.ToString("dd/MM/yyyy HH:mm"), responsavel);

                                    int rowIndex = dgv.Rows.Count - 1;
                                    ColorirLinhaChamado(dgv, rowIndex, status, prioridade);
                                }
                            }
                        }

                        if (dgv.Rows.Count == 0)
                        {
                            dgv.Rows.Add("", "Nenhum chamado encontrado", "", "", "", "", "");
                            dgv.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                            dgv.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                        }
                    }
                    else
                    {
                        dgv.Rows.Add("", "Erro de conexão com o banco", "", "", "", "", "");
                        dgv.Rows[0].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                dgv.Rows.Add("", $"Erro: {ex.Message}", "", "", "", "", "");
                dgv.Rows[0].DefaultCellStyle.ForeColor = Color.Red;
            }
        }

        private void CarregarCriarChamado()
        {
            panelContent.Controls.Clear();

            Label lblTitulo = new Label
            {
                Text = "Criar Novo Chamado",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitulo);

            int yPos = 80;

            Label lblTituloChamado = new Label
            {
                Text = "Título do Chamado:*",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtTitulo = new TextBox
            {
                Location = new Point(30, yPos + 25),
                Size = new System.Drawing.Size(500, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White
            };
            panelContent.Controls.Add(lblTituloChamado);
            panelContent.Controls.Add(txtTitulo);
            yPos += 80;

            Label lblCategoria = new Label
            {
                Text = "Categoria:*",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            ComboBox cbCategoria = new ComboBox
            {
                Location = new Point(30, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbCategoria.Items.AddRange(new object[] { "Hardware", "Software", "Rede", "Acesso", "Outro" });
            panelContent.Controls.Add(lblCategoria);
            panelContent.Controls.Add(cbCategoria);
            yPos += 80;

            Label lblPrioridade = new Label
            {
                Text = "Prioridade:",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtPrioridade = new TextBox
            {
                Location = new Point(30, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                Font = new Font("Segoe UI", 10),
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240),
                Text = "Selecione uma categoria"
            };
            panelContent.Controls.Add(lblPrioridade);
            panelContent.Controls.Add(txtPrioridade);
            yPos += 80;

            cbCategoria.SelectedIndexChanged += (s, e) =>
            {
                string categoria = cbCategoria.SelectedItem?.ToString();
                string prioridade = CalcularPrioridade(categoria);
                txtPrioridade.Text = prioridade;

                switch (prioridade)
                {
                    case "Urgente":
                        txtPrioridade.BackColor = Color.FromArgb(255, 100, 100);
                        txtPrioridade.ForeColor = Color.White;
                        break;
                    case "Alta":
                        txtPrioridade.BackColor = Color.FromArgb(255, 150, 100);
                        txtPrioridade.ForeColor = Color.White;
                        break;
                    case "Média":
                        txtPrioridade.BackColor = Color.FromArgb(255, 200, 100);
                        txtPrioridade.ForeColor = Color.Black;
                        break;
                    case "Baixa":
                        txtPrioridade.BackColor = Color.FromArgb(200, 255, 200);
                        txtPrioridade.ForeColor = Color.Black;
                        break;
                    default:
                        txtPrioridade.BackColor = Color.FromArgb(240, 240, 240);
                        txtPrioridade.ForeColor = Color.Black;
                        break;
                }
            };

            Label lblDescricao = new Label
            {
                Text = "Descrição Detalhada:*",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            RichTextBox rtbDescricao = new RichTextBox
            {
                Location = new Point(30, yPos + 25),
                Size = new System.Drawing.Size(600, 150),
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(lblDescricao);
            panelContent.Controls.Add(rtbDescricao);
            yPos += 200;

            // Substitua esta parte no CarregarCriarChamado():
            Label lblAnexos = new Label
            {
                Text = "Anexos (Opcional):",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(lblAnexos);
            yPos += 30;

            Button btnAdicionarAnexo = new Button
            {
                Text = "📎 Adicionar Arquivo",
                Location = new Point(30, yPos),
                Size = new System.Drawing.Size(150, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnAdicionarAnexo);

            // ListBox para mostrar arquivos selecionados
            ListBox lstAnexos = new ListBox
            {
                Location = new Point(190, yPos),
                Size = new System.Drawing.Size(300, 80),
                Visible = false
            };
            panelContent.Controls.Add(lstAnexos);

            // Lista para armazenar os arquivos
            List<Anexo> arquivosAnexos = new List<Anexo>();

            btnAdicionarAnexo.Click += (s, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Todos os arquivos (*.*)|*.*|Imagens (*.jpg;*.png;*.gif;*.bmp)|*.jpg;*.png;*.gif;*.bmp|Documentos (*.pdf;*.docx;*.doc;*.txt)|*.pdf;*.docx;*.doc;*.txt",
                    FilterIndex = 1,
                    Multiselect = true,
                    Title = "Selecionar arquivos para anexar"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string arquivo in openFileDialog.FileNames)
                    {
                        try
                        {
                            FileInfo fileInfo = new FileInfo(arquivo);

                            // Verificar tamanho máximo (10MB)
                            if (fileInfo.Length > 10 * 1024 * 1024)
                            {
                                MessageBox.Show($"O arquivo {fileInfo.Name} é muito grande. Tamanho máximo: 10MB", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue;
                            }

                            // Ler arquivo como bytes
                            byte[] dados = File.ReadAllBytes(arquivo);

                            Anexo anexo = new Anexo
                            {
                                NomeArquivo = fileInfo.Name,
                                TipoArquivo = fileInfo.Extension,
                                TamanhoArquivo = fileInfo.Length,
                                DadosArquivo = dados
                            };

                            arquivosAnexos.Add(anexo);
                            lstAnexos.Items.Add($"{fileInfo.Name} ({FormatarTamanho(fileInfo.Length)})");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao ler arquivo {arquivo}: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (lstAnexos.Items.Count > 0)
                    {
                        lstAnexos.Visible = true;
                        yPos += 90; // Ajustar posição
                    }
                }
            };
            yPos += 100;

            Button btnEnviar = new Button
            {
                Text = "📤 Enviar Chamado",
                Location = new Point(30, yPos),
                Size = new System.Drawing.Size(150, 45),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEnviar.Click += (s, e) => EnviarChamado(txtTitulo, cbCategoria, txtPrioridade, rtbDescricao, arquivosAnexos);
            panelContent.Controls.Add(btnEnviar);

            Button btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(190, yPos),
                Size = new System.Drawing.Size(150, 45),
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancelar.Click += (s, e) => CarregarDashboard();
            panelContent.Controls.Add(btnCancelar);

            Label lblObrigatorio = new Label
            {
                Text = "* Campos obrigatórios",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(30, yPos + 60),
                AutoSize = true
            };
            panelContent.Controls.Add(lblObrigatorio);
        }

        private string CalcularPrioridade(string categoria)
        {
            if (string.IsNullOrEmpty(categoria))
                return "Selecione uma categoria";

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

        private void EnviarChamado(TextBox txtTitulo, ComboBox cbCategoria, TextBox txtPrioridade, RichTextBox rtbDescricao, List<Anexo> arquivosAnexos)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("Por favor, preencha o título do chamado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return;
            }

            if (cbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione uma categoria!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategoria.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbDescricao.Text))
            {
                MessageBox.Show("Por favor, preencha a descrição do chamado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescricao.Focus();
                return;
            }

            try
            {
                string numeroChamado = $"{DateTime.Now:yyyy}-{GerarProximoNumero():D4}";
                int chamadoId = 0;

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string queryChamado = @"INSERT INTO Chamados 
                (NumeroChamado, Titulo, Descricao, DataAbertura, Status, Prioridade, Categoria, Responsavel, Solicitante) 
                OUTPUT INSERTED.Id
                VALUES 
                (@NumeroChamado, @Titulo, @Descricao, @DataAbertura, @Status, @Prioridade, @Categoria, @Responsavel, @Solicitante)";

                        using (SqlCommand cmd = new SqlCommand(queryChamado, conn))
                        {
                            cmd.Parameters.AddWithValue("@NumeroChamado", numeroChamado);
                            cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text.Trim());
                            cmd.Parameters.AddWithValue("@Descricao", rtbDescricao.Text.Trim());
                            cmd.Parameters.AddWithValue("@DataAbertura", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Status", "Aberto");
                            cmd.Parameters.AddWithValue("@Prioridade", txtPrioridade.Text);
                            cmd.Parameters.AddWithValue("@Categoria", cbCategoria.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Responsavel", "");
                            cmd.Parameters.AddWithValue("@Solicitante", UserSession.UserName);

                            chamadoId = (int)cmd.ExecuteScalar();

                            if (arquivosAnexos.Count > 0)
                            {
                                SalvarAnexos(chamadoId, arquivosAnexos, conn);
                            }

                            MessageBox.Show($"Chamado #{numeroChamado} criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CarregarDashboard();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar chamado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para salvar anexos
        private void SalvarAnexos(int chamadoId, List<Anexo> anexos, SqlConnection conn)
        {
            string queryAnexo = @"INSERT INTO Anexos 
    (ChamadoId, NomeArquivo, TipoArquivo, TamanhoArquivo, DadosArquivo, DataUpload, UploadPor)
    VALUES 
    (@ChamadoId, @NomeArquivo, @TipoArquivo, @TamanhoArquivo, @DadosArquivo, @DataUpload, @UploadPor)";

            foreach (Anexo anexo in anexos)
            {
                using (SqlCommand cmdAnexo = new SqlCommand(queryAnexo, conn))
                {
                    cmdAnexo.Parameters.AddWithValue("@ChamadoId", chamadoId);
                    cmdAnexo.Parameters.AddWithValue("@NomeArquivo", anexo.NomeArquivo);
                    cmdAnexo.Parameters.AddWithValue("@TipoArquivo", anexo.TipoArquivo);
                    cmdAnexo.Parameters.AddWithValue("@TamanhoArquivo", anexo.TamanhoArquivo);
                    cmdAnexo.Parameters.AddWithValue("@DadosArquivo", anexo.DadosArquivo);
                    cmdAnexo.Parameters.AddWithValue("@DataUpload", DateTime.Now);
                    cmdAnexo.Parameters.AddWithValue("@UploadPor", UserSession.UserName);

                    cmdAnexo.ExecuteNonQuery();
                }
            }
        }

        private int GerarProximoNumero()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = "SELECT COUNT(*) FROM Chamados WHERE YEAR(DataAbertura) = YEAR(GETDATE())";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            int count = (int)cmd.ExecuteScalar();
                            return count + 1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Em caso de erro, retorna um número baseado na data/hora
                return DateTime.Now.Second + DateTime.Now.Millisecond;
            }
            return 1;
        }


        private void CriarCard(Panel painel, string titulo, string valor, Color cor, int x, int y)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new System.Drawing.Size(220, 120),
                BackColor = cor,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblValor = new Label
            {
                Text = valor,
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 40),
                AutoSize = true
            };

            card.Controls.Add(lblTitulo);
            card.Controls.Add(lblValor);
            painel.Controls.Add(card);
        }

        private void CarregarMeusChamados()
        {
            panelContent.Controls.Clear();

            Label lblTitulo = new Label
            {
                Text = "Meus Chamados",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitulo);

            Button btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new Point(200, 30),
                Size = new System.Drawing.Size(100, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAtualizar.Click += (s, e) => CarregarMeusChamados();
            panelContent.Controls.Add(btnAtualizar);

            int yPosFiltros = 80;

            Label lblFiltrar = new Label
            {
                Text = "Filtrar por Status:",
                Location = new Point(30, yPosFiltros),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            panelContent.Controls.Add(lblFiltrar);

            ComboBox cbFiltroStatus = new ComboBox
            {
                Location = new Point(140, yPosFiltros - 3),
                Size = new System.Drawing.Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbFiltroStatus.Items.AddRange(new object[] { "Todos", "Aberto", "Em Andamento", "Resolvido", "Fechado" });
            cbFiltroStatus.SelectedIndex = 0;
            panelContent.Controls.Add(cbFiltroStatus);

            DataGridView dgvChamados = new DataGridView
            {
                Location = new Point(30, 120),
                Size = new System.Drawing.Size(panelContent.Width - 80, 400),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 9)
            };

            dgvChamados.Columns.Add("NumeroChamado", "Nº Chamado");
            dgvChamados.Columns.Add("Titulo", "Título");
            dgvChamados.Columns.Add("Categoria", "Categoria");
            dgvChamados.Columns.Add("Status", "Status");
            dgvChamados.Columns.Add("Prioridade", "Prioridade");
            dgvChamados.Columns.Add("DataAbertura", "Data Abertura");
            dgvChamados.Columns.Add("Responsavel", "Responsável");

            dgvChamados.Columns["NumeroChamado"].Width = 100;
            dgvChamados.Columns["Titulo"].Width = 200;
            dgvChamados.Columns["Categoria"].Width = 100;
            dgvChamados.Columns["Status"].Width = 120;
            dgvChamados.Columns["Prioridade"].Width = 100;
            dgvChamados.Columns["DataAbertura"].Width = 120;
            dgvChamados.Columns["Responsavel"].Width = 150;

            CarregarChamadosDoBanco(dgvChamados, cbFiltroStatus.Text);

            cbFiltroStatus.SelectedIndexChanged += (s, e) =>
            {
                CarregarChamadosDoBanco(dgvChamados, cbFiltroStatus.Text);
            };

            panelContent.Controls.Add(dgvChamados);

            int yPosBotoes = 540;

            Button btnVisualizar = new Button
            {
                Text = "👁️ Visualizar",
                Location = new Point(30, yPosBotoes),
                Size = new System.Drawing.Size(120, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnVisualizar.Click += (s, e) => VisualizarChamadoSelecionado(dgvChamados);
            panelContent.Controls.Add(btnVisualizar);

            Button btnEditar = new Button
            {
                Text = "✏️ Editar",
                Location = new Point(160, yPosBotoes),
                Size = new System.Drawing.Size(120, 40),
                BackColor = Color.FromArgb(241, 196, 15),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEditar.Click += (s, e) => EditarChamadoSelecionado(dgvChamados);
            panelContent.Controls.Add(btnEditar);

            Button btnExcluir = new Button
            {
                Text = "🗑️ Excluir",
                Location = new Point(290, yPosBotoes),
                Size = new System.Drawing.Size(120, 40),
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExcluir.Click += (s, e) => ExcluirChamadoSelecionado(dgvChamados);
            panelContent.Controls.Add(btnExcluir);

            Button btnNovoChamado = new Button
            {
                Text = "➕ Novo Chamado",
                Location = new Point(420, yPosBotoes),
                Size = new System.Drawing.Size(140, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNovoChamado.Click += (s, e) => CarregarCriarChamado();
            panelContent.Controls.Add(btnNovoChamado);
        }

        private void CarregarChamadosDoBanco(DataGridView dgv, string filtroStatus = "Todos")
        {
            try
            {
                dgv.Rows.Clear();

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        
                        bool colunaExiste = VerificarColunaExiste("Chamados", "Solicitante");

                        string query = @"SELECT NumeroChamado, Titulo, Categoria, Status, Prioridade, 
                                        DataAbertura, Responsavel";

                        if (colunaExiste)
                        {
                            query += ", Solicitante";
                        }

                        query += " FROM Chamados WHERE 1=1";

                        
                        if (colunaExiste)
                        {
                            query += " AND Solicitante = @Solicitante";
                        }
                        else
                        {
                           
                            query += " AND 1=1";
                        }

                        if (filtroStatus != "Todos")
                        {
                            query += " AND Status = @Status";
                        }

                        query += " ORDER BY DataAbertura DESC";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            if (colunaExiste)
                            {
                                cmd.Parameters.AddWithValue("@Solicitante", UserSession.UserName);
                            }

                            if (filtroStatus != "Todos")
                            {
                                cmd.Parameters.AddWithValue("@Status", filtroStatus);
                            }

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                int chamadosDoUsuario = 0;

                                while (reader.Read())
                                {
                                    string numeroChamado = reader["NumeroChamado"].ToString();
                                    string titulo = reader["Titulo"].ToString();
                                    string categoria = reader["Categoria"].ToString();
                                    string status = reader["Status"].ToString();
                                    string prioridade = reader["Prioridade"].ToString();
                                    DateTime dataAbertura = Convert.ToDateTime(reader["DataAbertura"]);
                                    string responsavel = reader["Responsavel"].ToString();

                                    dgv.Rows.Add(numeroChamado, titulo, categoria, status, prioridade,
                                                dataAbertura.ToString("dd/MM/yyyy HH:mm"), responsavel);

                                    int rowIndex = dgv.Rows.Count - 1;
                                    ColorirLinhaChamado(dgv, rowIndex, status, prioridade);
                                    chamadosDoUsuario++;
                                }

                                if (chamadosDoUsuario == 0)
                                {
                                    if (colunaExiste)
                                    {
                                        dgv.Rows.Add("", "Você ainda não criou nenhum chamado", "", "", "", "", "");
                                    }
                                    else
                                    {
                                        dgv.Rows.Add("", "Nenhum chamado encontrado", "", "", "", "", "");
                                    }
                                    dgv.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                                    dgv.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dgv.Rows.Add("", $"Erro: {ex.Message}", "", "", "", "", "");
                dgv.Rows[0].DefaultCellStyle.ForeColor = Color.Red;
            }
        }

        private bool VerificarColunaExiste(string tabela, string coluna)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = @Tabela AND COLUMN_NAME = @Coluna";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Tabela", tabela);
                            cmd.Parameters.AddWithValue("@Coluna", coluna);

                            int resultado = (int)cmd.ExecuteScalar();
                            return resultado > 0;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        private void ColorirLinhaChamado(DataGridView dgv, int rowIndex, string status, string prioridade)
        {
            Color corFundo = Color.White;
            Color corTexto = Color.Black;

            switch (status)
            {
                case "Aberto":
                    corFundo = Color.FromArgb(255, 243, 205); 
                    break;
                case "Em Andamento":
                    corFundo = Color.FromArgb(220, 237, 255);
                    break;
                case "Resolvido":
                    corFundo = Color.FromArgb(212, 237, 218); 
                    break;
            }

            if (prioridade == "Urgente")
            {
                corFundo = Color.FromArgb(255, 220, 220); 
            }

            dgv.Rows[rowIndex].DefaultCellStyle.BackColor = corFundo;
            dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = corTexto;
        }

        private void VisualizarChamadoSelecionado(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, selecione um chamado para visualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string numeroChamado = dgv.SelectedRows[0].Cells["NumeroChamado"].Value.ToString();

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = "SELECT Id FROM Chamados WHERE NumeroChamado = @NumeroChamado";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@NumeroChamado", numeroChamado);
                            object result = cmd.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                int idChamado = Convert.ToInt32(result);
                                VisualizarEditarChamadoForm form = new VisualizarEditarChamadoForm(idChamado, false);
                                form.ShowDialog();

                                CarregarMeusChamados();
                            }
                            else
                            {
                                MessageBox.Show("Chamado não encontrado no banco de dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao visualizar chamado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void EditarChamadoSelecionado(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, selecione um chamado para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string numeroChamado = dgv.SelectedRows[0].Cells["NumeroChamado"].Value.ToString();
            string status = dgv.SelectedRows[0].Cells["Status"].Value.ToString();

            if (status != "Aberto")
            {
                MessageBox.Show("Apenas chamados com status 'Aberto' podem ser editados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = "SELECT Id FROM Chamados WHERE NumeroChamado = @NumeroChamado";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@NumeroChamado", numeroChamado);
                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                int chamadoId = Convert.ToInt32(result);

                                VisualizarEditarChamadoForm form = new VisualizarEditarChamadoForm(chamadoId, true);
                                form.ShowDialog();

                                CarregarMeusChamados();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExcluirChamadoSelecionado(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, selecione um chamado para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string numeroChamado = dgv.SelectedRows[0].Cells["NumeroChamado"].Value.ToString();
            string titulo = dgv.SelectedRows[0].Cells["Titulo"].Value.ToString();
            string status = dgv.SelectedRows[0].Cells["Status"].Value.ToString();

            if (status != "Aberto")
            {
                MessageBox.Show("Apenas chamados com status 'Aberto' podem ser excluídos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = MessageBox.Show(
                $"Tem certeza que deseja excluir o chamado?\n\nNº: {numeroChamado}\nTítulo: {titulo}",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = DatabaseConnection.GetConnection())
                    {
                        if (conn != null)
                        {
                            string queryComentarios = "DELETE FROM Comentarios WHERE ChamadoId IN (SELECT Id FROM Chamados WHERE NumeroChamado = @NumeroChamado)";
                            using (SqlCommand cmdComentarios = new SqlCommand(queryComentarios, conn))
                            {
                                cmdComentarios.Parameters.AddWithValue("@NumeroChamado", numeroChamado);
                                cmdComentarios.ExecuteNonQuery();
                            }

                            string queryChamado = "DELETE FROM Chamados WHERE NumeroChamado = @NumeroChamado";
                            using (SqlCommand cmdChamado = new SqlCommand(queryChamado, conn))
                            {
                                cmdChamado.Parameters.AddWithValue("@NumeroChamado", numeroChamado);
                                int rowsAffected = cmdChamado.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Chamado excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CarregarMeusChamados();
                                }
                                else
                                {
                                    MessageBox.Show("Chamado não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir chamado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarPerfil()
        {
            panelContent.Controls.Clear();

            Label lblTitulo = new Label
            {
                Text = "Meu Perfil",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitulo);

            int yPos = 80;

            var dadosUsuario = CarregarDadosUsuario();

            Label lblNome = new Label
            {
                Text = "Nome Completo:*",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtNome = new TextBox
            {
                Location = new Point(30, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                Text = dadosUsuario.Nome,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(240, 240, 240),
                ReadOnly = true
            };
            panelContent.Controls.Add(lblNome);
            panelContent.Controls.Add(txtNome);
            yPos += 80;

            Label lblEmail = new Label
            {
                Text = "Email:*",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtEmail = new TextBox
            {
                Location = new Point(30, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                Text = dadosUsuario.Email,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(240, 240, 240),
                ReadOnly = true
            };
            panelContent.Controls.Add(lblEmail);
            panelContent.Controls.Add(txtEmail);
            yPos += 80;

            Label lblDataCadastro = new Label
            {
                Text = "Data de Cadastro:",
                Location = new Point(30, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtDataCadastro = new TextBox
            {
                Location = new Point(30, yPos + 25),
                Size = new System.Drawing.Size(400, 30),
                Text = dadosUsuario.DataCadastro,
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            panelContent.Controls.Add(lblDataCadastro);
            panelContent.Controls.Add(txtDataCadastro);
            yPos += 80;

            Button btnEditar = new Button
            {
                Text = "✏️ Editar Perfil",
                Location = new Point(30, yPos),
                Size = new System.Drawing.Size(150, 45),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = false 
            };
            panelContent.Controls.Add(btnEditar);

            Button btnAlterarSenha = new Button
            {
                Text = "🔒 Alterar Senha",
                Location = new Point(190, yPos),
                Size = new System.Drawing.Size(150, 45),
                BackColor = Color.FromArgb(241, 196, 15),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnAlterarSenha);

            Button btnSalvar = new Button
            {
                Text = "💾 Salvar Alterações",
                Location = new Point(30, yPos + 60), 
                Size = new System.Drawing.Size(150, 45),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false 
            };
            panelContent.Controls.Add(btnSalvar);

            Button btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(190, yPos + 60), 
                Size = new System.Drawing.Size(150, 45),
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false 
            };
            panelContent.Controls.Add(btnCancelar);

            btnEditar.Click += (s, e) =>
            {
                bool modoEdicao = !(bool)btnEditar.Tag;
                btnEditar.Tag = modoEdicao;

                if (modoEdicao)
                {
                    // Entrar no modo edição
                    btnEditar.Text = "👁️ Visualizar";
                    btnEditar.BackColor = Color.FromArgb(149, 165, 166);

                    // Mostrar botões Salvar e Cancelar
                    btnSalvar.Visible = true;
                    btnCancelar.Visible = true;
                    btnAlterarSenha.Enabled = false;

                    // Liberar campos para edição
                    txtNome.ReadOnly = false;
                    txtNome.BackColor = Color.White;

                    txtEmail.ReadOnly = false;
                    txtEmail.BackColor = Color.White;
                }
                else
                {
                    // Voltar ao modo visualização
                    btnEditar.Text = "✏️ Editar Perfil";
                    btnEditar.BackColor = Color.FromArgb(52, 152, 219);

                    // Esconder botões Salvar e Cancelar
                    btnSalvar.Visible = false;
                    btnCancelar.Visible = false;
                    btnAlterarSenha.Enabled = true;

                    // Bloquear campos
                    txtNome.ReadOnly = true;
                    txtNome.BackColor = Color.FromArgb(240, 240, 240);

                    txtEmail.ReadOnly = true;
                    txtEmail.BackColor = Color.FromArgb(240, 240, 240);

                    // Recarregar dados originais
                    var dadosOriginais = CarregarDadosUsuario();
                    txtNome.Text = dadosOriginais.Nome;
                    txtEmail.Text = dadosOriginais.Email;
                }
            };

            btnSalvar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("O nome é obrigatório!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("O email é obrigatório!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Salvar no banco de dados
                    using (SqlConnection conn = DatabaseConnection.GetConnection())
                    {
                        if (conn != null)
                        {
                            string query = @"UPDATE Usuarios 
                            SET Nome = @Nome, Email = @Email
                            WHERE Id = @UsuarioId";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                                cmd.Parameters.AddWithValue("@UsuarioId", UserSession.UserId);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Perfil atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Atualizar a sessão
                                    UserSession.UserName = txtNome.Text.Trim();
                                    UserSession.UserEmail = txtEmail.Text.Trim();

                                    // Atualizar label do usuário na sidebar
                                    lblUsuario.Text = $"Usuário: {UserSession.UserName}";

                                    // Voltar ao modo visualização
                                    btnEditar.PerformClick();
                                }
                                else
                                {
                                    MessageBox.Show("Erro ao atualizar perfil.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar perfil: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancelar.Click += (s, e) =>
            {
                // Recarregar dados originais e voltar ao modo visualização
                var dadosOriginais = CarregarDadosUsuario();
                txtNome.Text = dadosOriginais.Nome;
                txtEmail.Text = dadosOriginais.Email;

                btnEditar.PerformClick(); // Volta ao modo visualização
            };

            btnAlterarSenha.Click += (s, e) =>
            {
                AbrirFormAlterarSenha();
            };
        }

       

        private DadosUsuario CarregarDadosUsuario()
        {
            var dados = new DadosUsuario
            {
                Nome = UserSession.UserName,
                Email = UserSession.UserEmail,
                DataCadastro = DateTime.Now.ToString("dd/MM/yyyy")
            };

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = "SELECT Nome, Email, DataCadastro FROM Usuarios WHERE Id = @UsuarioId";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UsuarioId", UserSession.UserId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    dados.Nome = reader["Nome"].ToString();
                                    dados.Email = reader["Email"].ToString();
                                    dados.DataCadastro = Convert.ToDateTime(reader["DataCadastro"]).ToString("dd/MM/yyyy");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar dados do usuário: {ex.Message}");
            }

            return dados;
        }

        private void AbrirFormAlterarSenha()
        {
            Form formSenha = new Form
            {
                Text = "Alterar Senha",
                Size = new Size(450, 400),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(245, 245, 245),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(20)
            };

            int yPos = 20;

            Label lblTitulo = new Label
            {
                Text = "Alterar Senha",
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121)
            };
            formSenha.Controls.Add(lblTitulo);
            yPos += 50;

            // Senha Atual
            Label lblSenhaAtual = new Label
            {
                Text = "Senha Atual:*",
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtSenhaAtual = new TextBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(350, 30),
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            formSenha.Controls.Add(lblSenhaAtual);
            formSenha.Controls.Add(txtSenhaAtual);
            yPos += 70;

            // Nova Senha
            Label lblNovaSenha = new Label
            {
                Text = "Nova Senha:*",
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtNovaSenha = new TextBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(350, 30),
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            formSenha.Controls.Add(lblNovaSenha);
            formSenha.Controls.Add(txtNovaSenha);
            yPos += 70;

            // Confirmar Nova Senha
            Label lblConfirmarSenha = new Label
            {
                Text = "Confirmar Nova Senha:*",
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            TextBox txtConfirmarSenha = new TextBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(350, 30),
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };
            formSenha.Controls.Add(lblConfirmarSenha);
            formSenha.Controls.Add(txtConfirmarSenha);
            yPos += 90; 

            Button btnConfirmar = new Button
            {
                Text = "💾 Confirmar",
                Location = new Point(20, yPos),
                Size = new Size(150, 45),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            Button btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(180, yPos),
                Size = new Size(150, 45),
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnConfirmar.Click += (s, e) =>
            {
                // Validações básicas
                if (string.IsNullOrWhiteSpace(txtSenhaAtual.Text))
                {
                    MessageBox.Show("A senha atual é obrigatória!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSenhaAtual.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNovaSenha.Text))
                {
                    MessageBox.Show("A nova senha é obrigatória!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaSenha.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfirmarSenha.Text))
                {
                    MessageBox.Show("A confirmação da senha é obrigatória!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmarSenha.Focus();
                    return;
                }

                if (txtNovaSenha.Text != txtConfirmarSenha.Text)
                {
                    MessageBox.Show("As senhas não coincidem!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmarSenha.Focus();
                    txtConfirmarSenha.SelectAll();
                    return;
                }

                if (txtNovaSenha.Text.Length < 6)
                {
                    MessageBox.Show("A nova senha deve ter pelo menos 6 caracteres!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaSenha.Focus();
                    txtNovaSenha.SelectAll();
                    return;
                }

                if (txtSenhaAtual.Text == txtNovaSenha.Text)
                {
                    MessageBox.Show("A nova senha deve ser diferente da senha atual!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaSenha.Focus();
                    txtNovaSenha.SelectAll();
                    return;
                }

                // Simular alteração de senha
                try
                {
                    using (SqlConnection conn = DatabaseConnection.GetConnection())
                    {
                        if (conn != null)
                        {
                            // Verificar senha atual (EM PRODUÇÃO, USE HASH!)
                            string queryVerificar = "SELECT COUNT(*) FROM Usuarios WHERE Id = @UsuarioId AND Senha = @SenhaAtual";
                            using (SqlCommand cmdVerificar = new SqlCommand(queryVerificar, conn))
                            {
                                cmdVerificar.Parameters.AddWithValue("@UsuarioId", UserSession.UserId);
                                cmdVerificar.Parameters.AddWithValue("@SenhaAtual", txtSenhaAtual.Text);

                                int resultado = (int)cmdVerificar.ExecuteScalar();

                                if (resultado > 0)
                                {
                                    // Atualizar senha (EM PRODUÇÃO, USE HASH!)
                                    string queryAtualizar = "UPDATE Usuarios SET Senha = @NovaSenha WHERE Id = @UsuarioId";
                                    using (SqlCommand cmdAtualizar = new SqlCommand(queryAtualizar, conn))
                                    {
                                        cmdAtualizar.Parameters.AddWithValue("@NovaSenha", txtNovaSenha.Text);
                                        cmdAtualizar.Parameters.AddWithValue("@UsuarioId", UserSession.UserId);

                                        int rowsAffected = cmdAtualizar.ExecuteNonQuery();

                                        if (rowsAffected > 0)
                                        {
                                            MessageBox.Show("Senha alterada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            formSenha.Close();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Erro ao alterar senha.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Senha atual incorreta!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtSenhaAtual.Focus();
                                    txtSenhaAtual.SelectAll();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancelar.Click += (s, e) => formSenha.Close();

            formSenha.Controls.Add(btnConfirmar);
            formSenha.Controls.Add(btnCancelar);

            formSenha.Shown += (s, e) => txtSenhaAtual.Focus();

            formSenha.ShowDialog();
        }

        private void SairSistema()
        {
            DialogResult resultado = MessageBox.Show("Deseja realmente sair do sistema?", "Confirmar Saída", MessageBoxButtons.YesNo);
            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        public class DadosUsuario
        {
            public string Nome { get; set; }
            public string Email { get; set; }
            public string DataCadastro { get; set; }
        }

        public class Anexo
        {
            public string NomeArquivo { get; set; }
            public string TipoArquivo { get; set; }
            public long TamanhoArquivo { get; set; }
            public byte[] DadosArquivo { get; set; }
        }

        private string FormatarTamanho(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = bytes;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void CentralizarJanela()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
