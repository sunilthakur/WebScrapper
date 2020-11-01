import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from "src/environments/environment.prod";
import { map } from 'rxjs/operators';
import { User } from '../models/user';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(userModel: any) {
    return this.http.post<any>(`${environment.serverUrl}user/validateuserlogin`, userModel)
      .pipe(map(user => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        var obj: any = user;
        if (obj && obj.isAuthorised) {
          localStorage.setItem('currentUser', JSON.stringify(obj));
          localStorage.setItem('loggedInUserId', JSON.stringify(obj.userId));
          this.currentUserSubject.next(obj);
          return user;
        }
        else {
          return user;
        }
      }));
  }

  getToken() {
    return this.currentUserSubject.value.token;
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
