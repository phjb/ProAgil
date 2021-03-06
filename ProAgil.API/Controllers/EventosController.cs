using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.Dtos;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Context;
using ProAgil.Repository.IRepositories;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        private readonly ProAgilContext _context;
        private readonly IMapper _mapper;

        public EventosController(IProAgilRepository repo, ProAgilContext context, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var eventos = await _repo.GetAllEventosAsync(true);
                var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            try
            {
                var evento = await _repo.GetEventosAsyncById(id, true);
                var result = _mapper.Map<EventoDto>(evento);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

        }

        [HttpGet("getByTema/{tema}")]
        public async Task<ActionResult<Evento[]>> Get(string tema)
        {
            try
            {
                var evento = await _repo.GetAllEventosAsyncByTema(tema, true);
                var result = _mapper.Map<IEnumerable<EventoDto>>(evento);
                return Ok(result);
            }
            catch (System.Exception ex )
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<Evento>> Post(EventoDto evento)
        {
            try
            {
                var result = _mapper.Map<Evento>(evento);
                _repo.Add(result);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
            }

            return BadRequest();

        }

        [HttpPut("{EventoId}")]
        public async Task<ActionResult<Evento>> Put(int EventoId, EventoDto evento)
        {
            try
            {

                var _evento = await _repo.GetEventosAsyncById(EventoId, false);
                if (_evento == null)
                {
                    return NotFound();
                }

                _mapper.Map(_evento,evento);

                // _context.Update(evento);
                // _context.SaveChanges();

                _repo.Update(_evento);
                if (await _repo.SaveChangesAsync())
                {
                    //return Ok(await _repo.GetAllEventosAsync(true));
                    return Created($"/api/evento/{evento.Id}", evento);
                }

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
            }

            return BadRequest();

        }

        [HttpDelete("{EventoId}")]
        public async Task<ActionResult<Evento>> Delete(int EventoId)
        {
            try
            {
                var evento = await _repo.GetEventosAsyncById(EventoId, false);
                if (evento == null)
                {
                    return NotFound();
                }
                _repo.Delete(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }

            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest();

        }

    }
}