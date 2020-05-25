using System.Linq;
using AutoMapper;
using ProAgil.API.Dtos;
using ProAgil.Domain.Entities;

namespace ProAgil.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>()
            .ForMember(dest => dest.Palestrantes, opt =>{
                opt.MapFrom(src => src.PalestrantesEventos.Select(x => x.PalestranteId).ToList());
            });
            CreateMap<Palestrante, PalestranteDto>()
            .ForMember(dest => dest.Eventos, opt =>{
                opt.MapFrom(src =>src.PalestrantesEventos.Select(x=>x.Evento).ToList());
            });
            CreateMap<Lote, LoteDto>();
            CreateMap<RedeSocial, RedeSocialDto>();
        }
    }
}