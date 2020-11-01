import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';


@Injectable()
export class DataSharingService {
  public user = new Subject<any>();

  UserChanged(parm: any) {
    this.user.next(parm);
  }
}
