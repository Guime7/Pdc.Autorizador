using Pdc.Autorizador.Dominio.Dtos.Servicos;

namespace Pdc.Autorizador.Dominio.Interfaces.Servicos;

public interface IRecursosHumanosServico
{
    Task<UsuarioRhDto> ObterDadosUsuario(string funcional);
    Task<byte[]?> ObterFotoUsuario(string funcional);
}