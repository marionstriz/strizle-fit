using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class SetEntryService 
    : BaseEntityService<App.BLL.DTO.SetEntry, App.DAL.DTO.SetEntry, ISetEntryRepository>, ISetEntryService
{
    public SetEntryService(
        ISetEntryRepository repository, 
        IMapper<SetEntry, DAL.DTO.SetEntry> mapper
    ) : base(repository, mapper)
    {
    }
}