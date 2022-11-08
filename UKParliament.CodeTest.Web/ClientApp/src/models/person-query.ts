export class PersonQuery {
  name: string|null = null;
  ageFrom: number|null = null;
  ageTo: number|null = null;
  email: string|null = null;
  enabled: boolean|null = null;
  skip: number = 0;
  take: number = 2;

  validate(): string[] {
    const errors: string[] = [];

    if (this.ageTo != null && this.ageFrom != null && this.ageTo < this.ageFrom){
      //if age from is more than age to, flip them around
      [this.ageTo, this.ageFrom] = [this.ageFrom, this.ageTo];
    }

    if ((this.ageFrom != null && this.ageFrom < 0) || (this.ageTo != null && this.ageTo < 0)){
      errors.push("Age must be atleast than 0");
    }

    return errors;
  }
}
