using Dapper;
using System.Data.Common;
using System.Data;

namespace PruebaC.Interfaces
{
    public interface IDapper
    {
        DbConnection GetConnection();
        T Insert<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters properties, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
        T Get<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
    }
}
