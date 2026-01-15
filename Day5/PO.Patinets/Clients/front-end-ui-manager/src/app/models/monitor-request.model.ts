
export interface MonitorRequest {
  patientId: number;
  fever: number;
}
 
export interface MonitorStatus {
    patientId: number;
    fever: number;
    date: Date;
}

