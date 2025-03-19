using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using pokemonAPI.Data;
using pokemonAPI.DTO;
using pokemonAPI.Models;

namespace pokemonAPI.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;
        private readonly string _baseUrl = "https://pokeapi.co/api/v2/pokemon";

        public PokemonService(IHttpClientFactory httpClientFactory, AppDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }
        public async Task<List<Pokemon>> GetPokemonsAsync(int offset = 0, int limit = 20)
        {
            try
            {
                string apiUrl = $"{_baseUrl}?offset={offset}&limit={limit}";
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetStringAsync(apiUrl);

                var apiResult = JsonConvert.DeserializeObject<PokemonApiResponse>(response);
                if (apiResult == null || apiResult.Results == null) return new List<Pokemon>();

                return apiResult.Results.Select(p => new Pokemon
                {
                    Id = int.Parse(p.Url.TrimEnd('/').Split('/').Last()),
                    Name = p.Name,
                    ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{int.Parse(p.Url.TrimEnd('/').Split('/').Last())}.png",
                }).ToList();
            }
            catch (HttpRequestException)
            {
                throw new Exception("שגיאה בגישה ל-PokeAPI");
            }
        }

        public async Task<PokemonDetails?> GetPokemonDetailsAsync(string identifier)
        {
            try
            {
                string apiUrl = $"{_baseUrl}/{identifier}";
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetStringAsync(apiUrl);
                var details = JsonConvert.DeserializeObject<PokemonDetailApiResponse>(response);

                if (details == null) return null;

                return new PokemonDetails
                {
                    Height = details.Height,
                    Weight = details.Weight,
                    Types = details.Types.Select(t => t.Type.Name).ToList(),
                    Stats = details.Stats.ToDictionary(s => s.Stat.Name, s => s.BaseStat)
                };
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<FavoritePokemon?> GetFavoritePokemonByIdAsync(int pokemonId)
        {
            return await _context.FavoritePokemons.FirstOrDefaultAsync(f => f.PokemonId == pokemonId);
        }

        public async Task AddFavoritePokemonAsync(int pokemonId)
        {
            var favoritePokemon = new FavoritePokemon
            {
                PokemonId = pokemonId,
            };

            _context.FavoritePokemons.Add(favoritePokemon);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoritePokemonAsync(int pokemonId)
        {
            var favoritePokemon = await _context.FavoritePokemons.FirstOrDefaultAsync(f => f.PokemonId == pokemonId);

            if (favoritePokemon != null)
            {
                _context.FavoritePokemons.Remove(favoritePokemon);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<int>> GetFavoritePokemonsAsync()
        {
            return await _context.FavoritePokemons
                .Select(f => f.PokemonId)
                .ToListAsync();
        }
    }
}
