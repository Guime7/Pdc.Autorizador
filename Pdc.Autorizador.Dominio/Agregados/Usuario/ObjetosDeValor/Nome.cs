namespace Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
public record Nome
{
    public string PrimeiroNome { get; }
    private IReadOnlyList<string> Sobrenomes { get; }

    // Propriedade de conveniência para obter o último sobrenome.
    public string UltimoSobrenome => Sobrenomes.LastOrDefault() ?? string.Empty;

    // O construtor é privado para forçar a criação através do método estático `Criar`.
    // Isso nos dá mais controle e deixa a intenção mais clara.
    private Nome(string primeiroNome, List<string> sobrenomes)
    {
        PrimeiroNome = primeiroNome;
        Sobrenomes = sobrenomes.AsReadOnly();
    }

    /// <summary>
    /// Método de fábrica para criar uma instância de NomeCompleto a partir de uma string.
    /// </summary>
    /// <param name="nomeCompleto">A string do nome completo, ex: "José Carlos da Silva".</param>
    /// <returns>Uma instância válida de NomeCompleto.</returns>
    /// <exception cref="ArgumentException">Lançada se a string for nula, vazia ou inválida.</exception>
    public static Nome Criar(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
        {
            throw new ArgumentException("Nome completo não pode ser nulo ou vazio.", nameof(nomeCompleto));
        }

        // Remove espaços extras e divide o nome em partes.
        var partesDoNome = nomeCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (partesDoNome.Length < 2)
        {
            throw new ArgumentException("Nome completo deve conter pelo menos um nome e um sobrenome.", nameof(nomeCompleto));
        }

        var primeiroNome = partesDoNome[0];
        var sobrenomes = partesDoNome.Skip(1).ToList();

        return new Nome(primeiroNome, sobrenomes);
    }

    /// <summary>
    /// Retorna o nome abreviado, contendo o primeiro nome e o último sobrenome.
    /// Ex: "José Silva"
    /// </summary>
    public string NomeAbreviado()
    {
        return $"{PrimeiroNome} {UltimoSobrenome}";
    }

    /// <summary>
    /// Retorna a representação completa e formatada do nome.
    /// </summary>
    public override string ToString()
    {
        return $"{PrimeiroNome} {string.Join(" ", Sobrenomes)}";
    }

    // Opcional: permite converter o objeto de volta para string de forma implícita.
    public static implicit operator string(Nome nome) => nome.ToString();
}