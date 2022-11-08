import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {ProductServices} from "../../services/product.services";
import {PersonQuery} from "../../../models/person-query";
import {SearchResult} from "../../../models/search-result";
import {PersonSummary} from "../../../models/person-summary";
import {AlertMessage} from "../../../models/alert-message";
import {Subject} from "rxjs";
@Component({
  selector: 'people-search',
  templateUrl: './people-search.component.html',
  styleUrls: ['./people-search.component.scss']
})
export class PeopleSearchComponent implements OnInit, OnDestroy {
  @Output() messageEmitter = new EventEmitter<AlertMessage>();
  @Output() editEmitter = new EventEmitter<number>();
  @Output() deleteEmitter = new EventEmitter<number>();

  @Input() changeSearch: Subject<boolean> = new Subject<boolean>();

  query: PersonQuery = new PersonQuery();
  searchResult: SearchResult<PersonSummary>|null = null;
  isLoading = false;
  showSearch = true;

  constructor(private service: ProductServices) {
    this.searchPeople();
  }

  deletePerson(id: number): void {
    this.service.deletePerson(id)
      .subscribe({
        next: () => {
          this.messageEmitter.emit(new AlertMessage("Person was successfully deleted", "success"));
          this.deleteEmitter.emit(id);
          this.searchPeople();
        },
        error: error => this.messageEmitter.emit(new AlertMessage(error.error.Errors.join("\r\n"), "warning")),
        complete: () => this.isLoading = false
      });
  }

  editPerson(id: number): void {
    this.editEmitter.emit(id);
  }

  searchPeople(): void {
    const errors = this.query.validate();

    if (errors.length > 0){
      this.messageEmitter.emit(new AlertMessage(errors.join(".\r\n"), "warning"))
      return;
    }

    this.isLoading = true;
    this.service.searchPeople(this.query)
      .subscribe({
        next: result => this.searchResult = result,
        error: error => this.messageEmitter.emit(new AlertMessage(error.error.Errors.join("\r\n"), "warning")),
        complete: () => this.isLoading = false
      });
  }

  updatePage(skip: number): void {
    this.query.skip = skip;
    this.searchPeople();
  }

  updatePageSize(changeEvent: Event): void {
    const pageSize = parseInt((changeEvent.target as HTMLInputElement).value);

    if (!isNaN(pageSize) && pageSize > 0) {
      this.query.take = pageSize;
      this.searchPeople();
    }
  }

  public ngOnInit(): void {
    this.changeSearch.subscribe(() => {
      this.searchPeople();
    });
  }

  public ngOnDestroy(): void {
    this.changeSearch.unsubscribe();
  }
}
