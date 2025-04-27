using NodaTime;
using NodaTime.TimeZones;
using XptoAPI.src.Models;

namespace XptoAPI.src.Common.Validators
{
    public static class DataHoraValidator
    {
        private static readonly LocalTime CafeDaManhaInicio = new(6, 0);
        private static readonly LocalTime CafeDaManhaFim = new(10, 30);
        private static readonly LocalTime AlmocoInicio = new(11, 30);
        private static readonly LocalTime AlmocoFim = new(14, 30);

        private static readonly DateTimeZone BrasiliaTimeZone =
            DateTimeZoneProviders.Tzdb["America/Sao_Paulo"];

        // Alteração: Substituir SystemClock.Instance por SystemClock.Instance diretamente  
        private static readonly IClock SystemClock = NodaTime.SystemClock.Instance;

        /// <summary>  
        /// Verifica se o horário fornecido está dentro do horário permitido para a refeição.  
        /// </summary>  
        /// <param name="tipoRefeicao">Tipo de refeição (Café da Manhã ou Almoço)</param>  
        /// <param name="dataHora">Data e hora a serem validadas</param>  
        /// <returns>Resultado da validação contendo status e mensagem</returns>  
        public static (bool isValido, string mensagem) ValidarHorario(TipoRefeicao tipoRefeicao, DateTime dataHora)
        {
            var instant = Instant.FromDateTimeUtc(dataHora.ToUniversalTime());
            var zonedDateTime = instant.InZone(BrasiliaTimeZone);
            var localTime = zonedDateTime.TimeOfDay;

            var (inicio, fim) = tipoRefeicao switch
            {
                TipoRefeicao.CafedaManha => (CafeDaManhaInicio, CafeDaManhaFim),
                TipoRefeicao.Almoco => (AlmocoInicio, AlmocoFim),
                _ => throw new ArgumentException("Tipo de refeição inválido")
            };

            if (localTime < inicio)
            {
                return (false, $"Muito cedo para {tipoRefeicao}. O horário começa às {inicio:HH:mm}");
            }

            if (localTime > fim)
            {
                return (false, $"Muito tarde para {tipoRefeicao}. O horário termina às {fim:HH:mm}");
            }

            return (true, "Horário permitido");
        }

        /// <summary>  
        /// Verifica se a data fornecida é válida e está no futuro próximo (até 7 dias)  
        /// </summary>  
        /// <param name="dataHora">Data e hora a serem validadas</param>  
        /// <returns>Resultado da validação contendo status e mensagem</returns>  
        public static (bool isValido, string mensagem) ValidarData(DateTime dataHora)
        {
            var instant = Instant.FromDateTimeUtc(dataHora.ToUniversalTime());
            var now = SystemClock.GetCurrentInstant();
            var zonedDateTime = instant.InZone(BrasiliaTimeZone);
            var hoje = now.InZone(BrasiliaTimeZone);

            if (zonedDateTime.Date < hoje.Date)
            {
                return (false, "Não é possível fazer pedidos para datas passadas");
            }

            // Corrigido para usar Duration em vez de Period
            var maximaDataFutura = hoje.Plus(Duration.FromDays(7));
            if (zonedDateTime.Date > maximaDataFutura.Date)
            {
                return (false, "Pedidos só podem ser feitos com até 7 dias de antecedência");
            }

            return (true, "Data válida");
        }

        /// <summary>  
        /// Verifica se o restaurante está aberto no horário fornecido  
        /// </summary>  
        /// <param name="dataHora">Data e hora a serem validadas</param>  
        /// <returns>Resultado da validação contendo status e mensagem</returns>  
        public static (bool isValido, string mensagem) ValidarHorarioFuncionamento(DateTime dataHora)
        {
            var instant = Instant.FromDateTimeUtc(dataHora.ToUniversalTime());
            var zonedDateTime = instant.InZone(BrasiliaTimeZone);

            // Verifica se é fim de semana  
            if (zonedDateTime.DayOfWeek == IsoDayOfWeek.Saturday ||
                zonedDateTime.DayOfWeek == IsoDayOfWeek.Sunday)
            {
                return (false, "O restaurante não funciona aos finais de semana");
            }

            // Verifica se está dentro do horário de funcionamento (6h às 15h)  
            var horario = zonedDateTime.TimeOfDay;
            var abertura = new LocalTime(6, 0);
            var fechamento = new LocalTime(15, 0);

            if (horario < abertura || horario > fechamento)
            {
                return (false, "O restaurante funciona apenas das 06:00 às 15:00");
            }

            return (true, "Horário de funcionamento válido");
        }
    }
}
