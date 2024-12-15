import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { PatientModel } from '../models/patient.model';

@Injectable({
    providedIn: 'root'
})
export class PatientApiService {
    #baseUrl = 'https://localhost:7257';
    #http = inject(HttpClient);

    getPatients() {
        return this.#http.get<PatientModel[]>(`${this.#baseUrl}/api/catalog/patients`);
    }


}
