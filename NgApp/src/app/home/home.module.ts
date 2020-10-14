import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing/home-routing.module';
import { CommonModule } from '@angular/common';



@NgModule({
  imports: [
    HomeRoutingModule,
    CommonModule
  ],
  declarations: [ HomeComponent ],
  providers: [
      
  ]
})
export class HomeModule { }
