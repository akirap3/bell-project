import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/product.model';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit, OnDestroy {
  public isOpen = false;
  public isEditMode = false;
  public productForm!: FormGroup;
  private currentProductId?: number;

  constructor(private fb: FormBuilder, public productService: ProductService) {
    this.initForm();
  }

  ngOnInit(): void {
    window.addEventListener('add-product', this.handleOpenAddEvent);
    window.addEventListener('edit-product', this.handleOpenEditEvent);
  }

  ngOnDestroy(): void {
    window.removeEventListener('add-product', this.handleOpenAddEvent);
    window.removeEventListener('edit-product', this.handleOpenEditEvent);
  }

  private initForm(): void {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]],
      price: [null, [Validators.required, Validators.min(0.01)]],
      stock: [null, [Validators.required, Validators.min(0)]],
      category: ['', [Validators.required, Validators.maxLength(50)]]
    });
  }

  private handleOpenAddEvent = (): void => {
    this.isEditMode = false;
    this.currentProductId = undefined;
    this.productForm.reset();
    this.isOpen = true;
  };

  private handleOpenEditEvent = (event: Event): void => {
    const product = (event as CustomEvent).detail as Product;
    this.isEditMode = true;
    this.currentProductId = product.id;
    this.productForm.patchValue({
      name: product.name,
      description: product.description,
      price: product.price,
      stock: product.stock,
      category: product.category
    });
    this.isOpen = true;
  };

  public onClose(): void {
    this.isOpen = false;
  }

  public isInvalid(controlName: string): boolean {
    const control = this.productForm.get(controlName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }

  public onSubmit(): void {
    if (this.productForm.invalid) return;

    const formVal = this.productForm.value;

    if (this.isEditMode && this.currentProductId !== undefined) {
      const updateDto = {
        id: this.currentProductId,
        ...formVal
      };
      this.productService.update(updateDto, () => this.onClose());
    } else {
      const createDto = { ...formVal };
      this.productService.create(createDto, () => this.onClose());
    }
  }
}
