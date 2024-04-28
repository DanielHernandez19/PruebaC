using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PruebaC.Helpers
{
    public class Utils
    {
        public static bool ValidarTelefono(string telefono)
        {
            // Expresión regular para validar el formato (XXX) XXXX-XXXX
            string pattern = @"^\(\d{3}\)\s?\d{4}-\d{4}$";

            // Crear una nueva instancia de Regex
            Regex regex = new Regex(pattern);

            // Validar el número de teléfono
            bool isValid = regex.IsMatch(telefono);

            return isValid;
        }

        public static bool ValidarCorreo(string correo)
        {
            // Expresión regular para validar el formato XXXXXX@XXXX.XX
            string pattern = @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]+$";

            // Crear una nueva instancia de Regex
            Regex regex = new Regex(pattern);

            // Validar el correo electrónico
            bool isValid = regex.IsMatch(correo);

            return isValid;
        }

        //public static ValidationResult ValidateFechaContratacion(DateTime fechaContratacion, ValidationContext context)
        //{
        //    // Formato de fecha válido
        //    string formatoValido = "MM/dd/yyyy";

        //    // Intentar convertir la fecha a una cadena con el formato válido
        //    bool esValido = DateTime.TryParseExact(fechaContratacion.ToString(), formatoValido, null, DateTimeStyles.None, out DateTime fechaValidada);

        //    if (esValido)
        //        return ValidationResult.Success;
        //    else
        //        return new ValidationResult($"El formato de fecha de contratación no es válido. Debe ser {formatoValido}");
        //}

    }
}
