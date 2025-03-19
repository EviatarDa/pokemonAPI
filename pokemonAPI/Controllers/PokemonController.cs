using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pokemonAPI.Models;
using Newtonsoft.Json;
using pokemonAPI.DTO;
using System.Net.Http;
using pokemonAPI.Services;
using Microsoft.Data.SqlClient;


namespace pokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

   public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPokemons([FromQuery] int offset = 0, [FromQuery] int limit = 20)
        {
            try
            {
                var pokemons = await _pokemonService.GetPokemonsAsync(offset, limit);
                if (!pokemons.Any()) return NotFound("No pokemons found");

                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{identifier}")]
        public async Task<IActionResult> GetPokemonDetails(string identifier)
        {
            try
            {
                var pokemonDetails = await _pokemonService.GetPokemonDetailsAsync(identifier);
                if (pokemonDetails == null) return NotFound($"Pokemon '{identifier}' not found");

                return Ok(pokemonDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("{pokemonId}")]
        public async Task<IActionResult> AddFavoritePokemon(int pokemonId)
        {
            try
            {
                var existingFavorite = await _pokemonService.GetFavoritePokemonByIdAsync(pokemonId);

                if (existingFavorite == null)
                {
                    await _pokemonService.AddFavoritePokemonAsync(pokemonId);
                    return CreatedAtAction(nameof(GetPokemons), new { id = pokemonId }, null);
                }
                return Ok($"Pokemon with ID {pokemonId} is already marked as favorite.");
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Database is currently unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpDelete("{pokemonId}")]
        public async Task<IActionResult> RemoveFavoritePokemon(int pokemonId)
        {
            try
            {
                var existingFavorite = await _pokemonService.GetFavoritePokemonByIdAsync(pokemonId);

                if (existingFavorite != null)
                {
                    await _pokemonService.RemoveFavoritePokemonAsync(pokemonId);
                    return NoContent(); // מחזיר 204 No Content אחרי ההסרה
                }
                else
                {
                    return Ok($"Pokemon with ID {pokemonId} was not found in favorites.");
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Database is currently unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoritePokemons()
        {
            try
            {
                var favorites = await _pokemonService.GetFavoritePokemonsAsync();
                return Ok(favorites);
            }
            catch (SqlException ex)
            {
                return StatusCode(503, "Database is currently unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
