using System.Data;

namespace MoreMath.Application.Contracts.Services;

public interface IDbConnectionProvider
{
    public IDbConnection GetDbConnection();
}
