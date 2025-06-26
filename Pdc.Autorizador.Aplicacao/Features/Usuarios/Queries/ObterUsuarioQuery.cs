// Em: Aplicacao/Features/Usuarios/Queries/ObterUsuarioQuery.cs
using MediatR;

namespace Pdc.Autorizador.Aplicacao.Features.Usuarios.Queries;

public record ObterUsuarioQuery(string Funcional) : IRequest<UsuarioDto?>;

// --- Handler ---
public class ObterUsuarioQueryHandler : IRequestHandler<ObterUsuarioQuery, UsuarioDto?>
{
    // ... injeção de IUsuarioRepository ou UsuarioFactory ...
    public async Task<UsuarioDto?> Handle(ObterUsuarioQuery query, CancellationToken cancellationToken)
    {
        // 1. Buscar o agregado Usuario.
        // 2. Mapear o agregado para o UsuarioDto.
        // TODO: Implementar a lógica de busca e mapeamento.
        await Task.CompletedTask; // Placeholder
        return null; // Placeholder
    }
}