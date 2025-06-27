// PDC.Autorizador.Dominio/Fabricas/PermissaoFactory.cs

using Pdc.Autorizador.Dominio.Agregados.Menu;
using Pdc.Autorizador.Dominio.Agregados.Menu.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Agregados.Permissao;
using Pdc.Autorizador.Dominio.Agregados.Usuario;
using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Interfaces.Servico;
// Para o Perfil
// Para IFeatureToggleServico

// Para Identificador



namespace Pdc.Autorizador.Dominio.Factory;

public class PermissaoFactory
{
    private readonly IFeatureToggleServico _featureToggleServico;

    public PermissaoFactory(IFeatureToggleServico featureToggleServico)
    {
        _featureToggleServico = featureToggleServico ?? throw new ArgumentNullException(nameof(featureToggleServico));
    }

    /// <summary>
    /// Orquestra a criação do modelo completo de permissões a partir da fonte externa.
    /// </summary>
    /// <returns>Uma coleção de agregados de Menu, com suas funcionalidades filhas já contidas e validadas.</returns>
    public async Task<IEnumerable<Menu>> CriarModeloDePermissaoAsync() // MUDANÇA 1: O tipo de retorno agora é apenas IEnumerable<Menu>
    {
        var configDto = await _featureToggleServico.ObterConfiguracaoPermissoesMenuAsync();

        if (configDto?.Permissoes == null || !configDto.Permissoes.Any())
        {
            return Enumerable.Empty<Menu>();
        }

        var menusConstruidos = new List<Menu>();
        
        // MUDANÇA 2: A lógica do loop foi reestruturada.
        foreach (var permissaoItem in configDto.Permissoes)
        {
            var menuDto = permissaoItem.Menus;
            if (menuDto == null) continue;

            // ---- Construção do Menu (Vem primeiro) ----
            var idMenu = new Identificador(menuDto.IdMenu);
            // Usamos o método de fábrica estático do próprio agregado
            var menu = Menu.Criar(idMenu, menuDto.Nome, menuDto.Descricao);

            if (!menuDto.Ativo)
            {
                menu.Desativar();
            }

            // ---- Construção das Funcionalidades (Agora através do Menu) ----
            foreach (var funcDto in menuDto.Funcionalidades)
            {
                var idFunc = new Identificador(funcDto.Id);
                
                // MUDANÇA 3: O Menu é quem cria a funcionalidade.
                // Isso garante que a regra "funcionalidade não existe sem menu" seja cumprida.
                var novaFuncionalidade = menu.AdicionarNovaFuncionalidade(idFunc, funcDto.Nome, funcDto.Descricao);
                
                // Hidratação da funcionalidade recém-criada
                var perfis = MapearPerfisDaConfig(funcDto.PerfisPermitidos);
                foreach(var perfil in perfis)
                {
                    novaFuncionalidade.AdicionarPerfilPermitido(perfil);
                }

                if (!funcDto.Ativo)
                {
                    novaFuncionalidade.Desativar();
                }
            }
            
            // Adiciona o menu, agora completo com suas filhas, à lista final.
            menusConstruidos.Add(menu);
        }

        // MUDANÇA 4: Retornamos apenas a lista de menus.
        return menusConstruidos;
    }
    
    /// <summary>
    /// Mapeia a lista de strings de perfil da configuração para o Value Object Perfil.
    /// (Este método auxiliar continua o mesmo, pois sua lógica ainda é necessária).
    /// </summary>
    private IEnumerable<Perfil> MapearPerfisDaConfig(IEnumerable<string> perfisDaConfig)
    {
        if (perfisDaConfig == null)
        {
            yield break; 
        }

        foreach (var perfilStr in perfisDaConfig)
        {
            var perfil = Perfil.PorNome(perfilStr);
            if (perfil != null)
            {
                yield return perfil;
            }
        }
    }
}