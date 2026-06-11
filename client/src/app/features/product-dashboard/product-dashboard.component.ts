import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../core/services/product.service';
import { ProductListComponent } from '../product-list/product-list.component';
import { ProductDetailComponent } from '../product-detail/product-detail.component';
import { ProductFormComponent } from '../product-form/product-form.component';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-product-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ProductListComponent,
    ProductDetailComponent,
    ProductFormComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './product-dashboard.component.html',
  styleUrl: './product-dashboard.component.scss'
})
export class ProductDashboardComponent implements OnInit {
  constructor(public productService: ProductService) {}

  ngOnInit(): void {
    this.productService.loadAll();
  }

  onAddNew(): void {
    window.dispatchEvent(new CustomEvent('add-product'));
  }

  onClearError(): void {
    this.productService.loadAll();
  }
}
