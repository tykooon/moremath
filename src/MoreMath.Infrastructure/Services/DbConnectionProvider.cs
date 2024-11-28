using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MoreMath.Application.Contracts.Services;
using MySqlConnector;
using System.Data;

namespace MoreMath.Infrastructure.Services;

public class DbConnectionProvider : IDbConnectionProvider
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString; 

    public DbConnectionProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("MainDb:Development:MariaDb") ?? "";
    }

    public IDbConnection GetDbConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}
