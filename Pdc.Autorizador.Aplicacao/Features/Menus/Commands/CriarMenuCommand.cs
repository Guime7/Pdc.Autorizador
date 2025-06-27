// // Em: Aplicacao/Features/Menus/Commands/CriarMenuCommand.cs
// using MediatR;
// using Pdc.Autorizador.Dominio.Servicos; // Para o 'Resultado'
//
// namespace Pdc.Autorizador.Aplicacao.Features.Menus.Commands;
//
// public record CriarMenuCommand(string Id, string Nome, string Descricao) : IRequest<Resultado>;
//
// // --- Handler ---
// public class CriarMenuCommandHandler : IRequestHandler<CriarMenuCommand, Resultado>
// {
//     private readonly IMenuRepository _menuRepository;
//     public CriarMenuCommandHandler(IMenuRepository menuRepository) { _menuRepository = menuRepository; }
//     
//     public async Task<Resultado> Handle(CriarMenuCommand command, CancellationToken cancellationToken)
//     {
//         // 1. Validar se o ID já existe com _menuRepository.ObterPorIdAsync.
//         // 2. Chamar Menu.Criar(...).
//         // 3. Chamar _menuRepository.AdicionarAsync(...).
//         // TODO: Implementar a lógica de criação.
//         await Task.CompletedTask; // Placeholder
//         return Resultado.Ok(); // Placeholder
//     }
// }