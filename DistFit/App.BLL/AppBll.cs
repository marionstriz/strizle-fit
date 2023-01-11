using App.BLL.Services;
using App.BLL.Mappers;
using App.BLL.Mappers.Identity;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBll : BaseBll<IAppUnitOfWork>, IAppBll
{
    protected IAppUnitOfWork UnitOfWork;
    protected IMapper Mapper;
    
    public AppBll(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
    }
    public override int SaveChanges()
    {
        return UnitOfWork.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync()
    {
        return await UnitOfWork.SaveChangesAsync();
    }
    
        private IExerciseTypeService? _exerciseTypes;
    public virtual IExerciseTypeService ExerciseTypes 
        => _exerciseTypes ??= new ExerciseTypeService(UnitOfWork.ExerciseTypes, new ExerciseTypeMapper(Mapper));
    
    private IGoalService? _goals;
    public virtual IGoalService Goals 
        => _goals ??= new GoalService(UnitOfWork.Goals,  new GoalMapper(Mapper));

    private IMeasurementService? _measurements;
    public IMeasurementService Measurements 
        => _measurements ??= new MeasurementService(UnitOfWork.Measurements,  new MeasurementMapper(Mapper));

    private IMeasurementTypeService? _measurementTypes;
    public IMeasurementTypeService MeasurementTypes 
        => _measurementTypes ??= new MeasurementTypeService(UnitOfWork.MeasurementTypes,  new MeasurementTypeMapper(Mapper));

    private IPerformanceService? _performances;
    public IPerformanceService Performances 
        => _performances ??= new PerformanceService(UnitOfWork.Performances, new PerformanceMapper(Mapper));

    private IProgramService? _programs;
    public IProgramService Programs 
        => _programs ??= new ProgramService(UnitOfWork.Programs, new ProgramMapper(Mapper));

    private IProgramSavedService? _programsSaved;
    public IProgramSavedService ProgramsSaved 
        => _programsSaved ??= new ProgramSavedService(UnitOfWork.ProgramsSaved,  new ProgramSavedMapper(Mapper));

    private ISessionService? _sessions;
    public ISessionService Sessions 
        => _sessions ??= new SessionService(UnitOfWork.Sessions,  new SessionMapper(Mapper));

    private ISessionExerciseService? _sessionExercises;
    public ISessionExerciseService SessionExercises 
        => _sessionExercises ??= new SessionExerciseService(UnitOfWork.SessionExercises,  new SessionExerciseMapper(Mapper));

    private ISetEntryService? _setEntries;
    public ISetEntryService SetEntries 
        => _setEntries ??= new SetEntryService(UnitOfWork.SetEntries,  new SetEntryMapper(Mapper));

    private IUnitService? _units;
    public IUnitService Units 
        => _units ??= new UnitService(UnitOfWork.Units, new UnitMapper(Mapper));
    
    private IUserExerciseService? _userExercises;
    public IUserExerciseService UserExercises => 
        _userExercises ??= new UserExerciseService(UnitOfWork.UserExercises, new UserExerciseMapper(Mapper));
    
    private IUserSessionExerciseService? _userSessionExercises;
    public IUserSessionExerciseService UserSessionExercises => 
        _userSessionExercises ??= new UserSessionExerciseService(UnitOfWork.UserSessionExercises, new UserSessionExerciseMapper(Mapper));
    
    private IAppUserService? _appUsers;
    public IAppUserService AppUsers => 
        _appUsers ??= new AppUserService(UnitOfWork.AppUsers, new AppUserMapper(Mapper));
}