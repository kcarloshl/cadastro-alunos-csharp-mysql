using MySql.Data.MySqlClient;
using System.Data;
using System.Net.Mail;

namespace ConexaoBD;

public partial class CadastroForm : Form
{
    private const string ConnectionString = "Server=localhost;Port=3306;Database=inscricoes;" + "User ID=app_inscricoes;Password=localroot;";
    private Aluno? alunoEmEdicao;
    public CadastroForm()
    {
        MontarTela();
        erros.ContainerControl = this;
        ConectarEventos();
        PrepararNovo();
        CarregarEstudantesAsync();
    }
    private void ConectarEventos()
    {
        btnSalvar.Click += btnSalvar_Click;
        btnNovo.Click += NovoAluno;
        btnExcluir.Click += ExcluirAluno;
        grade.CellClick += SelecionarAluno;
    }

    private void NovoAluno(object? sender, EventArgs args)
    {
        PrepararNovo();
    }

    private void PrepararNovo()
    {
        alunoEmEdicao = null;

        txtNome.Clear();
        txtEmail.Clear();
        txtCurso.Clear();
        dtpData_Ingresso.Value = DateTime.Today;
        txtTelefone.Clear();
        txtCidade.Clear();

        grade.ClearSelection();
        erros.Clear();

        btnSalvar.Text = "Salvar";
        btnExcluir.Enabled = false;

        lblStatus.Text = "Novo aluno. Preencha e clique em Salvar.";
        txtNome.Focus();
    }

    private bool ValidarCampos()
    {
        erros.Clear();

        if (string.IsNullOrWhiteSpace(txtNome.Text) || txtNome.Text.Trim().Length < 3)
        {
            erros.SetError(txtNome, "Informe um nome com pelo menos 3 caracteres.");
            lblStatus.Text = "Revise o nome do aluno.";
            txtNome.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            erros.SetError(txtEmail, "Informe o e-mail.");
            lblStatus.Text = "Informe o e-mail do aluno.";
            txtEmail.Focus();
            return false;
        }

        try
        {
            var email = new MailAddress(txtEmail.Text.Trim());

            if (email.Address != txtEmail.Text.Trim())
                throw new FormatException();
        }
        catch (FormatException)
        {
            erros.SetError(txtEmail, "Informe um e-mail válido.");
            lblStatus.Text = "Revise o e-mail do aluno.";
            txtEmail.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtCurso.Text))
        {
            erros.SetError(txtCurso, "Informe o curso.");
            lblStatus.Text = "O curso é obrigatório.";
            txtCurso.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtTelefone.Text))
        {
            erros.SetError(txtTelefone, "Informe o telefone.");
            lblStatus.Text = "O telefone deve ser informado.";
            txtTelefone.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtCidade.Text))
        {
            erros.SetError(txtCidade, "Informe a cidade.");
            lblStatus.Text = "Informe a cidade que reside.";
            txtCidade.Focus();
            return false;
        }

        return true;
    }

    private void SelecionarAluno(object? sender, DataGridViewCellEventArgs args)
    {
        if (args.RowIndex < 0)
            return;

        var linha = grade.Rows[args.RowIndex].DataBoundItem as DataRowView;

        if (linha is null)
            return;

        alunoEmEdicao = new Aluno
        {
            Id = Convert.ToInt32(linha["id"]),
            Nome = linha["nome"].ToString() ?? "",
            Email = linha["email"].ToString() ?? "",
            Curso = linha["curso"].ToString() ?? "",
            Telefone = linha["telefone"].ToString() ?? "",
            Cidade = linha["cidade"].ToString() ?? "",
            Data_Ingresso = Convert.ToDateTime(linha["data_ingresso"])
        };

        txtNome.Text = alunoEmEdicao.Nome;
        txtEmail.Text = alunoEmEdicao.Email;
        txtCurso.Text = alunoEmEdicao.Curso;
        txtTelefone.Text = alunoEmEdicao.Telefone;
        txtCidade.Text = alunoEmEdicao.Cidade;

        if (alunoEmEdicao.Data_Ingresso.HasValue)
        {
            dtpData_Ingresso.Value = alunoEmEdicao.Data_Ingresso.Value;
        }

        btnSalvar.Text = "Atualizar";
        btnExcluir.Enabled = true;

        erros.Clear();

        lblStatus.Text = "Editando o aluno selecionado.";
    }

    private async void ExcluirAluno(object? sender, EventArgs args)
    {
        if (alunoEmEdicao is null)
            return;

        DialogResult resposta = MessageBox.Show(this,
        $"Excluir {alunoEmEdicao.Nome}?", "Confirmar exclusão",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2);

        if (resposta != DialogResult.Yes)
            return;

        try
        {
            const string sql = "DELETE FROM estudante WHERE id = @id";

            using var conexao = new MySqlConnection(ConnectionString);
            await conexao.OpenAsync();

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.Add("@id", MySqlDbType.Int32).Value = alunoEmEdicao.Id;

            await comando.ExecuteNonQueryAsync();

            await CarregarEstudantesAsync();

            PrepararNovo();

            MessageBox.Show("Estudante excluído com sucesso!", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (MySqlException ex)
        {
            MessageBox.Show($"Não foi possível excluir: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task TestarConexaoAsync()
    {
        try
        {
            using var conexao = new MySqlConnection(ConnectionString);
            await conexao.OpenAsync();
            MessageBox.Show("Conexao estabelecida com sucesso.");
        }
        catch (MySqlException ex)
        {
            MessageBox.Show($"Falha na conexão: {ex.Message}");
        }
    }

    private async Task SalvarEstudanteAsync(string nome, string email, string curso, DateTime data_ingresso, string telefone, string cidade)
    {
        const string sql =
        "INSERT INTO estudante (nome, email, curso, data_ingresso, telefone, cidade) VALUES (@nome, @email, @curso, @data_ingresso, @telefone, @cidade)";

        using var conexao = new MySqlConnection(ConnectionString);
        await conexao.OpenAsync();

        using var comando = new MySqlCommand(sql, conexao);

        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
        comando.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
        comando.Parameters.Add("@curso", MySqlDbType.VarChar).Value = curso;

        comando.Parameters.Add("@data_ingresso", MySqlDbType.DateTime).Value = DateTime.SpecifyKind(data_ingresso, DateTimeKind.Unspecified);

        comando.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = telefone;
        comando.Parameters.Add("@cidade", MySqlDbType.VarChar).Value = cidade;

        await comando.ExecuteNonQueryAsync();
    }

    private async Task AtualizarEstudanteAsync(Aluno aluno)
    {
        const string sql = "UPDATE estudante set nome = @nome, email = @email, curso = @curso, data_ingresso = @data_ingresso, telefone = @telefone, cidade = @cidade WHERE id = @id";

        using var conexao = new MySqlConnection(ConnectionString);
        await conexao.OpenAsync();

        using var comando = new MySqlCommand(sql, conexao);

        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = aluno.Id;

        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = aluno.Nome;
        comando.Parameters.Add("@email", MySqlDbType.VarChar).Value = aluno.Email;
        comando.Parameters.Add("@curso", MySqlDbType.VarChar).Value = aluno.Curso;

        comando.Parameters.Add("@data_ingresso", MySqlDbType.DateTime).Value = aluno.Data_Ingresso.HasValue ? DateTime.SpecifyKind(aluno.Data_Ingresso.Value, DateTimeKind.Unspecified) : DBNull.Value;

        comando.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = aluno.Telefone;
        comando.Parameters.Add("@cidade", MySqlDbType.VarChar).Value = aluno.Cidade;

        await comando.ExecuteNonQueryAsync();
    }

    private async void btnSalvar_Click(object sender, EventArgs e)
    {
        if (!ValidarCampos())
        {
            return;
        }

        // Lê os campos e remove espaços no início e no fim.
        string nome = txtNome.Text.Trim();
        string email = txtEmail.Text.Trim();
        string curso = txtCurso.Text.Trim();
        DateTime data_ingresso = DateTime.SpecifyKind(dtpData_Ingresso.Value, DateTimeKind.Unspecified);
        string telefone = txtTelefone.Text.Trim();
        string cidade = txtCidade.Text.Trim();

        // Impede o envio se algum campo estiver vazio.
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(curso))
        {
            MessageBox.Show("Preencha os dados corretamente.");
            return;
        }

        bool novoCadastro = alunoEmEdicao is null;

        try
        {

            if (novoCadastro)
            {
                // Salva no banco e atualiza a grade com os dados recentes.
                await SalvarEstudanteAsync(nome, email, curso, data_ingresso, telefone, cidade);
            }
            else
            {
                alunoEmEdicao.Nome = nome;
                alunoEmEdicao.Email = email;
                alunoEmEdicao.Curso = curso;
                alunoEmEdicao.Data_Ingresso = data_ingresso;
                alunoEmEdicao.Telefone = telefone;
                alunoEmEdicao.Cidade = cidade;

                await AtualizarEstudanteAsync(alunoEmEdicao);
            }

            await CarregarEstudantesAsync();
            MessageBox.Show(novoCadastro ? "Estudante cadastrado com sucesso!" : "Estudante atualizado com sucesso!", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);

            PrepararNovo();
        }
        catch (MySqlException ex)
        {
            MessageBox.Show($"Erro MySql: {ex.Message}\n\n" + $"Código: {ex.Number}\n\n" + $"Detalhes internos: {ex.InnerException?.ToString()}", "Erro ao salvar", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task CarregarEstudantesAsync()
    {
        const string sql =
        "SELECT id, nome, email, curso, data_ingresso, telefone, cidade FROM estudante ORDER BY id DESC";

        using var conexao = new MySqlConnection(ConnectionString);
        await conexao.OpenAsync();

        using var comando = new MySqlCommand(sql, conexao);

        // Executa o SELECT e obtém as linhas retornadas.
        using var leitor = await comando.ExecuteReaderAsync();

        var tabela = new DataTable();// Carrega os resultados em uma tabela
        tabela.Load(leitor); // Vincula os dados à grade para exibição no formulário.
        grade.DataSource = tabela;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            erros.Dispose();
        base.Dispose(disposing);
    }
}