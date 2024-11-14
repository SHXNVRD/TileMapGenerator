using TileMapGenerator.Extensions;

namespace TileMapGenerator
{
    public class Tile
    {
        public List<TileType> Possibilities { get; private set; }
        public int Entropy => Possibilities.Count;
        private readonly Dictionary<Direction, Tile> _neighbours = [];

        public Tile(List<TileType> possibleTypes) =>
            Possibilities = [..possibleTypes];
        
        public void AddNeighbour(Direction direction, Tile neighbour) =>
            _neighbours[direction] = neighbour;

        public Tile? GetNeighbour(Direction direction) =>
            _neighbours.GetValueOrDefault(direction);
        
        public void Collapse(Random random, Dictionary<TileType, int> tileWeights)
        {
            if (Entropy == 1)
                return;
            
            var weights = Possibilities
                .Select(p => tileWeights[p]);

            var choice = random.Choice(Possibilities, weights);
            Possibilities = [choice];
            Console.WriteLine($"Choice: {choice} | Entropy: {Entropy}");
        }
        
        public bool Constrain(IEnumerable<TileType> allowedTypes)
        {
            var initialCount = Possibilities.Count;
            Possibilities = Possibilities.Intersect(allowedTypes).ToList();
            return Possibilities.Count != initialCount;
        }
    }
}