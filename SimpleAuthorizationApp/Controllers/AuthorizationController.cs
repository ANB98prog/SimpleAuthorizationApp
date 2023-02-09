using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimpleAuthorizationApp.Authorize
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController
    {
        private HttpContext? _context;

        private List<Person> _persons = new List<Person>()
        {
            new Person("alex@gmail.com", "password"),
            new Person("tom@gmail.com", "tomspassword"),
            new Person("hulio@gmail.com", "huli")
        };

        public AuthorizationController(IHttpContextAccessor contextAccessor)
        {            
            _context = contextAccessor.HttpContext;
        }

        [HttpGet("data")]
        public string Data()
        {
            return "Hello, world!";
        }

        [HttpPost("login")]
        public async Task<IResult> Login([FromQuery] string? returnUrl, [FromBody] Person personData)
        {
            if (personData is null)
                return Results.BadRequest("Login data is empty!");
            else if(string.IsNullOrWhiteSpace(personData.Email))
                return Results.BadRequest("Email is empty!");
            else if (string.IsNullOrWhiteSpace(personData.Password))
                return Results.BadRequest("Password is empty!");

            var user = _persons.FirstOrDefault(u => u.Email.Equals(personData.Email)
                                                && u.Password.Equals(personData.Password));

            if (user is null)
                return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, personData.Email) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            await _context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Results.Ok(new { Email = personData.Email });
            
        }

        [HttpGet("logout")]
        public async Task<IResult> Logout()
        {
            await _context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/authorization/login");
        }

        public record Person(string Email, string Password);

        public record PersonLoginResponse(string Email, string JwtToken);

    }
}