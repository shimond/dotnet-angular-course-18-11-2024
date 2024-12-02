export interface Customer {
    id: number;
    name: string;
    gender: Gender;
    phone: string;
    email: string;
  }

  export enum Gender {
    Male = 'Male',
    Female = 'Female',
    Other = 'Other',
  }