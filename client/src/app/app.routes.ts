import { Routes } from '@angular/router';
import { ProductDashboardComponent } from './features/product-dashboard/product-dashboard.component';

export const routes: Routes = [
  { path: '', component: ProductDashboardComponent },
  { path: '**', redirectTo: '' }
];
