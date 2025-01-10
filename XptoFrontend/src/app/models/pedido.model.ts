import { MenuItem, TipoRefeicao } from './menu-item.model';

export enum StatusPedido {
  Recebido,
  EmPreparacao,
  Pronto,
  Entregue,
  Cancelado
}

export interface Pedido {
  id: number;
  usuarioId: string;
  dataHoraPedido: Date;
  tipoRefeicao: TipoRefeicao;
  status: StatusPedido;
  pedidoCompleto: boolean;
  itens: MenuItem[];
}