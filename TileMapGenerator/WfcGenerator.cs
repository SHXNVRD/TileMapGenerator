namespace TileMapGenerator
{
    public class WfcGenerator
    {
        public int Width { get; }
        public int Height { get; }
        private readonly Tile[,] _map;
        private readonly List<TileType> _allTypes;
        private readonly Dictionary<TileType, int> _tileWeights;
        private readonly Dictionary<TileType, Dictionary<Direction, List<TileType>>> _rules;
        private readonly Random _random = new();

        public WfcGenerator(
            int width, 
            int height, 
            List<TileType> allTypes, 
            Dictionary<TileType, int> tileWeights,
            Dictionary<TileType, Dictionary<Direction, List<TileType>>> rules)
        {
            Width = width;
            Height = height;
            _map = new Tile[Height, Width];
            _allTypes = allTypes;
            _tileWeights = tileWeights;
            _rules = rules;
        }

        private void InitializeMap()
        {
            FillMap();
            SetNeighbours();
        }

        private void FillMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _map[y, x] = new Tile(_allTypes);
                }
            }
        }
        
        private void SetNeighbours()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var tile = _map[y, x];
                    if (y > 0) tile.AddNeighbour(Direction.North, _map[y - 1, x]);
                    if (x < Width - 1) tile.AddNeighbour(Direction.East, _map[y, x + 1]);
                    if (y < Height - 1) tile.AddNeighbour(Direction.South, _map[y + 1, x]);
                    if (x > 0) tile.AddNeighbour(Direction.West, _map[y, x - 1]);
                }
            }
        }

        private void PropagateConstraints(Tile collapsedTile)
        {
            Stack<Tile> stack = [];
            stack.Push(collapsedTile);

            while (stack.Count > 0)
            {
                var tile = stack.Pop();

                foreach (var direction in Enum.GetValues<Direction>())
                {
                    var neighbour = tile.GetNeighbour(direction);
                    if (neighbour == null || neighbour.Entropy == 1) continue;

                    var allowedTypes = tile.Possibilities
                        .SelectMany(type => _rules[type][direction])
                        .Distinct()
                        .ToList();

                    if (neighbour.Constrain(allowedTypes))
                        stack.Push(neighbour);
                }
            }
        }

        private List<Tile> GetTilesLowestEntropy(List<Tile> source)
        {
            var lowestEntropy = _allTypes.Count;
            List<Tile> tilesLowestEntropy = [];

            foreach (var tile in source)
            {
                if (tile.Entropy == 1)
                    continue;

                if (tile.Entropy < lowestEntropy)
                {
                    tilesLowestEntropy.Clear();
                    lowestEntropy = tile.Entropy;
                }

                if (tile.Entropy == lowestEntropy)
                    tilesLowestEntropy.Add(tile);
            }

            return tilesLowestEntropy;
        }
        
        public void GenerateMap()
        {
            InitializeMap();
            
            var tilesToProcess = new List<Tile>(_map.Cast<Tile>());
            var count = 0;

            while (tilesToProcess.Count > 0)
            {
                // var tilesLowestEntropy = GetTilesLowestEntropy(tilesToProcess);
                // var index = _random.Next(tilesLowestEntropy.Count);
                // var minEntropyTile = tilesLowestEntropy[index];

                 var minEntropyTile = tilesToProcess
                    .Where(t => t.Entropy > 1)
                    .MinBy(t => t.Entropy);
                 
                 if (minEntropyTile == null)
                     return;
                 
                 minEntropyTile.Collapse(_random, _tileWeights);
                 Console.WriteLine($"Iteration: {count} | Possibility: {minEntropyTile.Possibilities[0]}:{minEntropyTile.Entropy}");
                 PropagateConstraints(minEntropyTile);
                 tilesToProcess.RemoveAll(t => t.Entropy == 1);

                 count++;
            }
        }
        public TileType GetTileType(int x, int y)
            => _map[y, x].Possibilities[0];
    }
}