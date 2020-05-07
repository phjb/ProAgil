import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventsFilters: Evento[];
  events: Evento[];
  imageWidth = 50;
  imageMargin = 2;
  showImage = false;
  titleShowImage = 'fa fa-eye';

  modalRef: BsModalRef;

  constructor(private eventoService: EventoService, private modalService: BsModalService) { }

  listingFilter: string;
  get listFilter(): string {
    return this.listingFilter;
  }

  set listFilter(value: string) {
    this.listingFilter = value;
    this.eventsFilters = this.listFilter ? this.filterEvents(this.listFilter) : this.events;

  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit(): void {
    this.getEvents();

  }

  changeImage(): void {
    this.showImage = this.showImage ? false : true;
    this.titleShowImage = this.showImage ? 'fa fa-eye-slash' : 'fa fa-eye';
  }

  getEvents(): void {
    this.eventoService.getEvento().subscribe(
      (eventos: Evento[]) => {
        this.events = eventos;
        console.log(eventos);
        this.eventsFilters = this.eventsFilters ? this.eventsFilters : eventos;

      },
      error => {
        console.log(error);
      });
  }

  filterEvents(filtersBy: string): Evento[] {
    filtersBy = filtersBy.toLocaleLowerCase();
    return this.events.filter(
      ev => ev.tema.toLocaleLowerCase().indexOf(filtersBy) !== -1
        || ev.local.toLocaleLowerCase().indexOf(filtersBy) !== -1
        || ev.lotes.find(l => l.nome.toLocaleLowerCase().indexOf(filtersBy) !== -1)
    );
  }


}
