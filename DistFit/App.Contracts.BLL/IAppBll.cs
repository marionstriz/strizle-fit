using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBll : IBll
{
    IExerciseTypeService ExerciseTypes { get; }
    IGoalService Goals { get; }
    IMeasurementService Measurements { get; }
    IMeasurementTypeService MeasurementTypes { get; }
    IPerformanceService Performances { get; }
    IProgramService Programs { get; }
    IProgramSavedService ProgramsSaved { get; }
    ISessionService Sessions { get; }
    ISessionExerciseService SessionExercises { get; }
    ISetEntryService SetEntries { get; }
    IUnitService Units { get; }
    IUserExerciseService UserExercises { get; }
    IUserSessionExerciseService UserSessionExercises { get; }
    IAppUserService AppUsers { get; }
}