using MediatR;
using Pdc.Autorizador.Dominio.Interfaces.Factory;
using Pdc.Autorizador.Dominio.Shared;

namespace Pdc.Autorizador.Aplicacao.Features.Usuarios.Queries;

public record ConsultarUsuarioRequest(
    string Funcional, 
    string[] Perfis, 
    bool? RetornarFoto = true);

public record ConsultarUsuarioResponse(
    string Funcional,
    string NomeCompleto,
    string NomeAbreviado,
    string Email,
    string Cargo,
    List<string> Perfis,
    byte[]? Foto = null);

public record ConsultarUsuarioQuery(ConsultarUsuarioRequest Request) : IRequest<Result<ConsultarUsuarioResponse>>;

public class ObterUsuarioQueryHandler(IUsuarioFactory usuarioFactory)
    : IRequestHandler<ConsultarUsuarioQuery, Result<ConsultarUsuarioResponse>>
{
    public async Task<Result<ConsultarUsuarioResponse>> Handle(ConsultarUsuarioQuery query,
        CancellationToken cancellationToken)
    {
        var resultadoUsuario = await usuarioFactory.CriarAsync(query.Request.Funcional, query.Request.Perfis);

        if (resultadoUsuario.IsFailure)
            return Result<ConsultarUsuarioResponse>.Failure(resultadoUsuario.Error);

        var usuarioAgregado = resultadoUsuario.Value;

        var foto = query.Request.RetornarFoto.HasValue && query.Request.RetornarFoto.Value
            ? usuarioAgregado!.FotoPerfil
            : null;
        var usuarioDto = new ConsultarUsuarioResponse(
            Funcional: usuarioAgregado!.Funcional,
            NomeCompleto: usuarioAgregado.Nome.ToString(),
            NomeAbreviado: usuarioAgregado.Nome.NomeAbreviado(),
            Email: usuarioAgregado.Email.Endereco,
            Cargo: usuarioAgregado.Cargo.ToString(),
            Foto: foto,
            Perfis: usuarioAgregado.Perfis
                .Select(p => p.Nome)
                .ToList()
        );

        return Result<ConsultarUsuarioResponse>.Success(usuarioDto);
    }
}