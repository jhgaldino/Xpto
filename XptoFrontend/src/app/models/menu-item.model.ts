
export enum TipoRefeicao {
  CafedaManha,
  Almoco
}

export enum TipodeItemMenu {
  Bebida,
  Acompanhamento,
  PratoPrincipal,
  Sobremesa
}

export interface MenuItem {
  id: number;
  nome: string;
  tipo: TipodeItemMenu;
  tipoRefeicao: TipoRefeicao;
  imagemUrl: string;
}

