namespace ApiGestionVentas.ViewModel
{
    public class VentaDto
    {
        public int Periodo { get; set; }
        public decimal Monto { get; set; }
        public decimal Puntos { get; set; }

        public int AsesorId { get; set; }
        public int ProductoId { get; set; }
        public int ClienteId { get; set; }
    }
}
