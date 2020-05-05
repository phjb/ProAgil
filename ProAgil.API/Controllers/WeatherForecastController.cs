using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.API.Data;
using ProAgil.API.Model;

namespace ProAgil.API.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class WeatherForecastController : ControllerBase {
        public DataContext _context { get; }

        public WeatherForecastController (DataContext context) {
            this._context = context;

        }

        [HttpGet]
        public async Task<ActionResult> Get () {
            try {
                return Ok (await _context.Eventos.ToListAsync ());
            } catch (System.Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }

        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Evento>> Get (int id) {

            return await _context.Eventos.FirstOrDefaultAsync (x => x.EventoId == id);
        }
    }
}