using XptoAPI.src.Models;

namespace XptoAPI.src.DTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipodeItemMenu Tipo { get; set; }
        public TipoRefeicao TipoRefeicao { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
    }
}