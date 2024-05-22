using System.Data;
using Dapper;
using Dapper.Transaction;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Deathwrap.GarageGenius.Data.DataAccess;

public class DataAccess: IDataAccess
{
    private readonly IConfiguration _config;
    public DataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<T>> GetData<T,P>(string query, P parameters, string connectionId="DefaultConnection")
    {
        using IDbConnection connection = 
            new NpgsqlConnection(_config.GetConnectionString(connectionId));
       
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        var response = await connection.QueryAsync<T>(query, parameters);
        return response;
    }
    
    public async Task<IEnumerable<T>> GetData<T,P>(string query, P parameters, NpgsqlTransaction transaction, string connectionId="DefaultConnection")
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        return await transaction.QueryAsync<T>(query, parameters);
    }

    public async Task<(IEnumerable<T1>, IEnumerable<T2>)> GetMultipleData<T1,T2,P>(string query, P parameters, string connectionId = "DefaultConnection")
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
            using(var connection =
                new NpgsqlConnection(_config.GetConnectionString(connectionId)))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var response = await transaction.QueryMultipleAsync(query, parameters);

                    var result1 = (await response.ReadAsync<T1>()).ToList();
                    var result2 = (await response.ReadAsync<T2>()).ToList();
                    await transaction.CommitAsync();
                    return (result1, result2);
                }
            }
    }

    public async Task SaveData<P>(string query, P parameters, string connectionId = "DefaultConnection")
    {
        using IDbConnection connection = 
            new NpgsqlConnection(_config.GetConnectionString(connectionId));
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await connection.ExecuteAsync(query, parameters);
    }
    
    public async Task SaveData<P>(string query, P parameters, NpgsqlTransaction transaction, string connectionId = "DefaultConnection")
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        await transaction.ExecuteAsync(query, parameters);
    }

    public async Task<NpgsqlTransaction> GetTransaction(string connectionId = "DefaultConnection")
    {
        var connection = new NpgsqlConnection(_config.GetConnectionString(connectionId));
        await connection.OpenAsync();
        return await connection.BeginTransactionAsync();
    }
}