import { Injectable } from '@angular/core';
import { MatSnackBarConfig, MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarNotifyService {

  constructor(private sb: MatSnackBar) { }

  openSnackBar(message: any, type: any, config?: MatSnackBarConfig) {
    const defaultConfig: MatSnackBarConfig = new MatSnackBarConfig();
    defaultConfig.panelClass = type;
    defaultConfig.duration = 3000;

    this.sb.open(message, '', { ...defaultConfig, ...config });
  }
}
