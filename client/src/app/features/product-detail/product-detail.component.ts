import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../core/services/product.service';
import { getStockClass } from '../../core/utils/product-helpers';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.scss'
})
export class ProductDetailComponent {
  public getStockClass = getStockClass;

  constructor(public productService: ProductService) {}

  onClose(): void {
    this.productService.selectProduct(null);
  }

  onEdit(product: any): void {
    window.dispatchEvent(new CustomEvent('edit-product', { detail: product }));
  }

  getStockPercentage(stock: number): number {
    return Math.min((stock / 100) * 100, 100);
  }
}
