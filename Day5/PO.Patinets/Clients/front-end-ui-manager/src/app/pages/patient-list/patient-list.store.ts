import { computed, inject } from "@angular/core";
import { PatientModel } from "../../models/patient.model";
import { patchState, signalStore, withComputed, withHooks, withMethods, withState } from '@ngrx/signals';
import { PatientApiService } from "../../services/patient-api.service";
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { catchError, of, switchMap, tap } from "rxjs";
import { MonitorNotificationService } from "../../services/monitor-notification.service";
import { MonitorStatus } from "../../models/monitor-request.model";
import { SnackbarService } from "../../services/snackbar.service";
export interface PatientListState {
    allPatients: PatientModel[];
    monitorStatus: MonitorStatus[];
    isBusy: boolean;
    selectedPatientId: number | null;
}

const initialState: PatientListState = {
    allPatients: [],
    monitorStatus: [],
    isBusy: false,
    selectedPatientId: null,
}

export const patientListStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withMethods((state,
        snackbarService = inject(SnackbarService),
        notificationService = inject(MonitorNotificationService), apiService = inject(PatientApiService)) => ({
            init: rxMethod<void>((_) => _.pipe(
                tap(() => notificationService.startConnection()),
                tap(() => patchState(state, { isBusy: true })),
                switchMap(() => apiService.getPatients()),
                catchError(() => {
                    snackbarService.openErrorSnackBar('לא ניתן לטעון את המידע', 'אישור', 5000)
                    return of([])
                }
                ),
                tap(allPatients => patchState(state, { allPatients })),
                tap(() => patchState(state, { isBusy: false })),
            )),
            setCurrentStatus: rxMethod<MonitorStatus>((status) => status.pipe(
                tap(status => patchState(state, { monitorStatus: [status, ...state.monitorStatus()] }))
            )),
            selectPatient: rxMethod<number>((id) => id.pipe(
                tap(() => patchState(state, { monitorStatus: [] })),
                tap(() => notificationService.unsubscribeFromPatientMonitor(state.selectedPatientId())),
                tap(id => patchState(state, { selectedPatientId: id })),
                switchMap(id => notificationService.subscribeToPatientMonitor(id))
            ))
        })),
    withComputed((state) => ({
        selectedPatient: computed(() => state.selectedPatientId() ? state.allPatients().find(p => p.id === state.selectedPatientId()) : null)
    })),
    withHooks((store, monitor = inject(MonitorNotificationService)) => ({
        onInit() {
            store.init();
            store.setCurrentStatus(monitor.monitorChanged$);
        }
    })));