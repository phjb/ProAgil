using System.Linq;
using System.Threading.Tasks;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Context;
using ProAgil.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ProAgil.Repository.Repositories
{

    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /* * * * * * * * * * * GERAIS * * * * * * * * * * * * * * * */
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }


        #region  * * * * * * * * * * EVENTOS * * * * * * * * * * * * * * *
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante); // inclui entidade relacionada a entidade Palestrante Evento
            }
            query = query.AsNoTracking()
            .OrderBy(c => c.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes = false)
        {

            IQueryable<Evento> query = _context.Eventos
           .Include(c => c.Lotes)
           .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante); // inclui entidade relacionada a entidade Palestrante Evento
            }
            query = query.AsNoTracking()
            .Where(c => c.Tema.ToLower().Contains(tema.ToLower()))
            .OrderBy(c => c.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventosAsyncById(int EventoId, bool includePalestrantes = false)
        {

            IQueryable<Evento> query = _context.Eventos
             .Include(c => c.Lotes)
             .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante); // inclui entidade relacionada a entidade Palestrante Evento
            }
            query = query.AsNoTracking().Where(c => c.Id == EventoId).OrderByDescending(c => c.DataEvento);
            return await query.FirstOrDefaultAsync();
        }
        #endregion

        #region  * * * * * * * * * * PALESTRANTES * * * * * * * * * * * * * * *
        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
             .Include(c => c.RedesSociais);

            if (includeEvento)
            {
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Evento); // inclui entidade relacionada a entidade Palestrante Evento
            }
            query = query.AsNoTracking().Where(c => c.Id == PalestranteId).OrderByDescending(c => c.Nome);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string nome, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                       .Include(c => c.RedesSociais);

            if (includeEvento)
            {
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Evento); // inclui entidade relacionada a entidade Palestrante Evento
            }
            query = query.AsNoTracking().Where(p => p.Nome.ToLower().Contains(nome.ToLower())).OrderByDescending(c => c.Nome);
            return await query.ToArrayAsync();
        }
    }
    #endregion
}