using Pdc.Autorizador.Dominio.Agregados.Menu.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Agregados.Permissao;

namespace Pdc.Autorizador.Dominio.Agregados.Menu;

public class Menu
{
    public Identificador Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public bool Ativo { get; private set; }
    
    private readonly List<Funcionalidade> _funcionalidades = [];
    public IReadOnlyCollection<Funcionalidade> Funcionalidades => _funcionalidades.AsReadOnly();

    private Menu(Identificador id, string nome, string descricao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        Ativo = false; //Desligado por padrão
    }
    
    public static Menu Criar(Identificador id, string nome, string descricao)
    {
        ArgumentNullException.ThrowIfNull(id);
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do menu é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição do menu é obrigatória.", nameof(descricao));
        
        return new Menu(id, nome, descricao);
    }

    // --- Métodos de Comportamento para o CRUD ---

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

    // Comportamento para gerenciar a associação
    public Funcionalidade AdicionarNovaFuncionalidade(Identificador funcionalidadeId, string nome, string descricao)
    {
        // Regra de negócio: Não permitir IDs duplicados DENTRO do mesmo menu.
        if (_funcionalidades.Any(f => f.Id.Equals(funcionalidadeId)))
        {
            throw new InvalidOperationException($"A funcionalidade com ID '{funcionalidadeId}' já existe neste menu.");
        }
        
        // O Menu cria sua própria entidade filha.
        var novaFuncionalidade = new Funcionalidade(funcionalidadeId, nome, descricao);
        
        _funcionalidades.Add(novaFuncionalidade);

        // Retornamos a nova funcionalidade para que a camada de aplicação
        // possa, por exemplo, adicionar perfis a ela na mesma operação.
        return novaFuncionalidade;
    }

    /// <summary>
    /// Remove uma funcionalidade da associação com este menu.
    /// </summary>
    public void RemoverFuncionalidade(Identificador funcionalidadeId)
    {
        var funcionalidade = _funcionalidades.FirstOrDefault(f => f.Id.Equals(funcionalidadeId));
        if (funcionalidade != null)
        {
            _funcionalidades.Remove(funcionalidade);
        }
    }

    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;
}