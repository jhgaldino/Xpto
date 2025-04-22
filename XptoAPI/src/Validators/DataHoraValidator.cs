using NodaTime;
using NodaTime.TimeZones;
using XptoAPI.src.Models;

namespace XptoAPI.src.Common.Validators
{
    public static class DataHoraValidator
    {
        // Definindo os horários em Brasília
        private static readonly LocalTime CafeDaManhaInicio = new(6, 0);  // 06:00 BRT
        private static readonly LocalTime CafeDaManhaFim = new(10, 30);   // 10:30 BRT
        private static readonly LocalTime AlmocoInicio = new(11, 30);     // 11:30 BRT
        private static readonly LocalTime AlmocoFim = new(14, 30);        // 14:30 BRT

        private static readonly DateTimeZone BrasiliaTimeZone =
            DateTimeZoneProviders.Tzdb["America/Sao_Paulo"];

        /// <summary>
        /// Verifica se o horário atual está dentro do horário permitido para a refeição.
        /// </summary>
        /// <param name="tipoRefeicao">Tipo de refeicao.</param>
        /// <returns>Retorna true se o horário atual está dentro do horário permitido para a refeição, caso contrario false.</returns>
        public static bool IsHorarioPermitido(TipoRefeicao tipoRefeicao, DateTime dataHora)
        {
            // Converte DateTime para Instant do NodaTime
            var instant = Instant.FromDateTimeUtc(dataHora.ToUniversalTime());

            // Converte para horário de Brasília
            var zonedDateTime = instant.InZone(BrasiliaTimeZone);
            var localTime = zonedDateTime.TimeOfDay;

            return tipoRefeicao switch
            {
                TipoRefeicao.CafedaManha => localTime >= CafeDaManhaInicio && localTime <= CafeDaManhaFim,
                TipoRefeicao.Almoco => localTime >= AlmocoInicio && localTime <= AlmocoFim,
                _ => false
            };
        }
    }
}