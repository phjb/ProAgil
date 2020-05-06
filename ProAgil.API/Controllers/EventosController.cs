using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Entities;
using ProAgil.Repository.IRepositories;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public EventosController(IProAgilRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _repo.GetAllEventosAsync(true));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

        }

        [HttpGet("{Eventoid}")]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            try
            {
                return Ok(await _repo.GetEventosAsyncById(id, true));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

        }

        [HttpGet("getByTema{tema}")]
        public async Task<ActionResult<Evento[]>> Get(string tema)
        {
            try
            {
                return await _repo.GetAllEventosAsyncByTema(tema, true);
            }
            catch (System.Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

        }

        [HttpPost]
        public async Task<ActionResult<Evento>> Post(Evento evento)
        {
            try
            {
                _repo.Add(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }

            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest();

        }

        [HttpPut]
        public async Task<ActionResult<Evento>> Put(int EventoId, Evento evento)
        {
            try
            {
                var _evento = await _repo.GetEventosAsyncById(EventoId, false);
                if (_evento == null)
                {
                    return NotFound();
                }
                _repo.Update(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }

            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

            return BadRequest();

        }

        [HttpDelete]
        public async Task<ActionResult<Evento>> Put(int EventoId)
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