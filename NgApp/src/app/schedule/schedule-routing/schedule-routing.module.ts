import { NgModule } from '@angular/core';
import { Routes, RouterModule} from '@angular/router';
import { ScheduleComponent } from '../schedule.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: ScheduleComponent,
    data: {
      title: 'Boat Schedule'
    }
  }
];
@NgModule({
    imports: [
      RouterModule.forChild(routes)
    ],
    exports: [
      RouterModule
    ]
})
export class ScheduleRoutingModule { }
