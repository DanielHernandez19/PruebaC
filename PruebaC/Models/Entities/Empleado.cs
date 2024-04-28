namespace PruebaC.Models.Entities
{
    public class Empleado
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public DateTime FechaContratacion { get; set; }
        public string? Foto { get; set; }
        public string? TipoArchivo { get; set; }
        public string? Archivo { get; set; }


    }
}
