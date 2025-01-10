
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItem, TipoRefeicao } from '../models/menu-item.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MenuItemService {
  private apiUrl = `${environment.apiUrl}/api/menuitem`;

  constructor(private http: HttpClient) { }

  getByTipoRefeicao(tipo: TipoRefeicao): Observable<MenuItem[]> {
    return this.http.get<MenuItem[]>(`${this.apiUrl}/tiporefeicao/${tipo}`);
  }
}