import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from 'environments/environment';

import { BoatsCounts } from '../model/BoatsCounts';
import { WindSpeed } from '../model/WindSpeed';
import { AtPerimeterBoats } from '../model/AtPerimeterBoats';
import { BoatSchedule } from 'app/model/boatSchedule';
import { InProcessBoat } from 'app/model/InProcessBoat';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public windSpeedAMH: number;
  boatSchedule: BoatSchedule = new BoatSchedule();
  // Base url
  baseurl = environment.baseUrl;

  constructor(private http: HttpClient) {


  }

  // Http Headers
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  // GET
  GetBoatsCounts(): Observable<BoatsCounts> {
    return this.http.get<BoatsCounts>(this.baseurl + '/Boats/GetBoatsCounts')
      .pipe(
        retry(1),
        catchError(this.errorHandl)
      )
  }

  GetWindSpeed(): Observable<WindSpeed> {

    return this.http.get<WindSpeed>(this.baseurl + '/Boats/GetWindSpeed')
      .pipe(
        retry(1),
        catchError(this.errorHandl)
      )
  }

  GetAtPerimeterBoats(): Observable<AtPerimeterBoats> {
    return this.http.get<AtPerimeterBoats>(this.baseurl + '/Boats/GetAllAtPerimeterBoats')
      .pipe(
        retry(1),
        catchError(this.errorHandl)
      )
  }
  // post
  ScheduleBoat(bId: number): Observable<any> {
    this.boatSchedule.id = bId;
    return this.http.post<any>(this.baseurl + '/Boats/ScheduleBoat', this.boatSchedule, this.httpOptions)
      .pipe(
        retry(1),
        catchError(this.errorHandl)
      )
  }

  GetInprogressBoat(): Observable<InProcessBoat> {
    return this.http.get<InProcessBoat>(this.baseurl + '/Boats/GetInProcessBoat')
      .pipe(
        retry(1),
        catchError(this.errorHandl)
      )
  }

  // Error handling
  errorHandl(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
   
    return throwError(errorMessage);
  }
}
