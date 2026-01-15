import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
    providedIn: 'root'
})
export class SnackbarService {
    snackBar = inject(MatSnackBar);

    //create method to open snackbar with error
    openErrorSnackBar(message: string, action: string, duration: number = 2000) {
        this.snackBar.open(message, action, {
            duration,
            panelClass: ['error-snackbar']
        });
    }


    openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
            duration: 2000,
        });
    }
}


    


