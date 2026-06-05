using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BooKStore.Dtos.Auth;
using BooKStore.HTTP;          // Imports your Result<T>
using BooKStore.HTTP.Responses; // Imports your ApiResponse helper
using BooKStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BooKStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return ApiResponse.ToResponse(Result<object>.Fail(modelErrors, "Validation failed."));
        }

        var userExists = await _userManager.FindByNameAsync(model.UserName);
        if (userExists != null)
        {
            return ApiResponse.ToResponse(Result<object>.Fail("Username already exists.", "Registration failed."));
        }

        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var identityErrors = result.Errors.Select(e => e.Description).ToList();
            return ApiResponse.ToResponse(Result<object>.Fail(identityErrors, "Registration failed."));
        }

        // Return a clean string message or metadata wrapped in your Result object
        var successResult = Result<string>.Ok("User registered successfully!", "Registration successful.");
        return ApiResponse.ToResponse(successResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return ApiResponse.ToResponse(Result<object>.Fail(modelErrors, "Validation failed."));
        }

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GenerateJwtToken(authClaims);

            // Shape your login success payload
            var loginData = new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserName = user.UserName
            };

            // Wrap it cleanly using your Result pattern!
            return ApiResponse.ToResponse(Result<object>.Ok(loginData, "Login successful."));
        }

        return ApiResponse.ToResponse(Result<object>.Fail("Invalid username or password.", "Authentication failed."));
    }

    private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
    {
        var secretString = _configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT Secret is missing.");
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretString));

        int.TryParse(_configuration["JWT:DurationInDays"], out int durationInDays);

        return new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddDays(durationInDays == 0 ? 7 : durationInDays),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}