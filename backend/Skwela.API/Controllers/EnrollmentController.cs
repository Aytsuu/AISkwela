
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Skwela.Application.UseCases.Enrollments;

namespace Skwela.API.Controllers;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController : ControllerBase
{
    private readonly CreateEnrollmentUseCase _createUseCase;
    private readonly GetEnrollmentUseCase _getUseCase;
    private readonly UpdateEnrollmentUseCase _updateUseCase;

    public EnrollmentController(CreateEnrollmentUseCase createUseCase, [FromServices]  GetEnrollmentUseCase getUseCase, UpdateEnrollmentUseCase updateUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
        _updateUseCase = updateUseCase;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> EnrollStudent(CreateEnrollmentDto request)
    {
        try
        {
            var enrollment = await _createUseCase.ExecuteAsync(request);
            return Ok(enrollment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("get/{userId}")]
    public async Task<IActionResult> GetStudentEnrollments(Guid userId)
    {
        try
        {
            var enrollments = await _getUseCase.ExecuteGetStudentEnrollmentAsync(userId);
            return Ok(enrollments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPatch("update/status/{classId}/{userId}")]
    public async Task<IActionResult> UpdateEnrollmentStatus(Guid classId, Guid userId)
    {
        try
        {
            var update = await _updateUseCase.ExecuteUpdateEnrollmentStatusAsync(classId, userId);
            return Ok(update);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}