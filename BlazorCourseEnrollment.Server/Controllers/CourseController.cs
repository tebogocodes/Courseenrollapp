using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCourseEnrollment.Models;
using BlazorCourseEnrollment.Services;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICourseService _courseService;

    public CourseController(ApplicationDbContext dbContext, ICourseService courseService)
    {
        _dbContext = dbContext;
        _courseService = courseService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetCourses()
    {
        return Ok(await _courseService.GetCoursesAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await _courseService.GetCourseAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateCourse([FromBody] Course course)
    {
        await _courseService.AddCourseAsync(course);

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
    {
        if (id != course.Id)
        {
            return BadRequest();
        }

        await _courseService.UpdateCourseAsync(course);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        await _courseService.DeleteCourseAsync(id);

        return NoContent();
    }
}