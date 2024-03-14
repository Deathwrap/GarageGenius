using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Workers;

public interface IWorkersRepository
{
    Task<IEnumerable<Worker>?> FindAll(int offset = 0, int limitSize = 10);
    Task<Worker> FindById(Guid workerId);
    Task Create(Worker worker);
    Task<Worker?> FindByLogin(string login);
}
