using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PerformanceMapper : BaseMapper<App.BLL.DTO.Performance, App.DAL.DTO.Performance>
{
    public PerformanceMapper(IMapper mapper) : base(mapper)
    {
    }
}