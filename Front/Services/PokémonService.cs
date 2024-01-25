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

        public async Task<int?> GetUserIdAsync()
        {
			return (_auth == null)? null: await _auth.GetAuthenticationIdAsync();
		}

        public async Task<Pokémon?> GetPokémon(string id)
		{
			var uid = await GetUserIdAsync();
			if (uid is not null) id += $"?id={uid}";

			var res = await _httpClient.GetAsync($"api/Poké/card/{id}");

            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<Pokémon>();
            return null;
        }

        public async Task<IEnumerable<Pokémon>?> Search(string query = "")
		{
			var id = await GetUserIdAsync();
            if (id is not null)
            {
                if (query.Length > 0) query += "&";
                else query = "?";
                query += $"id={id}";
            }
			var res = await _httpClient.GetAsync($"api/Poké/search{query}");

            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<IEnumerable<Pokémon>>();
            return null;
        }

        public async Task<IEnumerable<Pokémon>?> GetCollection(string query = "", int? id = null)
        {
            if (id is null) id = await GetUserIdAsync();
            if (id is null) return null;

            var res = await _httpClient.GetAsync($"api/Poké/collection/{id}/card{query}");

            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<IEnumerable<Pokémon>>();
            return null;
        }

        public async Task<bool> AddCard(string cid)
        {
            var res = await _httpClient.PostAsync($"api/Poké/collection/card/{cid}", null);

            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCard(string cid)
        {
            var res = await _httpClient.DeleteAsync($"api/Poké/collection/card/{cid}");

            return res.IsSuccessStatusCode;
        }

        public string[] GetTypes()
        {
            return new string[] { "Tous", "Incolore", "Plante", "Eau", "Feu", "Électrique", "Métal", "Psy", "Combat", "Obscurité", "Dragon", "Fée" };
        }

        public string GetTypeClass(string type)
        {
            return type switch
            {
                "Tous" => "energy-all",
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


