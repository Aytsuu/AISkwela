using Skwela.Application.Interfaces;

namespace Skwela.Application.UseCases.Enrollments;

public class  GetEnrollmentUseCase
{
    private IEnrollmentRepository _repository;

    public GetEnrollmentUseCase(IEnrollmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EnrollmentDto>> ExecuteGetStudentEnrollmentAsync(Guid userId)
    {
        var enrollments = await _repository.GetStudentEnrollmentAsync(userId);

        return enrollments.Select(e => new EnrollmentDto(
            e.class_id,
            e.classroom?.class_name ?? "Unknown Classroom",
            e.classroom?.class_description ?? string.Empty,
            e.enrolled_at
        ));
    }
}