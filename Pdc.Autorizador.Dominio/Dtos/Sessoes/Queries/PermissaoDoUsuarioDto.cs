// PDC.Autorizador.Dominio/Dtos/PermissaoDoUsuarioDto.cs
using Pdc.Autorizador.Dominio.Enums; // Para o Cargo
using System.Collections.Generic;

namespace Pdc.Autorizador.Dominio.Dtos;

/// <summary>
/// Representa a visão completa e rica das permissões e dados de um usuário.
/// Este é o objeto que a aplicação provavelmente retornará em um endpoint de "login" ou "sessão".
/// </summary>
public record PermissaoDoUsuarioDto
{
    // Propriedades do usuário
    public string Funcional { get; init; }
    public string Email { get; init; }
    public string NomeCompleto { get; init; }
    public string PrimeiroNome { get; init; }
    public string NomeAbreviado { get; init; }
    public byte[]? FotoPerfil { get; init; }
    public string Cargo { get; init; }
    
    // Propriedades de permissão
    public List<string> Perfis { get; init; }
    public string PerfilPrincipal { get; init; }
    public List<MenuPermitidoDto> Menus { get; init; }

    // Construtor para facilitar a criação (opcional, mas útil)
    public PermissaoDoUsuarioDto(
        string funcional, string email, string nomeCompleto, string primeiroNome, 
        string nomeAbreviado, byte[]? fotoPerfil, string cargo, List<string> perfis, 
        string perfilPrincipal, List<MenuPermitidoDto> menus)
    {
        Funcional = funcional;
        Email = email;
        NomeCompleto = nomeCompleto;
        PrimeiroNome = primeiroNome;
        NomeAbreviado = nomeAbreviado;
        FotoPerfil = fotoPerfil;
        Cargo = cargo;
        Perfis = perfis;
        PerfilPrincipal = perfilPrincipal;
        Menus = menus;
    }
}

/// <summary>
/// Representa um menu que o usuário tem permissão para ver (sem alterações).
/// </summary>
public record MenuPermitidoDto(
    string Id,
    string Nome,
    string Descricao,
    List<FuncionalidadePermitidaDto> Funcionalidades);

/// <summary>
/// Representa uma funcionalidade que o usuário pode acessar (sem alterações).
/// </summary>
public record FuncionalidadePermitidaDto(
    string Id,
    string Nome,
    string Descricao);