using System.Runtime.InteropServices;

namespace Day3;

public class JoltageFinder
{
    public static long Q1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        long sum = 0; 

        foreach (var line in File.ReadAllLines(filePath))
        {
            int maxJoltage = FindMaxJoltage(line.Select(c => c - 48).ToArray());

            sum += maxJoltage;

            Console.WriteLine(maxJoltage); 
        }

        Console.WriteLine($"Sum is {sum}");

        return sum; 
    }

    private static int FindMaxJoltage(int[] line)
    {
        var max = new Digits(line[0], line[1]);

        for (int i = 2; i < line.Length; i++)
        {
            var digitsOption1 = new Digits(max.Digit0, line[i]);
            var digitsOption2 = new Digits(max.Digit1, line[i]);

            if (max.Value() >= digitsOption1.Value())
            {
                if (max.Value() >= digitsOption2.Value())
                {
                    continue; // do nothing
                }
                else
                {
                    max = digitsOption2; 
                }
            }
            else
            {
                if (digitsOption1.Value() >= digitsOption2.Value())
                {
                    max = digitsOption1; 
                }
                else
                {
                    max = digitsOption2; 
                }
            }
        }

        return max.Value(); 
    }

    public record struct Digits(int Digit0, int Digit1)
    {
        public int Value() => Digit0 * 10 + Digit1;
    }


    public static long Q2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        long sum = 0;

        foreach (var line in File.ReadAllLines(filePath))
        {
            long maxJoltage = FindMaxJoltage12(line.Select(c => c - 48).ToArray());

            sum += maxJoltage;

            Console.WriteLine(maxJoltage);
        }

        Console.WriteLine($"Sum is {sum}");

        return sum;
    }

    private static long FindMaxJoltage12(int[] line)
    {
        Digits12 max = new Digits12(line[0], line[1], line[2], line[3], line[4], line[5], line[6], line[7], line[8], line[9], line[10], line[11]);

        List<long> maxValues = new();
        maxValues.Add(max.Value); 
        
        for (int i = 12; i < line.Length; i++)
        {
            var newDigitToCompare = line[i];
            var maxList = new List<Digits12>();
            maxList.Add(max); 
            
            // this index will be dropped as new element will be added to end for comparison
            for (int indexToDrop = 0; indexToDrop < 12; indexToDrop++)
            {
                var array = new int[12];
                array[11] = newDigitToCompare;
                var nextIndex = 0;

                for (int k = 0; k < 12; k++)
                {
                    if (k == indexToDrop)
                        continue; 

                    array[nextIndex++] = max.Myvalues[k];
                }

                var candidateMax = new Digits12(array);

                maxList.Add(candidateMax); 
            }

            max = maxList.OrderByDescending(m => m.Value).First();  
        }

        return max.Value;
    }

    public class Digits12
    {
        public Digits12(params int[] values)
        {
            Value = GetValues(values); 
            Myvalues = values;
        }
        public int[] Myvalues { get; }

        public static Digits12 Default = new Digits12(0);

        public long Value { get; private set; }

        private long GetValues(params int[] values)
        {
            Value = 0; 

            for (int i = 0; i < values.Length; i++)
            {
                Value += values[i] * (long)Math.Pow(10, 11 - i); 
            }

            return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
