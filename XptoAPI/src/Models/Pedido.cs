namespace XptoAPI.src.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataHoraPedido { get; set; }
        public TipoRefeicao TipoRefeicao { get; set; }
        public bool PedidoCompleto { get; set; }
        public required List<MenuItem> Itens { get; set; }
    }
}
