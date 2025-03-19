using Newtonsoft.Json;

namespace pokemonAPI.DTO
{
    public class PokemonDetailApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<PokemonTypeWrapper> Types { get; set; }
        public List<PokemonStatWrapper> Stats { get; set; }
    }

    public class PokemonTypeWrapper
    {
        public TypeInfo Type { get; set; }
    }

    public class TypeInfo
    {
        public string Name { get; set; }
    }

    public class PokemonStatWrapper
    {
        [JsonProperty("base_stat")]
        public int BaseStat { get; set; }
        public StatInfo Stat { get; set; }
    }

    public class StatInfo
    {
        public string Name { get; set; }
    }
}
