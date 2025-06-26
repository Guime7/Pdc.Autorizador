// Em: Aplicacao/Features/Menus/Queries/ListarMenusQuery.cs
public record ListarMenusQuery() : IRequest<IEnumerable<MenuResumidoDto>>;

// --- Handler ---
public class ListarMenusQueryHandler : IRequestHandler<ListarMenusQuery, IEnumerable<MenuResumidoDto>>
{
    // ... Injeta IMenuRepository, busca todos e mapeia para MenuResumidoDto.
}

// Em: Aplicacao/Features/Menus/Queries/ObterMenuDetalhadoQuery.cs
public record ObterMenuDetalhadoQuery(string MenuId) : IRequest<MenuDetalhadoDto?>;

// --- Handler ---
public class ObterMenuDetalhadoQueryHandler : IRequestHandler<ObterMenuDetalhadoQuery, MenuDetalhadoDto?>
{
    // ... Injeta IMenuRepository, busca por ID e mapeia para MenuDetalhadoDto.
}