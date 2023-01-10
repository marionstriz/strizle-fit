using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class SetEntryMapper : BaseMapper<SetEntry, Domain.SetEntry>
{
    public SetEntryMapper(IMapper mapper) : base(mapper)
    {
    }
}