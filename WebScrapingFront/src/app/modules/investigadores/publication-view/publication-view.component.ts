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

  exportExcel() {
    var arrayToExport = [];
    var objectToExport: any = {};
    objectToExport['Publicación'] = this.data.element.title;
    if(this.indicators.citaRecibida) {
      if(this.indicators.citaRecibida.scopusCount) {
        objectToExport['Citas recibidas Scopus'] = this.indicators.citaRecibida.scopusCount;
      }
      if(this.indicators.citaRecibida.webScienceCount) {
        objectToExport['Citas recibidas Web of Science'] = this.indicators.citaRecibida.webScienceCount;
      }
      if(this.indicators.citaRecibida.dimensionsCount) {
        objectToExport['Citas recibidas Dimensions'] = this.indicators.citaRecibida.dimensionsCount;
      }
    }
    if(this.indicators.journalImpactFactor) {
      objectToExport['Journal Impact Factor'] = 'Año: ' + this.indicators.journalImpactFactor.year +
                                                '\nImpacto de la revista: ' + this.indicators.journalImpactFactor.magazineImpact +
                                                '\nImpacto sin autocitas: ' + this.indicators.journalImpactFactor.noAutoImpact +
                                                '\nCuartil mayor: ' + this.indicators.journalImpactFactor.majorQuartil;
      if(this.indicators.journalImpactFactor.journalImpactFactorAreas) {
        objectToExport['Journal Impact Factor Áreas'] = this._areasToText(this.indicators.journalImpactFactor.journalImpactFactorAreas);
      }
    }
    if(this.indicators.journalCitationIndicator) {
      objectToExport['Journal Citation Indicator'] = 'Año: ' + this.indicators.journalCitationIndicator.year +
                                                '\nJCI de la revista: ' + this.indicators.journalCitationIndicator.magazineJCI                                                +
                                                '\nCuartil mayor: ' + this.indicators.journalCitationIndicator.majorQuartil;
      if(this.indicators.journalCitationIndicator.journalCitationIndicatorAreas) {
        objectToExport['Journal Citation Indicator Áreas'] = this._areasToText(this.indicators.journalCitationIndicator.journalCitationIndicatorAreas);
      }
    }
    if(this.indicators.scImagoJournalRank) {
      objectToExport['SCImago Journal Rank'] = 'Año: ' + this.indicators.scImagoJournalRank.year +
                                                '\nImpacto SJR de la revista: ' + this.indicators.scImagoJournalRank.sjrImpactMagazine +
                                                '\nCuartil mayor: ' + this.indicators.scImagoJournalRank.majorQuartil;
      if(this.indicators.scImagoJournalRank.scImagoJournalRankAreas) {
        objectToExport['SCImago Journal Rank Áreas'] = this._areasToText(this.indicators.scImagoJournalRank.scImagoJournalRankAreas);
      }
    }
    if(this.indicators.scopusCitescore) {
      objectToExport['Scopus CiteScore'] = 'Año: ' + this.indicators.scopusCitescore.year +
                                                '\nCiteScore de la revista: ' + this.indicators.scopusCitescore.magazineCitescore;
      if(this.indicators.scopusCitescore.scopusCitescoreAreas) {
        objectToExport['Scopus CiteScore Áreas'] = this._areasToText2(this.indicators.scopusCitescore.scopusCitescoreAreas);
      }
    }
    if(this.indicators.dimensions) {
      objectToExport['Dimensions'] = 'Field Citation Ratio: ' + this.indicators.dimensions.fieldCitationRatio
    }
    if(this.indicators.dialnetRevista) {
      objectToExport['Dialnet Revistas'] = 'Año: ' + this.indicators.dialnetRevista.year +
                                                '\nImpacto de la revista: ' + this.indicators.dialnetRevista.magazineImpact +
                                                '\nÁmbito: ' + this.indicators.dialnetRevista.ambit +
                                                '\nCuartil: ' + this.indicators.dialnetRevista.quartil +
                                                '\nPosición en el ámbito: ' + this.indicators.dialnetRevista.position;
    }

    arrayToExport.push(objectToExport)

    this.excelExportService.exportAsExcelFile(arrayToExport, 'IndicadoresPublicacion-' + this.data.element.id);
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
