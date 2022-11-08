import {PeopleSearchComponent} from "./people-search.component";
import {ProductServices} from "../../services/product.services";
import {of} from "rxjs";
import {SearchResult} from "../../../models/search-result";
import {PersonSummary} from "../../../models/person-summary";
import {TestBed} from "@angular/core/testing";

describe("PeopleSearchComponent", () => {
  let fakeProductService: jasmine.SpyObj<ProductServices>;
  let component: PeopleSearchComponent;

  beforeEach(() => {
    fakeProductService = jasmine.createSpyObj('ProductServices', ['getPersonById', 'searchPeople', 'updatePerson', 'deletePerson']);

    fakeProductService.searchPeople.and.returnValue(of(new SearchResult<PersonSummary>()));
    component = new PeopleSearchComponent(fakeProductService);
  });

  it('search people should throw error if invalid query', () => {
    spyOn(component.messageEmitter, 'emit');

    component.query.ageTo = -1;
    component.searchPeople();
    expect(component.messageEmitter.emit).toHaveBeenCalledWith(jasmine.objectContaining({alertType: 'warning'}));

    component.query.ageTo = 0;
    component.query.ageTo = -1;
    component.searchPeople();

    expect(component.messageEmitter.emit).toHaveBeenCalledWith(jasmine.objectContaining({alertType: 'warning'}));
  });

  it('delete person should return success message and reloads search on success', () => {
    fakeProductService.deletePerson.and.returnValue(of(undefined));

    spyOn(component.messageEmitter, 'emit');

    component.deletePerson(1);

    expect(component.messageEmitter.emit).toHaveBeenCalledWith(jasmine.objectContaining({alertType: 'success'}));
    expect(fakeProductService.searchPeople).toHaveBeenCalled();
  });
})
