using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace Front.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5000/");
        }

        public async Task<UserDTO?> AuthenticateUser(string username, string password)
        {
            UserLogin u = new UserLogin() { Name=username, Pass=password};
            var res = await _httpClient.PostAsJsonAsync<UserLogin>("api/User/login", u);
            if (res.IsSuccessStatusCode)
            {
                try
                {
                    UserDTO? udto = await res.Content.ReadFromJsonAsync<UserDTO>();
                    return udto;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}

