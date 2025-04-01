using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMasterPeace.Helpers;
using TestMasterPeace.Models;
using TestMasterPeace.Services;

namespace TestMasterPeace.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly MasterPeiceContext _dbContext;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(MasterPeiceContext dbContext, JwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
    }

    // POST: /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var user = _dbContext.Users
            .FirstOrDefault(u => u.Username == login.Username);

        if (user == null) return Unauthorized("Invalid credentials");


        // Here we are using a simple password check for demonstration purposes
        // Use a proper password hash check like bcrypt or PBKDF2 in production
        if (!PasswordHasher.VerifyPassword(login.Password, user.Password)) return Unauthorized("Invalid credentials");


        var token = _jwtTokenService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterModel register)
    {
        await _dbContext.Users.AddAsync(new Models.User
        {
            Username = register.Username,
            Email = register.Email,
            Password = PasswordHasher.HashPassword(register.Password),
            Role = register.Role,
            CreatedAt = DateTime.Now,
        });
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("currentUser")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userName = User.Identity.Name;
        var user = _dbContext.Users.Select(user => new { user.Username, user.Orders }).FirstOrDefault(user => user.Username == userName);
        return Ok(user);
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RegisterModel
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}