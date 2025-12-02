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

        var result = subRanges.Sum(r => r.StartIndex);

        Console.WriteLine(result);

        return result;
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
