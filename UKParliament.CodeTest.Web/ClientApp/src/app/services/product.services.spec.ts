import {ProductServices} from "./product.services";
import {HttpClient} from "@angular/common/http";
import {PersonQuery} from "../../models/person-query";
import {PersonDetail} from "../../models/person-detail";

describe("ProductServices", () => {
  let fakeHttpClient: jasmine.SpyObj<HttpClient>;
  let productService: ProductServices;
  let baseUrl = "test.com/";

  beforeEach(() => {
    fakeHttpClient = jasmine.createSpyObj('HttpClient', ['get', 'post', 'delete']);
    productService = new ProductServices(fakeHttpClient, baseUrl);
  });

  it('search should request without null query parameters', () => {
    let query = new PersonQuery();
    query.skip = 0;
    query.take = 2;
    query.ageFrom = 20;
    query.ageTo = null;

    productService.searchPeople(query);

    expect(fakeHttpClient.get).toHaveBeenCalledOnceWith(`${baseUrl}api/person/?ageFrom=20&skip=0&take=2`);
  });

  it('update should post person with persons id', () => {
    let person = new PersonDetail();
    person.id = 2;

    productService.updatePerson(person);

    expect(fakeHttpClient.post).toHaveBeenCalledOnceWith(`${baseUrl}api/person/${person.id}`, person);
  });

  it('delete should request delete person with given persons id', () => {
    let personId = 2;

    productService.deletePerson(personId);

    expect(fakeHttpClient.delete).toHaveBeenCalledOnceWith(`${baseUrl}api/person/${personId}`);
  });
})
