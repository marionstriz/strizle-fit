using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using MeasurementType = App.DAL.DTO.MeasurementType;

namespace App.DAL.EF.Repositories;

public class MeasurementTypeRepository 
    : BaseEntityRepository<DAL.DTO.MeasurementType, Domain.MeasurementType, AppDbContext>, IMeasurementTypeRepository
{
    public MeasurementTypeRepository(AppDbContext dbContext, IMapper<MeasurementType, Domain.MeasurementType> mapper) 
        : base(dbContext, mapper)
    {
    }
}