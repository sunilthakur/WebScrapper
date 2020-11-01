import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NotesService {

  constructor(private http: HttpClient) { }

  scrapeSite(siteUrl: any) {
    return this.http.get<any>(`${environment.serverUrl}/scrapSite?url=${siteUrl}`);
  }
}
