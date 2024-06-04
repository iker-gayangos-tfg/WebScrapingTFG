import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { WebScrapingService } from '../../services/web-scraping.service';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { BindInvestigatorComponent } from './bind-investigator/bind-investigator.component';

@Component({
  selector: 'investigadores',
  templateUrl: './investigadores.component.html',
  styleUrls: ['./investigadores.component.scss']
})
export class InvestigadoresComponent implements OnInit, AfterViewInit{

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  investigadoresDatasource = [];
  totalInvestigadores = 0;

  investigadorCtrl = new FormControl(null);

  public selection: SelectionModel<any>;

  constructor(
    private webScrapingService: WebScrapingService,
    private dialog: MatDialog
  )
  {
    const initialSelection: any[] = [];
    const allowMultiSelect = true;
    this.selection = new SelectionModel<any>(allowMultiSelect, initialSelection);
  }
  ngOnInit(): void {
    this.investigadorCtrl.valueChanges.subscribe(
      () => {
        this.updateTable();
      }
    )
  }

  ngAfterViewInit(): void {
    this.updateTable()
  }

  updateTable() {
    this.webScrapingService.getInvestigadores({
      Nombre: this.investigadorCtrl.value ? this.investigadorCtrl.value : '',
      InvestigatorIds: [1, 4],
      Page: this.paginator.pageIndex,
      Limit: this.paginator.pageSize
    }).subscribe(
      res => {
        console.log(res);
        this.totalInvestigadores = res.total;
        this.investigadoresDatasource = res.items;
      }
    );
  }

  bindInvestigators() {
    const dialogRef = this.dialog.open(BindInvestigatorComponent, {
      width: '1200px',
      data: this.selection.selected,
      disableClose: true
    });
    dialogRef.afterClosed().subscribe((result: any) => {  
      if (result) {
        this.selection.clear()
        this.updateTable();
      }
    });
  }


}

