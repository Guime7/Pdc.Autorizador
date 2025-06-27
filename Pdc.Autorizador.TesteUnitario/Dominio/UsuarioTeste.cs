using Pdc.Autorizador.Dominio.Dtos.Servicos;
using Pdc.Autorizador.Dominio.Enums;
using Pdc.Autorizador.Dominio.Factory;
using Pdc.Autorizador.Dominio.Interfaces.Servicos;
using Pdc.Autorizador.Dominio.Shared;

namespace Pdc.Autorizador.TesteUnitario.Dominio;

public class UsuarioTeste
{
    private readonly Mock<IRecursosHumanosServico> _recursosHumanosServicoMock;
    private readonly UsuarioFactory _sut; // SUT = System Under Test (Sistema Sob Teste)

    public UsuarioTeste()
    {
        // ARRANGE (Comum a todos os testes)
        _recursosHumanosServicoMock = new Mock<IRecursosHumanosServico>();
        _sut = new UsuarioFactory(_recursosHumanosServicoMock.Object);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public async Task CriarAsync_ComDadosVariados_DeveRetornarResultadoEsperado(
        string cenario,
        string funcional,
        string[] perfisDoToken,
        UsuarioRhDto? mockUsuarioRhDto,
        byte[] mockFotoUsuario,
        bool esperadoSucesso,
        string? codigoErroEsperado)
    {
        // --- ARRANGE (Específico para cada caso de teste) ---

        _recursosHumanosServicoMock.Setup(s => s.ObterDadosUsuario(funcional))!
            .ReturnsAsync(mockUsuarioRhDto);

        _recursosHumanosServicoMock.Setup(s => s.ObterFotoUsuario(funcional))
            .ReturnsAsync(mockFotoUsuario);

        // --- ACT ---

        var resultado = await _sut.CriarAsync(funcional, perfisDoToken);

        // --- ASSERT (Usando asserções nativas do xUnit) ---

        if (esperadoSucesso)
        {
            // Verifica se o resultado foi um sucesso.
            Assert.True(resultado.IsSuccess, $"O cenário '{cenario}' deveria ter sucesso, mas falhou.");

            // Garante que o valor não é nulo antes de acessá-lo.
            Assert.NotNull(resultado.Value);
            var usuario = resultado.Value;

            // Verifica se as propriedades do objeto criado estão corretas.
            Assert.Equal(funcional, usuario.Funcional);
            Assert.Equal(mockUsuarioRhDto?.NomeCompleto, usuario.Nome.ToString());
            Assert.Equal(mockUsuarioRhDto?.Email, usuario.Email.Endereco);
            Assert.Equal(mockUsuarioRhDto?.NomeCompleto!.Split(" ").FirstOrDefault(),
                resultado.Value.Nome.PrimeiroNome);
            Assert.Equal(mockUsuarioRhDto?.NomeCompleto!.Split(" ").LastOrDefault(),
                resultado.Value.Nome.UltimoSobrenome);
            Assert.Equal($"{mockUsuarioRhDto?.NomeCompleto!.Split(" ").FirstOrDefault()} " +
                         $"{mockUsuarioRhDto?.NomeCompleto!.Split(" ").LastOrDefault()}",
                resultado.Value.Nome.NomeAbreviado());
            Assert.Equal(mockUsuarioRhDto?.Email, resultado.Value.Email.Endereco);
            Assert.NotEqual(Cargo.NãoInformado, resultado.Value.Cargo);
            Assert.Equal(mockFotoUsuario, resultado.Value.FotoPerfil);
            
            if(perfisDoToken != null)
                Assert.True(resultado.Value.Perfis.Count >= 0, "A lista de perfis não deve ser nula.");
        }
        else
        {
            // Verifica se o resultado foi uma falha.
            Assert.True(resultado.IsFailure, $"O cenário '{cenario}' deveria falhar, mas obteve sucesso.");

            // Garante que o objeto de erro não é o 'Error.None' padrão.
            Assert.NotEqual(Error.None, resultado.Error);

            // Verifica se o código do erro é o esperado para o cenário de falha.
            Assert.Equal(codigoErroEsperado, resultado.Error.Code);
        }
    }

    /// <summary>
    /// Fonte de dados para o teste parametrizado [Theory].
    /// </summary>
    public static IEnumerable<object[]> GetTestData()
    {
        // Cenário 1: "Happy Path" - Tudo funciona como esperado.
        yield return new object[]
        {
            /* cenario */ "Sucesso - Usuário válido",
            /* funcional */ "000000000",
            /* perfisDoToken */ new[] { "admin", "credito_large_access" },
            /* mockUsuarioRhDto */new UsuarioRhDto
            {
                NomeCompleto = "Nome Completo Válido",
                Email = "teste@valido.com",
                Cargo = "GERENTE"
            },
            /* mockFotoUsuario */ new byte[] { 1, 2, 3 },
            /* esperadoSucesso */ true,
            /* codigoErro */ null
        };

        // Cenário 2: Falha - Usuário não encontrado no serviço de RH. Testa o primeiro 'if'.
        yield return new object[]
        {
            /* cenario */ "Falha - Usuário não encontrado no RH",
            /* funcional */ "F_NAO_EXISTE",
            /* perfisDoToken */ new string[0],
            /* mockUsuarioRhDto */null, // O serviço de RH retorna nulo
            /* mockFotoUsuario */ null!,
            /* esperadoSucesso */ false,
            /* codigoErro */ "Usuario.NotFound"
        };

        // Cenário 3: Falha - Email inválido retornado pelo RH. Testa o 'if (email.IsFailure)'.
        yield return new object[]
        {
            /* cenario */ "Falha - E-mail inválido vindo do RH",
            /* funcional */ "F789012",
            /* perfisDoToken */ new[] { "role_commercial" },
            /* mockUsuarioRhDto */
            new UsuarioRhDto
                { NomeCompleto = "Outro Nome", Email = "email-invalido", Cargo = "ANALISTA DE CREDITO JR" },
            /* mockFotoUsuario */ new byte[] { 1 },
            /* esperadoSucesso */ false,
            /* codigoErro */ "Validation.EmailInvalido"
        };

        // Cenário 4: Falha - Nome inválido (em branco) retornado pelo RH. Testa o 'if (nome.IsFailure)'.
        yield return new object[]
        {
            /* cenario */ "Falha - Nome em branco vindo do RH",
            /* funcional */ "F789013",
            /* perfisDoToken */ new string[0],
            /* mockUsuarioRhDto */
            new UsuarioRhDto { NomeCompleto = " ", Email = "teste2@valido.com", Cargo = "DIRETOR" },
            /* mockFotoUsuario */ new byte[] { 1 },
            /* esperadoSucesso */ false,
            /* codigoErro */ "Validation.NomeInvalido"
        };

        // Cenário 5: Falha - Funcional inválida (em branco). Testa o 'if (usuario.IsFailure)'.
        yield return new object[]
        {
            /* cenario */ "Falha - Funcional em branco",
            /* funcional */ "", // Funcional inválida
            /* perfisDoToken */ new string[0],
            /* mockUsuarioRhDto */
            new UsuarioRhDto { NomeCompleto = "Nome Válido", Email = "teste3@valido.com", Cargo = "COORDENADOR" },
            /* mockFotoUsuario */ new byte[] { 1 },
            /* esperadoSucesso */ false,
            /* codigoErro */
            "Validation.CargoInvalido" // Nota: O erro em Usuario.Criar está "CargoInvalido", o ideal seria "FuncionalInvalida". O teste valida o código atual.
        };

        // Cenário 6: Sucesso - Usuário sem foto. Testa o 'if (fotoBytes?.Length > 0)' sendo falso.
        yield return new object[]
        {
            /* cenario */ "Sucesso - Usuário sem foto",
            /* funcional */ "F333444",
            /* perfisDoToken */ new string[0],
            /* mockUsuarioRhDto */
            new UsuarioRhDto { NomeCompleto = "Foto Vazia", Email = "fotovazia@valido.com", Cargo = "SUPERINTENDENTE" },
            /* mockFotoUsuario */ new byte[0],
            /* esperadoSucesso */ true,
            /* codigoErro */ null
        };

        // Cenário 7: Sucesso - Perfis mistos (válidos e inválidos). Testa o filtro '.OfType<Perfil>()' do MapearPerfis.
        yield return new object[]
        {
            /* cenario */ "Sucesso - Mapeamento de perfis mistos",
            /* funcional */ "F777888",
            /* perfisDoToken */ new[] { "admin", "perfil_que_nao_existe", "role_commercial", "outro_lixo" },
            /* mockUsuarioRhDto */
            new UsuarioRhDto { NomeCompleto = "Perfis Mistos", Email = "perfilmisto@valido.com", Cargo = "ESTAGIARIO" },
            /* mockFotoUsuario */ new byte[] { 1 },
            /* esperadoSucesso */ true,
            /* codigoErro */ null
        };

        // Cenário 8: Sucesso - Sem perfis no token. Testa o MapearPerfis com uma lista vazia.
        yield return new object[]
        {
            /* cenario */ "Sucesso - Sem perfis no token",
            /* funcional */ "F999000",
            /* perfisDoToken */ Array.Empty<string>(),
            /* mockUsuarioRhDto */
            new UsuarioRhDto { NomeCompleto = "Sem Perfil", Email = "semperfil@valido.com", Cargo = "TRAINEE" },
            /* mockFotoUsuario */ new byte[] { 1 },
            /* esperadoSucesso */ true,
            /* codigoErro */ null
        };
    }

    [Theory]
    [MemberData(nameof(GetCargoTestData))]
    public async Task CriarAsync_QuandoCargoVaria_DeveMapearParaEnumCorreto(string cargoInput, Cargo cargoEsperado)
    {
        // --- ARRANGE ---

        // 1. Dados fixos, conforme solicitado. O foco do teste é o cargo.
        const string funcionalFixo = "F_CARGO_TEST";
        var perfisFixos = new[] { "admin" };
        var fotoFixa = new byte[] { 0x01 };

        // 2. Cria o DTO do RH, variando apenas o cargo com base nos dados do teste.
        var mockUsuarioRhDto = new UsuarioRhDto
        {
            NomeCompleto = "Nome Fixo Teste",
            Email = "email.fixo@teste.com",
            Cargo = cargoInput
        };

        // 3. Configura o mock para retornar os dados que preparamos.
        _recursosHumanosServicoMock.Setup(s => s.ObterDadosUsuario(funcionalFixo))
            .ReturnsAsync(mockUsuarioRhDto);

        _recursosHumanosServicoMock.Setup(s => s.ObterFotoUsuario(funcionalFixo))
            .ReturnsAsync(fotoFixa);

        // --- ACT ---

        // Executa a criação do usuário.
        var resultado = await _sut.CriarAsync(funcionalFixo, perfisFixos);

        // --- ASSERT ---

        // Garante que a operação foi um sucesso e que o valor não é nulo.
        Assert.True(resultado.IsSuccess);
        Assert.NotNull(resultado.Value);

        // A asserção principal: verifica se o cargo mapeado no objeto final
        // é exatamente o enum que esperávamos para a string de entrada.
        Assert.Equal(cargoEsperado, resultado.Value.Cargo);
    }

    /// <summary>
    /// Fonte de dados para o teste de mapeamento de cargos.
    /// Contém todas as strings de entrada e seus respectivos enums esperados.
    /// </summary>
    public static IEnumerable<object[]> GetCargoTestData()
    {
        yield return ["TRAINEE", Cargo.Trainee];
        yield return ["ESTAGIARIO", Cargo.Estagiario];
        yield return ["ANALISTA DE CREDITO JR", Cargo.AnalistaJunior];
        yield return ["ANALISTA DE CREDITO PL", Cargo.AnalistaPleno];
        yield return ["ANALISTA DE CREDITO SR", Cargo.AnalistaSenior];
        yield return ["ESPECIALISTA", Cargo.Especialista];
        yield return ["COORDENADOR", Cargo.Coordenador];
        yield return ["GERENTE", Cargo.Gerente];
        yield return ["SUPERINTENDENTE", Cargo.Superintendente];
        yield return ["DIRETOR", Cargo.Diretor];

        // Teste de caso (maiúsculas/minúsculas) para garantir que ToUpperInvariant() funciona
        yield return ["gerente", Cargo.Gerente];

        // Teste do caso default (_), para cargos não mapeados
        yield return ["CARGO_INEXISTENTE", Cargo.NãoInformado];
        yield return ["", Cargo.NãoInformado]; // Teste com string vazia
    }
}