using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class ExerciseType : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    public LangStr Name { get; set; } = default!;

    public ICollection<UserExercise>? UserExercises { get; set; }
    public ICollection<SessionExercise>? SessionExercises { get; set; }
    public ICollection<Goal>? Goals { get; set; }
}