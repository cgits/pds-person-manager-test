import {PersonSummary} from "../../models/person-summary";
import {PersonDetail} from "../../models/person-detail";
import {PersonQuery} from "../../models/person-query";
import {SearchResult} from "../../models/search-result";
import {map, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {Inject, Injectable} from "@angular/core";
import {PhoneNumbers} from "../../models/phone-numbers";

@Injectable()
export class ProductServices {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  getPersonById(id: number): Observable<PersonDetail> {
    return this.http.get<PersonDetail>(`${this.baseUrl}api/person/${id}`)
      .pipe(map(x =>
      {
        const mapped = new PersonDetail();

        mapped.id = x.id;
        mapped.title = x.title;
        mapped.name = x.name;
        mapped.dateOfBirth = x.dateOfBirth;
        mapped.email = x.email;
        mapped.accountEnabled = x.accountEnabled;
        mapped.lineOne = x.lineOne;
        mapped.lineTwo = x.lineTwo;
        mapped.city = x.city;
        mapped.country = x.country;
        mapped.postCode = x.postCode;
        mapped.phoneNumbers = x.phoneNumbers.map(y => {
          const phone = new PhoneNumbers();

          phone.id = y.id;
          phone.number = y.number;
          phone.description = y.description;
          phone.isPrimary = y.isPrimary;

          return phone;
        });

        return mapped;
      }));
  }

  searchPeople(query: PersonQuery): Observable<SearchResult<PersonSummary>> {
    //get param without null properties
    const queryParams = Object.fromEntries(Object.entries(query).filter(([_, v]) => v != null));
    return this.http.get<SearchResult<PersonSummary>>(`${this.baseUrl}api/person/?${new URLSearchParams(queryParams).toString()}`);
  }

  updatePerson(person: PersonDetail): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}api/person/${person.id}`, person);
  }

  deletePerson(personId: number): Observable<unknown> {
    return this.http.delete(`${this.baseUrl}api/person/${personId}`);
  }
}
