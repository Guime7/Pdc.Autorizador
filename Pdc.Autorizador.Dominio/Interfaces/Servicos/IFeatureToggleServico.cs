using Pdc.Autorizador.Dominio.Dtos;
using Pdc.Autorizador.Dominio.Dtos.Servicos;

namespace Pdc.Autorizador.Dominio.Interfaces.Servico;

public interface IFeatureToggleServico
{
    /// <summary>
    /// Busca a configuração bruta de permissões da fonte externa (ex: AWS AppConfig).
    /// </summary>
    /// <returns>Um DTO que representa a estrutura exata do JSON.</returns>
    Task<ConfiguracaoPermissaoDto> ObterConfiguracaoPermissoesMenuAsync();
}