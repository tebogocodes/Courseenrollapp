using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorCourseEnrollment.Models;
using BlazorCourseEnrollment.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAuthenticationService _authenticationService;

    public AuthController(ApplicationDbContext dbContext, IAuthenticationService authenticationService)
    {
        _dbContext = dbContext;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authenticationService.AuthenticateAsync(request.Username, request.Password);

        if (user == null)
        {
            return Unauthorized();
        }return Ok(new
        {
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        await _dbContext.Students.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        return Ok();
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}