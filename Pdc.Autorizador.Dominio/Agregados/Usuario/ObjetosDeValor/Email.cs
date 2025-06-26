using System.Text.RegularExpressions;

namespace Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
public record Email
{
    public string Endereco { get; }

    public Email(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco) || !IsValid(endereco))
        {
            throw new ArgumentException("Formato de e-mail inválido.", nameof(endereco));
        }
        Endereco = endereco;
    }

    // Validação simples de e-mail
    private static bool IsValid(string email)
    {
        // Uma validação mais robusta pode ser necessária.
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }

    // Permite conversão implícita de Email para string
    public static implicit operator string(Email email) => email.Endereco;
}