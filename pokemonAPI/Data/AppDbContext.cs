using Microsoft.EntityFrameworkCore;
using pokemonAPI.Models;

namespace pokemonAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<FavoritePokemon> FavoritePokemons { get; set; }
    }
}
