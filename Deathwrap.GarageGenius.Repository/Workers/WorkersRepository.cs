using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Workers;

public class WorkersRepository: IWorkersRepository
{
    private readonly IDataAccess _db;

    public  WorkersRepository(IDataAccess db)
    {
        _db = db;
    }

    public async Task Create(Worker worker)
    {
        var query = @"insert into workers (id, position_id, name, login, pass_hash) values (@Id, @PositionId, @Name, @Login, @PassHash)";

        await _db.SaveData(query, worker);
    }

    public async Task<Worker?> FindByLogin(string login)
    {
        var query = @"select * from workers where login = @Login";

        var worker = await _db.GetData<Worker, dynamic>(query, new { Login = login });

        return worker.FirstOrDefault();
    }

    public async Task<IEnumerable<Worker>?> FindAll(int offset = 0, int limitSize = 10)
    {
        var query = @"select * from workers order by id offset @Offset limit @LimitSize";

        var workers = await _db.GetData<Worker, dynamic>(query, new {Offset = offset, LimitSize = limitSize});

        return workers;
    }

    public async Task<Worker?> FindById(Guid workerId)
    {
        var query = @"select * from workers where id = @Id";

        var worker = (await _db.GetData<Worker, dynamic>(query, new { Id = workerId })).FirstOrDefault();

        return worker;
    }

}
