using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace Front.Services
{
    public class RegisterService
    {
        private readonly HttpClient _httpClient;

        public RegisterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5000/");
        }

        public async Task<bool> CreateUser(string email, string username, string password)
        {
            UserCreateModel u = new UserCreateModel() { Email = email, Name = username, Password = password };
            var res = await _httpClient.PostAsJsonAsync<UserCreateModel>("api/User/register", u);

            return res.IsSuccessStatusCode;


        }
    }
}


