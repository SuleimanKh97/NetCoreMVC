using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // ✅ تسجيل الدخول
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

        if (user == null || !PasswordHasher.VerifyPassword(login.Password, user.Password))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // ✅ إضافة الدور إلى التوكن
        var token = _jwtTokenService.GenerateJwtToken(user);

        return Ok(new { token, role = user.Role, username = user.Username });
    }

    // ✅ تسجيل مستخدم جديد
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel register)
    {
        // 🛑 التحقق مما إذا كان اسم المستخدم أو البريد الإلكتروني مستخدمًا بالفعل
        if (await _dbContext.Users.AnyAsync(u => u.Username == register.Username || u.Email == register.Email))
        {
            return BadRequest(new { message = "Username or Email already exists" });
        }

        var newUser = new User
        {
            Username = register.Username,
            Email = register.Email,
            Password = PasswordHasher.HashPassword(register.Password),
            Role = register.Role, // "Buyer" or "Seller"
            CreatedAt = DateTime.Now
        };

        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "User registered successfully" });
    }

    // ✅ الحصول على معلومات المستخدم الحالي
    [HttpGet("currentUser")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userName = User.Identity.Name;
        var user = await _dbContext.Users
            .Where(u => u.Username == userName)
            .Select(u => new { u.Username, u.Role, u.Email, u.CreatedAt, u.Orders })
            .FirstOrDefaultAsync();

        if (user == null) return NotFound(new { message = "User not found" });

        return Ok(user);
    }
}

// ✅ نماذج الطلبات (Request Models)
public class LoginModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class RegisterModel
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}
