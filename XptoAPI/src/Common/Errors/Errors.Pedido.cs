using ErrorOr;

namespace XptoAPI.src.Common.Errors
{
    public static partial class Errors
    {
        public static class Pedido
        {
            public static Error PedidoNaoEncontrado => Error.NotFound(
                code: "Pedido.NaoEncontrado",
                description: "Pedido não foi encontrado");

            public static Error HorarioNaoPermitido => Error.Validation(
                code: "Pedido.HorarioNaoPermitido",
                description: "Horário não permitido para este tipo de refeição");

            public static Error TransicaoStatusInvalida => Error.Validation(
                code: "Pedido.TransicaoStatusInvalida",
                description: "Transição de status inválida para o pedido");

            public class None
            {
                public static readonly Error NotFound = Error.NotFound(
                    code: "Pedido.NaoEncontrado",
                    description: "Nenhum Pedido Encontrado para a Cozinha");
            }

        }
    }
}
