import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { MonitorRequest, MonitorStatus } from '../models/monitor-request.model';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MonitorNotificationService {

    private monitorChanged = new Subject<MonitorStatus>();
    monitorChanged$ = this.monitorChanged.asObservable();

    private hubConnection!: signalR.HubConnection;
    private readonly hubUrl = 'http://localhost:1147/api/monitor/monitorhub'; // Replace with your backend URL

    constructor() { }

    // Initialize the connection
    public startConnection(): void {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl) // Add authentication token here if needed
            .withAutomaticReconnect() // Automatically reconnect on disconnection
            .build();

        this.hubConnection
            .start()
            .then(() => {
                this.registerOnPatientMonitorUpdate();
                console.log('SignalR connection established.');
            })
            .catch((err) => console.error('Error establishing SignalR connection:', err));
    }

    // Stop the connection
    public stopConnection(): void {
        this.hubConnection
            .stop()
            .then(() => console.log('SignalR connection stopped.'))
            .catch((err) => console.error('Error stopping SignalR connection:', err));
    }


    private registerOnPatientMonitorUpdate(): void {
        this.hubConnection.on('ReceivePatientData', (data: MonitorRequest) => {
            console.log('Received patient data:', data);
            this.monitorChanged.next({
                patientId: data.patientId,
                fever: data.fever,
                date: new Date()
            });
        });
    }

    public subscribeToPatientMonitor(patientId: number): Promise<void> {
        return this.hubConnection
            .invoke('SubscibeToPatientMonitor', patientId) // invoke the server method
            .then(() => console.log(`Subscribed to patient:${patientId}`))
            .catch((err) =>
                console.error(`Error subscribing to patient:${patientId}:`, err)
            );
    }

    // Unsubscribe from a patient monitor group
    public unsubscribeFromPatientMonitor(patientId: number | null): Promise<void> {
        if (!patientId) return Promise.resolve();
        return this.hubConnection
            .invoke('UnsubscribeFromPatientMonitor', patientId) // invoke the server method
            .then(() => console.log(`Unsubscribed from patient:${patientId}`))
            .catch((err) =>
                console.error(`Error unsubscribing from patient:${patientId}:`, err)
            );
    }
}
