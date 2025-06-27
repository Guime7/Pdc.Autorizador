using Pdc.Autorizador.Dominio.Agregados.Usuario;
using Pdc.Autorizador.Dominio.Shared;

namespace Pdc.Autorizador.Dominio.Interfaces.Factory;

public interface IUsuarioFactory
{
    Task<Result<Usuario>> CriarAsync(string funcional, IEnumerable<string> perfisDoToken);
}