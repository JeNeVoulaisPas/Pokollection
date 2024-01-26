using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static Front.Components.Pages.Register;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Authorization;


namespace Front.Services
{
    public class UserService: AuthService
    {
        public UserService(HttpClient httpClient, AuthenticationStateProvider customAuthenticationStateProvider) : base(httpClient, customAuthenticationStateProvider) {
        }

        // Login
        public async Task<UserDTO?> AuthenticateUser(UserLogin login)
        {
            var res = await _httpClient.PostAsJsonAsync<UserLogin>("api/User/login", login);
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
            else return null;
        }

        // Delete
        public async Task<bool> DeleteAccount()
        {
            var res = await _httpClient.DeleteAsync($"api/User");
            if (res.IsSuccessStatusCode) await Logout();
            return res.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await _auth.Logout();
        }

        // Register

        public async Task<string?> CreateUser(UserCreate uc)
        {
            var res = await _httpClient.PostAsJsonAsync<UserCreate>("api/User/register", uc);

            return res.IsSuccessStatusCode? null: await res.Content.ReadAsStringAsync();

        }

        public string GetGender(Gender g)
        {
            return g switch
            {
                Gender.Man => "Homme",
                Gender.Woman => "Femme",
                Gender.Both => "Les deux",
                Gender.NoBinary => "Non binaire",
                Gender.Dog => "Chien",
                Gender.Yes => "Oui",
                Gender.No => "Non",
                Gender.Patatoes => "Patate",
                Gender.Unknown => "Inconnue",
                Gender.Other => "Autre",
                Gender.Unown => "Zarbi",
                _ => "Inconnue",
            };
        }

        public Gender GetValue(string s)
        {
            return s switch
            {
                "Homme" => Gender.Man,
                "Femme" => Gender.Woman,
                "Les deux" => Gender.Both,
                "Non binaire" => Gender.NoBinary,
                "Chien" => Gender.Dog,
                "Oui" => Gender.Yes,
                "Non" => Gender.No,
                "Patate" => Gender.Patatoes,
                "Inconnue" => Gender.Unknown,
                "Autre" => Gender.Other,
                "Zarbi" => Gender.Unown,
                _ => Gender.Unknown,
            };
        }
    }
}

