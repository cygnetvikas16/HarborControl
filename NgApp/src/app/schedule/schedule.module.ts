
import { NgModule } from '@angular/core';
import { ScheduleRoutingModule } from './schedule-routing/schedule-routing.module';
import { ScheduleComponent } from './schedule.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    ScheduleRoutingModule,
    FormsModule
  ],
  declarations: [ ScheduleComponent ],
  providers: [],
  exports: [ScheduleComponent]
})
export class ScheduleModule { }
