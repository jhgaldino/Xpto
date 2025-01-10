using XptoAPI.src.Models;

namespace XptoAPI.src.Common.Validators
{
    public static class DataHoraValidator
    {
        private static readonly TimeSpan CafeDaManhaInicio = new(6, 0, 0);
        private static readonly TimeSpan CafeDaManhaFim = new(10, 30, 0);
        private static readonly TimeSpan AlmocoInicio = new(11, 30, 0);
        private static readonly TimeSpan AlmocoFim = new(14, 30, 0);

        /// <summary>
        /// Verifica se o horário atual está dentro do horário permitido para a refeição.
        /// </summary>
        /// <param name="tipoRefeicao">Tipo de refeicao.</param>
        /// <returns>Retorna true se o horário atual está dentro do horário permitido para a refeição, caso contrario false.</returns>
        public static bool IsHorarioPermitido(TipoRefeicao tipoRefeicao, DateTime dataHora)
        {
            var hora = dataHora.TimeOfDay;

            return tipoRefeicao switch
            {
                TipoRefeicao.CafedaManha => hora >= CafeDaManhaInicio && hora <= CafeDaManhaFim,
                TipoRefeicao.Almoco => hora >= AlmocoInicio && hora <= AlmocoFim,
                _ => false
            };
        }
    }
}