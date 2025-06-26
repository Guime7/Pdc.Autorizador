using Pdc.Autorizador.Dominio.Agregados.Menu.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Interfaces.Servico;

namespace Pdc.Autorizador.Dominio.Servicos;

public record Resultado(bool Sucesso, string? Erro = null)
{
    public static Resultado Ok() => new(true);
    public static Resultado Falha(string erro) => new(false, erro);
}


public class ServicoDeGerenciamentoDeMenu
{
    private readonly IMenuRepositorio _menuRepository;

    public ServicoDeGerenciamentoDeMenu(IMenuRepositorio menuRepository)
    {
        _menuRepository = menuRepository;
    }

    /// <summary>
    /// Orquestra a remoção de um menu, aplicando as regras de negócio do domínio.
    /// </summary>
    public async Task<Resultado> RemoverMenuAsync(Identificador menuId)
    {
        // 1. O serviço primeiro busca o estado atual do agregado.
        var menu = await _menuRepository.ObterPorIdAsync(menuId);

        if (menu is null)
        {
            return Resultado.Falha("Menu não encontrado.");
        }

        // 2. A REGRA DE DOMÍNIO É APLICADA AQUI!
        // O serviço verifica a condição ANTES de prosseguir.
        if (menu.FuncionalidadeIds.Any())
        {
            return Resultado.Falha("Não é possível remover um menu que possui funcionalidades associadas.");
        }

        // 3. Se a regra de negócio passar, o serviço comanda o repositório para executar a remoção.
        await _menuRepository.RemoverAsync(menu.Id);

        return Resultado.Ok();
    }
}