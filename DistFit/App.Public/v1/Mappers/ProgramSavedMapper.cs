using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class ProgramSavedMapper : BaseMapper<App.Public.DTO.v1.ProgramSaved, App.BLL.DTO.ProgramSaved>
{
    public ProgramSavedMapper(IMapper mapper) : base(mapper)
    {
    }
}