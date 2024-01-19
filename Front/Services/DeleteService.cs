using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Front.Services;


namespace Front.Services
{
    public class DeleteService
    {
        private readonly HttpClient _httpClient;
        public CustomAuthenticationStateProvider state { get; set; }

        public DeleteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5000/");
        }

        public async Task<bool> DeleteAccount(int id)
        {
            var res = await _httpClient.DeleteAsync($"api/User/{id}");
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}