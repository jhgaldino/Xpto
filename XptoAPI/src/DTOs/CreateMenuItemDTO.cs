using XptoAPI.src.Models;

namespace XptoAPI.src.DTOs{
    public class CreateMenuItemDTO
    {
        public string Nome { get; set; } = string.Empty;
        public TipodeItemMenu Tipo { get; set; }
        public TipoRefeicao TipoRefeicao { get; set; }
    }
}