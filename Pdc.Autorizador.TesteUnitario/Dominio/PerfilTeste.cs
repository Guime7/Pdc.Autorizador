using Pdc.Autorizador.Dominio.Agregados.Usuario;
using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;

namespace Pdc.Autorizador.TesteUnitario.Dominio;

public class PerfilTests
{
    #region Testes do Método PorNome

    [Fact(DisplayName = "PorNome deve retornar o perfil correto para um nome válido")]
    public void PorNome_ComNomeValido_DeveRetornarPerfilCorreto()
    {
        // Arrange
        var nomeDoPerfil = "Administrador";

        // Act
        var resultado = Perfil.PorNome(nomeDoPerfil);

        // Assert
        Assert.NotNull(resultado);
        Assert.Same(Perfil.Administrador, resultado); // 'Same' verifica se é a mesma instância
    }

    [Fact(DisplayName = "PorNome deve ser case-insensitive e retornar o perfil correto")]
    public void PorNome_ComNomeEmCaixaDiferente_DeveRetornarPerfilCorreto()
    {
        // Arrange
        var nomeDoPerfil = "crédito large"; // Nome em minúsculas

        // Act
        var resultado = Perfil.PorNome(nomeDoPerfil);

        // Assert
        Assert.NotNull(resultado);
        Assert.Same(Perfil.CreditoLarge, resultado);
    }

    [Fact(DisplayName = "PorNome deve retornar nulo para um nome inválido")]
    public void PorNome_ComNomeInvalido_DeveRetornarNulo()
    {
        // Arrange
        var nomeDoPerfil = "Perfil Que Não Existe";

        // Act
        var resultado = Perfil.PorNome(nomeDoPerfil);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region Testes de Comparação (CompareTo e Operadores)

    [Fact(DisplayName = "Operador > deve retornar verdadeiro para perfil com maior hierarquia")]
    public void OperadorMaiorQue_ComNiveisDiferentes_DeveRetornarVerdadeiro()
    {
        // Arrange
        var perfilSuperior = Perfil.Administrador; // Nível 1000
        var perfilInferior = Perfil.Comercial;     // Nível 200

        // Act & Assert
        Assert.True(perfilSuperior > perfilInferior);
    }

    [Fact(DisplayName = "Operador < deve retornar verdadeiro para perfil com menor hierarquia")]
    public void OperadorMenorQue_ComNiveisDiferentes_DeveRetornarVerdadeiro()
    {
        // Arrange
        var perfilInferior = Perfil.Geral;        // Nível 10
        var perfilSuperior = Perfil.CreditoAgro;  // Nível 500

        // Act & Assert
        Assert.True(perfilInferior < perfilSuperior);
    }

    [Fact(DisplayName = "CompareTo deve retornar 0 para perfis de mesmo nível hierárquico")]
    public void CompareTo_ComNiveisIguais_DeveRetornarZero()
    {
        // Arrange
        var perfil1 = Perfil.CreditoLarge; // Nível 500
        var perfil2 = Perfil.CreditoMiddle; // Nível 500

        // Act
        var resultado = perfil1.CompareTo(perfil2);

        // Assert
        Assert.Equal(0, resultado);
    }

    [Fact(DisplayName = "CompareTo deve retornar 1 ao comparar com um perfil nulo")]
    public void CompareTo_ComPerfilNulo_DeveRetornarUm()
    {
        // Arrange
        var perfil = Perfil.Administrador;

        // Act
        var resultado = perfil.CompareTo(null);

        // Assert
        Assert.Equal(1, resultado);
    }

    #endregion

    #region Testes de Igualdade (Equals e GetHashCode)

    [Fact(DisplayName = "Equals deve retornar verdadeiro para perfis com o mesmo nome")]
    public void Equals_ComMesmoPerfil_DeveRetornarVerdadeiro()
    {
        // Arrange
        var perfil1 = Perfil.Comercial;
        var perfil2 = Perfil.PorNome("Comercial"); // Pega a mesma instância

        // Act & Assert
        Assert.True(perfil1.Equals(perfil2));
    }

    [Fact(DisplayName = "Equals deve retornar falso para perfis com nomes diferentes")]
    public void Equals_ComPerfisDiferentes_DeveRetornarFalso()
    {
        // Arrange
        var perfil1 = Perfil.Administrador;
        var perfil2 = Perfil.Comercial;

        // Act & Assert
        Assert.False(perfil1.Equals(perfil2));
    }

    [Fact(DisplayName = "Equals deve retornar falso ao comparar com nulo")]
    public void Equals_ComObjetoNulo_DeveRetornarFalso()
    {
        // Arrange
        var perfil = Perfil.Geral;

        // Act & Assert
        Assert.False(perfil.Equals(null));
    }

    [Fact(DisplayName = "GetHashCode deve retornar o mesmo código para perfis iguais")]
    public void GetHashCode_ParaMesmoPerfil_DeveRetornarMesmoCodigo()
    {
        // Arrange
        var hash1 = Perfil.CreditoReestruturacao.GetHashCode();
        var hash2 = Perfil.PorNome("Crédito Reestruturação").GetHashCode();

        // Act & Assert
        Assert.Equal(hash1, hash2);
    }

    #endregion

    #region Testes do Método ToString

    [Fact(DisplayName = "ToString deve retornar o nome do perfil")]
    public void ToString_DeveRetornarNomeDoPerfil()
    {
        // Arrange
        var perfil = Perfil.CreditoUnidadeExterna;
        var nomeEsperado = "Crédito Unidade Externa";

        // Act
        var resultado = perfil.ToString();

        // Assert
        Assert.Equal(nomeEsperado, resultado);
    }

    #endregion
}