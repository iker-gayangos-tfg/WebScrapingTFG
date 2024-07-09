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

  openIndicators(element: any) {
    const dialogRef = this.dialog.open(PublicationViewComponent, {
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

  openPublication(element: any) {
    window.open(element.url);
  }

  parseText(texto: string) {
    if(texto) {
      if(texto.length < 30){        
        return texto
      }else{
        return texto.substr(0, 30) + '...'
      }
    }else{
      return null
    }
  }

  parseText2(texto: string) {
    if(texto) {
      if(texto.length <= 9){        
        return texto
      }else{
        return texto.substr(0, 7) + '...'
      }
    }else{
      return null
    }
  }

  parseTitle(texto: string) {
    if(texto) {
      if(texto.length < 65){        
        return texto
      }else{
        return texto.substr(0, 65) + '...'
      }
    }else{
      return null
    }
  }

  exportExcel() {
    this.webScrapingService.getPublicacionesInvestigadorCompleto({
      idInvestigador: this.data.element.id
    }).subscribe(
      res => {
        var publicationsCompleto = res;
        var arrayToExport: any[] = [];
        publicationsCompleto.forEach(
          (element: any) => {
            var objectToExport: any = {'Publicación': '', 'Citas recibidas Scopus': '', 'Citas recibidas Web of Science': '', 'Citas recibidas Dimensions': '', 'Journal Impact Factor': '', 'Journal Impact Factor Áreas': '', 'Journal Citation Indicator': '', 'Journal Citation Indicator Áreas': '', 'SCImago Journal Rank': '', 'SCImago Journal Rank Áreas': '', 'Scopus CiteScore': '', 'Scopus CiteScore Áreas': '', 'Dimensions': '', 'Dialnet Revistas': ''};
            objectToExport['Publicación'] = element.title;
            if(element.citaRecibida) {
              if(element.citaRecibida.scopusCount) {
                objectToExport['Citas recibidas Scopus'] = element.citaRecibida.scopusCount;
              }
              if(element.citaRecibida.webScienceCount) {
                objectToExport['Citas recibidas Web of Science'] = element.citaRecibida.webScienceCount;
              }
              if(element.citaRecibida.dimensionsCount) {
                objectToExport['Citas recibidas Dimensions'] = element.citaRecibida.dimensionsCount;
              }
            }
            if(element.journalImpactFactor) {
              objectToExport['Journal Impact Factor'] = 'Año: ' + element.journalImpactFactor.year +
                                                        '\nImpacto de la revista: ' + element.journalImpactFactor.magazineImpact +
                                                        '\nImpacto sin autocitas: ' + element.journalImpactFactor.noAutoImpact +
                                                        '\nCuartil mayor: ' + element.journalImpactFactor.majorQuartil;
              if(element.journalImpactFactor.journalImpactFactorAreas) {
                objectToExport['Journal Impact Factor Áreas'] = this._areasToText(element.journalImpactFactor.journalImpactFactorAreas);
              }
            }
            if(element.journalCitationIndicator) {
              objectToExport['Journal Citation Indicator'] = 'Año: ' + element.journalCitationIndicator.year +
                                                        '\nJCI de la revista: ' + element.journalCitationIndicator.magazineJCI                                                +
                                                        '\nCuartil mayor: ' + element.journalCitationIndicator.majorQuartil;
              if(element.journalCitationIndicator.journalCitationIndicatorAreas) {
                objectToExport['Journal Citation Indicator Áreas'] = this._areasToText(element.journalCitationIndicator.journalCitationIndicatorAreas);
              }
            }
            if(element.scImagoJournalRank) {
              objectToExport['SCImago Journal Rank'] = 'Año: ' + element.scImagoJournalRank.year +
                                                        '\nImpacto SJR de la revista: ' + element.scImagoJournalRank.sjrImpactMagazine +
                                                        '\nCuartil mayor: ' + element.scImagoJournalRank.majorQuartil;
              if(element.scImagoJournalRank.scImagoJournalRankAreas) {
                objectToExport['SCImago Journal Rank Áreas'] = this._areasToText(element.scImagoJournalRank.scImagoJournalRankAreas);
              }
            }
            if(element.scopusCitescore) {
              objectToExport['Scopus CiteScore'] = 'Año: ' + element.scopusCitescore.year +
                                                        '\nCiteScore de la revista: ' + element.scopusCitescore.magazineCitescore;
              if(element.scopusCitescore.scopusCitescoreAreas) {
                objectToExport['Scopus CiteScore Áreas'] = this._areasToText2(element.scopusCitescore.scopusCitescoreAreas);
              }
            }
            if(element.dimensions) {
              objectToExport['Dimensions'] = 'Field Citation Ratio: ' + element.dimensions.fieldCitationRatio
            }
            if(element.dialnetRevista) {
              objectToExport['Dialnet Revistas'] = 'Año: ' + element.dialnetRevista.year +
                                                        '\nImpacto de la revista: ' + element.dialnetRevista.magazineImpact +
                                                        '\nÁmbito: ' + element.dialnetRevista.ambit +
                                                        '\nCuartil: ' + element.dialnetRevista.quartil +
                                                        '\nPosición en el ámbito: ' + element.dialnetRevista.position;
            }
            arrayToExport.push(objectToExport);
          }
        );
        this.excelExportService.exportAsExcelFile(arrayToExport, 'IndicadoresPublicaciones-' + this.data.element.id);
      }
    )
  }

  private _areasToText(arrayAreas: any) {
    var stringToReturn = '';
    arrayAreas.forEach(
      (element: any) => {
        stringToReturn = stringToReturn + '-Área: ' + element.area + '\n    Cuartil: ' + element.quartil + '\n    Posición en el área: ' + element.position + '\n'
      }
    );
    return stringToReturn;
  }
  private _areasToText2(arrayAreas: any) {
    var stringToReturn = '';
    arrayAreas.forEach(
      (element: any) => {
        stringToReturn = stringToReturn + '-Área: ' + element.area + '\n    Percentil: ' + element.percentil + '\n'
      }
    );
    return stringToReturn;
  }

  closeModal(dialogResult?: any) {
    this.dialogRef.close(dialogResult);
  }

}
