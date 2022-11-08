import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {PersonSummary} from "../../models/person-summary";
import {ProductServices} from "../services/product.services";
import {AlertMessage} from "../../models/alert-message";
import {Subject} from "rxjs";

@Component({
  selector: 'app-home',
  templateUrl: './people-dashboard.component.html',
  styleUrls: ['./people-dashboard.component.scss'],
})
export class PeopleDashboardComponent {
  changeSearch: Subject<boolean> = new Subject();

  errorMessage: AlertMessage|null = null;
  personId = 0;

  constructor() {
  }

  toggleErrorMessage(error: AlertMessage|null){
    this.errorMessage = error;
  }

  refreshSearch(personId: number) {
    this.personId = personId;
    this.changeSearch.next(true);
  }

  itemDeleted(personId: number) {
    if (this.personId === personId){
      this.personId = 0;
    }
  }
}
