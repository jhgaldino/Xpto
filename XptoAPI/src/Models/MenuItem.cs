namespace XptoAPI.src.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public TipodeItemMenu Tipo { get; set; }
        public TipoRefeicao TipoRefeicao { get; set; }
        public string ImagemUrl { get; set; }
    }

    public enum TipodeItemMenu
    {
        Bebida,
        Acompanhamento,
        PratoPrincipal,
        Sobremesa
    }

    public enum TipoRefeicao
    {
        CafedaManha,
        Almoco
    }
}
