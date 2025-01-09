using XptoAPI.src.Models;

namespace XptoAPI.src.DTOs
{
    public class UpdateMenuItemDTO
    {
        public string Nome { get; set; } = string.Empty;
        public TipodeItemMenu Tipo { get; set; }
        public TipoRefeicao TipoRefeicao { get; set; }
    }
}