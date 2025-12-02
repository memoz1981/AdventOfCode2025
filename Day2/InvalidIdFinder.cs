using System.Text;

namespace Day2;

public class InvalidIdFinder
{
    public static long FindInvalidIds_Q1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var subRanges = File.ReadAllText(filePath).Split(',')
            .SelectMany(tx => new Range(tx).SubRanges);

        foreach (var range in subRanges)
        {
            Console.WriteLine(range.ToString());
        }

        var result = subRanges.SelectMany(r => SplitBasedOnFirstHalf(r)).Sum(r => r.StartIndex);

        Console.WriteLine(result); 

        return result; 
    }

    // this works for first quesiton...
    private static IEnumerable<Range> SplitBasedOnFirstHalf(Range range)
    {
        if (range.NumberOfDigits % 2 == 1)
            yield break;


        var startString = range.StartIndex.ToString().Substring(0, range.NumberOfDigits.Value / 2);
        var endString = range.EndIndex.ToString().Substring(0, range.NumberOfDigits.Value / 2);

        var start = int.Parse(startString);
        var end = int.Parse(endString);

        for (int i = start; i <= end; i++)
        {
            var potentialOption = long.Parse($"{i}{i}");

            if (potentialOption >= range.StartIndex && potentialOption <= range.EndIndex)
                yield return new Range(potentialOption, potentialOption, range.NumberOfDigits.Value);
        }
    }

    public static long FindInvalidIds_Q2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var subRanges = File.ReadAllText(filePath).Split(',')
            .SelectMany(tx => new Range(tx).SubRanges);

        foreach (var range in subRanges)
        {
            Console.WriteLine(range.ToString());
        }

        var result = subRanges.SelectMany(r => SplitBasedOnRepeatingDigits(r)).Distinct();

        foreach (var res in result)
            Console.WriteLine(res); 

        Console.WriteLine(result.Sum());

        return result.Sum(); 
    }

    // this should work for second quesiton...
    private static IEnumerable<long> SplitBasedOnRepeatingDigits(Range range)
    {
        if (range.NumberOfDigits <= 1)
            yield break; 
        
        
        var startString = range.StartIndex.ToString().Substring(0, range.NumberOfDigits.Value / 2);
        var endString = range.EndIndex.ToString().Substring(0, range.NumberOfDigits.Value / 2);

        var start = int.Parse(startString);
        var end = int.Parse(endString);

        for (int i = start; i <= end; i++) // we just divide by first half
        {
            var currentString = i.ToString(); 

            for (int j = 1; j <= range.NumberOfDigits / 2; j++)
            {
                if (range.NumberOfDigits % j != 0)
                    continue; 

                var numberOfRepeats = range.NumberOfDigits / j;

                var builder = new StringBuilder();

                for (int k = 0; k < numberOfRepeats; k++)
                {
                    builder.Append(currentString.Substring(0, j));
                }

                var potentialOption = long.Parse(builder.ToString());

                if (potentialOption >= range.StartIndex && potentialOption <= range.EndIndex)
                    yield return potentialOption;
            }
            
            
        }
    }
}

public class Range
{
    public Range(string rangeString)
    {
        var startString = rangeString.Split('-').First(); 
        var endString = rangeString.Split('-').Last();

        StartIndex = long.Parse(startString);
        EndIndex = long.Parse(endString);

        var startLength = startString.Length;
        var endLength = endString.Length;

        for (int i = startLength; i <= endLength; i++)
        {
            SubRanges.Add(SplitBasedOnDigitCount(i)); 
        }
    }

    private Range SplitBasedOnDigitCount(int length)
    {
        var start = (long)Math.Max(Math.Pow(10, length - 1), StartIndex);
        var end = (long)Math.Min(Math.Pow(10, length) - 1, EndIndex);

        return new Range(start, end, length);
    }

    public Range(long startIndex, long endIndex, int numberOfDigits)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        NumberOfDigits = numberOfDigits; 
    }
    

    public long StartIndex { get; set; }
    public long EndIndex { get; set; }
    public int? NumberOfDigits { get; set; } = null; 
    public List<Range> SubRanges { get; set; } = new();

    public override string ToString()
        => $"{StartIndex} - {EndIndex} - {NumberOfDigits ?? -1}";
}
