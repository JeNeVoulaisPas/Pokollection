using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace Front.Services
{
    public class RegisterService: AuthService
    {
        public RegisterService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<bool> CreateUser(string email, string username, string password)
        {
            UserCreateModel u = new UserCreateModel() { Email = email, Name = username, Password = password };
            var res = await _httpClient.PostAsJsonAsync<UserCreateModel>("api/User/register", u);

            return res.IsSuccessStatusCode;
        }
    }
}


