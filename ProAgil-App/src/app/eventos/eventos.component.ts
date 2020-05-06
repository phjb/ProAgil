import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventsFilters: any = [];
  events: any = [];
  imageWidth = 50;
  imageMargin = 2;
  showImage = false;
titleShowImage = 'Mostrar Imagem';

  listingFilter: string;
  get listFilter(): string {
    return this.listingFilter;
  }
  set listFilter(value: string) {
    this.listingFilter = value;
    this.eventsFilters = this.listFilter ? this.filterEvents(this.listFilter) : this.events;
  }

  url = 'http://localhost:5000/api/eventos/';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEvents();

  }

  changeImage(): void {
    this.showImage = this.showImage ? false : true;
    this.titleShowImage = this.showImage ? 'Ocultar Imagem' : 'Mostrar Imagem';
  }

  getEvents(): void {
    this.http.get(this.url).subscribe(
      response => {
        this.events = response;
        console.log(response);
        this.eventsFilters = !this.eventsFilters.length ? response : this.eventsFilters;
      },
      error => {
        console.log(error);
      });
  }

  filterEvents(filtersBy: string): any {
    filtersBy = filtersBy.toLocaleLowerCase();
    return this.events.filter(
      ev => ev.tema.toLocaleLowerCase().indexOf(filtersBy) !== -1
         || ev.local.toLocaleLowerCase().indexOf(filtersBy) !== -1
         || ev.lote.toLocaleLowerCase().indexOf(filtersBy) !== -1
    );
  }

}
