namespace ConexaoBD;

public partial class CadastroForm
{
    private readonly TextBox txtNome = new()
    {
        Dock = DockStyle.Fill,
        MaxLength = 120,
        TabIndex = 0
    };

    private readonly TextBox txtEmail = new()
    {
        Dock = DockStyle.Fill,
        MaxLength = 180,
        TabIndex = 1
    };
    private readonly TextBox txtCurso = new()
    {
        Dock = DockStyle.Fill,
        MaxLength = 120,
        TabIndex = 2
    };
    private readonly DateTimePicker dtpData_Ingresso = new()
    {
        Dock = DockStyle.Fill,
        Format = DateTimePickerFormat.Custom,
        CustomFormat = "dd/MM/yyyy",
        TabIndex = 3
    };
    private readonly TextBox txtTelefone = new()
    {
        Dock = DockStyle.Fill,
        MaxLength = 11,
        TabIndex = 4
    };
    private readonly TextBox txtCidade = new()
    {
        Dock = DockStyle.Fill,
        MaxLength = 150,
        TabIndex = 5
    };

    private readonly Button btnSalvar = new()
    {
        Text = "Salvar",
        AutoSize = true
    };

    private readonly Button btnNovo = new()
    {
        Text = "Novo",
        AutoSize = true
    };

    private readonly Button btnExcluir = new()
    {
        Text = "Excluir",
        AutoSize = true
    };

    private readonly Label lblStatus = new()
    {
        Text = "Novo aluno.",
        AutoSize = true,
        Dock = DockStyle.Fill
    };

    private readonly DataGridView grade = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        TabIndex = 3,
        AllowUserToAddRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoGenerateColumns = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

    private readonly ErrorProvider erros = new();

    private void MontarTela()
    {
        Text = "Cadastro de alunos";
        ClientSize = new Size(760, 600);
        MinimumSize = new Size(650, 500);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Font;
        AcceptButton = btnSalvar;

        var lblNome = new Label { Text = "Nome:", AutoSize = true };
        var lblEmail = new Label { Text = "Email:", AutoSize = true };
        var lblCurso = new Label { Text = "Curso:", AutoSize = true };
        var lblData_Ingresso = new Label { Text = "Data de Ingresso:", AutoSize = true };
        var lblTelefone = new Label { Text = "Telefone:", AutoSize = true };
        var lblCidade = new Label { Text = "Cidade:", AutoSize = true };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 2,
            RowCount = 9
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        txtNome.Margin = new Padding(3);
        txtEmail.Margin = new Padding(3);
        txtCurso.Margin = new Padding(3);
        dtpData_Ingresso.Margin = new Padding(3);
        txtTelefone.Margin = new Padding(3);
        txtCidade.Margin = new Padding(3);

        // nome
        layout.Controls.Add(lblNome, 0, 0);
        layout.Controls.Add(txtNome, 1, 0);
        
        // email
        layout.Controls.Add(lblEmail, 0, 1);
        layout.Controls.Add(txtEmail, 1, 1);

        // curso
        layout.Controls.Add(lblCurso, 0, 2);
        layout.Controls.Add(txtCurso, 1, 2);

        // data
        layout.Controls.Add(lblData_Ingresso, 0, 3);
        layout.Controls.Add(dtpData_Ingresso, 1, 3);

        // telefone
        layout.Controls.Add(lblTelefone, 0, 4);
        layout.Controls.Add(txtTelefone, 1, 4);

        // cidade
        layout.Controls.Add(lblCidade, 0, 5);
        layout.Controls.Add(txtCidade, 1, 5);

        var botoes = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            TabIndex = 6
        };

        botoes.Controls.AddRange(new Control[]{
            btnSalvar, btnNovo, btnExcluir
        });

        layout.Controls.Add(botoes, 0, 6);
        layout.SetColumnSpan(botoes, 2);

        layout.Controls.Add(grade, 0, 7);
        layout.SetColumnSpan(grade, 2);

        layout.Controls.Add(lblStatus, 0, 8);
        layout.SetColumnSpan(lblStatus, 2);
        
        Controls.Add(layout);
    }
}
