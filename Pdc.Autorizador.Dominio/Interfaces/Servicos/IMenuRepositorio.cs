namespace Pdc.Autorizador.Dominio.Interfaces.Servico;

public interface IMenuRepositorio
{
    Task<Menu?> ObterPorIdAsync(Identificador id);
    Task<IEnumerable<Menu>> ListarTodosAsync();
    Task AdicionarAsync(Menu menu);
    Task SalvarAsync(Menu menu); // Para updates
    Task RemoverAsync(Identificador id); // A nova operação
}