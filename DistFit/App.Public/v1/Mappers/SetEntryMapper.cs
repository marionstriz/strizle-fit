using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class SetEntryMapper : BaseMapper<App.Public.DTO.v1.SetEntry, App.BLL.DTO.SetEntry>
{
    public SetEntryMapper(IMapper mapper) : base(mapper)
    {
    }
}