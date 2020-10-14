import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { BlankTemplateComponent } from "./template/blank-template.component";
import { LeftNavTemplateComponent } from "./template/left-nav-template.component";
import { AppRoutingModule, routes } from "./app.routing";
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";
import { HeaderComponent } from "./shared/header/header.component";
import { NavigationComponent } from "./shared/navigation/navigation.component";
import { HttpClientModule } from '@angular/common/http';
import { ApiService } from '../app/services/api.service';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    BlankTemplateComponent,
    PageNotFoundComponent,
    HeaderComponent,
    LeftNavTemplateComponent,
    NavigationComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
   
    AppRoutingModule,
    HttpClientModule,
    
    RouterModule.forRoot(routes, { useHash: true })
  ],
  providers: [ApiService],
  bootstrap: [AppComponent]
})
export class AppModule {}
