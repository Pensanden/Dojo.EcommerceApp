import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/productType';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http:HttpClient) { }

  getProducts(brandId?: number,typeId?: number) {
    let params = new HttpParams();

    if (brandId){
      params = params.append('brandId',brandId.toString());
    }
    if (typeId) {
      params = params.append('typeId', typeId.toString())
    }
    return this.http.get<IPagination>(this.baseUrl + 'products',{ observe: 'response',params })
        .pipe(
          map(response => {
            return response.body;
          })
        );
  }

  getBrands() {
    return this.http.get<IBrand[]>(this.baseUrl + 'Products/ProductBrands')
  }

  getTypes() {
    
    return this.http.get<IType[]>(this.baseUrl + 'Products/ProductTypes')
  }
}
