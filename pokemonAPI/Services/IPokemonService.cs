using Microsoft.EntityFrameworkCore;
using pokemonAPI.Models;

//using pokemonAPI.DTO;
//using System.Threading.Tasks;

namespace pokemonAPI.Services
{
    public interface IPokemonService
    {
        Task<List<Pokemon>> GetPokemonsAsync(int offset = 0, int limit = 20);
        Task<PokemonDetails?> GetPokemonDetailsAsync(string identifier);
        Task<FavoritePokemon?> GetFavoritePokemonByIdAsync(int pokemonId);
        Task AddFavoritePokemonAsync(int pokemonId);
        Task RemoveFavoritePokemonAsync(int pokemonId);
        Task<List<int>> GetFavoritePokemonsAsync();
    }
}
