using static Day9.TileAreaCalculator;

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

        var minMaxByRow = ReturnTilesArray(tiles);

        long maxArea = 0; 
        foreach (var tile in tiles)
        {
            foreach (var tile2 in tiles)
            {
                if (tile == tile2)
                    continue;
                
                var (isValid, area) = ReturnArea(minMaxByRow, tile, tile2);

                if (isValid && area > maxArea)
                    maxArea = area;
            }
        }

        return maxArea;
    }

    private static (bool isValid, long area) ReturnArea(Dictionary<long, (long minIndex, long maxIndex)> minMaxByRow, Tile a, Tile b)
    {
        var minX = Math.Min(a.X, b.X);
        var maxX = Math.Max(a.X, b.X);

        var minY = Math.Min(a.Y, b.Y);
        var maxY = Math.Max(a.Y, b.Y);

        for (long h = minX; h <= maxX; h++)
        {
            var (min, max) = minMaxByRow[(int)h];
            if (minY < min || maxY > max)
                return (false, -1);
        }

        return (true, Tile.Area(a, b));
            
    }
    
    private static Dictionary<long, (long min, long max)> ReturnTilesArray(List<Tile> tiles)
    {
        long height = 0;
        long length = 0;
        foreach (var tile in tiles)
        {
            height = Math.Max(height, tile.X + 1);
            length = Math.Max(length, tile.Y + 1);
        }

        var dictionary = new Dictionary<long, (long min, long max)>(); 

        foreach (var tile in tiles)
        {
            long min = int.MaxValue;
            long max = -1; 

            if (dictionary.TryGetValue(tile.X, out var value))
            {
                min = Math.Min(value.min, min);
                max = Math.Max(value.max, max); 
            }

            min = Math.Min(tile.Y, min); 
            max = Math.Max(tile.Y, max);

            dictionary[tile.X] = (min, max); 
        }

        for (int h = 1; h < height; h++)
        {
            long min = int.MaxValue;
            long max = -1; 
            if (dictionary.TryGetValue(h - 1, out var previous))
            {
                min = previous.min;
                max = previous.max;
            }

            if (dictionary.TryGetValue(h, out var current))
            {
                min = Math.Min(current.min, min);
                max = Math.Max(current.max, max); 
            }

            dictionary[h] = (min, max); 
        }

        return dictionary; 
    }

    public record Tile(long X, long Y)
    {
        public static long Area(Tile a, Tile b)
        {
            return Math.Abs((a.X - b.X + 1) * (a.Y - b.Y + 1)); 
        }
    }

}
