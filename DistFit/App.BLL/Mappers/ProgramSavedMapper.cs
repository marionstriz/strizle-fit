using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ProgramSavedMapper : BaseMapper<App.BLL.DTO.ProgramSaved, App.DAL.DTO.ProgramSaved>
{
    public ProgramSavedMapper(IMapper mapper) : base(mapper)
    {
    }
}