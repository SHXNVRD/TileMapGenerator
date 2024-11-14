namespace TileMapGenerator.Extensions
{
    public static class RandomExtensions
    {
        public static T? Choice<T>(this Random rnd, IEnumerable<T> choices, IEnumerable<int> weights)
        {
            var cumulativeWeight = new List<int>();
            int last = 0;
            
            foreach (var cur in weights)
            {
                last += cur;
                cumulativeWeight.Add(last);
            }
            
            var choice = rnd.Next(last);
            int i = 0;
            
            foreach (var cur in choices)
            {
                if (choice < cumulativeWeight[i])
                {
                    return cur;
                }
                i++;
            }
            return default;
        }
    }
}