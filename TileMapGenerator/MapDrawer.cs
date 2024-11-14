using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TileMapGenerator
{
    public class MapDrawer
    {
        private readonly string _spritesheetPath;
        private readonly int _tileSize;
        private readonly int _scaleTile;
        private readonly WfcGenerator _generator;

        public MapDrawer(int tileSize, int tileScale, string spritesheetPath, WfcGenerator generator)
        {
            _tileSize = tileSize;
            _scaleTile = tileScale;
            _spritesheetPath = spritesheetPath;
            _generator = generator;
        }
        
        public void RenderMap(string outputPath)
        {
            _generator.GenerateMap();
            using var spritesheet = Image.Load<Rgba32>(_spritesheetPath);
            using var mapImage = new Image<Rgba32>(_generator.Width * _tileSize * _scaleTile, _generator.Height * _tileSize * _scaleTile);
            
            for (int y = 0; y < _generator.Height; y++)
            {
                for (int x = 0; x < _generator.Width; x++)
                {
                    var tileType = _generator.GetTileType(x, y);
                    
                    var tileRectangle = GetTileRectangle(spritesheet, tileType);

                    using var tileImage = spritesheet.Clone(ctx => ctx.Crop(tileRectangle));

                    tileImage.Mutate(ctx => ctx.Resize(_tileSize * _scaleTile, _tileSize * _scaleTile));
                    
                    mapImage.Mutate(ctx => ctx.DrawImage(tileImage, new Point(x * _tileSize * _scaleTile, y * _tileSize * _scaleTile), 1));
                }
            }

            mapImage.Save(outputPath, new PngEncoder());
            Console.WriteLine($"Карта сохранена по пути: {outputPath}");
        }
        
        private Rectangle GetTileRectangle(Image<Rgba32> spritesheet, TileType tileType)
        {
            int tilesPerRow = spritesheet.Width / _tileSize;
            int x = ((int)tileType % tilesPerRow) * _tileSize;

            return new Rectangle(x, 0, _tileSize, _tileSize);
        }
    }
}