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
public class StudentController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStudentService _studentService;

    public StudentController(ApplicationDbContext dbContext, IStudentService studentService)
    {
        _dbContext = dbContext;
        _studentService = studentService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetStudents()
    {
        return Ok(await _studentService.GetStudentsAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _studentService.GetStudentAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateStudent([FromBody] Student student)
    {
        await _studentService.AddStudentAsync(student);

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
    {
        if (id != student.Id)
        {
            return BadRequest();
        }

        await _studentService.UpdateStudentAsync(student);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await _studentService.DeleteStudentAsync(id);

        return NoContent();
    }
}