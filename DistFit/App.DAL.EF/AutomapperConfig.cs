using App.DAL.DTO;
using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<ExerciseType, Domain.ExerciseType>().ReverseMap();
        CreateMap<Goal, Domain.Goal>().ReverseMap();
        CreateMap<Measurement, Domain.Measurement>().ReverseMap();
        CreateMap<MeasurementType, Domain.MeasurementType>().ReverseMap();
        CreateMap<Performance, Domain.Performance>().ReverseMap();
        CreateMap<Program, Domain.Program>().ReverseMap();
        CreateMap<ProgramSaved, Domain.ProgramSaved>().ReverseMap();
        CreateMap<SessionExercise, Domain.SessionExercise>().ReverseMap();
        CreateMap<Session, Domain.Session>().ReverseMap();
        CreateMap<SetEntry, Domain.SetEntry>().ReverseMap();
        CreateMap<Unit, Domain.Unit>().ReverseMap();
        CreateMap<UserExercise, Domain.UserExercise>().ReverseMap();
        CreateMap<UserSessionExercise, Domain.UserSessionExercise>().ReverseMap();
        CreateMap<AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}