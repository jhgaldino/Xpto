using Microsoft.AspNetCore.Mvc;
using Moq;
using XptoAPI.src.Controllers;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;
using ErrorOr;
using XptoAPI.src.Common.Errors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XptoAPI.Tests.UnitTests
{
    public class PedidoControllerTests
    {
        private readonly Mock<IPedidoService> _mockPedidoService;
        private readonly PedidoController _controller;

        public PedidoControllerTests()
        {
            _mockPedidoService = new Mock<IPedidoService>();
            _controller = new PedidoController(_mockPedidoService.Object);
        }

        [Fact]
        public async Task CreatePedido_WhenValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var pedido = new Pedido
            {
                UsuarioId = "user123",
                DataHoraPedido = DateTime.Now,
                TipoRefeicao = TipoRefeicao.Almoco,
                Status = StatusPedido.Recebido,
                Itens = new List<MenuItem>
                {
                    new() { Id = 1, Nome = "Item1", TipoRefeicao = TipoRefeicao.Almoco }
                }
            };

            _mockPedidoService
                .Setup(s => s.CreatePedidoAsync(It.IsAny<Pedido>()))
                .ReturnsAsync(pedido);

            // Act
            var result = await _controller.CreatePedido(pedido);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Pedido>(createdAtActionResult.Value);
            Assert.Equal(pedido.UsuarioId, returnValue.UsuarioId);
        }

        [Fact]
        public async Task CreatePedidoWhenInvalidInputReturnsBadRequest()
        {
            // Arrange
            var pedido = new Pedido
            {
                UsuarioId = string.Empty, // Invalid input
                Itens = new List<MenuItem>()
            };

            _mockPedidoService
                .Setup(s => s.CreatePedidoAsync(It.IsAny<Pedido>()))
                .ReturnsAsync(ErrorOr.Error.Validation("Pedido.ValidationError", "UsuarioId e obrigatorio"));

            // Act
            var result = await _controller.CreatePedido(pedido);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateStatus_WhenValidTransition_ReturnsNoContent()
        {
            // Arrange
            var pedidoId = 1;
            var novoStatus = StatusPedido.EmPreparacao;

            _mockPedidoService
                .Setup(s => s.UpdateStatusAsync(pedidoId, novoStatus))
                .ReturnsAsync(Result.Updated);

            // Act
            var result = await _controller.UpdateStatus(pedidoId, novoStatus);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateStatusWhenPedidoNotFoundReturnsNotFound()
        {
            // Arrange
            var pedidoId = 999;
            var novoStatus = StatusPedido.EmPreparacao;

            _mockPedidoService
                .Setup(s => s.UpdateStatusAsync(pedidoId, novoStatus))
                .ReturnsAsync(Errors.Pedido.PedidoNaoEncontrado);

            // Act
            var result = await _controller.UpdateStatus(pedidoId, novoStatus);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPedidosCozinha_ReturnsOrderedList()
        {
            // Arrange
            Pedido pedidoTeste = new()
            {
                Id = 2,
                DataHoraPedido = DateTime.Now,
                Status = StatusPedido.EmPreparacao,
                Itens = new List<MenuItem>() // Added required property initialization
            };
            var pedidos = new List<Pedido>
            {
                new() {
                    Id = 1,
                    DataHoraPedido = DateTime.Now.AddMinutes(-30),
                    Status = StatusPedido.Recebido,
                    Itens = new List<MenuItem>() // Added required property initialization
                },
                pedidoTeste
            };

            _mockPedidoService
                .Setup(s => s.GetPedidosCozinhaAsync())
                .ReturnsAsync(pedidos);

            // Act
            var result = await _controller.GetPedidosCozinha();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Pedido>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetPedidosCozinha_ReturnsOrderedItemsWithinPedidos()
        {
            // Arrange
            var pedido = new Pedido
            {
                Id = 1,
                DataHoraPedido = DateTime.Now,
                Status = StatusPedido.EmPreparacao,
                // Itens em ordem desordenada
                Itens = new List<MenuItem>
                {
                    new() { Id = 1, Nome = "Sobremesa", Tipo = TipodeItemMenu.Sobremesa },
                    new() { Id = 2, Nome = "Acompanhamento", Tipo = TipodeItemMenu.Acompanhamento },
                    new() { Id = 3, Nome = "Bebida", Tipo = TipodeItemMenu.Bebida },
                    new() { Id = 4, Nome = "Principal", Tipo = TipodeItemMenu.PratoPrincipal }
                }
            };

            _mockPedidoService
                .Setup(s => s.GetPedidosCozinhaAsync())
                .ReturnsAsync(new List<Pedido> { pedido })
                .Callback(() =>
                {
                    // Simula a ordenação dos itens conforme a lógica do serviço real
                    pedido.Itens = pedido.Itens
                        .OrderBy(item => item.Tipo switch
                        {
                            TipodeItemMenu.Bebida => 1,
                            TipodeItemMenu.Acompanhamento => 2,
                            TipodeItemMenu.PratoPrincipal => 3,
                            TipodeItemMenu.Sobremesa => 4,
                            _ => 99
                        })
                        .ToList();
                });

            // Act
            var result = await _controller.GetPedidosCozinha();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPedidos = Assert.IsAssignableFrom<IEnumerable<Pedido>>(okResult.Value);
            var firstPedido = returnedPedidos.First();

            // Verifica a ordem dos itens
            var items = firstPedido.Itens.ToList();
            Assert.Equal(4, items.Count);
            Assert.Equal(TipodeItemMenu.Bebida, items[0].Tipo);
            Assert.Equal(TipodeItemMenu.Acompanhamento, items[1].Tipo);
            Assert.Equal(TipodeItemMenu.PratoPrincipal, items[2].Tipo);
            Assert.Equal(TipodeItemMenu.Sobremesa, items[3].Tipo);
        }

    }
}