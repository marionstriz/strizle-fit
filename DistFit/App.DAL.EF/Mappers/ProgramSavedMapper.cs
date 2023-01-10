using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ProgramSavedMapper : BaseMapper<ProgramSaved, Domain.ProgramSaved>
{
    public ProgramSavedMapper(IMapper mapper) : base(mapper)
    {
    }
}