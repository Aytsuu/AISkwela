using Skwela.Application.Interfaces;
using Skwela.Domain.Entities;

namespace Skwela.Application.UseCases.Enrollments;

public class UpdateEnrollmentUseCase
{
    private readonly IEnrollmentRepository _repository;

    public UpdateEnrollmentUseCase(IEnrollmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Enrollment> ExecuteUpdateEnrollmentStatusAsync(Guid classId, Guid userId)
    {
        return await _repository.UpdateEnrollmentStatusAsync(classId, userId);
    }
}