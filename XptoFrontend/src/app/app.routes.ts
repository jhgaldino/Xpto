import { Routes } from '@angular/router';
import { ClienteComponent } from './components/cliente/cliente.component';
import { CozinhaComponent } from './components/cozinha/cozinha.component';

export const routes: Routes = [
  { path: 'cliente', component: ClienteComponent },
  { path: 'cozinha', component: CozinhaComponent },
  { path: '', redirectTo: '/cliente', pathMatch: 'full' }
];
