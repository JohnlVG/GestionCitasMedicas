using System.Text.Json.Serialization;

namespace GestionCitasMedicas
{
    public static class Entidades
    {
        public class Paciente
        {
            public required int IdPaciente { get; set; }

            [JsonPropertyName("nombre")]
            public required string Nombre { get; set; } = string.Empty;

            [JsonPropertyName("apellido")]
            public required string Apellido { get; set; } = string.Empty;

            [JsonPropertyName("telefono")]
            private string telefono = string.Empty;

            public string Telefono
            {
                get => telefono;
                set
                {
                    if (!EsTelefonoValido(value))
                        throw new ArgumentException("El teléfono no es válido.");
                    telefono = value;
                }
            }

            private static bool EsTelefonoValido(string telefono)
            {
                if (string.IsNullOrWhiteSpace(telefono))
                    return false;

                // Permitir números, espacios, guiones y paréntesis
                string cleanedTelefono = new string(telefono.Where(char.IsDigit).ToArray());

                // Validar longitud después de limpiar el número
                return cleanedTelefono.Length >= 7 && cleanedTelefono.Length <= 15; // Ejemplo de rango típico.
            }


            // Relaciones
            [JsonIgnore]
            public ICollection<Cita> Citas { get; set; } = new List<Cita>();
        }

        public class Doctor
        {
            public required int IdDoctor { get; set; }

            [JsonPropertyName("nombre")]
            public required string Nombre { get; set; } = string.Empty;

            [JsonPropertyName("especialidad")]
            public required string Especialidad { get; set; } = string.Empty;

            // Relaciones
            [JsonIgnore]
            public ICollection<Cita> Citas { get; set; } = new List<Cita>();
        }

        public class Cita
        {
            public int IdCita { get; set; }

            [JsonPropertyName("fecha")]
            public DateTime Fecha { get; set; }

            [JsonPropertyName("id_paciente")]
            public int IdPaciente { get; set; }

            [JsonPropertyName("id_doctor")]
            public int IdDoctor { get; set; }

            // Relacionar solo por IDs sin requerir objetos completos
            [JsonIgnore] // No incluir estos campos en la serialización/deserialización del JSON
            public Paciente? Paciente { get; set; }

            [JsonIgnore] // No incluir estos campos en la serialización/deserialización del JSON
            public Doctor? Doctor { get; set; }

            // Puedes mantener la relación con Procedimientos, si es necesario
            [JsonIgnore]
            public ICollection<Procedimiento> Procedimientos { get; set; } = new List<Procedimiento>();
        }




        public class Procedimiento
        {
            public required int IdProcedimiento { get; set; }

            [JsonPropertyName("descripcion")]
            public required string Descripcion { get; set; } = string.Empty;

            [JsonPropertyName("costo")]
            public required decimal Costo { get; set; }

            [JsonPropertyName("id_cita")]
            public required int IdCita { get; set; }

            // Relación con Cita
            [JsonPropertyName("cita")]
            public Cita Cita { get; set; } = null!;
        }







    }
}
