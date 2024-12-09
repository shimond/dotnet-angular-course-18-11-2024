import { inject, computed } from '@angular/core';
import {
    patchState,
    signalStore,
    withComputed,
    withHooks,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { switchMap, tap } from 'rxjs';
import { Customer } from '../../../models/customer.model';
import { CustomerApiService } from '../../../services/customer-api.service';

export interface CustomerState {
    readonly keyword: string;
    readonly customerList: Customer[];
    readonly selectedCustomer: Customer | null;
    readonly isLoading: boolean;
}

const initialState: CustomerState = {
    keyword: '',
    customerList: [],
    selectedCustomer: null,
    isLoading: false,
};

export const CustomerStore = signalStore(
    { providedIn: 'root', protectedState: false },
    withState(initialState),

    // Define computed properties
    withComputed((state) => ({
        filteredCustomers: computed(() =>
            state.customerList().filter((customer) =>
                customer.name.toLowerCase().includes(state.keyword().toLowerCase())
            )
        ),
    })),

    // Define methods
    withMethods((state, customerApiService = inject(CustomerApiService)) => ({
        setKeyword: rxMethod<string>((keyword$) =>
            keyword$.pipe(
                tap((keyword) => patchState(state, { keyword }))
            )
        ),
        loadCustomers: rxMethod<void>((trigger$) =>
            trigger$.pipe(
                tap(() => patchState(state, { isLoading: true })),
                switchMap(() => customerApiService.getCustomers()),
                tap((customers) =>
                    patchState(state, { customerList: customers, isLoading: false })
                )
            )
        ),

        selectCustomer: (customerID: number) => {
            patchState(state, { selectedCustomer: state.customerList().find((c) => c.id === customerID) });
        },
    })),

    // Lifecycle hooks
    withHooks((store) => ({
        onInit() {
            // Optionally, initialize by loading customers on store creation
            store.loadCustomers();
        },
    }))
);
