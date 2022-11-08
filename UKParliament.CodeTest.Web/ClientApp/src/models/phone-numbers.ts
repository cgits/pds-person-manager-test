export class PhoneNumbers {
  id: number = 0;
  number: string = "";
  description: string = "";
  isPrimary: boolean = false;

  validate(): string[] {
    const errors: string[] = [];

    if (!this.number){
      errors.push("Number must not be empty")
    }
    if (!this.description){
      errors.push("Number must have a description")
    }

    return errors;
  }
}
