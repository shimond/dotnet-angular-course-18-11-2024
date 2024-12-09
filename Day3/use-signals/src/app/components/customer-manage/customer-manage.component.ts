import { Component, inject } from '@angular/core';
import { CustomerStore } from './store/customer-manage.store';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, map } from 'rxjs';

@Component({
  selector: 'app-customer-manage',
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './customer-manage.component.html',
  styleUrl: './customer-manage.component.scss'
})
export class CustomerManageComponent {
  store = inject(CustomerStore);
  searchControl = new FormControl<string>('');
  constructor() {
   const searchObservableWithDebounce = this.searchControl.valueChanges.pipe(debounceTime(300),
    map(x=> x || '')
  );  

   this.store.setKeyword(searchObservableWithDebounce);
  }
}
