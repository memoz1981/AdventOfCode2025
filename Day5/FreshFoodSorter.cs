using System.Globalization;
using System.Net.Sockets;

namespace Day5;

public class FreshFoodSorter
{
    public static void Question1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var idsStart = false; 
        var foodRangeStorage = new FoodRangeStorage();
        int freshCount = 0; 

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                idsStart = true;
                continue; 
            }
                

            if (!idsStart)
            {
                var min = long.Parse(line.Split('-').First());
                var max = long.Parse(line.Split('-').Last());

                foodRangeStorage.AddRange((min, max)); 
                
                continue; 
            }

            // process the ids
            if (foodRangeStorage.IsFresh(long.Parse(line)))
                freshCount++; 
        }

        Console.WriteLine(freshCount); 
    }

    public static void Question2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var idsStart = false;
        var foodRangeStorage = new FoodRangeStorage();

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            if (!idsStart)
            {
                var min = long.Parse(line.Split('-').First());
                var max = long.Parse(line.Split('-').Last());

                foodRangeStorage.AddRange((min, max));

                continue;
            }
        }

        var freshCount = foodRangeStorage.GetCount(); 

        Console.WriteLine(freshCount);
    }

    public class FoodRangeStorage()
    {
        private List<(long min, long max)> _ranges = new();

        public void AddRange((long min, long max) values)
        {
            _ranges.Add(values);
        }

        public long GetCount()
        {
            long count = 0;
            var (rangesNew, anyMatchFound) = Combine(_ranges);
            while (true)
            {
                (rangesNew, anyMatchFound) = Combine(rangesNew); 

                if (!anyMatchFound)
                {
                    foreach (var range in rangesNew)
                        count += range.max - range.min + 1;
                    break; 
                }
            }

            return count; 
        }

        private static (List<(long min, long max)> rangesNew, bool matchFound) Combine(List<(long min, long max)> ranges)
        {
            var rangesNew = new List<(long min, long max)>();
            bool anyMatchFound = false;
            // for each range in all ranges -> we need to see if we can combine it with others
            // we really don't want to count all ranges and get distinct values... (brute force)
            // for this -> we need to iterate over the ranges, see if 2 ranges can be merged... 
            // for 2 ranges to merge -> below conditions should exist
            // either range1.min should be in range 2 or range1.max should be in that range inclusive
            // in that case -> we just take the min as min of 2 and max as max of 2
            // if no matching range found -> we just add that as a separate line
            for (int j = 0; j < ranges.Count; j++)
            {
                var range = ranges[j];
                var matchFound = false;

                for (int i = 0; i < rangesNew.Count; i++)
                {
                    var rangeNew = rangesNew[i];
                    //match found
                    if ((range.min >= rangeNew.min && range.min <= rangeNew.max)
                        || (range.max >= rangeNew.min && range.max <= rangeNew.max))
                    {
                        rangeNew.min = Math.Min(range.min, rangeNew.min);
                        rangeNew.max = Math.Max(range.max, rangeNew.max);

                        rangesNew[i] = rangeNew;
                        matchFound = true;
                        break;
                    }
                }

                anyMatchFound |= matchFound; 

                if (!matchFound)
                    rangesNew.Add(range);
            }

            rangesNew = rangesNew.OrderBy(r => r.min).ToList(); 

            return (rangesNew,  anyMatchFound);
        }


        public bool IsFresh(long id)
        {
            foreach (var range in _ranges)
            {
                if (id >= range.min && id <= range.max)
                    return true; 
            }

            return false;
        }
    }
}
