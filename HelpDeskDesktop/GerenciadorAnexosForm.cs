using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    public partial class GerenciadorAnexosForm : Form
    {
        private int chamadoID;
        private Panel panelConteudo;

        public GerenciadorAnexosForm(int idChamado)
        {
            this.chamadoID = idChamado;
            InitializeComponent();
            CarregarAnexos();
        }

        private void InitializeComponent()
        {
            this.Text = $"Anexos - Chamado #{chamadoID:D3}";
            this.Size = new Size(700, 500); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            panelConteudo = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };
            this.Controls.Add(panelConteudo);

            Label lblTitulo = new Label
            {
                Text = $"Anexos do Chamado #{chamadoID:D3}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121),
                Location = new Point(0, 0),
                AutoSize = true
            };
            panelConteudo.Controls.Add(lblTitulo);

            Button btnAdicionar = new Button
            {
                Text = "➕ Adicionar Anexo",
                Location = new Point(500, 0), 
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.MouseEnter += (s, e) => btnAdicionar.BackColor = Color.FromArgb(39, 174, 96);
            btnAdicionar.MouseLeave += (s, e) => btnAdicionar.BackColor = Color.FromArgb(46, 204, 113);
            btnAdicionar.Click += (s, e) => AdicionarNovoAnexo();
            panelConteudo.Controls.Add(btnAdicionar);
        }

        private void CarregarAnexos()
        {
            try
            {
                int yPos = 50; 

                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = @"SELECT Id, NomeArquivo, TipoArquivo, TamanhoArquivo, DataUpload, UploadPor
                                 FROM Anexos 
                                 WHERE ChamadoId = @ChamadoID
                                 ORDER BY DataUpload DESC";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ChamadoID", this.chamadoID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int anexoId = Convert.ToInt32(reader["Id"]);
                                    string nomeArquivo = reader["NomeArquivo"].ToString();
                                    string tipoArquivo = reader["TipoArquivo"].ToString();
                                    long tamanho = Convert.ToInt64(reader["TamanhoArquivo"]);
                                    DateTime dataUpload = Convert.ToDateTime(reader["DataUpload"]);
                                    string uploadPor = reader["UploadPor"].ToString();

                                    CriarCardAnexo(anexoId, nomeArquivo, tipoArquivo, tamanho, dataUpload, uploadPor, yPos);
                                    yPos += 120;
                                }
                            }
                        }

                        if (yPos == 50)
                        {
                            Label lblSemAnexos = new Label
                            {
                                Text = "Nenhum anexo encontrado para este chamado.",
                                Location = new Point(0, 80),
                                AutoSize = true,
                                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                                ForeColor = Color.Gray
                            };
                            panelConteudo.Controls.Add(lblSemAnexos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar anexos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CriarCardAnexo(int anexoId, string nomeArquivo, string tipoArquivo, long tamanho, DateTime dataUpload, string uploadPor, int yPos)
        {
            int cardWidth = panelConteudo.Width - 60; // Mais margem para centralizar
            int cardHeight = 100;

            Panel card = new Panel
            {
                Location = new Point(20, yPos), // Centralizado com margem maior
                Size = new Size(cardWidth, cardHeight),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            // Ícone do arquivo baseado no tipo
            string icone = GetIconeArquivo(tipoArquivo);

            Label lblIcone = new Label
            {
                Text = icone,
                Location = new Point(15, 15),
                Font = new Font("Segoe UI", 24), // Ícone maior
                AutoSize = true
            };
            card.Controls.Add(lblIcone);

            // Informações do arquivo - alinhadas à esquerda
            Label lblNome = new Label
            {
                Text = nomeArquivo,
                Location = new Point(70, 15),
                Size = new Size(cardWidth - 200, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121)
            };
            card.Controls.Add(lblNome);

            Label lblInfo = new Label
            {
                Text = $"{FormatarTamanho(tamanho)} • {dataUpload:dd/MM/yyyy HH:mm}",
                Location = new Point(70, 45),
                Size = new Size(cardWidth - 200, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray
            };
            card.Controls.Add(lblInfo);

            Label lblUploadPor = new Label
            {
                Text = $"Enviado por: {uploadPor}",
                Location = new Point(70, 65),
                Size = new Size(cardWidth - 200, 20),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.DarkGray
            };
            card.Controls.Add(lblUploadPor);

            // Container para os botões 
            Panel panelBotoes = new Panel
            {
                Location = new Point(cardWidth - 180, 20),
                Size = new Size(160, 60),
                BackColor = Color.Transparent
            };
            card.Controls.Add(panelBotoes);

            // Botão Visualizar 
            Button btnVisualizar = new Button
            {
                Text = "👁️",
                Location = new Point(0, 0),
                Size = new Size(40, 30), 
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Cursor = Cursors.Hand,
                Tag = anexoId,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnVisualizar.FlatAppearance.BorderSize = 0;

            btnVisualizar.Click += new EventHandler((s, e) => VisualizarAnexo(anexoId));

            // Botão Download
            Button btnDownload = new Button
            {
                Text = "⬇️",
                Location = new Point(50, 0),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Cursor = Cursors.Hand,
                Tag = anexoId,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter 
            };
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.Click += (s, e) => DownloadAnexo(anexoId, nomeArquivo);

            // Botão Excluir
            Button btnExcluir = new Button
            {
                Text = "🗑️",
                Location = new Point(100, 0),
                Size = new Size(40, 30),
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Cursor = Cursors.Hand,
                Tag = anexoId,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter 
            };
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.Click += (s, e) => ExcluirAnexo(anexoId, nomeArquivo);

            // ToolTip para os botões
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnVisualizar, "Visualizar arquivo");
            toolTip.SetToolTip(btnDownload, "Download arquivo");
            toolTip.SetToolTip(btnExcluir, "Excluir arquivo");

            panelBotoes.Controls.Add(btnVisualizar);
            panelBotoes.Controls.Add(btnDownload);
            panelBotoes.Controls.Add(btnExcluir);

            btnVisualizar.BringToFront();
            panelBotoes.BringToFront();
            card.BringToFront();

            // Efeito hover nos botões
            btnVisualizar.MouseEnter += (s, e) => btnVisualizar.BackColor = Color.FromArgb(41, 128, 185);
            btnVisualizar.MouseLeave += (s, e) => btnVisualizar.BackColor = Color.FromArgb(52, 152, 219);

            btnDownload.MouseEnter += (s, e) => btnDownload.BackColor = Color.FromArgb(39, 174, 96);
            btnDownload.MouseLeave += (s, e) => btnDownload.BackColor = Color.FromArgb(46, 204, 113);

            btnExcluir.MouseEnter += (s, e) => btnExcluir.BackColor = Color.FromArgb(231, 76, 60);
            btnExcluir.MouseLeave += (s, e) => btnExcluir.BackColor = Color.FromArgb(192, 57, 43);

            panelConteudo.Controls.Add(card);
        }

        private void ExcluirAnexo(int anexoId, string nomeArquivo)
        {
            DialogResult resultado = MessageBox.Show(
                $"Tem certeza que deseja excluir o anexo?\n\n{nomeArquivo}",
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
                            string query = "DELETE FROM Anexos WHERE Id = @AnexoId";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@AnexoId", anexoId);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Anexo excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    RecarregarAnexos();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir anexo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetIconeArquivo(string tipoArquivo)
        {
            tipoArquivo = tipoArquivo.ToLower();
            if (tipoArquivo.Contains(".pdf")) return "📕";
            if (tipoArquivo.Contains(".doc")) return "📘";
            if (tipoArquivo.Contains(".xls")) return "📗";
            if (tipoArquivo.Contains(".jpg") || tipoArquivo.Contains(".png") || tipoArquivo.Contains(".gif")) return "🖼️";
            if (tipoArquivo.Contains(".zip") || tipoArquivo.Contains(".rar")) return "📦";
            return "📄";
        }

        private void VisualizarAnexo(int anexoId)
        {

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn != null)
                    {
                        string query = "SELECT NomeArquivo, TipoArquivo, DadosArquivo FROM Anexos WHERE Id = @AnexoId";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@AnexoId", anexoId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string nomeArquivo = reader["NomeArquivo"].ToString();
                                    string tipoArquivo = reader["TipoArquivo"].ToString();
                                    byte[] dados = (byte[])reader["DadosArquivo"];

                                    // Criar arquivo temporário
                                    string tempPath = Path.GetTempFileName() + tipoArquivo;
                                    File.WriteAllBytes(tempPath, dados);

                                    // Abrir arquivo com programa padrão
                                    System.Diagnostics.Process.Start(tempPath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao visualizar anexo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DownloadAnexo(int anexoId, string nomeArquivo)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    FileName = nomeArquivo,
                    Filter = "Todos os arquivos (*.*)|*.*"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (SqlConnection conn = DatabaseConnection.GetConnection())
                    {
                        if (conn != null)
                        {
                            string query = "SELECT DadosArquivo FROM Anexos WHERE Id = @AnexoId";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@AnexoId", anexoId);

                                byte[] dados = (byte[])cmd.ExecuteScalar();
                                File.WriteAllBytes(saveDialog.FileName, dados);

                                MessageBox.Show($"Arquivo salvo em: {saveDialog.FileName}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao fazer download: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdicionarNovoAnexo()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Todos os arquivos (*.*)|*.*",
                Multiselect = true,
                Title = "Selecionar arquivos para anexar"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (SqlConnection conn = DatabaseConnection.GetConnection())
                    {
                        if (conn != null)
                        {
                            string query = @"INSERT INTO Anexos 
                    (ChamadoId, NomeArquivo, TipoArquivo, TamanhoArquivo, DadosArquivo, DataUpload, UploadPor)
                    VALUES 
                    (@ChamadoId, @NomeArquivo, @TipoArquivo, @TamanhoArquivo, @DadosArquivo, @DataUpload, @UploadPor)";

                            foreach (string arquivo in openFileDialog.FileNames)
                            {
                                FileInfo fileInfo = new FileInfo(arquivo);

                                if (fileInfo.Length > 10 * 1024 * 1024)
                                {
                                    MessageBox.Show($"O arquivo {fileInfo.Name} é muito grande. Tamanho máximo: 10MB", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    continue;
                                }

                                byte[] dados = File.ReadAllBytes(arquivo);

                                using (SqlCommand cmd = new SqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@ChamadoId", this.chamadoID);
                                    cmd.Parameters.AddWithValue("@NomeArquivo", fileInfo.Name);
                                    cmd.Parameters.AddWithValue("@TipoArquivo", fileInfo.Extension);
                                    cmd.Parameters.AddWithValue("@TamanhoArquivo", fileInfo.Length);
                                    cmd.Parameters.AddWithValue("@DadosArquivo", dados);
                                    cmd.Parameters.AddWithValue("@DataUpload", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@UploadPor", UserSession.UserName);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("Anexos adicionados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // CORREÇÃO: Recarregar corretamente sem perder o título
                            RecarregarAnexos();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao adicionar anexos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       

        private void RecarregarAnexos()
        {
            var controlesParaRemover = new List<Control>();

            foreach (Control control in panelConteudo.Controls)
            {
                if (!(control is Label && control.Location.Y == 0) &&
                    !(control is Button && control.Text == "➕ Adicionar Anexo"))
                {
                    controlesParaRemover.Add(control);
                }
            }

            foreach (Control control in controlesParaRemover)
            {
                panelConteudo.Controls.Remove(control);
                control.Dispose();
            }

            CarregarAnexos();

            panelConteudo.Refresh();
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
    }
}