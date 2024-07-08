import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { WebScrapingService } from '../../services/web-scraping.service';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { BindInvestigatorComponent } from './bind-investigator/bind-investigator.component';
import { Observable, debounceTime, map, startWith } from 'rxjs';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { InvestigatorViewComponent } from './investigator-view/investigator-view.component';

@Component({
  selector: 'investigadores',
  templateUrl: './investigadores.component.html',
  styleUrls: ['./investigadores.component.scss']
})
export class InvestigadoresComponent implements OnInit, AfterViewInit{

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  investigadoresDatasource = [];
  totalInvestigadores = 0;

  investigadorCtrl = new FormControl('a');

  investigadorNamesCtrl = new FormControl<any>(null);
  filteredInvestigadorNames!: Observable<any[]>;
  allInvestigadorNames: any[] = [];
  investigadorNames: any[] = [];

  separatorKeysCodes: number[] = [ENTER, COMMA];
  
  @ViewChild('investigadorInput') investigadorInput!: ElementRef<HTMLInputElement>;

  isLoading = true;

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
    this.investigadorCtrl.valueChanges.pipe(debounceTime(500)).subscribe(
      () => {
        this.updateTable();
      }
    )
  }

  ngAfterViewInit(): void {
    this.updateTable();
    this.loadLists();
    this.paginator.page.subscribe(
      () => {
        this.updateTable();
      }
    );
  }

  updateTable() {
    this.isLoading = true;
    var idsInvestigadoresRelated: number[] = []
    this.investigadorNames.forEach(element => {
      idsInvestigadoresRelated.push(element.id);
    });
    this.webScrapingService.getInvestigadores({
      Nombre: this.investigadorCtrl.value ? this.investigadorCtrl.value : '',
      InvestigatorIds: idsInvestigadoresRelated,
      Page: this.paginator.pageIndex,
      Limit: this.paginator.pageSize
    }).subscribe(
      res => {
        this.totalInvestigadores = res.total;
        this.investigadoresDatasource = res.items;
        this.isLoading = false;
      }, 
      err => {
        this.isLoading = false;
      }
    );
  }

  loadLists() {
    this.webScrapingService.getInvestigadoresList().subscribe(
      res => {
        this.allInvestigadorNames = res;
        this.filteredInvestigadorNames = this.investigadorNamesCtrl.valueChanges.pipe(
          startWith(''),
          map((value: any) => {
            const fullName = typeof value === 'string' ? value : value?.fullname;
            return fullName ? this._filterInvestigador(fullName as string) : this.allInvestigadorNames.slice();
          }),
        );
      }
    );
  }

  private _filterInvestigador(value: any): any[] {
    var filterValue = value.toLowerCase();
    return this.allInvestigadorNames.filter(investigador => investigador.fullName.toLowerCase().includes(filterValue));
  };

  removeInvestigador(investigador: any): void {
    const index = this.investigadorNames.indexOf(investigador);
    if (index >= 0) {
      this.investigadorNames.splice(index, 1);
    }
    this.allInvestigadorNames.push(investigador)
    this.investigadorNamesCtrl.reset();
    this.updateTable();
  }

  selectedInvestigador(event: MatAutocompleteSelectedEvent): void {
    this.investigadorNames.push(event.option.value);
    this.allInvestigadorNames.splice(this.allInvestigadorNames.indexOf(event.option.value), 1)
    this.investigadorInput.nativeElement.value = '';
    this.investigadorNamesCtrl.reset();
    this.updateTable();
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

  openView(element: any) {
    const dialogRef = this.dialog.open(InvestigatorViewComponent, {
      maxHeight: '99vh',
      width: '99%',
      maxWidth: '99%',
      data: {
        element
      },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {  
      if (result) {
        this.updateTable();
      }
    });
  }

  openInvestigator(element: any) {
    window.open("https://investigacion.ubu.es/investigadores/" + element.idInvestigador + "/detalle");
  }


}

