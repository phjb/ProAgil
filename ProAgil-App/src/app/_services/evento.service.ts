import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  baseUrl = 'http://localhost:5000/api/eventos';
  constructor(private http: HttpClient) { }

  getEvento(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseUrl);
  }

  getEventoByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseUrl}/getByTema/${tema}`);
  }

  getEventoById(id: number): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseUrl}/${id}`);
  }

  postEvento(evento: Evento) {
    return this.http.post(this.baseUrl, evento);
  }

  putEvento(evento: Evento, id: number) {
    return this.http.put(`${this.baseUrl}/${id}`, evento);
  }

  removeEvento(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
