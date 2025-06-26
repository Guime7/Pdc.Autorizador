// PDC.Autorizador.Dominio/Factory/UsuarioFactory.cs
using Pdc.Autorizador.Dominio.Agregados.Usuario;
using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Dtos;
using Pdc.Autorizador.Dominio.Enums;
using Pdc.Autorizador.Dominio.Interfaces.Servico;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pdc.Autorizador.Dominio.Factory;

public class UsuarioFactory
{
    private readonly IRecursosHumanosServico _recursosHumanosServico;

    public UsuarioFactory(IRecursosHumanosServico recursosHumanosServico)
    {
        _recursosHumanosServico = recursosHumanosServico;
    }

    // Assinatura atualizada: recebe a lista de perfis do token.
    public async Task<Usuario?> CriarAsync(string funcional, IEnumerable<string> perfisDoToken)
    {
        // 1. Orquestração das chamadas ao serviço externo
        var dadosRhTask = _recursosHumanosServico.ObterDadosUsuario(funcional);
        var fotoTask = _recursosHumanosServico.ObterFotoUsuario(funcional);

        // Aguarda as duas chamadas em paralelo para otimizar o tempo
        await Task.WhenAll(dadosRhTask, fotoTask);

        var dadosRH = dadosRhTask.Result;
        if (dadosRH == null) return null; // Usuário não existe no RH.

        // 2. Tradução e Mapeamento (Regras de Negócio do seu Domínio)
        var cargoInterno = MapearCargo(dadosRH.Cargo); // Regra de negócio para Cargo
        var perfisInternos = MapearPerfis(perfisDoToken); // Regra de negócio para Perfil

        // 3. Criação dos Value Objects (que se auto-validam)
        var email = new Email(dadosRH.Email);
        var nome = Nome.Criar(dadosRH.NomeCompleto);
        
        // 4. Criação da instância base do Usuário
        var usuario = new Usuario(
            funcional,
            email,
            nome,
            cargoInterno
        );

        // 5. "Hidratação" do objeto com os dados restantes
        usuario.AnexarPerfis(perfisInternos);
        
        var fotoBytes = fotoTask.Result;
        if(fotoBytes?.Length > 0)
        {
            usuario.AtualizarFoto(fotoBytes);
        }

        return usuario;
    }

    /// <summary>
    /// Mapeia a string do cargo vinda do RH para o enum interno.
    /// Isso é uma camada de "tradução", protegendo seu domínio.
    /// </summary>
    private Cargo MapearCargo(string cargoDoRh)
    {
        // A lógica aqui depende de como o RH envia os dados.
        // Exemplo:
        return cargoDoRh.ToUpperInvariant() switch
        {
            "ANALISTA DE CREDITO JR" => Cargo.AnalistaJunior,
            "ANALISTA DE CREDITO PL" => Cargo.AnalistaPleno,
            "ANALISTA DE CREDITO SR" => Cargo.AnalistaSenior,
            "COORDENADOR COMERCIAL" => Cargo.Coordenador,
            "GERENTE" => Cargo.Gerente,
            "DIRETOR" => Cargo.Diretor,
            _ => Cargo.AnalistaJunior // Um valor padrão ou lançar uma exceção
        };
    }

    /// <summary>
    /// Mapeia a lista de strings de perfil do token para o enum interno.
    /// </summary>
    private IEnumerable<Perfil> MapearPerfis(IEnumerable<string> perfisDoToken)
    {
        return perfisDoToken.Select(perfilToken => perfilToken.ToLowerInvariant() switch
            {
                "credito_large_access" => Perfil.CreditoLarge,
                "credito_middle_access" => Perfil.CreditoMiddle,
                "role_commercial" => Perfil.Comercial,
                "admin" => Perfil.Administrador,
                _ => null // Para tipos de referência, retornamos 'null' diretamente
            })
            .OfType<Perfil>()
            .ToList();
    }
}