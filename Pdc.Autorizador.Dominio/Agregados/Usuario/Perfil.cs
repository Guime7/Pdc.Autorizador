namespace Pdc.Autorizador.Dominio.Agregados.Usuario;
public class Perfil : IComparable<Perfil>
{
    public string Nome { get; }
    public int NivelHierarquico { get; }
    
    private Perfil(string nome, int nivelHierarquico)
    {
        Nome = nome;
        NivelHierarquico = nivelHierarquico;
    }
    
    // Quanto maior o nível, mais "valor" o perfil tem.
    public static readonly Perfil Administrador = new("Administrador", 1000);
    public static readonly Perfil CreditoLarge = new("Crédito Large", 500);
    public static readonly Perfil CreditoReestruturacao = new("Crédito Reestruturação", 500);
    public static readonly Perfil CreditoMiddle = new("Crédito Middle", 500);
    public static readonly Perfil CreditoAgro = new("Crédito Agro", 500);
    public static readonly Perfil CreditoUnidadeExterna = new("Crédito Unidade Externa", 500);
    public static readonly Perfil Comercial = new("Comercial", 200);
    public static readonly Perfil Geral = new("Geral", 10);

    // Lista de todos os perfis para facilitar buscas
    private static readonly List<Perfil> TodosOsPerfis =
    [
        Administrador, CreditoLarge, CreditoReestruturacao, CreditoMiddle,
        CreditoAgro, CreditoUnidadeExterna, Comercial, Geral
    ];

    // Método para buscar um perfil pelo nome (usado no mapeamento)
    public static Perfil? PorNome(string nome) =>
        TodosOsPerfis.FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

    public int CompareTo(Perfil? perfil)
    {
        return perfil is null ? 1 : NivelHierarquico.CompareTo(perfil.NivelHierarquico);
    }
    
    public override bool Equals(object? obj) => obj is Perfil perfil && Nome == perfil.Nome;
    public override int GetHashCode() => Nome.GetHashCode();
    public static bool operator >(Perfil left, Perfil right) => left.CompareTo(right) > 0;
    public static bool operator <(Perfil left, Perfil right) => left.CompareTo(right) < 0;
    public override string ToString() => Nome;
}