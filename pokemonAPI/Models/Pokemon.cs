using System.ComponentModel.DataAnnotations;

namespace pokemonAPI.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
    
    public class PokemonDetails
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<string> Types { get; set; }
        public Dictionary<string, int> Stats { get; set; } 
    }
}
