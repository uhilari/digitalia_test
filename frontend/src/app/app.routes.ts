import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { PrivateComponent } from './layouts/private/private.component';
import { LoginComponent } from './pages/login/login.component';
import { authGuard } from './services/auth.guard';
import { RegistroComponent } from './pages/registro/registro.component';
import { PerfilComponent } from './pages/perfil/perfil.component';
import { CambioPwdComponent } from './pages/cambio-pwd/cambio-pwd.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'registro',
    component: RegistroComponent
  },
  {
    path: '',
    component: PrivateComponent,
    children: [
      {
        path: '',
        component: HomeComponent,
        canActivate: [authGuard]
      },
      {
        path: 'perfil',
        component: PerfilComponent,
        canActivate: [authGuard]
      },
      {
        path: 'cambio-pwd',
        component: CambioPwdComponent,
        canActivate: [authGuard]
      }
    ]
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];
