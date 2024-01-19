using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using UserService.Entities;

namespace GatewayService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // api/User/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLogin model)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/login", model);

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode)
                {
                    // You can deserialize the response content here if needed
                    var result = await response.Content.ReadFromJsonAsync<UserDTO>();
                    if (result != null) result.Token = GenerateJwtToken(result.Id);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(await response.Content.ReadAsStringAsync());
                }
            }
        }


        // api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateModel model)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/register", model);

                // Check if the response status code is 201 (Created)
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    // Account created, now create a collection for the user
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    if (user != null)
                    {
                        client.BaseAddress = new System.Uri("http://localhost:5002/");
                        response = await client.PostAsync($"api/Poké/collection/{user.Id}", null);
                        if (!response.IsSuccessStatusCode) StatusCode((int)HttpStatusCode.ServiceUnavailable, "Gateway failed to connect to the collection server"); 

                    } else StatusCode((int)HttpStatusCode.InternalServerError, "Register service returned incoherent results");

                    return Created();
                }
                else
                {
                    return BadRequest(await response.Content.ReadAsStringAsync());
                }
            }
        }

        // TODO: merge delete account and delete collection

        private string GenerateJwtToken(int userId)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", userId.ToString()) // add the user id as a claim
            };

            // create a key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Fun fact: Ash's Pikachu name is Jean-Luc"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // token configuration
            var token = new JwtSecurityToken(
                issuer: "Pokollection", // Who created this token
                audience: "localhost:5000", // Who can use this token
                claims: claims, // data inside the token
                expires: DateTime.Now.AddMinutes(3000), // validity of the token
                signingCredentials: creds); // key to encrypt the token

            // return the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
