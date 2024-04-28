using Microsoft.AspNetCore.Mvc;
using PruebaC.Interfaces;
using PruebaC.Services;
using PruebaC.DTOs.Request;
using PruebaC.DTOs.Response;
using PruebaC.Models.Entities;
using PruebaC.Helpers;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PruebaC.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("pruebaC/api/rest/v{version:apiVersion}/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmpleadoController(IEmpleadoService empleadoService, IWebHostEnvironment hostingEnvironment)
        {
            _empleadoService = empleadoService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (Request.Query.Any())
            {
                return BadRequest(new
                {
                    Title = "BAD REQUEST",
                    Status = 400,
                    UserMessage = "No se permiten parametros en la URL.",
                    Message = "No se permiten parametros en la URL."
                });
            }

            var result = await _empleadoService.GetAll();
            return Ok(result);
        }

        [HttpGet("getById/")]
        public async Task<ActionResult<Empleado>> GetById([FromQuery] int id)
        {
            List<EmpleadoResponseDto> response = await _empleadoService.GetById(id);
            
            //Validando que se encuentren registros

            if (response.Count == 0)
            {
                return BadRequest(new
                {
                    Title = "BAD REQUEST",
                    Status = 400,
                    UserMessage = "No se encontraron registros.",
                    Message = "No se encontraron registros en la base de datos."
                });
            } else
            {
                var result = await _empleadoService.GetById(id);
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmpleadoDto model)
        {

            //Validando que el telefono y el correo tengan el formato correcto
            var telefonoValido = Utils.ValidarTelefono(model.Telefono!);
            if (!telefonoValido)
            {
                return UnprocessableEntity(new
                {
                    Title = "UNPROCESSABLE ENTITY",
                    Status = 422,
                    UserMessage = "El telefono no tiene el formato correcto.",
                    Message = "El telefono no tiene formato correcto(XXX)XXXX-XXXX."
                });
            }

            var correoValido = Utils.ValidarCorreo(model.Correo!);
            if (!correoValido)
            {
                return UnprocessableEntity(new
                {
                    Title = "UNPROCESSABLE ENTITY",
                    Status = 422,
                    UserMessage = "El correo no tiene el formato correcto.",
                    Message = "El correo no tiene formato correcto (XXXXXXX@XXXX.XXX)."
                });
            }

            //Convirtiendo la imagen a byte[]
            byte[] byteArray = Convert.FromBase64String(model.Archivo!);

            //Generando un nombre aleatorio para la imagen
            var nombreArchivo = Guid.NewGuid().ToString();

            //Obteniendo el directorio de la aplicacion
            var relativePAth = _hostingEnvironment.ContentRootPath;
            var fotosDirectoryPath = Path.Combine(relativePAth, "fotos");

            //Creando el directorio si no existe
            if (!Directory.Exists(fotosDirectoryPath))
            {
                Directory.CreateDirectory(fotosDirectoryPath);
            }

            //Creando el path del archivo
            var filePath = Path.Combine(fotosDirectoryPath, nombreArchivo + model.TipoArchivo);

            //Creanod la imagen en el directorio
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            //Incluyendo la ruta de la imagen
            model.Foto = "fotos/" + nombreArchivo + model.TipoArchivo;

            //Verificando que se guarden los datos
            var result = await _empleadoService.Add(model);
            if(result == 0)
            {
                return StatusCode(500,new
                {
                    Title = "BAD REQUEST",
                    Status = 422,
                    UserMessage = "Hubo un problema a la hora de guardar los datos.",
                    Message = "Hubo un problema a la hora de guardar en la base de datos."
                });
            }else
            {
                var msg = new
                {
                    IdEmpleado = result,
                    UserMessage = "Datos del Empleado ingresados exitosamente.",
                    Message = "Datos del Empleado ingresados en la base de datos."
                };
                return Ok(msg);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<EmpleadoDto>> Update(EmpleadoDto model)
        {

            //Validando que el telefono y el correo tengan el formato correcto
            if (model.Telefono != null)
            {
                var telefonoValido = Utils.ValidarTelefono(model.Telefono);
                if (!telefonoValido)
                {
                    return UnprocessableEntity(new
                    {
                        Title = "UNPROCESSABLE ENTITY",
                        Status = 422,
                        UserMessage = "El telefono no tiene el formato correcto.",
                        Message = "El telefono no tiene formato correcto(XXX)XXXX-XXXX."
                    });
                }
            }

            if (model.Correo != null)
            {
                var correoValido = Utils.ValidarCorreo(model.Correo);
                if (!correoValido)
                {
                    return UnprocessableEntity(new
                    {
                        Title = "UNPROCESSABLE ENTITY",
                        Status = 422,
                        UserMessage = "El correo no tiene el formato correcto.",
                        Message = "El correo no tiene formato correcto (XXXXXXX@XXXX.XXX)."
                    });
                }
            }

            //Verificando que se actualicen los datos
            var result = await _empleadoService.Update(model);
            if(result == 0)
            {
                return NotFound(new
                {
                    Title = "NOT FOUND",
                    Status = 404,
                    UserMessage = "No se encontraron registros.",
                    Message = "No se encontraron registros en la base de datos."
                });
            }else
            {
                var msg = new
                {
                    IdEmpleado = result,
                    UserMessage = "Datos del Empleado actualizados exitosamente.",
                    Message = "Datos del Empleado actualizados en la base de datos."
                };
                return Ok(msg);
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<EmpleadoDto>> Delete(EmpleadoDto reqid)
        {

            //Verificando que se seleccione el Id correcto para eliminar los datos
            var result = await _empleadoService.Delete(reqid.Id);
            if (result == 0)
            {
                return NotFound(new
                {
                    Title = "NOT FOUND",
                    Status = 404,
                    UserMessage = "No se encontraron registros.",
                    Message = "No se encontraron registros en la base de datos."
                });
            } else
            {
                var msg = new
                {
                    UserMessage = "Empleado eliminado exitosamente.",
                    Message = "Empleado eliminado en la base de datos."
                };
                return Ok(msg);
            }
        }
    }
}
