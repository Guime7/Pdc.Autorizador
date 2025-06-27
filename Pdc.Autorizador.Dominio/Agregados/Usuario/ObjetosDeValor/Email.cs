using System.Text.RegularExpressions;
 
 namespace Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
 public partial record Email
 {
     public string Endereco { get; }
     private Email(string endereco)
     {
         Endereco = endereco;
     }
     
     public static Result<Email> Criar(string endereco)
     {
         if (string.IsNullOrWhiteSpace(endereco) || !IsValid(endereco))
             return Result<Email>.Failure(Error.Validation("Validation.EmailInvalido", "O e-mail fornecido não é válido."));
         
         return Result<Email>.Success(new Email(endereco));
     }

     private static bool IsValid(string email)
     {
         return EmailRegex().IsMatch(email);
     }
     
     [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, "pt-BR")]
     private static partial Regex EmailRegex();
 }
