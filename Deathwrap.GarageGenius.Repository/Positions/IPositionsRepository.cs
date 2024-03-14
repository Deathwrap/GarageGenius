using Deathwrap.GarageGenius.Data.Models;

namespace Deathwrap.GarageGenius.Repository.Positions;

public interface IPositionsRepository
{
    Task<int> GetNextId();
    Task Create(Position position);
    Task<IEnumerable<Position>> FindAll();
    Task<Position?> FindById(int positionid);
}