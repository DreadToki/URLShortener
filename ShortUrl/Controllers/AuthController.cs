using Microsoft.AspNetCore.Mvc;
using ShortUrl.Dtos;
using ShortUrl.Models;
using ShortUrl.Services;

namespace ShortUrl.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly DataContext _dataContext;
    private readonly JwtService _jwtService;

    public AuthController(DataContext dataContext, JwtService jwtService)
    {
        _dataContext = dataContext;
        _jwtService = jwtService;
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register(CredentialsDto dto)
    {
        var userExists = _dataContext.Users.Any(u => u.Login == dto.Login);
        if (userExists)
        {
            return BadRequest($"User {dto.Login} already exists.");
        }

        var user = new User
        {
            Login = dto.Login,
            Password = dto.Password,
            Role = UserRole.User,
        };

        _dataContext.Add(user);
        _dataContext.SaveChanges();

        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(CredentialsDto dto)
    {
        var getUser = _dataContext.Users.FirstOrDefault(u => u.Login == dto.Login);

        if (getUser == null || getUser.Password != dto.Password)
        {
            return Unauthorized("Incorrect login or password.");
        }

        var jwt = _jwtService.Generate(getUser.Login);

        Response.Cookies.Append("jwt", jwt, new CookieOptions
        {
            HttpOnly = true,
        });

        return Ok();
    }

    [HttpGet]
    [Route("user")]
    public IActionResult GetUser()
    {
        var jwt = Request.Cookies["jwt"];

        if (jwt == null)
        {
            return Unauthorized("Unauthorized user.");
        }

        var token = _jwtService.Verify(jwt);

        var user = _dataContext.Users.FirstOrDefault(u => u.Login == token.Issuer);

        if (user == null)
        {
            return Unauthorized("Unauthorized user.");
        }

        return Ok(new UserDto
        {
            Login = user.Login,
        });
    }

    [HttpGet]
    [Route("iselevateduser")]
    public IActionResult IsElevatedUser()
    {
        var jwt = Request.Cookies["jwt"];

        if (jwt == null)
        {
            return Unauthorized("Unauthorized user.");
        }

        var token = _jwtService.Verify(jwt);

        var user = _dataContext.Users.FirstOrDefault(u => u.Login == token.Issuer);

        if (user == null)
        {
            return Unauthorized("Unauthorized user.");
        }

        return Ok(user.Role == UserRole.Admin);
    }

    [HttpPost]
    [Route("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");

        return Ok();
    }
}
