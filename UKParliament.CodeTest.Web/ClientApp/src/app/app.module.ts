import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { PeopleDashboardComponent } from './people-dashboard/people-dashboard.component';
import {PeopleSearchComponent} from "./people-dashboard/people-search/people-search.component";
import {PersonDetailComponent} from "./people-dashboard/people-detail/person-detail.component";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {TrueFalseSelector} from "./components/true-false-selector/true-false-selector.component";
import {ProductServices} from "./services/product.services";
import {Pagination} from "./components/pagination/pagination.component";
import {Loading} from "./components/loading/loading.component";

@NgModule({
  declarations: [
    AppComponent,
    PeopleDashboardComponent,
    PeopleSearchComponent,
    PersonDetailComponent,
    TrueFalseSelector,
    Pagination,
    Loading
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: PeopleDashboardComponent, pathMatch: 'full' }
    ]),
    NgbModule
  ],
  providers: [ProductServices],
  bootstrap: [AppComponent]
})
export class AppModule { }
