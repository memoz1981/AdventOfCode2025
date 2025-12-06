namespace Day6;

public class MathProblemSolver
{
    public static long Question1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var lines = File.ReadAllLines(filePath);

        var height = lines.Count() - 1; // we ignore the line with symbols
        var length = lines.First().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        var array = new int[height, length];

        for (int h = 0; h < height; h++)
        {
            var splitNums = lines[h].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
            for (int l = 0; l < length; l++)
            {
                array[h, l] = int.Parse(splitNums[l]);
            }
        }

        var chars = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(l => char.Parse(l)).ToArray();

        long resultAll = 0;
        resultAll = FindResult(height, length, array, chars, resultAll);

        return resultAll;
    }

    public static long Question2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var lines = File.ReadAllLines(filePath).ToArray();

        var height = lines.Length - 1;
        var length = lines[0].Count();
        var array = new char[height, length]; 

        for (int h = 0; h < height; h++)
        {
            for (int ll = 0; ll < length; ll++)
                array[h, ll] = lines[h][ll]; 
        }
        var chars = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(l => char.Parse(l)).ToArray();
        int l = 0;

        long resultAll = 0;
        var operationIndex = 0; 

        while (true)
        {
            var resultList = new List<long>(); //one array for each digit

            for (; l < length; l++)
            {
                long lineNum = 0;
                var pow = 0; 
                for (int h = height - 1; h >= 0; h--)
                {
                    if (!char.IsDigit(array[h, l]))
                    {
                        continue; 
                    }
                    lineNum += (array[h, l] - '0') * (long)Math.Pow(10, pow++); 
                }

                if (lineNum == 0)
                {
                    l++;
                    break;
                }

                resultList.Add(lineNum);
            }

            if (!resultList.Any())
                continue;

            // calculate result
            long result = chars[operationIndex] == '+' ? 0 : chars[operationIndex] == '*' ? 1 : throw new ArgumentException();

            foreach (var num in resultList)
            {
                if (chars[operationIndex] == '+')
                    result += num;
                else
                    result *= num; 
            }

            operationIndex++;
            resultAll += result; 
            if (l >= length)
                break; 
        }

        return resultAll; 

    }

    private static long FindResult(int height, int length, int[,] array, char[] chars, long resultAll)
    {
        for (int l = 0; l < length; l++)
        {
            long result = chars[l] == '+' ? 0 : chars[l] == '*' ? 1 : throw new ArgumentException();

            for (int h = 0; h < height; h++)
            {
                if (chars[l] == '+')
                {
                    result += array[h, l];
                }
                else
                {
                    result *= array[h, l];
                }
            }

            resultAll += result;
        }

        return resultAll;
    }
}
