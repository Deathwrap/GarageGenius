using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Service.Models;

namespace Deathwrap.GarageGenius.Service.Workers;

public interface IWorkersService
{
    Task<Guid> AddWorker(WorkerMin workerMin);
    Task<IEnumerable<Worker>> GetWorkers(int page = 1, int pageSize = 10);
    Task<Worker> GetWorkerById(Guid workerId);
    Task<Worker> GetAndCheckWorker(string login, string password);
    Task<string> GetPositionName(int positionid);
}