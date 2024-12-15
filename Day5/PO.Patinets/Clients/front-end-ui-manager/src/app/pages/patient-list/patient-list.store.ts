import { computed, inject } from "@angular/core";
import { PatientModel } from "../../models/patient.model";
import {patchState, signalStore, withComputed, withHooks, withMethods, withState}  from '@ngrx/signals';
import { PatientApiService } from "../../servcies/patient-api.service";
import {rxMethod} from '@ngrx/signals/rxjs-interop';
import { switchMap, tap } from "rxjs";
export interface PatientListState {
    allPatients: PatientModel[];
    isBusy: boolean;
    selectedPatientId: number | null;
}

const initialState: PatientListState = {
    allPatients: [],
    isBusy: false,
    selectedPatientId: null,
}

export const patientListStore = signalStore(
    { providedIn:'root'},
    withState(initialState),
    withMethods((state, apiService = inject(PatientApiService)) => ({
        init: rxMethod<void>((_)=> _.pipe(
            tap(()=> patchState(state, {isBusy: true})),
            switchMap(()=> apiService.getPatients()),
            tap(allPatients => patchState(state, {allPatients})),
            tap(()=> patchState(state, {isBusy: false})),
        )),
        selectPatient:rxMethod<number>((id)=> id.pipe(
            tap(id=> patchState(state, {selectedPatientId: id}))
        ))
    })),
    withComputed((state) => ({ 
        selectedPatient: computed(()=> state.selectedPatientId() ?  state.allPatients().find(p => p.id === state.selectedPatientId()) : null)
     })),
    withHooks((store) => ({
        onInit(){
            store.init();
        }
    })));