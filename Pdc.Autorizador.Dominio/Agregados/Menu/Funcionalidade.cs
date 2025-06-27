// PDC.Autorizador.Dominio/Agregados/Menu/Funcionalidade.cs

using Pdc.Autorizador.Dominio.Agregados.Menu.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Agregados.Usuario;
using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor; // Perfil

namespace Pdc.Autorizador.Dominio.Agregados.Permissao;

public class Funcionalidade
{
    // Usando um Value Object para o ID para garantir o formato e validação.
    public Identificador Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public bool Ativo { get; private set; }

    private readonly List<Perfil> _perfisPermitidos = new();
    public IReadOnlyCollection<Perfil> PerfisPermitidos => _perfisPermitidos.AsReadOnly();

    // Construtor protegido para ser usado pela Factory
    internal Funcionalidade(Identificador id, string nome, string descricao)
    {
        // Validações ricas, como em Usuario
        ArgumentNullException.ThrowIfNull(id);
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da funcionalidade é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição da funcionalidade é obrigatória.", nameof(descricao));

        Id = id;
        Nome = nome;
        Descricao = descricao;
        Ativo = true; // Uma nova funcionalidade nasce ativa por padrão
    }
    public void AlterarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("O novo nome não pode ser vazio.", nameof(novoNome));
        
        Nome = novoNome;
    }

    public void AlterarDescricao(string novaDescricao)
    {
        if (string.IsNullOrWhiteSpace(novaDescricao))
            throw new ArgumentException("A nova descrição não pode ser vazia.", nameof(novaDescricao));

        Descricao = novaDescricao;
    }
    
    // Métodos de comportamento que encapsulam as regras de negócio do CRUD futuro
    public void AdicionarPerfilPermitido(Perfil perfil)
    {
        ArgumentNullException.ThrowIfNull(perfil);
        if (!_perfisPermitidos.Contains(perfil))
        {
            _perfisPermitidos.Add(perfil);
        }
    }

    public void RemoverPerfilPermitido(Perfil perfil)
    {
        _perfisPermitidos.Remove(perfil);
    }
    
    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;

    // A lógica principal de verificação continua aqui, perfeita como estava.
    public bool EhPermitidoPara(IEnumerable<Perfil> perfisDoUsuario)
    {
        if (!Ativo) return false;
        if (!_perfisPermitidos.Any()) return false;
        return perfisDoUsuario.Any(perfilDoUsuario => _perfisPermitidos.Contains(perfilDoUsuario));
    }
}