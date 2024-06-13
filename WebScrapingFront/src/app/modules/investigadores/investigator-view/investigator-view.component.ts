import { Component, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { WebScrapingService } from '../../../services/web-scraping.service';
import { SnackbarNotifyService } from '../../snackbar-notify/snackbar-notify.service';
import { MatPaginator } from '@angular/material/paginator';
import { PublicationViewComponent } from '../publication-view/publication-view.component';
import { ExcelExportService } from '../../../services/excel-export.service';

@Component({
  selector: 'investigator-view',
  templateUrl: './investigator-view.component.html',
  styleUrls: ['./investigator-view.component.scss']
})
export class InvestigatorViewComponent {

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public tableColumns: string[] = ['title', 'magazine', 'book', 'bookCollection', 'editorial', 'ISSN', 'ISBN', 'year', 'volumen', 'number', 'pages', 'type', 'actions'];

  publications: any[] = [];
  totalPublications = 0;

  constructor(
    private dialogRef: MatDialogRef<InvestigatorViewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private webScrapingService: WebScrapingService,
    private excelExportService: ExcelExportService,
    private dialog: MatDialog
  ) {
    console.log(data)
  }
  

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {
    this.updateTable();
    this.paginator.page.subscribe(
      () => {
        this.updateTable();
      }
    );
  }

  updateTable() {
    this.webScrapingService.getPublicacionesInvestigador({
      IdInvestigador: this.data.element.id,
      Page: this.paginator.pageIndex,
      Limit: this.paginator.pageSize
    }).subscribe(
      res => {
        this.publications = res.items;
        this.totalPublications = res.total
      }
    )
  }
  openPublication(element: any) {
    window.open(element.url);
  }
}
