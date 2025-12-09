using System.IO;

namespace Day9;

public class TileAreaCalculator
{
    public static long Question1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);


        var tiles = new List<Tile>();

        foreach (var line in File.ReadAllLines(filePath))
        {
            var nums = line.Split(',').Select(t => int.Parse(t)).ToArray();

            tiles.Add(new (nums[0], nums[1]));
        }

        long maxArea = 0; 

        foreach (var tile in tiles)
        {
            foreach (var tile2 in tiles)
            {
                var area = Tile.Area(tile, tile2); 

                if(area > maxArea)
                    maxArea = area;
            }
        }

        return maxArea; 
    }

    public static long Question2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var tiles = new List<Tile>();

        var file = File.ReadAllLines(filePath);

        foreach (var line in File.ReadAllLines(filePath))
        {
            var nums = line.Split(',').Select(t => int.Parse(t)).ToArray();

            tiles.Add(new(nums[0], nums[1]));
        }

        var mapX = new Dictionary<long, int>();
        var mapY = new Dictionary<long, int>();

        var mapXR = new Dictionary<int, long>();
        var mapYR = new Dictionary<int, long>();

        var (linesX, linesY) = ReturnTilesArray(tiles, mapX, mapY, mapXR, mapYR);


        var tilesNew = new List<Tile>();

        foreach (var tile in tiles)
        {
            tilesNew.Add(new Tile(mapX[tile.X], mapY[tile.Y]));
        }

        //var fileName2 = "3.csv";
        //var location2 = AppContext.BaseDirectory;
        //var filePath2 = Path.Combine(location2, fileName2);
        //using var writer = new StreamWriter(filePath2);
        //var array = new int[linesX.Count, linesY.Count];

        //for (int x = 1; x < linesX.Count; x++)
        //{
        //    var line = linesX[x];
        //    var min = Math.Min(line.start, line.end);
        //    var max = Math.Max(line.start, line.end);

        //    for (int y = 0; y < linesY.Count; y++)
        //    {
        //        if (y >= min && y <= max)
        //            array[x, y] = 1;
        //    }
        //}

        //for (int y = 1; y < linesY.Count; y++)
        //{
        //    var line = linesY[y];
        //    var min = Math.Min(line.start, line.end);
        //    var max = Math.Max(line.start, line.end);

        //    for (int x = 0; x < linesX.Count; x++)
        //    {
        //        if (x >= min && x <= max)
        //            array[x, y] = 1;
        //    }
        //}

        //for (int x = 0; x < linesX.Count; x++)
        //{
        //    writer.WriteLine();
            
        //    for (int y = 0; y < linesY.Count; y++)
        //    {
        //        if (y != 0)
        //            writer.Write(",");

        //        if (array[x, y] == 1)
        //            writer.Write(1);
        //        else
        //            writer.Write(0);
        //    }
        //}

        long maxArea = 0;
        Tile a = default;
        Tile b = default;

        List<(Tile a, Tile b, long result)> areas = new();


        foreach (var tile in tilesNew)
        {
            foreach (var tile2 in tilesNew)
            {
                if (tile == tile2 || tile.X == tile2.X || tile.Y == tile2.Y)
                    continue;

                var (isValid, area) = ReturnArea(linesX, linesY, tile, tile2, mapXR, mapYR, mapX, mapY);

                if (isValid)
                {
                    a = tile;
                    b = tile2;
                    areas.Add((a, b, area));

                    if(area > maxArea)
                        maxArea = area;
                    
                    
                }

            }
        }

        areas = areas.OrderByDescending(a => a.result).ToList(); 

        return maxArea;
    }

    private static (Dictionary<int, Line> linesX, Dictionary<int, Line> linesY) ReturnTilesArray(List<Tile> tiles, Dictionary<long, int> mapX, 
        Dictionary<long, int> mapY, Dictionary<int, long> mapXR, Dictionary<int, long> mapYR)
    {
        mapX[0] = 0;
        mapY[0] = 0;
        mapXR[0] = 0;
        mapYR[0] = 0;

        var indexX = 1;
        var indexY = 1;

        foreach (var tile in tiles.OrderBy(t => t.X))
        {
            if (mapX.ContainsKey(tile.X))
                continue; ;

            mapX[tile.X] = indexX;
            mapXR[indexX] = tile.X;
            indexX += 1; 
        }

        foreach (var tile in tiles.OrderBy(t => t.Y))
        {
            if (mapY.ContainsKey(tile.Y))
                continue;

            mapY[tile.Y] = indexY;
            mapYR[indexY] = tile.Y;
            indexY += 1;
        }

        var set = tiles.ToHashSet();

        //var array = new int[indexX, indexY];
        //var fileName2 = "2.csv";
        //var location2 = AppContext.BaseDirectory;
        //var filePath2 = Path.Combine(location2, fileName2);
        //var writer = new StreamWriter(filePath2);

        var linesX = new Dictionary<int, Line>(); 

        for (int x = 1; x < indexX; x++)
        {
            var min = int.MaxValue;
            var max = -1; 
            for (int y = 1; y < indexY; y++)
            {
                var tileX = mapXR[x];
                var tileY = mapYR[y];

                if (set.Contains(new Tile(tileX, tileY)))
                {
                    min = Math.Min(min, y);
                    max = Math.Max(max, y); 
                }
                    
            }

            linesX[x] = new(x, min, max); // horizontal line
        }

        var linesY = new Dictionary<int, Line>();
        for (int y = 1; y < indexY; y++)
        {
            var min = int.MaxValue;
            var max = -1;
            for (int x = 1; x < indexX; x++)
            {
                var tileX = mapXR[x];
                var tileY = mapYR[y];

                if (set.Contains(new Tile(tileX, tileY)))
                {
                    min = Math.Min(min, x);
                    max = Math.Max(max, x); 
                }
                    
            }

            linesY[y] = new(y, min, max); // horizontal line
        }

        return (linesX, linesY);
    }

    private static (bool isValid, long area) ReturnArea(Dictionary<int, Line> linesX, Dictionary<int, Line> linesY, 
        Tile a, Tile b, Dictionary<int, long> MapXR, Dictionary<int, long> MapYR,
        Dictionary<long, int> MapX, Dictionary<long, int> MapY)
    {
        var minX = Math.Min(a.X, b.X);
        var maxX = Math.Max(a.X, b.X);

        var minY = Math.Min(a.Y, b.Y);
        var maxY = Math.Max(a.Y, b.Y);

        for (long h = minX + 1; h < maxX; h++)
        {
           
            var line = linesX[(int)h];
            var start = Math.Min(line.start, line.end);
            var end = Math.Max(line.start, line.end);

            var overlaps = (minY > start && minY < end) ||
                (maxY > start && maxY < end)
                || (start > minY && start < maxY)
                || (end > minY && end < maxY);
            if (overlaps) // if there's a line in between -> one side is not included
                return (false, -1);

        }

        var lineYsMax = linesY.OrderByDescending(l => l.Value.Length).Take(2).ToList(); 

        for (long l = minY + 1; l < maxY; l++)
        {

            var line = linesY[(int)l];
            var start = Math.Min(line.start, line.end);
            var end = Math.Max(line.start, line.end);

            var overlaps = (start > minX && start < maxX) ||
                (minX > start && minX < end) ||
                (maxX > start && maxX < end) || 
                (end > minX && end < maxX);
            if (overlaps) // if there's a line in between -> one side is not included
                return (false, -1);
        }

        var aNew = new Tile(MapXR[(int)a.X], MapYR[(int)a.Y]);
        var bNew = new Tile(MapXR[(int)b.X], MapYR[(int)b.Y]);

        return (true, Tile.Area(aNew, bNew));
            
    }

    public record struct Tile(long X, long Y)
    {
        public static long Area(Tile a, Tile b)
        {
            return (Math.Abs(a.X - b.X) + 1) * (Math.Abs(a.Y - b.Y) + 1); 
        }
    }

    public record struct Line(int index, int start, int end) 
    {
        public int Length => Math.Abs(end - start + 1);
    }

}
