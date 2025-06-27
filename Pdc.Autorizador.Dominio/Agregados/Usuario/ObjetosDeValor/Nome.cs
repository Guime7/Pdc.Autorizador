namespace Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
public record Nome
{
    public string PrimeiroNome { get; }
    private IReadOnlyList<string> Sobrenomes { get; }

    // Propriedade de conveniência para obter o último sobrenome.
    public string UltimoSobrenome => Sobrenomes.LastOrDefault() ?? string.Empty;
    private Nome(string primeiroNome, List<string> sobrenomes)
    {
        PrimeiroNome = primeiroNome;
        Sobrenomes = sobrenomes.AsReadOnly();
    }
    public static Result<Nome> Criar(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            return Result<Nome>.Failure(Error.Validation("Validation.NomeInvalido", "O nome completo não pode ser vazio."));

        
        var partesDoNome = nomeCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        var primeiroNome = partesDoNome[0];
        var sobrenomes = partesDoNome.Skip(1).ToList();

        return Result<Nome>.Success(new Nome(primeiroNome, sobrenomes));
    }

    /// <summary>
    /// Retorna o nome abreviado, contendo o primeiro nome e o último sobrenome.
    /// Ex: "José Silva"
    /// </summary>
    public string NomeAbreviado() => $"{PrimeiroNome} {UltimoSobrenome}";

    
    /// <summary>
    /// Retorna a representação completa e formatada do nome.
    /// </summary>
    public override string ToString() => $"{PrimeiroNome} {string.Join(" ", Sobrenomes)}";
    
}