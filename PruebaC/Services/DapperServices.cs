using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using PruebaC.Interfaces;

namespace PruebaC.Services
{
    public class DapperServices : IDapper
    {
        private readonly IConfiguration _configuration;
        public DapperServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SQL"));
        }

        public T Insert<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection cnn = GetConnection();
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                using var transaction = cnn.BeginTransaction();

                try
                {
                    result = cnn.Query<T>(sp, parameters, commandType: commandType, transaction: transaction).FirstOrDefault()!;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new InvalidOperationException(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                if (cnn.State == ConnectionState.Open) cnn.Close();
            }
            return result;
        }

        // envia una lista de parametros
        public T Insert<T>(string sp, List<DynamicParameters> dynamicParametersList, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection cnn = GetConnection();
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                using var transaction = cnn.BeginTransaction();

                try
                {
                    var parameters = new DynamicParameters();
                    foreach (var dynamicParameters in dynamicParametersList)
                    {
                        parameters.AddDynamicParams(dynamicParameters);
                    }
                    result = cnn.Query<T>(sp, parameters, commandType: commandType, transaction: transaction).FirstOrDefault()!;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new InvalidOperationException(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                if (cnn.State == ConnectionState.Open) cnn.Close();
            }
            return result;
        }

        public T Update<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection cnn = GetConnection();
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                using var transaction = cnn.BeginTransaction();

                try
                {
                    result = cnn.Query<T>(sp, parameters, commandType: commandType, transaction: transaction).FirstOrDefault()!;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new InvalidOperationException(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                if (cnn.State == ConnectionState.Open) cnn.Close();
            }
            return result;
        }

        public int Execute(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection cnn = GetConnection();
            return cnn.Execute(sp, parameters, commandType: commandType);
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection cnn = GetConnection();
            return cnn.Query<T>(sp, parameters, commandType: commandType).ToList();
        }

        public T Get<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection cnn = GetConnection();
            return cnn.Query<T>(sp, parameters, commandType: commandType).FirstOrDefault()!;
        }
    }
}