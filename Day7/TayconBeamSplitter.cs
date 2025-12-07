namespace Day7;

public class TayconBeamSplitter
{
    public static int Question1()
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

        var indexS = lines[0].IndexOf('S');
        array[0, indexS] = '|'; 

        var splitCount = 0;

        for (int h = 1; h < height; h++)
        {
            for (int l = 0; l < length; l++)
            {
                if (array[h, l] == '.')
                {
                    if (array[h - 1, l] == '|')
                        array[h, l] = '|'; 
                }
                else if (array[h, l] == '^')
                {
                    if (array[h - 1, l] != '|')
                        continue;

                    if (l > 0)
                        array[h, l - 1] = '|';

                    if (l < length - 1)
                        array[h, l + 1] = '|'; 

                    splitCount++;
                }
                else
                    continue; 
            }
        }

        return splitCount; 
    }


    public static long Question2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var lines = File.ReadAllLines(filePath).ToArray();

        var height = lines.Length - 1;
        var length = lines[0].Count();
        var array = new long[height, length];

        for (int h = 0; h < height; h++)
        {
            for (int l = 0; l < length; l++)
            {
                if (lines[h][l] == 'S')
                    array[h, l] = 1;
                else if (lines[h][l] == '^')
                    array[h, l] = -1;
                else if (lines[h][l] == '.')
                    array[h, l] = 0;
                else
                    throw new ArgumentException(); 
            }
        }

        for (int h = 1; h < height; h++)
        {
            for (int l = 0; l < length; l++)
            {
                if (array[h, l] != -1)
                {
                    array[h, l] += Math.Max(0, array[h - 1, l]);
                    continue; 
                }
                
                if (array[h - 1, l] <= 0)
                    continue;

                if (l > 0)
                    array[h, l - 1] += Math.Max(0, array[h - 1, l]);

                if (l < length - 1)
                    array[h, l + 1] += Math.Max(0, array[h - 1, l]);
            }
        }

        for (int h = 0; h < height; h++)
        {
            Console.WriteLine(); 
            for(int l = 0; l < length; l++)
                Console.Write($" {array[h, l]} ");
        }

        long sum = 0;
        for (int l = 0; l < length; l++)
            sum += Math.Max(0, array[height - 1, l]);

        return sum;
    }

}
