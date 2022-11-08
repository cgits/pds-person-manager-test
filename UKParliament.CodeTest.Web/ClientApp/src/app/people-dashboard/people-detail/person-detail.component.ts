import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import {ProductServices} from "../../services/product.services";
import {PersonDetail} from "../../../models/person-detail";
import {AlertMessage} from "../../../models/alert-message";
import {PhoneNumbers} from "../../../models/phone-numbers";

@Component({
  selector: 'person-detail',
  templateUrl: './person-detail.component.html',
  styleUrls: ['./person-detail.component.scss']
})
export class PersonDetailComponent {
  private _personId: number = 0;
  @Input() set personId(value: number){
    this._personId = value;
    this.getPerson();
  }
  get personId(): number {
    return this._personId;
  }

  @Output() messageEmitter = new EventEmitter<AlertMessage>();
  @Output() savedPersonEmitter = new EventEmitter<number>();

  isLoading: boolean = false;
  person: PersonDetail = new PersonDetail();
  hasAddress: boolean = false;

  constructor(private service: ProductServices) {

  }

  getPerson() {
    if (this.personId == 0){
      this.hasAddress = false;
      this.person = new PersonDetail();
      return;
    }

    this.isLoading = true;

    this.service.getPersonById(this.personId)
      .subscribe({
        next: person => {
          this.person = person
          this.hasAddress = this.person.hasAddress();
        },
        error: error => this.messageEmitter.emit(new AlertMessage(error.error.Errors.join("\r\n"), "warning")),
        complete: () => this.isLoading = false
      })
  }

  addPhoneNumber() {
    this.person.phoneNumbers.push(new PhoneNumbers());
  }

  togglePrimaryPhoneNumber(phoneNo: PhoneNumbers) {
    this.person.phoneNumbers.forEach(x => x.isPrimary = false);
    phoneNo.isPrimary = true;
  }

  clearAddress(){
    this.hasAddress = false;
    this.person.lineOne = this.person.lineTwo = this.person.city = this.person.country = this.person.postCode = "";
  }

  removePhoneNo(phoneNo: PhoneNumbers){
    this.person.phoneNumbers = this.person.phoneNumbers.filter(x => x != phoneNo);
  }

  savePerson() {
    if (this.person.phoneNumbers?.length === 1) {
      //if person only has one number, it is the primary number
      this.person.phoneNumbers[0].isPrimary = true;
    }

    const errors = this.person.validate();

    if (errors.length > 0){
      this.messageEmitter.emit(new AlertMessage(errors.join(".\r\n"), "warning"))
      return;
    }

    this.isLoading = true;
    this.service.updatePerson(this.person)
      .subscribe({
        next: personId => {
          this.personId = personId;
          this.savedPersonEmitter.emit(personId);
          this.messageEmitter.emit(new AlertMessage("Person was successfully saved", "success"))
          this.getPerson();
        },
        error: error => {
          this.messageEmitter.emit(new AlertMessage(error.error.Errors.join("\r\n"), "warning"))
        },
        complete: () => this.isLoading = false
      });
  }
}
