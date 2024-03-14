using Deathwrap.GarageGenius.Data.DataAccess;
using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Positions;

public class PositionsRepository: IPositionsRepository
{
    private readonly IDataAccess _db;
    public PositionsRepository(IDataAccess db)
    {
        _db = db;
    }
    
    public async Task<int> GetNextId()
    {
        var query = @"select nextval('positions_id_seq')";

        return (await _db.GetData<int, dynamic>(query, new { })).FirstOrDefault();    
    }

    public Task Create(Position position)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Position>> FindAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Position?> FindById(int positionid)
    {
        var query = @"select * from positions where id = @Id";

        var positions = await _db.GetData<Position, dynamic>(query, new { Id = positionid });

        return positions.FirstOrDefault();
    }
}