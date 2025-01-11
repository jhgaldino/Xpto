using XptoAPI.src.Models;

namespace XptoAPI.src.Common.Validators
{
    public static class DataHoraValidator
    {
        // Horários ajustados para UTC (Brasília + 3h)
        private static readonly TimeSpan CafeDaManhaInicio = new(9, 0, 0);  // 06:00 BRT = 09:00 UTC
        private static readonly TimeSpan CafeDaManhaFim = new(13, 30, 0);   // 10:30 BRT = 13:30 UTC
        private static readonly TimeSpan AlmocoInicio = new(14, 30, 0);     // 11:30 BRT = 14:30 UTC
        private static readonly TimeSpan AlmocoFim = new(17, 30, 0);        // 14:30 BRT = 17:30 UTC

        /// <summary>
        /// Verifica se o horário atual está dentro do horário permitido para a refeição.
        /// </summary>
        /// <param name="tipoRefeicao">Tipo de refeicao.</param>
        /// <returns>Retorna true se o horário atual está dentro do horário permitido para a refeição, caso contrario false.</returns>
        public static bool IsHorarioPermitido(TipoRefeicao tipoRefeicao, DateTime dataHora)
        {
            // Usa o horário UTC diretamente
            var horaAtual = dataHora.ToUniversalTime().TimeOfDay;

            // Valida contra os horários UTC
            return tipoRefeicao switch
            {
                TipoRefeicao.CafedaManha => horaAtual >= CafeDaManhaInicio && horaAtual <= CafeDaManhaFim,
                TipoRefeicao.Almoco => horaAtual >= AlmocoInicio && horaAtual <= AlmocoFim,
                _ => false
            };
        }
    }
}