
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skwela.Application.UseCases.Classrooms;
using Skwela.Domain.Enums;

namespace Skwela.API.Controllers;

[ApiController]
[Route("api/classroom")]
public class ClassroomsController : ControllerBase
{
    private readonly CreateClassroomUseCase _createUseCase;
    private readonly GetClassroomUseCase _getUseCase;

    public ClassroomsController(CreateClassroomUseCase createUseCase, GetClassroomUseCase getUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateClassroom(CreateClassroomDto request)
    {
        try
        {
            var classId = await _createUseCase.ExecuteAsync(request);

            return Ok(new { classId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("get/{userId}")]
    public async Task<IActionResult> GetClassroomsByUserId(Guid userId)
    {
        try
        {
            var classrooms = await _getUseCase.ExecuteGetByUserIdAsync(userId);
            return Ok(classrooms);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("get/{classId}/{userId}/{role}")]
    public async Task<IActionResult> GetClassroomData(Guid classId, Guid userId, UserRole role)
    {
        try
        {
            var classroom = await _getUseCase.GetClassroomDataAsync(classId, userId, role);
            return Ok(classroom);
        }
        catch (KeyNotFoundException knfEx)
        {
            return NotFound(knfEx.Message);
        }
        catch (UnauthorizedAccessException uaEx)
        {
            return Forbid(uaEx.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}