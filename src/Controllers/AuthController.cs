using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Data.DTO;
using src.Models;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext dbContext, TokenService tokenService) : ControllerBase
{
    public readonly TokenService _tokenService = tokenService;
    public readonly AppDbContext _dbContext = dbContext;

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null)
        {
            return Unauthorized();
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new { Token = token });
    }

    [HttpPost("add-user")]
    public async Task<ActionResult> AddUser([FromBody] RegisterDto request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Password is required." });
            }


            // New user
            user = new User
            {
                Email = request.Email,
                Name = request.Name,

                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new { Token = token });
    }
}
