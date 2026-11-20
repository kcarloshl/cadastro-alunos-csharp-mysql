namespace ConexaoBD;

public class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Curso { get; set; } = "";
    public DateTime? Data_Ingresso { get; set; }
    public string? Telefone { get; set; }
    public string? Cidade { get; set; }
}