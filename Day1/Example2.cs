using System.ComponentModel.DataAnnotations;

namespace Day1;

public class Example2 {
    
    // Brute force method (fast and effective:) )
    public static int ReturnZerosClicked()
    {
        var fileName = "1.txt";

        var dial = 50;

        var zeroCount = 0;

        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (line is null)
                throw new ArgumentException();

            var number = int.Parse(line.Substring(1));
            var decrement = 1; 

            if (line[0] == 'L')
                decrement = -1;
            else if (line[0] != 'R')
                throw new ArgumentException();

            for (int i = 0; i < number; i++)
            {
                dial = (dial + decrement)%100;

                if (dial == 0)
                    zeroCount++; 
            }
        }

        return zeroCount;
    }

    // correct approach but slow (in terms of getting into for untrained mind)
    public static int ReturnZerosClickedNew()
    {
        var fileName = "1.txt";
        
        var dialPlus = 50;
        var dialMinus = -50; 

        var zeroCount = 0;

        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (line is null)
                throw new ArgumentException();

            var number = int.Parse(line.Substring(1));

            if (line[0] == 'L')
            {
                var total = dialMinus - number;
                zeroCount += Math.Abs(total / 100);

                dialPlus = ((total % 100) + 100) % 100;
                dialMinus = (dialPlus - 100) % 100;
                continue; 
            }

            if (line[0] == 'R')
            {
                var total = dialPlus + number;
                zeroCount += Math.Abs(total / 100);

                dialPlus = ((total % 100) + 100) % 100;
                dialMinus = (dialPlus - 100) % 100;
                continue; 
            }
            else
                throw new ArgumentException();
        }

        return zeroCount;
    }
}

