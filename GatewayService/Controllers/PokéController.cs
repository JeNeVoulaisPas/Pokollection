using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokéService.Entities;
using System.Net;

namespace GatewayService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PokéController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public PokéController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Collection: api/Poké/collection

        // GET: api/Poké/collection/5
        [HttpGet("collection/{id}")]
        public async Task<ActionResult<string[]>> GetCollection(int id) // get all cards ids
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.GetAsync($"api/Poké/collection/{id}");

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode) return Ok(await response.Content.ReadFromJsonAsync<string[]>());
                else return NotFound(await response.Content.ReadAsStringAsync());
            }
        }


        // GET: api/Poké/collection/5/card?name=&set=&category=&lid=&type1=&type2=&hp=&illustrator=&limit=&offset=
        [HttpGet("collection/{id}/card")]
        public async Task<ActionResult<IEnumerable<Pokémon>>> GetPokémonCollection(int id, // get all cards data
            string? name = null,
            string? set = null,
            Categories? category = null,
            string? lid = null,
            string? type1 = null,
            string? type2 = null,
            string? illustrator = null,
            int? hp = null,
            int limit = 50,
            int offset = 0)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                var arg = this.HttpContext.Request.QueryString;

                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.GetAsync($"api/Poké/collection/{id}/card{arg}"); // fast forward query arguments

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode) return Ok(await response.Content.ReadFromJsonAsync<IEnumerable<Pokémon>>());
                else return NotFound(await response.Content.ReadAsStringAsync());
                // avoid deserialize+reserialize ?? -> return Content(await apiResponse.Content.ReadAsStringAsync(), apiResponse.Content.Headers.ContentType.MediaType);
            }
        }


        // POST: api/Poké/collection -> /!\ MERGED WITH USER/REGISTER
        /*
        [Authorize]
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCollection() // create collection
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (UserId == null || !int.TryParse(UserId, out int id)) return Unauthorized();

            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.PostAsync($"api/Poké/collection/{id}", null);

                if (response.StatusCode == HttpStatusCode.OK) return Ok(); // Already exists
                if (response.StatusCode == HttpStatusCode.Created) return Created(); // collection created
                return NotFound(); // unexpected failure
            }
        }*/


        // DELETE: api/Poké/collection
        /*
        [Authorize]
        [HttpDelete("collection")]
        public async Task<IActionResult> DeleteCollection() // delete collection
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (UserId == null || !int.TryParse(UserId, out int id)) return Unauthorized();

            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.DeleteAsync($"api/Poké/collection/{id}");

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode) return Ok();
                else return NotFound(await response.Content.ReadAsStringAsync());
            }
        }*/


        // POST: api/Poké/collection/card/1
        [Authorize]
        [HttpPost("collection/card/{cid}")]
        public async Task<IActionResult> AddCard(string cid) // add card to collection
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (UserId == null || !int.TryParse(UserId, out int id)) return Unauthorized();

            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.PostAsync($"api/Poké/collection/{id}/card/{cid}", null);

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode) return Ok();
                else if (response.StatusCode == HttpStatusCode.NotFound) return NotFound(await response.Content.ReadAsStringAsync());
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }

        // DELETE: api/Poké/collection/card/1
        [Authorize]
        [HttpDelete("collection/card/{cid}")]
        public async Task<IActionResult> DeleteCard(string cid) // delete card from collection
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (UserId == null || !int.TryParse(UserId, out int id)) return Unauthorized();

            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.DeleteAsync($"api/Poké/collection/{id}/card/{cid}");

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode) return Ok();
                return NotFound(await response.Content.ReadAsStringAsync());
            }
        }



        // Pokémon: api/Poké/card



        // GET: api/Poké/card/5
        [HttpGet("card/{id}")]
        public async Task<ActionResult<Pokémon>> GetCard(string id)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.GetAsync($"api/Poké/card/{id}");

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode) return Ok(await response.Content.ReadFromJsonAsync<Pokémon>());
                else return NotFound(await response.Content.ReadAsStringAsync());
                // avoid deserialize+reserialize ?? -> return Content(await apiResponse.Content.ReadAsStringAsync(), apiResponse.Content.Headers.ContentType.MediaType);
            }
        }

        // GET: api/Poké/search?name=&set=&category=&lid=&type1=&type2=&hp=&illustrator=&limit=&offset=
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Pokémon>>> SearchCard(
            string? name = null,
            string? set = null,
            Categories? category = null,
            string? lid = null,
            string? type1 = null,
            string? type2 = null,
            string? illustrator = null,
            int? hp = null,
            int limit = 50,
            int offset = 0)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                var arg = this.HttpContext.Request.QueryString;

                client.BaseAddress = new System.Uri("http://localhost:5002/");

                HttpResponseMessage response = await client.GetAsync(
                    $"api/Poké/search{arg}"); // fast forward query arguments

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode) return Ok(await response.Content.ReadFromJsonAsync<IEnumerable<Pokémon>>());
                else return NotFound(await response.Content.ReadAsStringAsync());
                // avoid deserialize+reserialize ?? -> return Content(await apiResponse.Content.ReadAsStringAsync(), apiResponse.Content.Headers.ContentType.MediaType);
            }
        }
    }
}
