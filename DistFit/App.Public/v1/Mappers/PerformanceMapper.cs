using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class PerformanceMapper : BaseMapper<App.Public.DTO.v1.Performance, App.BLL.DTO.Performance>
{
    public PerformanceMapper(IMapper mapper) : base(mapper)
    {
    }
}