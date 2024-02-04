import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { BehaviorSubject } from 'rxjs';
import { Product } from './types/product.type';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private _data: BehaviorSubject<Product | null> = new BehaviorSubject<Product | null>(null);
  public data$ = this._data.asObservable();
  private hubConnection!: signalR.HubConnection
    public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('http://localhost:5140/notify')
                              .build();
      this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err))
    }
    
    public productChangedDataListener = () => {
      this.hubConnection.on('ProductChanged', (data) => {
        this._data.next(data);
      });
    }
}