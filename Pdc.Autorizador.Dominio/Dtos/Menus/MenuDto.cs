namespace Pdc.Autorizador.Dominio.Dtos.Menus.Queries;

public record FuncionalidadeDto(string Id, string Nome, string Descricao, bool Ativo, List<string> PerfisPermitidos);

public record MenuResumidoDto(string Id, string Nome, string Descricao, bool Ativo, int QuantidadeFuncionalidades);

public record MenuDetalhadoDto(string Id, string Nome, string Descricao, bool Ativo, int QuantidadeFuncionalidades, List<FuncionalidadeDto> Funcionalidades);