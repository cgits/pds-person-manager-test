import {ProductServices} from "../../services/product.services";
import {of} from "rxjs";
import {SearchResult} from "../../../models/search-result";
import {PersonSummary} from "../../../models/person-summary";
import {PersonDetailComponent} from "./person-detail.component";
import {PersonDetail} from "../../../models/person-detail";
import {PhoneNumbers} from "../../../models/phone-numbers";
import {TestBed} from "@angular/core/testing";

describe("PersonDetailComponent", () => {
  let fakeProductService: jasmine.SpyObj<ProductServices>;
  let component: PersonDetailComponent;

  beforeEach(() => {
    fakeProductService = jasmine.createSpyObj('ProductServices', ['getPersonById', 'searchPeople', 'updatePerson', 'deletePerson']);
    component = new PersonDetailComponent(fakeProductService);
  });

  it('get person returns empty person if person id is 0', () => {
    component.person.id = 1;
    component.person.name = "Test Name";
    component.person.phoneNumbers = [new PhoneNumbers()];
    component.person.dateOfBirth = new Date();
    component.hasAddress = true;

    component.personId = 0;

    expect(component.hasAddress).toBeFalse();
    expect(component.person.id).toEqual(0);
    expect(component.person.name).toEqual("");
    expect(component.person.dateOfBirth).toBeNull();
    expect(component.person.phoneNumbers).toEqual([]);
  });

  it('get person assigns person and address if person id not 0 and they have a address', () => {
    let personDetail = new PersonDetail();
    personDetail.id = 1;
    personDetail.lineOne = "test";
    fakeProductService.getPersonById.and.returnValue(of(personDetail));

    component = new PersonDetailComponent(fakeProductService);
    component.personId = 1;

    component.getPerson();
    expect(component.person).toEqual(personDetail);
    expect(component.hasAddress).toEqual(true);

    personDetail.lineOne = "";
    component.getPerson();
    expect(component.hasAddress).toEqual(false);
  });

  it('toggle primary phone number sets others to false', () => {
    component.person.phoneNumbers = [
      new PhoneNumbers(),
      new PhoneNumbers(),
      new PhoneNumbers(),
      new PhoneNumbers()
    ];

    component.person.phoneNumbers.forEach(x => {
      component.togglePrimaryPhoneNumber(x);

      const primary = component.person.phoneNumbers.filter(x => x.isPrimary);
      expect(primary.length).toBe(1);
      expect(primary[0]).toBe(x);
    })
  });


  it('clear address to clear all address feilds', () => {
    component.hasAddress = true;
    component.person.lineOne = component.person.lineTwo = component.person.city = component.person.country = component.person.postCode = "line 1";
    expect(component.person.lineOne + component.person.lineTwo + component.person.city + component.person.country + component.person.postCode).toBe("line 1line 1line 1line 1line 1");

    component.clearAddress()
    expect(component.hasAddress).toBeFalse();
    expect(component.person.lineOne + component.person.lineTwo + component.person.city + component.person.country + component.person.postCode).toBe("");
  });
})
