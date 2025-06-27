using Pdc.Autorizador.Dominio.Agregados.Usuario.ObjetosDeValor;

namespace Pdc.Autorizador.Dominio.Agregados.Usuario;

public class Usuario
{
    public string Funcional { get; private set; }
    public Email Email { get; private set; }
    public Nome Nome { get; private set; } 
    public byte[] FotoPerfil { get; private set; } = [];
    public Cargo Cargo { get; private set; }

    private readonly List<Perfil> _perfis = [];
    public IReadOnlyCollection<Perfil> Perfis => _perfis.AsReadOnly();

    private Usuario(string funcional, Email email, Nome nome, Cargo cargo)
    {
        Funcional = funcional;
        Email = email;
        Nome = nome;
        Cargo = cargo;
    }
    public static Result<Usuario> Criar(string funcional, Email email, Nome nome, Cargo cargo)
    {
       if(string.IsNullOrWhiteSpace(funcional))
           return Result<Usuario>.Failure(Error.Validation("Validation.CargoInvalido", "O cargo não pode ser nulo."));
       
       return Result<Usuario>.Success(new Usuario(funcional, email, nome, cargo));
    }

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
        FotoPerfil = novaFoto;
    }
}