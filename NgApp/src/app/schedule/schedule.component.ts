import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { ApiService } from 'app/services/api.service';
import { AtPerimeterBoats } from 'app/model/AtPerimeterBoats';
import { WindSpeed } from 'app/model/WindSpeed';
import { renderFlagCheckIfStmt } from '@angular/compiler/src/render3/view/template';
import { InProcessBoat } from 'app/model/InProcessBoat';

@Component({
  selector: 'app-forms',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
  public pageData;
  isSelected = false;
  boatSelect1 = -1;
  wp: WindSpeed = new WindSpeed();
  inpBoat: InProcessBoat = new InProcessBoat();
  boatList: AtPerimeterBoats[] = [];
  isCreated = false;
  isWindError = false;
  isInprogressMessage = false;
  constructor(router: Router, private route: ActivatedRoute, public apiService: ApiService) {

  }


  ngOnInit() {
    this.loadBoats();
    this.pageData = <any>this.route.snapshot.data;
   
  }

  // Choose select dropdown
  changeBoat(e) {
   
    if (e.target.value == -1) {
      this.isSelected = false;
    } else {
      this.isSelected = true;
    }

  }



  // Getter method to access formcontrols

  onSubmit() {
    this.isCreated = false;
    this.isWindError = false;
    this.isInprogressMessage = false;
    // go for the save 
    if (this.isSelected) {
    
      this.apiService.GetInprogressBoat().subscribe((res: InProcessBoat) => {
        this.inpBoat = res;
        if (res.id > 0) {
          this.isInprogressMessage = true;
          return false;
        } else { // check another validation
          this.apiService.GetWindSpeed().subscribe((data: WindSpeed) => {
            this.wp = data;
            var findBoat = this.boatList.find(x => x.id == this.boatSelect1);
            if (findBoat.boatType === 'Sailboat') {
              if (this.wp.windKmH < 10 || this.wp.windKmH > 30) {
                // errror of wind
                this.isWindError = true;
                return false;
              }
            }
            this.apiService.ScheduleBoat(+this.boatSelect1).subscribe(res => {
              this.isCreated = true;

              this.loadBoats();
              this.boatSelect1=-1;
              //this.ngZone.run(() => this.router.navigateByUrl('/issues-list'))
            });
          });
        }
      });


    } else {
      this.isSelected = false;
      return;
    }


  }

  // Issues list
  loadBoats() {
    this.boatList = [];
    return this.apiService.GetAtPerimeterBoats().subscribe((data: any) => {
    
      this.boatList = data;
    
    })
  }
}
