using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ProgramMapper : BaseMapper<App.BLL.DTO.Program, App.DAL.DTO.Program>
{
    public ProgramMapper(IMapper mapper) : base(mapper)
    {
    }
}