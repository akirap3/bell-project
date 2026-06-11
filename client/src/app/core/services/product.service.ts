import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product, CreateProductDto, UpdateProductDto } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:5138/api/products';

  private productsSignal = signal<Product[]>([]);
  private loadingSignal = signal<boolean>(false);
  private errorSignal = signal<string | null>(null);
  private selectedProductSignal = signal<Product | null>(null);
  private confirmDeleteProductSignal = signal<Product | null>(null);

  public products = this.productsSignal.asReadonly();
  public loading = this.loadingSignal.asReadonly();
  public error = this.errorSignal.asReadonly();
  public selectedProduct = this.selectedProductSignal.asReadonly();
  public confirmDeleteProduct = this.confirmDeleteProductSignal.asReadonly();

  public totalProducts = computed(() => this.productsSignal().length);
  public totalStock = computed(() => this.productsSignal().reduce((acc, p) => acc + p.stock, 0));
  public averagePrice = computed(() => {
    const list = this.productsSignal();
    return list.length ? (list.reduce((acc, p) => acc + p.price, 0) / list.length) : 0;
  });

  constructor(private http: HttpClient) {}

  public loadAll(): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);
    this.http.get<Product[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.productsSignal.set(data);
        this.loadingSignal.set(false);
      },
      error: (err) => {
        this.errorSignal.set(err.message || 'Failed to load products');
        this.loadingSignal.set(false);
      }
    });
  }

  public loadById(id: number): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);
    this.http.get<Product>(`${this.apiUrl}/${id}`).subscribe({
      next: (product) => {
        this.selectedProductSignal.set(product);
        this.loadingSignal.set(false);
      },
      error: (err) => {
        this.errorSignal.set(err.message || `Failed to load product ${id}`);
        this.loadingSignal.set(false);
      }
    });
  }

  public selectProduct(product: Product | null): void {
    this.selectedProductSignal.set(product);
  }

  public create(dto: CreateProductDto, callback?: () => void): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);
    this.http.post<Product>(this.apiUrl, dto).subscribe({
      next: (newProduct) => {
        this.productsSignal.update((list) => [...list, newProduct]);
        this.loadingSignal.set(false);
        if (callback) callback();
      },
      error: (err) => {
        this.errorSignal.set(err.message || 'Failed to create product');
        this.loadingSignal.set(false);
      }
    });
  }

  public update(dto: UpdateProductDto, callback?: () => void): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);
    this.http.put(`${this.apiUrl}/${dto.id}`, dto).subscribe({
      next: () => {
        this.productsSignal.update((list) =>
          list.map((p) => (p.id === dto.id ? { ...p, ...dto, updatedAt: new Date().toISOString() } : p))
        );
        this.loadingSignal.set(false);
        if (this.selectedProductSignal()?.id === dto.id) {
          this.selectedProductSignal.update((p) => p ? { ...p, ...dto } : null);
        }
        if (callback) callback();
      },
      error: (err) => {
        this.errorSignal.set(err.message || 'Failed to update product');
        this.loadingSignal.set(false);
      }
    });
  }

  public delete(id: number): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);
    this.http.delete(`${this.apiUrl}/${id}`).subscribe({
      next: () => {
        this.productsSignal.update((list) => list.filter((p) => p.id !== id));
        this.loadingSignal.set(false);
        if (this.selectedProductSignal()?.id === id) {
          this.selectedProductSignal.set(null);
        }
      },
      error: (err) => {
        this.errorSignal.set(err.message || 'Failed to delete product');
        this.loadingSignal.set(false);
      }
    });
  }

  public requestDelete(product: Product): void {
    this.confirmDeleteProductSignal.set(product);
  }

  public cancelDelete(): void {
    this.confirmDeleteProductSignal.set(null);
  }

  public confirmDelete(): void {
    const product = this.confirmDeleteProductSignal();
    if (product) {
      this.delete(product.id);
      this.confirmDeleteProductSignal.set(null);
    }
  }
}
