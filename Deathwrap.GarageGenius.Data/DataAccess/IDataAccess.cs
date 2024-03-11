using System.Data;
using Dapper;
using Npgsql;

namespace Deathwrap.GarageGenius.Data.DataAccess;

public interface IDataAccess
{
    Task<IEnumerable<T>> GetData<T, P>(string query, P parameters, string connectionId = "DefaultConnection");

    Task<IEnumerable<T>> GetData<T, P>(string query, P parameters, NpgsqlTransaction transaction, string connectionId = "DefaultConnection");
    Task SaveData<P>(string query, P parameters, string connectionId = "DefaultConnection");
    Task SaveData<P>(string query, P parameters, NpgsqlTransaction transaction, string connectionId = "DefaultConnection");
    Task<(IEnumerable<T1>,IEnumerable<T2>)> GetMultipleData<T1, T2 ,P>(string query, P parameters, string connectionId = "DefaultConnection");
    Task<NpgsqlTransaction> GetTransaction(string connectionId = "DefaultConnection");



}