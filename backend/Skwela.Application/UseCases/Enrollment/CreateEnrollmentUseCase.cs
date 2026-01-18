using Skwela.Application.Interfaces;
using Skwela.Domain.Entities;

namespace Skwela.Application.UseCases.Enrollments;

public class CreateEnrollmentUseCase
{
    private readonly IEnrollmentRepository _repository;

    public CreateEnrollmentUseCase(IEnrollmentRepository repository)
    {
        _repository = repository;
    }

    public Task<Enrollment> ExecuteAsync(CreateEnrollmentDto dto)
    {
        var enrollment = Enrollment.Build(dto.classId, dto.userId);
        return _repository.AddEnrollmentAsync(enrollment);
    }
}