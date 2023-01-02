import { 
  Component,
  Input, 
  Output,
  EventEmitter, 
  forwardRef} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.sass'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => InputComponent),
    },
  ],
})
export class InputComponent implements ControlValueAccessor {

  public text = '';

  @Input() class = 'ipt';
  @Input() label = '';
  @Input() errorMessage = '';
  @Input() type = '';
  @Input() placeholder = '';

  @Input() isErrorDisplay = false;
  @Input() isDisabled = false;
  @Input() cleanAfterEnter = false;

  @Output() value = new EventEmitter<string>();
  @Output() keyDownEnter = new EventEmitter<string>();

  public emitValue(): void {
    this.value.emit(this.text);
  }

  public emitValueKeyDownEnterEvent(): void {
    this.keyDownEnter.emit(this.text);
    
    if(this.cleanAfterEnter){
      this.text = '';
    }
  }

  writeValue(val: string): void {
    this.text = val;
  }

  onChange: (value: Event) => void = () => { };

  onTouched: (value: Event) => void = () => { };

  registerOnChange(fn: (value: Event) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: (value: Event) => void): void {
    this.onTouched = fn;
  }

}
