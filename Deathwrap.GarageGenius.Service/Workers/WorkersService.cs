using Deathwrap.GarageGenius.Data.Models;
using Deathwrap.GarageGenius.Helper;
using Deathwrap.GarageGenius.Repository.Positions;
using Deathwrap.GarageGenius.Repository.Workers;
using Deathwrap.GarageGenius.Service.Models;

namespace Deathwrap.GarageGenius.Service.Workers;

public class WorkersService: IWorkersService
{
    
    private readonly IWorkersRepository _workersRepository;
    private readonly IPositionsRepository _positionsRepository;

    public WorkersService(IWorkersRepository workersRepository,
        IPositionsRepository positionsRepository)
    {
        _workersRepository = workersRepository;
        _positionsRepository = positionsRepository;
    }

    public async Task<Guid> AddWorker(WorkerMin workerMin)
    {
        var workerId = Guid.NewGuid();

        var worker = new Worker()
        {
            Id = workerId,
            Login = workerMin.Login,
            Name = workerMin.Name,
            PassHash = UtilsExtensions.HashPassword(workerMin.Password, workerMin.Login),
            PositionId = workerMin.PositionId
        };

        await _workersRepository.Create(worker);

        return workerId;
    }

    public async Task<IEnumerable<Worker>?> GetWorkers(int page = 1, int pageSize = 10)
    {
        var offset = (page - 1) * pageSize;
        return await _workersRepository.FindAll(offset, pageSize);
    }

    public async Task<Worker> GetWorkerById(Guid workerId)
    {
        return await _workersRepository.FindById(workerId);
    }

    public async Task<Worker> GetAndCheckWorker(string login, string password)
    {
        var worker = await _workersRepository.FindByLogin(login);
        if (worker == null)
        {
            return null;
        }
        var hashedEnteredPassword = UtilsExtensions.HashPassword(password, login);
        if (!worker.PassHash.Equals(hashedEnteredPassword))
        {
            return null;
        }

        return worker;
    }

    public async Task<string> GetPositionName(int positionId)
    {
        var position = await _positionsRepository.FindById(positionId);

        return position.Name;
    }
}