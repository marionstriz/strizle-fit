using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ProgramMapper : BaseMapper<Program, Domain.Program>
{
    public ProgramMapper(IMapper mapper) : base(mapper)
    {
    }
}