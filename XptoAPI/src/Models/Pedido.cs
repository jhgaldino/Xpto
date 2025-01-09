namespace XptoAPI.src.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public DateTime DataHoraPedido { get; set; }
        public TipoRefeicao TipoRefeicao { get; set; }
        public StatusPedido Status { get; set; }
        public bool PedidoCompleto { get; set; }
        public required List<MenuItem> Itens { get; set; }
    }
public enum StatusPedido
{
    Recebido,
    EmPreparacao, 
    Pronto,
    Entregue,
    Cancelado
}

}
