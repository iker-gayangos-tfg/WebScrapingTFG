import { Component, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { WebScrapingService } from '../../../services/web-scraping.service';
import { ExcelExportService } from '../../../services/excel-export.service';

@Component({
  selector: 'publication-view',
  templateUrl: './publication-view.component.html',
  styleUrls: ['./publication-view.component.scss']
})
export class PublicationViewComponent {

  indicators: any[any] = [];

  constructor(
    private dialogRef: MatDialogRef<PublicationViewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private webScrapingService: WebScrapingService,
    private excelExportService: ExcelExportService
  ) {

  }
  

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {
    this.updateTable();
  }

  updateTable() {
    this.webScrapingService.getIndicadoresPublicacion({
      idPublicacion: this.data.element.id
    }).subscribe(
      res => {
        this.indicators = res;
      }
    )
  }

  openLink(element: any) {
    if(element) {
      window.open(element);
    }
  }
  closeModal(dialogResult?: any) {
    this.dialogRef.close(dialogResult);
  }

}
