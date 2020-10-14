import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { BoatsCounts } from '../model/BoatsCounts';
import * as moment from 'moment';
import { WindSpeed } from 'app/model/WindSpeed';
import { InProcessBoat } from 'app/model/InProcessBoat';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  cBoats: BoatsCounts = new BoatsCounts();
  wp: WindSpeed = new WindSpeed();
  inpBoat: InProcessBoat = new InProcessBoat();
  isBoatInProgress = false;
  timeOfArr = '';
  constructor(public apiService: ApiService, private ref: ChangeDetectorRef) {
    setInterval(() => {
      this.ref.detectChanges()
    }, 1000);
    var that = this;
    setInterval(function () {
      that.GetWindSpeed();
      that.loadCounts();
    }, 30000);
  }

  ngOnInit() {
    this.loadCounts();
    this.GetInprogressBoat();
    this.GetWindSpeed();
  }
  loadCounts() {
    return this.apiService.GetBoatsCounts().subscribe((data: BoatsCounts) => {
      this.cBoats = data;
    });
  }
  GetWindSpeed() {
    return this.apiService.GetWindSpeed().subscribe((data: WindSpeed) => {
      this.wp = data;
    });
  }

  GetInprogressBoat() {
    this.apiService.GetInprogressBoat().subscribe((res: InProcessBoat) => {
      this.inpBoat = res;
      if (res.id > 0) {
        this.isBoatInProgress = true;
        if (res.boatMasterId == 1) {
          this.inpBoat.boatType = "Speedboat";
        } else if (res.boatMasterId == 2) {
          this.inpBoat.boatType = "Sailboat";
        }
        else if (res.boatMasterId == 3) {
          this.inpBoat.boatType = "Cargo ship";
        }
        this.inpBoat.startdatetime = moment(res.processStartDate).format("dddd, MMMM Do YYYY, h:mm:ss a");
        this.inpBoat.enddatetime = moment(res.processEndDate).format("dddd, MMMM Do YYYY, h:mm:ss a");
        var that = this;
        var timer = setInterval(function () {
          var now = moment();
          var then = moment(res.processEndDate);
          that.timeOfArr = moment.utc(moment(then, "DD/MM/YYYY HH:mm:ss").diff(moment(now, "DD/MM/YYYY HH:mm:ss"))).format("HH:mm:ss")
          if (that.timeOfArr == "00:00:00") {
            that.isBoatInProgress = false;
            that.timeOfArr = "00:00:00"
            clearInterval(this.timer);
            that.loadCounts();
            that.GetWindSpeed();
          }

        }, 1000);


      } else {
        this.isBoatInProgress = false;
      }
    });
  }

}
