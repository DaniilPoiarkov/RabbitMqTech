import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.sass']
})
export class AuthComponent implements OnInit {

  // constructor() { }

  ngOnInit(): void {
    this.move();
  }

  move() {
    const elem = document.getElementById("title") as HTMLElement;
    let counter = 0;
    let pos = 0;
    let pos2 = 350;

    setInterval(frame, 5);

    // Just remembered how I watched DVD and it's logo never fall into angle

    function frame() {
      if (counter == 1400) {
        counter = 0;
        pos = 0;
        pos2 = 350;

      } else if(counter < 350) {
        pos++;
        counter++;
        elem.style.top = pos + 'px';
        elem.style.left = pos + 'px';

      } else if(counter < 700) {
        counter++;
        pos--;
        elem.style.left = pos + 'px';

      } else if(counter < 1050) {
        counter++;
        pos++;
        pos2--;
        
        elem.style.top = pos2 +'px';
        elem.style.left = pos + 'px';

      } else if(counter < 1400) {
        counter++;
        pos--;

        elem.style.left = pos + 'px';
      }
    }
  }

}
