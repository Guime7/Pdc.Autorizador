using System.Text.Json.Serialization;

namespace Pdc.Autorizador.Dominio.Dtos.Servicos;
public class ConfiguracaoPermissaoDto
{
    [JsonPropertyName("permissoes")]
    public List<PermissaoItemDto> Permissoes { get; set; } = [];
}

public class PermissaoItemDto
{
    [JsonPropertyName("menus")]
    public MenuConfigDto Menus { get; set; } = null!;
}

// DTO que espelha o objeto "menus" no JSON
public class MenuConfigDto
{
    [JsonPropertyName("id-menu")]
    public string IdMenu { get; set; } = null!;

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = null!;

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = null!;

    [JsonPropertyName("ativo")]
    public bool Ativo { get; set; }

    [JsonPropertyName("funcionalidades")]
    public List<FuncionalidadeConfigDto> Funcionalidades { get; set; } = [];
}

// DTO que espelha cada funcionalidade no JSON
public class FuncionalidadeConfigDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = null!;

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = null!;

    [JsonPropertyName("ativo")]
    public bool Ativo { get; set; }

    [JsonPropertyName("perfisPermitidos")]
    public List<string> PerfisPermitidos { get; set; } = [];
}