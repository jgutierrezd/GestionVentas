global using System.Text.Json.Serialization;

namespace ApiGestionVentas.Models
{
    public class Asesor
    {
        public int Id { get; set; }
        public int Matricula { get; set; }

        public string? Nombres { get; set; }

        public string? Apellidos { get; set; }

        [JsonIgnore]
        public ICollection<Venta> Ventas { get; set; }
    }
}
