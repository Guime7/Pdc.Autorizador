// // Em: Aplicacao/Features/Menus/Commands/AtualizarFuncionalidadeCommand.cs
// public record AtualizarFuncionalidadeCommand(string MenuId, string FuncionalidadeId, string NovoNome, string NovaDescricao) : IRequest<Resultado>;
//
// // --- Handler ---
// public class AtualizarFuncionalidadeCommandHandler : IRequestHandler<AtualizarFuncionalidadeCommand, Resultado>
// {
//     private readonly IMenuRepository _menuRepository;
//     public AtualizarFuncionalidadeCommandHandler(IMenuRepository menuRepository) { _menuRepository = menuRepository; }
//
//     public async Task<Resultado> Handle(AtualizarFuncionalidadeCommand command, CancellationToken cancellationToken)
//     {
//         // 1. Carregar o Menu pai.
//         // 2. Encontrar a Funcionalidade filha na lista menu.Funcionalidades.
//         // 3. Chamar funcionalidade.AlterarNome(...) e funcionalidade.AlterarDescricao(...).
//         // 4. Salvar o Menu pai.
//         await Task.CompletedTask;
//         return Resultado.Ok();
//     }
// }