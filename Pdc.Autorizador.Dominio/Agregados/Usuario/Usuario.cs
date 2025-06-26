// PDC.Autorizador.Dominio/Agregados/Usuario/Usuario.cs
using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;
using Pdc.Autorizador.Dominio.Enums;

namespace Pdc.Autorizador.Dominio.Agregados.Usuario;

public class Usuario
{
    public string Funcional { get; private set; }
    public Email Email { get; private set; }
    public Nome Nome { get; private set; } // << CORRIGIDO para usar o Value Object
    public byte[]? FotoPerfil { get; private set; }
    public Cargo Cargo { get; private set; }

    private readonly List<Perfil> _perfis = [];
    public IReadOnlyCollection<Perfil> Perfis => _perfis.AsReadOnly();

    // Construtor agora recebe o Value Object Nome e não tem mais os perfis.
    // O construtor é 'internal' para que apenas a Factory possa usá-lo.
    internal Usuario(string funcional, Email email, Nome nome, Cargo cargo)
    {
        // Validações mais quebradas, como solicitado.
        ValidarFuncional(funcional);
        ValidarEntradasObrigatorias(email, nome);

        Funcional = funcional;
        Email = email;
        Nome = nome;
        Cargo = cargo;
    }

    // Métodos para modificar o estado interno. Devem ser 'internal'
    // para que apenas partes controladas do domínio (como a Factory) possam chamá-los.
    internal void AnexarPerfis(IEnumerable<Perfil> perfisParaAdicionar)
    {
        _perfis.Clear(); // Garante que a lista está limpa antes de adicionar os novos perfis
        foreach (var perfil in perfisParaAdicionar)
        {
            if (!_perfis.Contains(perfil))
            {
                _perfis.Add(perfil);
            }
        }
    }
    
    internal void AtualizarFoto(byte[] novaFoto)
    {
        // Poderíamos ter validações aqui (tamanho máximo, formato, etc.)
        FotoPerfil = novaFoto;
    }

    #region Métodos de Validação Privados
    private static void ValidarFuncional(string funcional)
    {
        if (string.IsNullOrWhiteSpace(funcional))
            throw new ArgumentException("Funcional não pode ser nulo ou vazio.", nameof(funcional));
    }

    private static void ValidarEntradasObrigatorias(Email email, Nome nome)
    {
        // A validação agora é checar se os objetos de valor não são nulos.
        // A lógica interna de cada VO (formato do email, nome com sobrenome) já foi executada na sua criação.
        if (email is null)
            throw new ArgumentNullException(nameof(email), "Objeto Email não pode ser nulo.");

        if (nome is null)
            throw new ArgumentNullException(nameof(nome), "Objeto Nome não pode ser nulo.");
    }
    #endregion
}