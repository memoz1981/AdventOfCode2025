namespace Day10;

public class IndicatorLightHelper
{
    public static long Question1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var lines = new List<Line>(); 
        foreach (var line in File.ReadAllLines(filePath))
        {
            lines.Add(new Line(line)); 
        }

        var result = 0; 

        foreach (var line in lines)
        {
            var depth = 1;
            var numbers = line.Buttons.Select(b => b).ToHashSet(); 

            while (true)
            {
                var (resultFound, numbersNew) = ReturnNewNumbersOrResult(numbers, line);

                if (resultFound)
                {
                    result += depth;
                    break; 
                }

                depth++;
                numbers = numbersNew; 
            }
        }

        return result; 
    }

    public static (bool resultFound, HashSet<int> numbersNew) ReturnNewNumbersOrResult(HashSet<int> numbers, Line line)
    {
        foreach (var num in numbers)
        {
            if (num == line.TargetSum)
                return (true, []); 
        }

        var numbersNew = new HashSet<int>();    

        foreach(var num in numbers)
        {
            foreach (var item in line.Buttons)
            {
                var sum = num ^ item;

                numbersNew.Add(sum);
            }
        }

        return (false, numbersNew); 
    }


    public class Line
    {
        public int Mod { get; set; }
        public HashSet<int> Buttons { get; set; } = new();
        public int TargetSum { get; set; }
        public Line(string line)
        {
            var parts = line.Split(' ').ToArray();

            for (int i = 0; i < parts.Length; i++)
            {
                var substr = parts[i];
                if (i == 0)
                {
                    Mod = (int)Math.Pow(2, substr.Length - 2);

                    for (int j = 1; j <= substr.Length - 2; j++)
                    {
                        var num = substr[j] == '.' ? 0 : substr[j] == '#' ? 1 : throw new ArgumentException();
                        TargetSum += (int)Math.Pow(2, j - 1) * num;
                    }
                }
                else if (i == parts.Length - 1)
                {
                    // for now do nothing
                }
                else
                {
                    var numsStr = substr.Substring(1, substr.Length - 2);

                    var button = numsStr.Split(',').Select(x => int.Parse(x)).Sum(x => (int)Math.Pow(2, x));

                    Buttons.Add(button);
                }

            }
        }
    }
}
