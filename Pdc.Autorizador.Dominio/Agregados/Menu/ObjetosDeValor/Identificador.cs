using System.Text.RegularExpressions;

namespace Pdc.Autorizador.Dominio.Agregados.Menu.ObjetosDeValor;
public partial record Identificador
{
    public string Valor { get; }
    
    // Expressão Regular para validar o formato kebab-case (palavra-palavra)
    private static readonly Regex FormatoKebabCase = MyRegex();

    public Identificador(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("O valor do identificador não pode ser vazio.", nameof(valor));
        
        if (!FormatoKebabCase.IsMatch(valor))
            throw new FormatException($"O ID '{valor}' não está no formato 'kebab-case' (ex: 'cadastro-cliente').");

        Valor = valor;
    }
    public static implicit operator string(Identificador id) => id.Valor;

    [GeneratedRegex(@"^[a-z0-9]+(-[a-z0-9]+)*$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}