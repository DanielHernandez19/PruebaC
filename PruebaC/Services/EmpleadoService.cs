using PruebaC.Interfaces;
using Dapper;
using System.Data;
using PruebaC.DTOs.Request;
using PruebaC.DTOs.Response;

namespace PruebaC.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IDapper _dapper;
        public EmpleadoService(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<int> Add(EmpleadoDto model)
        {
            var dbParams = new DynamicParameters();
            dbParams.Add("Option", 1, DbType.Int32);
            dbParams.Add("Nombre", model.Nombre, DbType.String);
            dbParams.Add("Apellido", model.Apellido, DbType.String);
            dbParams.Add("Telefono", model.Telefono, DbType.String);
            dbParams.Add("Correo", model.Correo, DbType.String);
            dbParams.Add("FechaContratacion", model.FechaContratacion, DbType.DateTime);
            dbParams.Add("Foto", model.Foto, DbType.String);
            dbParams.Add("TipoArchivo", model.TipoArchivo, DbType.String);
            dbParams.Add("Archivo", model.Archivo, DbType.String);
            var result = await Task.FromResult(_dapper.Insert<int>("[dbo].[sp_trn_Empleado]", dbParams, commandType: CommandType.StoredProcedure));
            return result;
        }

        public Task<int> Update(EmpleadoDto model)
        {
            var dbParams = new DynamicParameters();
            dbParams.Add("Option", 2, DbType.Int32);
            dbParams.Add("Id", model.Id, DbType.Int32);
            dbParams.Add("Nombre", model.Nombre, DbType.String);
            dbParams.Add("Apellido", model.Apellido, DbType.String);
            dbParams.Add("Telefono", model.Telefono, DbType.String);
            dbParams.Add("Correo", model.Correo, DbType.String);
            var result = Task.FromResult(_dapper.Update<int>("[dbo].[sp_trn_Empleado]", dbParams, commandType: CommandType.StoredProcedure));
            return result;
        }

        public Task<int> Delete(int id)
        {
            var dbParams = new DynamicParameters();
            dbParams.Add("Option", 3, DbType.Int32);
            dbParams.Add("Id", id, DbType.Int32);
            var result = Task.FromResult(_dapper.Execute("[dbo].[sp_trn_Empleado]", dbParams, commandType: CommandType.StoredProcedure));
            return result;
        }

        public Task<List<EmpleadoResponseDto>> GetById(int id)
        {
            var dbParams = new DynamicParameters();
            dbParams.Add("Option", 2, DbType.Int32);
            dbParams.Add("Id", id, DbType.Int32);
            var result = Task.FromResult(_dapper.GetAll<EmpleadoResponseDto>("[dbo].[sp_sel_empleados]", dbParams, commandType: CommandType.StoredProcedure));
            return result;
        }

        public Task<List<EmpleadoResponseDto>> GetAll()
        {
            var dbParams = new DynamicParameters();
            dbParams.Add("Option", 1, DbType.Int32);
            var result = Task.FromResult(_dapper.GetAll<EmpleadoResponseDto>("[dbo].[sp_sel_empleados]", dbParams, commandType: CommandType.StoredProcedure));
            return result;
        }
    }
}
