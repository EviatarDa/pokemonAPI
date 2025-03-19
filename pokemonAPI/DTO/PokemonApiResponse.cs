namespace pokemonAPI.DTO
{
    public class PokemonApiResponse
    {
        public List<PokemonApiResult> Results { get; set; }
    }

    public class PokemonApiResult
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
