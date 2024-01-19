using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Front.Services;
using Microsoft.AspNetCore.Components;

namespace Front.Services
{
    public class DeleteService : AuthService
    {
        public DeleteService(HttpClient httpClient) : base(httpClient)
        {
        }



        public async Task<bool> DeleteAccount()
        {
            var res = await _httpClient.DeleteAsync($"api/User");
            if (res.IsSuccessStatusCode)
            {
                await Logout();
                return true;
            }
            else
            {

                return false;
            }
        }

        public async Task<bool> Logout()
        {
            if (_auth != null)
            {
                await _auth.Logout();
                return true;
            }
            return false;
        }
    }
}