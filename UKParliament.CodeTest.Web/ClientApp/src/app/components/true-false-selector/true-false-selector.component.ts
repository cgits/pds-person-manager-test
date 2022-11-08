import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'true-false-selector',
  templateUrl: './true-false-selector.component.html',
  styleUrls: ['./true-false-selector.component.scss']
})
export class TrueFalseSelector {
  @Input() isSelected: boolean|null = null;
  @Output() isSelectedEmitter: EventEmitter<boolean|null> = new EventEmitter<boolean | null>();

  constructor() {
  }

  ChangeSelected(selection: boolean|null): void {
    this.isSelected = selection;
    this.isSelectedEmitter.emit(this.isSelected);
  }
}
