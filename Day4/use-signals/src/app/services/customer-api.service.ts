import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Customer, Gender } from '../models/customer.model';

@Injectable({
  providedIn: 'root',
})
export class CustomerApiService {
  private customers: Customer[] = [
    {
      id: 1,
      name: 'Alice Johnson',
      gender: Gender.Female,
      phone: '123-456-7890',
      email: 'alice@example.com',
    },
    {
      id: 2,
      name: 'Bob Smith',
      gender: Gender.Male,
      phone: '987-654-3210',
      email: 'bob@example.com',
    },
    {
      id: 3,
      name: 'Charlie Brown',
      gender: Gender.Other,
      phone: '555-666-7777',
      email: 'charlie@example.com',
    },
    {
      id: 4,
      name: 'Diana Prince',
      gender: Gender.Female,
      phone: '444-333-2222',
      email: 'diana@example.com',
    },
  ];

  getCustomers(): Observable<Customer[]> {
    return of(this.customers);
  }
}
