import { 
  Component, 
  OnInit, 
  Input, 
  Output, 
  EventEmitter } from '@angular/core';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.sass']
})
export class InputComponent implements OnInit {

  constructor() { }

  public text = '';

  @Input() class = 'ipt';
  @Input() label = '';
  @Input() errorMessage = '';
  @Input() type = '';
  @Input() placeholder = '';

  @Input() isDisabled = false;
  @Input() cleanAfterEnter = false;

  @Output() value = new EventEmitter<string>();
  @Output() keyDownEnter = new EventEmitter<string>();

  ngOnInit(): void {
  }

  public emitValue(): void {
    this.value.emit(this.text);
  }

  public emitValueKeyDownEnterEvent(): void {
    this.keyDownEnter.emit(this.text);
    
    if(this.cleanAfterEnter){
      this.text = '';
    }
  }
}
