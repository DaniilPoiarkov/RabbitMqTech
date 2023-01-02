import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.sass']
})
export class MainPageComponent implements OnInit {

  //constructor() { }

  ngOnInit(): void {
    this.animate();
  }

  animate(): void {
    const el = document.getElementById('am1') as HTMLElement;
    let counter = 0;
    let op = 0;

    setInterval(frame, 5);

    function frame(): void {
      if(counter === 600) {
        counter = 0;
        el.style.opacity = '0';
      } else if(counter < 300) {
        counter++;
        op++;
        el.style.opacity = String(op / 100);

      } else if(counter < 600) {
        counter++;
        op--;
        el.style.opacity = String(op / 100);
      }
    }
  }

}
