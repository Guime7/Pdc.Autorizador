namespace Pdc.Autorizador.Dominio.Dtos.Usuarios.Queries;

public record UsuarioDto(string Funcional, string NomeCompleto, string Email, string Cargo, List<string> Perfis);
