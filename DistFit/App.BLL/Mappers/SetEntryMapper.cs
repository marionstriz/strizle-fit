using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class SetEntryMapper : BaseMapper<App.BLL.DTO.SetEntry, App.DAL.DTO.SetEntry>
{
    public SetEntryMapper(IMapper mapper) : base(mapper)
    {
    }
}