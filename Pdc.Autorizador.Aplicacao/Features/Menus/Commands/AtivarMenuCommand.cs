// Em: Aplicacao/Features/Menus/Commands/AtivarMenuCommand.cs
public record AtivarMenuCommand(string MenuId) : IRequest<Resultado>;

// --- Handler ---
public class AtivarMenuCommandHandler : IRequestHandler<AtivarMenuCommand, Resultado>
{
    // ... Lógica similar: buscar, chamar menu.Ativar(), salvar.
}

// Em: Aplicacao/Features/Menus/Commands/DesativarMenuCommand.cs
public record DesativarMenuCommand(string MenuId) : IRequest<Resultado>;
// ... Handler similar ...