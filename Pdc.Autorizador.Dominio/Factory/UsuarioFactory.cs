using Pdc.Autorizador.Dominio.Agregados.Usuario;
using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Interfaces.Factory;
using Pdc.Autorizador.Dominio.Interfaces.Servicos;

namespace Pdc.Autorizador.Dominio.Factory;

public class UsuarioFactory(IRecursosHumanosServico recursosHumanosServico) : IUsuarioFactory
{
    public async Task<Result<Usuario>> CriarAsync(string funcional, IEnumerable<string> perfisDoToken)
    {
        // 1. Orquestração das chamadas ao serviço externo
        var dadosRhTask = recursosHumanosServico.ObterDadosUsuario(funcional);
        var fotoTask = recursosHumanosServico.ObterFotoUsuario(funcional);

        // Aguarda as duas chamadas em paralelo para otimizar o tempo
        await Task.WhenAll(dadosRhTask, fotoTask);

        var dadosRh = dadosRhTask.Result;
        if (dadosRh == null) 
            return Result<Usuario>.Failure(Error.NotFound("Usuario.NotFound", $"Usuario {funcional} não encontrado.")); // Usuário não existe no RH.

        // 2. Tradução e Mapeamento (Regras de Negócio do seu Domínio)
        var cargoInterno = MapearCargo(dadosRh.Cargo!); // Regra de negócio para Cargo
        var perfisInternos = MapearPerfis(perfisDoToken); // Regra de negócio para Perfil

        // 3. Criação dos Value Objects
        var email = Email.Criar(dadosRh.Email!);
        if (email.IsFailure)
            return Result<Usuario>.Failure(email.Error); // Retorna erro se o e-mail for inválido
        
        var nome = Nome.Criar(dadosRh.NomeCompleto!);
        if (nome.IsFailure)
            return Result<Usuario>.Failure(nome.Error); // Retorna erro se o nome for inválido

        // 4. Criação da instância base do Usuário
        var usuario = Usuario.Criar(funcional, email.Value!, nome.Value!, cargoInterno);
        if (usuario.IsFailure)
            return Result<Usuario>.Failure(usuario.Error); // Retorna erro se a criação do usuário falhar
        
        // 5. "Hidratação" do objeto com os dados restantes
        usuario.Value!.AnexarPerfis(perfisInternos);

        var fotoBytes = fotoTask.Result;
        if (fotoBytes?.Length > 0)
        {
            usuario.Value.AtualizarFoto(fotoBytes);
        }

        return usuario;
    }

    /// <summary>
    /// Mapeia a string do cargo vinda do RH para o enum interno.
    /// Isso é uma camada de "tradução", protegendo seu domínio.
    /// </summary>
    private static Cargo MapearCargo(string cargoDoRh)
    {
        return cargoDoRh.ToUpperInvariant() switch
        {
            "TRAINEE" => Cargo.Trainee,
            "ESTAGIARIO" => Cargo.Estagiario,
            "ANALISTA DE CREDITO JR" => Cargo.AnalistaJunior,
            "ANALISTA DE CREDITO PL" => Cargo.AnalistaPleno,
            "ANALISTA DE CREDITO SR" => Cargo.AnalistaSenior,
            "COORDENADOR" => Cargo.Coordenador,
            "ESPECIALISTA" => Cargo.Especialista,
            "GERENTE" => Cargo.Gerente,
            "SUPERINTENDENTE" => Cargo.Superintendente,
            "DIRETOR" => Cargo.Diretor,
            _ => Cargo.NãoInformado // Um valor padrão ou lançar uma exceção
        };
    }

    /// <summary>
    /// Mapeia a lista de strings de perfil do token para o enum interno.
    /// </summary>
    private static IEnumerable<Perfil> MapearPerfis(IEnumerable<string> perfisDoToken)
    {
        return perfisDoToken.Select(perfilToken => perfilToken.ToLowerInvariant() switch
            {
                "credito_large_access" => Perfil.CreditoLarge,
                "credito_middle_access" => Perfil.CreditoMiddle,
                "role_commercial" => Perfil.Comercial,
                "admin" => Perfil.Administrador,
                _ => null
            })
            .OfType<Perfil>()
            .ToList();
    }
}