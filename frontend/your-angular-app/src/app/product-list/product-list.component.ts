import { Component, OnInit } from '@angular/core';
import { ProductService } from '../product.service';
import { SignalrService } from '../signalr.service';
import { Product } from '../types/product.type';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService, private signalrService: SignalrService) {}

  ngOnInit(): void {
    this.signalrService.data$.subscribe((product: Product | null) => {
      if(product !== null) {
        const productToUpdateIndex = this.products.findIndex(p => p.id === product.id);

        if (productToUpdateIndex !== -1) {
          const updatedProduct = { ...this.products[productToUpdateIndex], name: product.name, price: product.price };

          this.products = [
            ...this.products.slice(0, productToUpdateIndex),
            updatedProduct,
            ...this.products.slice(productToUpdateIndex + 1)
          ];
        }
      }      
    });

    this.productService.getProducts().subscribe((data) => {
      this.products = data;
    });
  }
}