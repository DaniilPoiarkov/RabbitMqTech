import { 
  Component,
  Input, 
  Output, 
  EventEmitter } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.sass']
})
export class ButtonComponent {

  @Input() btnText = '';
  @Input() class = 'btn';

  @Output() clicked = new EventEmitter<void>();

  public click(): void {
    this.clicked.emit();
  }

}
