import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PedidoService } from '../../services/pedido.service';
import { Pedido, StatusPedido } from '../../models/pedido.model';
import { TipodeItemMenu } from '../../models/menu-item.model';

@Component({
  selector: 'app-cozinha',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cozinha.component.html',
  styleUrl: './cozinha.component.scss'
})
export class CozinhaComponent implements OnInit {
  pedidos: Pedido[] = [];
  StatusPedido = StatusPedido; // Para usar no template

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.loadPedidos();
    // Atualiza a cada 30 segundos
    setInterval(() => this.loadPedidos(), 30000);
  }

  loadPedidos(): void {
    this.pedidoService.getPedidosCozinha()
      .subscribe({
        next: (pedidos) => this.pedidos = pedidos,
        error: (error) => console.error('Erro ao carregar pedidos:', error)
      });
  }

  updateStatus(pedido: Pedido, newStatus: StatusPedido): void {
    this.pedidoService.updateStatus(pedido.id, newStatus)
      .subscribe({
        next: () => {
          pedido.status = newStatus;
          if (newStatus === StatusPedido.Pronto) {
            pedido.pedidoCompleto = true;
          }
        },
        error: (error) => alert('Erro ao atualizar status: ' + error.error?.errors?.[0] || 'Erro desconhecido')
      });
  }

  getStatusLabel(status: StatusPedido): string {
    return StatusPedido[status];
  }
}
