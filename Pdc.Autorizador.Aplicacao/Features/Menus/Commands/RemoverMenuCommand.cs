// Em: Aplicacao/Features/Menus/Commands/RemoverMenuCommand.cs
public record RemoverMenuCommand(string MenuId) : IRequest<Resultado>;

// --- Handler ---
public class RemoverMenuCommandHandler : IRequestHandler<RemoverMenuCommand, Resultado>
{
    private readonly ServicoDeGerenciamentoDeMenu _servicoDeMenu;
    public RemoverMenuCommandHandler(ServicoDeGerenciamentoDeMenu servicoDeMenu) { _servicoDeMenu = servicoDeMenu; }

    public async Task<Resultado> Handle(RemoverMenuCommand command, CancellationToken cancellationToken)
    {
        return await _servicoDeMenu.RemoverMenuAsync(new Identificador(command.MenuId));
    }
}