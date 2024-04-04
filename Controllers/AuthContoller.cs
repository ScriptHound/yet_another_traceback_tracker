using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Yet_Another_Traceback_Tracker.Services;

namespace Yet_Another_Traceback_Tracker.Controllers;


public class UsernamePasswordModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

[ApiController]
[Route("login/[controller]")]
public class AuthContoller
{
    private readonly IUserService _userService;
    private IConfiguration _config;
    public AuthContoller(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }
    
    private string GenerateToken(string username, string password)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var Sectoken = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            [new Claim("username", username), new Claim("password", password)],
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(Sectoken);
    }
    
    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] UsernamePasswordModel loginData)
    {
        var loggedIn = _userService.AuthenticateUser(loginData.Username, loginData.Password);
        if (!loggedIn)
        {
            return new UnauthorizedResult();
        }
        var token = GenerateToken(loginData.Username, loginData.Password);
        return new OkObjectResult(token);
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] UsernamePasswordModel loginData)
    {
        _userService.RegisterUser(loginData.Username, loginData.Password);
        var token = GenerateToken(loginData.Username, loginData.Password);
        return new OkObjectResult(token);
    }
}