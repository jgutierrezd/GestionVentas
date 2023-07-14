global using System.Text.Json.Serialization;

namespace ApiGestionVentas.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public int Matricula { get; set; }

        public string? Nombres { get; set; }

        public string? Apellidos { get; set; }

        public string? Celular { get; set; }

        [JsonIgnore]
        public ICollection<Venta> Ventas { get; set; }
    }
}
