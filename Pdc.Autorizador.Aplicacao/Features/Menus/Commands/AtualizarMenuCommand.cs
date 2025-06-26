// Em: Aplicacao/Features/Menus/Commands/AtualizarMenuCommand.cs
public record AtualizarMenuCommand(string MenuId, string NovoNome, string NovaDescricao) : IRequest<Resultado>;

// --- Handler ---
public class AtualizarMenuCommandHandler : IRequestHandler<AtualizarMenuCommand, Resultado>
{
    private readonly IMenuRepository _menuRepository;
    public AtualizarMenuCommandHandler(IMenuRepository menuRepository) { _menuRepository = menuRepository; }
    
    public async Task<Resultado> Handle(AtualizarMenuCommand command, CancellationToken cancellationToken)
    {
        // 1. Carregar o Menu com _menuRepository.ObterPorIdAsync.
        // 2. Chamar menu.AlterarNome(...) e menu.AlterarDescricao(...).
        // 3. Chamar _menuRepository.SalvarAsync(menu).
        await Task.CompletedTask;
        return Resultado.Ok();
    }
}