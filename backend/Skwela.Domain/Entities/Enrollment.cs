using Skwela.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skwela.Domain.Entities;

public class Enrollment
{
    public Guid class_id { get; set; }
    public Guid user_id { get; set; }
    public DateTime enrolled_at { get; set; }
    public string enrolled_status { get; set; } = "active";

    // Foreign Keys
    [ForeignKey("class_id")]
    public Classroom? classroom { get; set; }
    [ForeignKey("user_id")]
    public User? user { get; set; }

    public static Enrollment Build(Guid classId, Guid userId)
    {
        // Validation (Guard Clauses)
        if (classId == Guid.Empty)
            throw new DomainException("Enrollment must have a valid class ID.");
        if (userId == Guid.Empty)
            throw new DomainException("Enrollment must have a valid user ID.");

        return new Enrollment
        {
            class_id = classId,
            user_id = userId,
            enrolled_at = DateTime.UtcNow,
            enrolled_status = "active"
        };
    }
}