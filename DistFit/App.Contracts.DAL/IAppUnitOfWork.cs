using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IExerciseTypeRepository ExerciseTypes { get; }
    IGoalRepository Goals { get; }
    IMeasurementRepository Measurements { get; }
    IMeasurementTypeRepository MeasurementTypes { get; }
    IPerformanceRepository Performances { get; }
    IProgramRepository Programs { get; }
    IProgramSavedRepository ProgramsSaved { get; }
    ISessionRepository Sessions { get; }
    ISessionExerciseRepository SessionExercises { get; }
    ISetEntryRepository SetEntries { get; }
    IUnitRepository Units { get; }
    IUserExerciseRepository UserExercises { get; }
    IUserSessionExerciseRepository UserSessionExercises { get; }
    IAppUserRepository AppUsers { get; }
}