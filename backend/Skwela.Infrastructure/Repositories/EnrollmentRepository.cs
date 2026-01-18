using Microsoft.EntityFrameworkCore;
using Skwela.Infrastructure.Data;
using Skwela.Domain.Entities;
using Skwela.Application.Interfaces;

namespace Skwela.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
    {
        var alreadyEnrolled =  await _context.Enrollments
            .AnyAsync(e => e.class_id == enrollment.class_id && e.user_id == enrollment.user_id);

        if (alreadyEnrolled == true)
        {
            return await UpdateEnrollmentStatusAsync(enrollment.class_id, enrollment.user_id);
        } 
        else
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }
        
        return enrollment;
    }

    public async Task<IEnumerable<Enrollment>> GetStudentEnrollmentAsync(Guid userId)
    {
        return await _context.Enrollments
            .Where(e => e.user_id == userId && e.enrolled_status == "active")
            .Include(e => e.classroom)
            .ToListAsync();
    }

    public async Task<Enrollment> UpdateEnrollmentStatusAsync(Guid classId, Guid userId)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.class_id == classId && e.user_id == userId);

        if (enrollment == null)
        {
            throw new KeyNotFoundException("Student is not enrolled in this class.");
        }

        enrollment.enrolled_status = enrollment.enrolled_status == "active" ? "inactive" : "active";
        await _context.SaveChangesAsync();

        return enrollment;
    }
}