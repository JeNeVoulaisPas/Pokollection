using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using PokéService.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Security.Claims;


namespace Front.Services
{
    public class PokémonService: AuthService
    {


        public PokémonService(HttpClient httpClient) : base(httpClient) { }

        public async Task<Pokémon?> GetPokémon(int id)
        {
            var res = await _httpClient.GetAsync($"api/Poké/card/{id}");

            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<Pokémon>();
            return null;
        }
    }
}


