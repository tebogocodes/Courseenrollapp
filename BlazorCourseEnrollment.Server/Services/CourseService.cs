using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCourseEnrollment.Models;

public interface ICourseService
{
    Task<List<Course>> GetCoursesAsync();
    Task<Course> GetCourseAsync(int id);
    Task AddCourseAsync(Course course);
    Task UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(int id);
}

public classCourseService : ICourseService
{
    private readonly ApplicationDbContext _dbContext;

    public CourseService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Course>> GetCoursesAsync()
    {
        return await _dbContext.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student).ToListAsync();
    }

    public async Task<Course> GetCourseAsync(int id)
    {
        return await _dbContext.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCourseAsync(Course course)
    {
        await _dbContext.Courses.AddAsync(course);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCourseAsync(Course course)
    {
        _dbContext.Entry(course).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCourseAsync(int id)
    {
        var course = await _dbContext.Courses.FindAsync(id);

        if (course != null)
        {
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}