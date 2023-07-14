global using System.Text.Json.Serialization;

namespace ApiGestionVentas.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }

        public string? Nombre { get; set; }

        public int? Puntos { get; set; }

        public decimal? Porcentaje { get; set; }

        [JsonIgnore]
        public ICollection<Venta> Ventas { get; set; }
    }
}
