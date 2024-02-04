import { Component } from '@angular/core';
import { SignalrService } from './signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'your-angular-app';

  constructor(public signalRService: SignalrService) { }
  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.productChangedDataListener();
  }
}
