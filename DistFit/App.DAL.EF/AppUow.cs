using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUow : BaseUow<AppDbContext>, IAppUnitOfWork
{
    private readonly AutoMapper.IMapper _mapper;
    
    public AppUow(AppDbContext dbContext, AutoMapper.IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }
    
    private IExerciseTypeRepository? _exerciseTypes;
    public virtual IExerciseTypeRepository ExerciseTypes 
        => _exerciseTypes ??= new ExerciseTypeRepository(UowDbContext, new ExerciseTypeMapper(_mapper));
    
    private IGoalRepository? _goals;
    public virtual IGoalRepository Goals 
        => _goals ??= new GoalRepository(UowDbContext,  new GoalMapper(_mapper));

    private IMeasurementRepository? _measurements;
    public IMeasurementRepository Measurements 
        => _measurements ??= new MeasurementRepository(UowDbContext,  new MeasurementMapper(_mapper));

    private IMeasurementTypeRepository? _measurementTypes;
    public IMeasurementTypeRepository MeasurementTypes 
        => _measurementTypes ??= new MeasurementTypeRepository(UowDbContext,  new MeasurementTypeMapper(_mapper));

    private IPerformanceRepository? _performances;
    public IPerformanceRepository Performances 
        => _performances ??= new PerformanceRepository(UowDbContext, new PerformanceMapper(_mapper));

    private IProgramRepository? _programs;
    public IProgramRepository Programs 
        => _programs ??= new ProgramRepository(UowDbContext, new ProgramMapper(_mapper));

    private IProgramSavedRepository? _programsSaved;
    public IProgramSavedRepository ProgramsSaved 
        => _programsSaved ??= new ProgramSavedRepository(UowDbContext,  new ProgramSavedMapper(_mapper));

    private ISessionRepository? _sessions;
    public ISessionRepository Sessions 
        => _sessions ??= new SessionRepository(UowDbContext,  new SessionMapper(_mapper));

    private ISessionExerciseRepository? _sessionExercises;
    public ISessionExerciseRepository SessionExercises 
        => _sessionExercises ??= new SessionExerciseRepository(UowDbContext,  new SessionExerciseMapper(_mapper));

    private ISetEntryRepository? _setEntries;
    public ISetEntryRepository SetEntries 
        => _setEntries ??= new SetEntryRepository(UowDbContext,  new SetEntryMapper(_mapper));

    private IUnitRepository? _units;
    public IUnitRepository Units 
        => _units ??= new UnitRepository(UowDbContext, new UnitMapper(_mapper));

    private IUserExerciseRepository? _userExercises;
    public IUserExerciseRepository UserExercises => 
        _userExercises ??= new UserExerciseRepository(UowDbContext, new UserExerciseMapper(_mapper));
    
    private IUserSessionExerciseRepository? _userSessionExercises;
    public IUserSessionExerciseRepository UserSessionExercises => 
        _userSessionExercises ??= new UserSessionExerciseRepository(UowDbContext, new UserSessionExerciseMapper(_mapper));
}