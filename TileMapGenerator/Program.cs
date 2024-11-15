﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TileMapGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 190;
            int height = 100;
            int generationCount = 1;
            
            WfcGenerator generator = new(width, height,
                [
                    TileType.CornerN,
                    TileType.CornerE,
                    TileType.CornerS,
                    TileType.CornerW,
                    TileType.CrossWE,
                    TileType.CrossNS,
                    TileType.LineNS,
                    TileType.LineWE,
                    TileType.Empty
                ],
            new Dictionary<TileType, int>
            {
                { TileType.CornerN, 1 },
                { TileType.CornerE, 1 },
                { TileType.CornerS, 1 },
                { TileType.CornerW, 1 },
                { TileType.CrossWE, 1 },
                { TileType.CrossNS, 1 },
                { TileType.LineNS, 1 },
                { TileType.LineWE, 1 },
                { TileType.Empty, 1 }
            }, 
            new Dictionary<TileType, Dictionary<Direction, List<TileType>>>
            {
                { TileType.CornerN, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North,[TileType.CornerE, TileType.CornerS, TileType.CrossNS, TileType.CrossWE, TileType.LineNS]},
                    { Direction.East, [TileType.CornerW, TileType.CornerS, TileType.CrossNS, TileType.CrossWE, TileType.LineWE] },
                    { Direction.South, [TileType.CornerE, TileType.CornerS, TileType.LineWE, TileType.Empty] },
                    { Direction.West, [TileType.CornerW, TileType.CornerS, TileType.LineNS, TileType.Empty] }
                }},
                { TileType.CornerE, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerN, TileType.CornerW, TileType.LineWE, TileType.Empty] },
                    { Direction.East, [TileType.CornerW, TileType.CornerS, TileType.CrossNS, TileType.CrossWE, TileType.LineWE] },
                    { Direction.South, [TileType.CornerN, TileType.CornerW, TileType.CrossNS, TileType.CrossWE, TileType.LineNS]},
                    { Direction.West, [TileType.CornerW, TileType.CornerS, TileType.LineNS, TileType.Empty] }
                }},
                { TileType.CornerS, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerN, TileType.CornerW, TileType.LineWE, TileType.Empty] },
                    { Direction.East, [TileType.CornerN, TileType.CornerE, TileType.LineNS, TileType.Empty] },
                    { Direction.South, [TileType.CornerN, TileType.CornerW, TileType.CrossNS, TileType.CrossWE, TileType.LineNS] },
                    { Direction.West, [TileType.CornerN, TileType.CornerN, TileType.CrossNS, TileType.CrossWE, TileType.LineWE] }
                }},
                { TileType.CornerW, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerS, TileType.CornerE, TileType.CrossNS, TileType.CrossWE, TileType.LineNS] },
                    { Direction.East, [TileType.CornerN, TileType.CornerE, TileType.LineNS, TileType.Empty] },
                    { Direction.South, [TileType.CornerS, TileType.CornerE, TileType.LineWE, TileType.Empty] },
                    { Direction.West, [TileType.CornerN, TileType.CornerE, TileType.CrossNS, TileType.CrossWE, TileType.LineWE] }
                }},
                {TileType.CrossWE, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerS, TileType.CornerE, TileType.CrossWE, TileType.LineNS, TileType.LineNS] },
                    { Direction.East, [TileType.CornerW, TileType.CornerS, TileType.CrossNS, TileType.CrossWE, TileType.LineWE]},
                    { Direction.South, [TileType.CornerN, TileType.CornerW, TileType.CrossNS, TileType.CrossWE, TileType.LineNS]},
                    { Direction.West, [TileType.CornerN, TileType.CornerE, TileType.CrossNS, TileType.CrossWE, TileType.LineWE]}
                }},
                {TileType.CrossNS, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerS, TileType.CornerE, TileType.CrossWE, TileType.LineNS, TileType.LineNS] },
                    { Direction.East, [TileType.CornerW, TileType.CornerS, TileType.CrossNS, TileType.CrossWE, TileType.LineWE]},
                    { Direction.South, [TileType.CornerN, TileType.CornerW, TileType.CrossNS, TileType.CrossWE, TileType.LineNS]},
                    { Direction.West, [TileType.CornerN, TileType.CornerE, TileType.CrossNS, TileType.CrossWE, TileType.LineWE]}
                }},
                {TileType.LineNS, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerE, TileType.CornerS, TileType.LineNS, TileType.CrossNS, TileType.CrossWE]},
                    { Direction.East, [TileType.CornerE, TileType.CornerN, TileType.LineNS, TileType.Empty]},
                    { Direction.South, [TileType.CornerN, TileType.CornerW, TileType.LineNS, TileType.CrossNS, TileType.CrossWE]},
                    { Direction.West, [TileType.CornerS, TileType.CornerW, TileType.LineNS, TileType.Empty]}
                }},
                {TileType.LineWE, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerN, TileType.CornerW, TileType.LineWE, TileType.Empty] },
                    { Direction.East, [TileType.CornerW, TileType.CornerS, TileType.LineWE, TileType.CrossNS, TileType.CrossWE]},
                    { Direction.South, [TileType.CornerS, TileType.CornerE, TileType.LineWE, TileType.Empty]},
                    { Direction.West, [TileType.CornerN, TileType.CornerE, TileType.LineWE, TileType.CrossNS, TileType.CrossWE]}
                }},
                {TileType.Empty, new Dictionary<Direction, List<TileType>>
                {
                    { Direction.North, [TileType.CornerN, TileType.CornerW, TileType.Empty] },
                    { Direction.East, [TileType.CornerN, TileType.CornerE, TileType.Empty]},
                    { Direction.South, [TileType.CornerE, TileType.CornerS, TileType.Empty]},
                    { Direction.West, [TileType.CornerS, TileType.CornerW, TileType.Empty]}
                }}
            });

            var tileSheetPath = $"{AppDomain.CurrentDomain.BaseDirectory}/TileSheets/corners2.png";
            var outputPath = "/home/rodion/source/TileMapGenerator/TileMapGenerator/Maps";
            MapDrawer drawer = new(10, 1, tileSheetPath, generator);

            for (int i = 0; i < generationCount; i++)
            {
                drawer.RenderMap($"{outputPath}/New-tile-map-{i}.png");
            }
        }
    }
}
