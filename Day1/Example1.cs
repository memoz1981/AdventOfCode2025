namespace Day1;

public class Example1
{
    public static int ReturnZerosReached()
    {
        var fileName = "1.txt";

        var start = 50;

        var zeroCount = 0;

        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (line is null)
                throw new ArgumentException();

            var number = int.Parse(line.Substring(1));

            if (line[0] == 'L')
                number = -number;
            else if (line[0] != 'R')
                throw new ArgumentException();

            start += (number + 100);

            start %= 100;

            if (start == 0)
                zeroCount++;

        }

        return zeroCount;
    }
}
