import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient, private router: Router) { }


  createUser(userModel: any) {
    return this.http.post<any>(`${environment.serverUrl}user/createuser`, userModel)
      .pipe(map(res => {
        var obj: any = res;
        if (obj) {
          alert('You have successfully registered for Sticky Notes App.');
        }
      }
      ));
  }
}
