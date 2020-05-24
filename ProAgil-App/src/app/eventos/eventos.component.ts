import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';

import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { Observable } from 'rxjs';
defineLocale('pt-br', ptBrLocale);


@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventsFilters: Evento[];
  events: Evento[];
  event: Evento;
  imageWidth = 50;
  imageMargin = 2;
  showImage = false;
  titleShowImage = 'fa fa-eye';

  registerForm: FormGroup;
  actionRegisterForm = 'post';
  idEvento = 0;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService) {
    this.localeService.use('pt-br');
  }

  listingFilter: string;
  get listFilter(): string {
    return this.listingFilter;
  }

  set listFilter(value: string) {
    this.listingFilter = value;
    this.eventsFilters = this.listFilter ? this.filterEvents(this.listFilter) : this.events;

  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  openEditModal(template: any, evento: Evento) {
    console.log(evento);
    this.actionRegisterForm = 'put';
    this.registerForm.reset();
    this.registerForm.patchValue(evento);
    this.idEvento = evento.id;
    template.show();
  }



  ngOnInit(): void {
    this.validation();
    this.getEvents();

  }

  changeImage(): void {
    this.showImage = this.showImage ? false : true;
    this.titleShowImage = this.showImage ? 'fa fa-eye-slash' : 'fa fa-eye';
  }
  validation() {
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', Validators.required]
    });
    // this.registerForm = new FormGroup({
    //   tema: new FormControl('',
    //     [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
    //   local: new FormControl('', Validators.required),
    //   dataEvento: new FormControl('', Validators.required),
    //   qtdPessoas: new FormControl('',
    //     [Validators.required, Validators.max(120000)]),
    //   telefone: new FormControl('', Validators.required),
    //   email: new FormControl('', [Validators.required, Validators.email]),
    //   imagemUrl: new FormControl('', Validators.required)
    // });
  }

  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {
      this.event = Object.assign({}, this.registerForm.value);
      console.log(this.actionRegisterForm);
      if (this.actionRegisterForm === 'post') {
        this.eventoService.postEvento(this.event).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEvents();
          }, error => {
            console.log(error);
          }
        );
      } else {
        this.event.id = this.idEvento;
        console.log(this.event);
        this.eventoService.putEvento(this.event, this.idEvento).subscribe(
          () => {
            template.hide();
            this.getEvents();
          }, error => {
            console.log(error);
          }
        );
      }

    }
  }

  removeEvent(evento: Evento) {
    this.eventoService.removeEvento(evento.id).subscribe(
      (e: Evento) => { }
    );
    this.getEvents();
  }

  getEvents(): void {
    setTimeout(() => {
      this.eventoService.getEvento().subscribe(
        (eventos: Evento[]) => {
          this.events = eventos;
          console.log(eventos);
          this.eventsFilters = eventos;
        },
        error => {
          console.log(error);
        });
    }, 1000);

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
