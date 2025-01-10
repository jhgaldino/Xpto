import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pedido } from '../models/pedido.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {
  private apiUrl = `${environment.apiUrl}/api/pedido`;

  constructor(private http: HttpClient) { }

  getPedidosCozinha(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(`${this.apiUrl}/cozinha`);
  }

  createPedido(pedido: Pedido): Observable<Pedido> {
    return this.http.post<Pedido>(this.apiUrl, pedido);
  }

  updateStatus(id: number, status: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/status?status=${status}`, {});
  }
}