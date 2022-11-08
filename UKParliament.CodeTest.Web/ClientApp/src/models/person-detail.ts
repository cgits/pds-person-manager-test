import {PersonSummary} from "./person-summary";
import {PhoneNumbers} from "./phone-numbers";

export class PersonDetail extends PersonSummary {
  lineOne: string = "";
  lineTwo: string = "";
  city: string = "";
  country: string = "";
  postCode: string = "";
  phoneNumbers: PhoneNumbers[] = [];

  hasAddress = (): boolean => {
    return [this.lineOne, this.city, this.country, this.postCode].some(x => x)
  }

  validate = (): string[] => {
    let errors: string[] = [];

    if (!this.title) {
      errors.push("Person must have a title");
    }

    if (!this.name) {
      errors.push("Person must have a name");
    }

    if (!this.email || !/\S+@\S+\.\S+/.test(this.email)) {
      errors.push("Person must have a email");
    }

    if (!this.dateOfBirth || new Date(this.dateOfBirth).getTime() > new Date().getTime()) {
      errors.push("Person must have a valid date of birth");
    }

    if (this.hasAddress() && [this.lineOne, this.city, this.country, this.postCode].some(x => !x)) {
      errors.push("If a person has an address, their line one, city, country and postcode must be entered");
    }

    errors = errors.concat([...new Set(this.phoneNumbers.flatMap(x => x.validate()))]);

    if(this.phoneNumbers.length > 0 && this.phoneNumbers.filter(x => x.isPrimary).length != 1) {
      errors.push("If a person has phone numbers, they must have only one primary phone number.");
    }

    return errors;
  }
}
