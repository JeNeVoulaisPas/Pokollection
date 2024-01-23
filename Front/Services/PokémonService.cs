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

        public async Task<Pokémon?> GetPokémon(string id)
        {
            var res = await _httpClient.GetAsync($"api/Poké/card/{id}");

            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<Pokémon>();
            return null;
        }

        public async Task<IEnumerable<Pokémon>?> Search(string query = "")
        {
            var res = await _httpClient.GetAsync($"api/Poké/search{query}");

            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<IEnumerable<Pokémon>>();
            return null;
        }

        public async Task<IEnumerable<Pokémon>?> GetCollection(string query = "", int? id = null)
        {
            if (id == null)
            {
                if (_auth == null) return null;
                id = await _auth.GetAuthenticationIdAsync();
            }
            if (id == -1) return null;

            var res = await _httpClient.GetAsync($"api/Poké/collection/{id}/card{query}");

            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<IEnumerable<Pokémon>>();
            return null;
        }

        public async Task<bool> AddCard(int cid)
        {
            var res = await _httpClient.PostAsync($"api/Poké/collection/card/{cid}", null);

            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCard(int cid)
        {
            var res = await _httpClient.DeleteAsync($"api/Poké/collection/card/{cid}");

            return res.IsSuccessStatusCode;
        }

        public string GetTypeClass(string type)
        {
            return type switch
            {
				"Plante" => "energy-grass",
				"Feu" => "energy-fire",
				"Eau" => "energy-water",
				"Psy" => "energy-psychic",
				"Combat" => "energy-fighting",
				"Obscurité" => "energy-darkness",
				"Métal" => "energy-metal",
				"Fée" => "energy-fairy",
				"Dragon" => "energy-dragon",
				"Électrique" => "energy-lightning",
				"Incolore" => "energy-colorless",
				_ => "energy-colorless",
			};
		}
    }
}


