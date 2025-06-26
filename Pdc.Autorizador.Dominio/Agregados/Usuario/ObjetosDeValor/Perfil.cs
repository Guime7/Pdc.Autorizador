// PDC.Autorizador.Dominio/Agregados/Usuario/ObjetosDeValor/Perfil.cs
// (Sim, Perfil se torna um VO, você pode mover o arquivo para a pasta de VOs)

using System;
using System.Collections.Generic;
using System.Linq;

namespace Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;

// Implementa IComparable para que possamos comparar e ordenar perfis.
public class Perfil : IComparable<Perfil>
{
    public string Nome { get; }
    public int NivelHierarquico { get; }

    // Construtor privado
    private Perfil(string nome, int nivelHierarquico)
    {
        Nome = nome;
        NivelHierarquico = nivelHierarquico;
    }

    // Membros estáticos públicos, assim como um enum.
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
    private static readonly List<Perfil> _todosOsPerfis =
    [
        Administrador, CreditoLarge, CreditoReestruturacao, CreditoMiddle,
        CreditoAgro, CreditoUnidadeExterna, Comercial, Geral
    ];

    // Método para buscar um perfil pelo nome (usado no mapeamento)
    public static Perfil? PorNome(string nome)
    {
        return _todosOsPerfis.FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
    }

    // O coração da hierarquia!
    public int CompareTo(Perfil? other)
    {
        if (other is null) return 1;
        return NivelHierarquico.CompareTo(other.NivelHierarquico);
    }
    
    // É uma boa prática sobrescrever Equals, GetHashCode e operadores para VOs
    public override bool Equals(object? obj) => obj is Perfil perfil && Nome == perfil.Nome;
    public override int GetHashCode() => Nome.GetHashCode();
    public static bool operator >(Perfil left, Perfil right) => left.CompareTo(right) > 0;
    public static bool operator <(Perfil left, Perfil right) => left.CompareTo(right) < 0;
    public override string ToString() => Nome;
}