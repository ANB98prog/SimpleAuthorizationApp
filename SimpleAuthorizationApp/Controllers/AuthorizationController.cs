using Microsoft.AspNetCore.Mvc;

namespace SimpleAuthorizationApp.Authorize
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController
    {
        private List<Person> _persons = new List<Person>()
        {
            new Person("alex@gmail.com", "password"),
            new Person("tom@gmail.com", "tomspassword"),
            new Person("hulio@gmail.com", "huli")
        };

        [HttpGet("data")]
        public string Data()
        {
            return "Hello, world!";
        }

        [HttpPost("login")]
        public IResult Login(Person personData)
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

            var token = JwtTokenCreator.CreateToken(user.Email);

            return Results.Json(new PersonLoginResponse(user.Email, token));
            
        }

        public record Person(string Email, string Password);

        public record PersonLoginResponse(string Email, string JwtToken);

    }
}