import { Component, OnInit } from '@angular/core';
import { ApiService } from 'app/services/api.service';
import { WindSpeed } from '../../model/WindSpeed';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  wp: WindSpeed= new WindSpeed();
  constructor(public apiService: ApiService) { 
    
    this.GetWindSpeed(); 
  
      // check wind speed at every 5 mins
      var that=this;
      setInterval(function () {
        that.GetWindSpeed();
      }, 30000);
  }

  ngOnInit() {

  }
  GetWindSpeed() {
    return this.apiService.GetWindSpeed().subscribe((data: WindSpeed) => {
      this.wp = data;
    });
  }

 
}
