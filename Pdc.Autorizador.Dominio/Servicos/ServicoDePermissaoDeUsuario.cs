// // PDC.Autorizador.Dominio/Servicos/ServicoDePermissaoDeUsuario.cs
// using Pdc.Autorizador.Dominio.Agregados.Permissao;
// using Pdc.Autorizador.Dominio.Agregados.Usuario;
// using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor; // Perfil
// using Pdc.Autorizador.Dominio.Dtos;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Pdc.Autorizador.Dominio.Agregados.Menu;
//
// namespace Pdc.Autorizador.Dominio.Servicos;
//
// public class ServicoDePermissaoDeUsuario
// {
//     public ServicoDePermissaoDeUsuario() { }
//
//     /// <summary>
//     /// Calcula a árvore de menus e funcionalidades permitidas e junta com os dados
//     /// completos do usuário para criar um DTO de sessão.
//     /// </summary>
//     public PermissaoDoUsuarioDto ObterPermissoesParaUsuario(
//         Usuario usuario, 
//         IEnumerable<Menu> todosOsMenus) // MUDANÇA 1: Não precisamos mais da lista de funcionalidades aqui.
//     {
//         ArgumentNullException.ThrowIfNull(usuario);
//         ArgumentNullException.ThrowIfNull(todosOsMenus);
//
//         var menusPermitidos = new List<MenuPermitidoDto>();
//
//         foreach (var menu in todosOsMenus.Where(m => m.Ativo))
//         {
//             // MUDANÇA 2: A lógica de filtragem agora é muito mais direta e elegante.
//             // Iteramos diretamente sobre as funcionalidades filhas do menu.
//             var funcionalidadesPermitidasNesteMenu = menu.Funcionalidades
//                 .Where(f => f.EhPermitidoPara(usuario.Perfis)) // A verificação de permissão continua a mesma
//                 .Select(f => new FuncionalidadePermitidaDto(f.Id, f.Nome, f.Descricao))
//                 .ToList();
//
//             if (funcionalidadesPermitidasNesteMenu.Any())
//             {
//                 menusPermitidos.Add(new MenuPermitidoDto(menu.Id, menu.Nome, menu.Descricao, funcionalidadesPermitidasNesteMenu));
//             }
//         }
//         
//         // A construção do DTO rico no final do método não muda, pois já estava correta.
//         var nomesDosPerfis = usuario.Perfis.Select(p => p.Nome).ToList();
//         var perfilPrincipal = usuario.Perfis.Any() ? usuario.Perfis.Max()!.Nome : "Nenhum";
//
//         return new PermissaoDoUsuarioDto(
//             funcional: usuario.Funcional,
//             email: usuario.Email,
//             nomeCompleto: usuario.Nome,
//             primeiroNome: usuario.Nome.PrimeiroNome,
//             nomeAbreviado: usuario.Nome.NomeAbreviado(),
//             fotoPerfil: usuario.FotoPerfil,
//             cargo: usuario.Cargo.ToString(),
//             perfis: nomesDosPerfis,
//             perfilPrincipal: perfilPrincipal,
//             menus: menusPermitidos
//         );
//     }
// }