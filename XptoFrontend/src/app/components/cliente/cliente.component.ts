import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PedidoService } from '../../services/pedido.service';
import { MenuItemService } from '../../services/menu-item.service';
import { MenuItem, TipoRefeicao, TipodeItemMenu } from '../../models/menu-item.model';
import { Pedido, StatusPedido } from '../../models/pedido.model';

@Component({
  selector: 'app-cliente',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cliente.component.html',
  styleUrl: './cliente.component.scss'
})
export class ClienteComponent implements OnInit {
  menuItems: MenuItem[] = [];
  selectedItems: MenuItem[] = [];
  tipoRefeicao: TipoRefeicao = TipoRefeicao.CafedaManha;

  constructor(
    private menuItemService: MenuItemService,
    private pedidoService: PedidoService
  ) {}

  ngOnInit(): void {
    this.loadMenuItems();
  }

  loadMenuItems(): void {
    this.menuItemService.getByTipoRefeicao(this.tipoRefeicao)
      .subscribe({
        next: (items) => this.menuItems = items,
        error: (error) => console.error('Erro ao carregar itens:', error)
      });
  }

  changeTipoRefeicao(tipo: TipoRefeicao): void {
    this.tipoRefeicao = tipo;
    this.selectedItems = [];
    this.loadMenuItems();
  }

  toggleItemSelection(item: MenuItem): void {
    const index = this.selectedItems.findIndex(i => i.id === item.id);
    if (index === -1) {
      this.selectedItems.push(item);
    } else {
      this.selectedItems.splice(index, 1);
    }
  }

  getTipoLabel(tipo: TipodeItemMenu): string {
    switch (tipo) {
      case TipodeItemMenu.Bebida:
        return "Bebida";
      case TipodeItemMenu.Acompanhamento:
        return "Acompanhamento";
      case TipodeItemMenu.PratoPrincipal:
        return "Prato Principal";
      case TipodeItemMenu.Sobremesa:
        return "Sobremesa";
      default:
        return "Desconhecido";
    }
  }

  private isHorarioPermitido(tipoRefeicao: TipoRefeicao): boolean {
    const hora = new Date().getHours();
    const minutos = new Date().getMinutes();
    const horaAtual = hora + (minutos / 60);

    switch (tipoRefeicao) {
      case TipoRefeicao.CafedaManha:
        return horaAtual >= 6 && horaAtual <= 10.5; // 6:00 às 10:30
      case TipoRefeicao.Almoco:
        return horaAtual >= 11.5 && horaAtual <= 14.5; // 11:30 às 14:30
      default:
        return false;
    }
  }

  createPedido(): void {
    if (this.selectedItems.length === 0) {
      alert('Selecione pelo menos um item');
      return;
    }

    if (!this.isHorarioPermitido(this.tipoRefeicao)) {
      alert('Horário não permitido para este tipo de refeição');
      return;
    }

    const agora = new Date();
    const dataHoraPedido = new Date(
      agora.getFullYear(),
      agora.getMonth(),
      agora.getDate(),
      agora.getHours(),
      agora.getMinutes(),
      agora.getSeconds()
    );

    const pedido: Pedido = {
      id: 0,
      usuarioId: 'user123',
      dataHoraPedido: dataHoraPedido,
      tipoRefeicao: this.tipoRefeicao,
      status: StatusPedido.Recebido,
      pedidoCompleto: false,
      itens: this.selectedItems
    };

    this.pedidoService.createPedido(pedido)
      .subscribe({
        next: () => {
          alert('Pedido criado com sucesso!');
          this.selectedItems = [];
        },
        error: (error) => {
          const mensagemErro = error.error?.errors?.[0] || 'Erro desconhecido';
          alert('Erro ao criar pedido: ' + mensagemErro);
          console.error('Erro detalhado:', error);
        }
      });
  }
}