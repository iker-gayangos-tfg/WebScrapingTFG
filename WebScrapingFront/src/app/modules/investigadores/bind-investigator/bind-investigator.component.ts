import { SelectionModel } from '@angular/cdk/collections';
import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { WebScrapingService } from '../../../services/web-scraping.service';
import { SnackbarNotifyService } from '../../snackbar-notify/snackbar-notify.service';

@Component({
  selector: 'bind-investigator',
  templateUrl: './bind-investigator.component.html',
  styleUrls: ['./bind-investigator.component.scss']
})
export class BindInvestigatorComponent {

  btnDisabled = true;

  public selection: SelectionModel<any>;

  public tableColumns: string[] = ['select', 'code', 'description', 'manufacturer', 'manufacturerCode', 'technicalLocationName'];

  investigators: any[] = [];

  constructor(
    private dialogRef: MatDialogRef<BindInvestigatorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private webScrapingService: WebScrapingService,
    private snackbar: SnackbarNotifyService,
  ) {
    this.investigators = data;
    const initialSelection: any[] = [];
    const allowMultiSelect = false;
    this.selection = new SelectionModel<any>(allowMultiSelect, initialSelection);
  }
  

  ngOnInit(): void {
    this.selection.changed.subscribe(
      () => {
        this.validateFields();
      }
    )
  }

  validateFields () {
    if(this.selection.selected.length == 1) {
      this.btnDisabled = false;
    } else {
      this.btnDisabled = true;
    }
  }

  public onSubmit() {
    var idInvestigatorSelected = this.selection.selected[0].id;
    var idsInvestigatorsToBind: any[] = [];
    this.investigators.forEach(
      element => {
        if(idInvestigatorSelected != element.id){
          idsInvestigatorsToBind.push(element.id)
        }
      }
    );
    this.webScrapingService.bindInvestigators({
      Id: idInvestigatorSelected,
      InvestigatorIds: idsInvestigatorsToBind
    }).subscribe(
      () => {
        this.snackbar.openSnackBar('Investigadores fusionados correctamente', 'sb-success');
        this.closeModal(true)
      },
      () => {
        this.snackbar.openSnackBar('Los investigadores no se han podido fusionadar', 'sb-error');
      }
    )
  }

  closeModal(dialogResult?: any) {
    this.dialogRef.close(dialogResult);
  }
}
