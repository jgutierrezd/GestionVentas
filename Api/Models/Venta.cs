global using System.Text.Json.Serialization;

namespace ApiGestionVentas.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public int Periodo { get; set; }
        public decimal Monto { get; set; }
        public decimal Puntos { get; set; }
        public DateTime FechaOperacion { get; set; }
        
        public int AsesorId { get; set; }
        public int ProductoId { get; set; }
        public int ClienteId { get; set; }

        [JsonIgnore]
        public Asesor Asesor { get; set; }
        public Producto Producto { get; set; }
        public Cliente Cliente { get; set; }

    }
}
