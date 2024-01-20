﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PokéService.Data;
using PokéService.Entities;

namespace PokéService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokéController : ControllerBase
    {
        private readonly PokéServiceContext _context;

        public PokéController(PokéServiceContext context)
        {
            _context = context;
        }

        // Collection: api/Poké/collection

        // GET: api/Poké/collection/5
        [HttpGet("collection/{id}")]
        public async Task<ActionResult<string[]?>> GetCollection(int id) // get all cards ids
        {
            var c = await _context.CardsCollection.FindAsync(id);

            if (c == null) return NotFound("collection not found");

            return Ok(c.CardsArray);
        }

        // GET: api/Poké/collection/5/card
        [HttpGet("collection/{id}/card")]
        public async Task<ActionResult<IEnumerable<Pokémon>>> GetPokémonCollection(int id) // get all cards data
        {
            var c = await _context.CardsCollection.FindAsync(id);

            if (c == null) return NotFound("collection not found");

            var p = new List<Pokémon>();

            string[]? cd = c.CardsArray;

            if (cd != null) foreach (var cid in cd)
            {
                var pk = await _context.Pokémon.FindAsync(cid);
                if (pk != null) p.Add(pk);
            }

            return Ok(p);
        }


        // POST: api/Poké/collection/5
        [HttpPost("collection/{id}")]
        public async Task<ActionResult> CreateCollection(int id) // create collection
        {
            var c = await _context.CardsCollection.FindAsync(id);

            if (c == null) return Ok(); // Already exists

            await _context.AddAsync(new CardsCollection { Id = id });
            await _context.SaveChangesAsync();
            return Created();
        }

        // DELETE: api/Poké/collection/5
        [HttpDelete("collection/{id}")]
        public async Task<ActionResult> DeleteCollection(int id) // delete collection
        {
            var c = await _context.CardsCollection.FindAsync(id);

            if (c == null) return NotFound("collection not found");

            _context.CardsCollection.Remove(c);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Poké/collection/5/card/1
        [HttpPost("collection/{id}/card/{cid}")]
        public async Task<ActionResult> AddCard(int id, string cid) // add card to collection
        {
            var c = await _context.CardsCollection.FindAsync(id);

            if (c == null) return NotFound("collection not found");

            var p = await _context.Pokémon.FindAsync(cid);

            if (p == null) return NotFound("card not found");

            if (c.AddCard(cid)) return Ok();

            return BadRequest("card already in the collection");
        }

        // DELETE: api/Poké/collection/5/card/1
        [HttpDelete("collection/{id}/card/{cid}")]
        public async Task<ActionResult> DeleteCard(int id, string cid) // delete card from collection
        {
            var c = await _context.CardsCollection.FindAsync(id);

            if (c == null) return NotFound("collection not found");

            if (c.RemoveCard(cid)) return Ok();

            return NotFound("card not in the collection");
        }



        // Pokémon: api/Poké/card

        // GET: api/Poké/card/5
        [HttpGet("card/{id}")]
        public async Task<ActionResult<Pokémon>> GetCard(string id)
        {
            var p = await _context.Pokémon.FindAsync(id);

            if (p == null) return NotFound();

            return Ok(p);
        }

        // GET: api/Poké/search?name=&set=&category=&lid=&type1=&type2=&hp=&illustrator=&limit=&offset=
        [HttpGet("search")]
        public ActionResult<IEnumerable<Pokémon>> SearchCard(
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
            Console.WriteLine($">>{category}");
            var pkmns = _context.Pokémon.AsQueryable();
            if (pkmns == null) return NotFound();
            if (name != null) pkmns = pkmns.Where(p => p.Name.ToLower().StartsWith(name.ToLower()) ||    // check name start: "Pika" -> "Pikachu"
                                                       p.Name.ToLower().Contains(' ' + name.ToLower())); // check subwords starts: "Pika" -> "Trainer's Pikachu"
            if (set != null) pkmns = pkmns.Where(p => p.Set == set);
            if (category != null) pkmns = pkmns.Where(p => p.Category == category);
            if (lid != null) pkmns = pkmns.Where(p => p.LocalId == lid);
            if (type1 != null) pkmns = pkmns.Where(p => p.Types != null ? p.Types.Contains(type1): false);
            if (type2 != null) pkmns = pkmns.Where(p => p.Types != null ? p.Types.Contains(type2) : false);
            if (hp != null) pkmns = pkmns.Where(p => p.Hp == hp);
            if (illustrator != null) pkmns = pkmns.Where(p => p.Illustrator.Contains(illustrator));
            pkmns = pkmns.Skip(offset).Take(limit);

            return Ok(pkmns);
        }

    }
}
