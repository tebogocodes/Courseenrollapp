using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCourseEnrollment.Models;

public interface IStudentService
{
    Task<List<Student>> GetStudentsAsync();
    Task<Student> GetStudentAsync(int id);
    Task AddStudentAsync(Student student);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(int id);
}

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _dbContext;

    public StudentService(ApplicationDbContext dbContext){
        _dbContext = dbContext;
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        return await _dbContext.Students.Include(s => s.StudentCourses).ThenInclude(sc => sc.Course).ToListAsync();
    }

    public async Task<Student> GetStudentAsync(int id)
    {
        return await _dbContext.Students.Include(s => s.StudentCourses).ThenInclude(sc => sc.Course).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddStudentAsync(Student student)
    {
        await _dbContext.Students.AddAsync(student);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateStudentAsync(Student student)
    {
        _dbContext.Entry(student).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(int id)
    {
        var student = await _dbContext.Students.FindAsync(id);

        if (student != null)
        {
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
        }
    }
}