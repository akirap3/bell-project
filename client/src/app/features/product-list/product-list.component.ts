import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/product.model';
import { getStockClass } from '../../core/utils/product-helpers';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent {
  public getStockClass = getStockClass;

  constructor(public productService: ProductService) {}

  trackById(index: number, product: Product): number {
    return product.id;
  }

  onSelect(product: Product): void {
    this.productService.selectProduct(product);
  }

  onEdit(product: Product): void {
    this.productService.selectProduct(product);
    window.dispatchEvent(new CustomEvent('edit-product', { detail: product }));
  }

  onDelete(product: Product): void {
    this.productService.requestDelete(product);
  }

  getCategoryColor(category: string): string {
    const hash = category.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0);
    const colors = ['#6366f1', '#3b82f6', '#10b981', '#f59e0b', '#ec4899', '#8b5cf6'];
    return colors[hash % colors.length];
  }
}
