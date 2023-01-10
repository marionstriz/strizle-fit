using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PerformanceMapper : BaseMapper<Performance, Domain.Performance>
{
    public PerformanceMapper(IMapper mapper) : base(mapper)
    {
    }
}