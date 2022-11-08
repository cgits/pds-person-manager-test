import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class Pagination {
  @Input() take: number = 0;
  @Input() skip: number = 0;
  @Input() total: number = 0;

  @Output() skipEmitter: EventEmitter<number> = new EventEmitter<number>();

  constructor() {
  }

  getPageNumbers(): number[] {
    const maxPage =  Math.ceil(this.total / this.take);
    return Array.from({length: maxPage}, (v,k)=>k+1)
  }

  getCurrentPage(): number {
    //this is a slightly naive implementation, if users manually tinker with skip or take then this will give decimals; if this happens just fall back to lower page no
    return Math.floor((this.skip/this.take) + 1);
  }

  updatePage(pageNo: number) {
    this.skipEmitter.emit((pageNo - 1) * this.take);
  }
}
