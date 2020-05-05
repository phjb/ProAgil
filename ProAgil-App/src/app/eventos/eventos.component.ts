import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventos: any = [];

  url = 'http://localhost:5000/api/eventos/';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  getEventos(): void {
    this.http.get(this.url).subscribe(
      response => {
        this.eventos = response;
      },
      error => {
        console.log(error);
      });
  }
}
