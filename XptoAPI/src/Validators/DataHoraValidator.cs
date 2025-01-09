using XptoAPI.src.Models;

namespace XptoAPI.src.Common.Validators
{
    public static class DataHoraValidator
    {
        private static readonly TimeSpan CafeDaManhaInicio = new(6, 0, 0);
        private static readonly TimeSpan CafeDaManhaFim = new(10, 30, 0);
        private static readonly TimeSpan AlmocoInicio = new(11, 30, 0);
        private static readonly TimeSpan AlmocoFim = new(14, 30, 0);

        public static bool IsHorarioPermitido(TipoRefeicao tipoRefeicao)
        {
            var horaAtual = DateTime.Now.TimeOfDay;

            return tipoRefeicao switch
            {
                TipoRefeicao.CafedaManha => horaAtual >= CafeDaManhaInicio && horaAtual <= CafeDaManhaFim,
                TipoRefeicao.Almoco => horaAtual >= AlmocoInicio && horaAtual <= AlmocoFim,
                _ => false
            };
        }
    }
}