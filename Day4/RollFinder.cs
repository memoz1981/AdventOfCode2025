namespace Day4; 

public class RollFinder
{
    public static long Question1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var length = File.ReadLines(filePath).First().Length;
        var height = File.ReadLines(filePath).Count();

        var array = new int[height, length];
        int h = 0; 

        foreach (var line in File.ReadLines(filePath))
        {
            for (int l = 0; l < length; l++)
            {
                array[h, l] = line[l] == '.' ? 0 : line[l] == '@' ? 1 : throw new ArgumentException(); 
            }

            h++; 
        }

        var arrayUpdated = new int[height, length];
        var sum = 0; 

        for (int i = 0; i < height; i++)
        {
            Console.WriteLine(); 
            for (int j = 0; j < length; j++)
            {
                var w = j >= 1 ? array[i, j - 1] : 0;
                var e = j < length - 1 ? array[i, j + 1] : 0;
                var n = i >= 1 ? array[i - 1, j] : 0;
                var s = i < height - 1 ? array[i + 1, j] : 0;

                var nw = i >= 1 && j >= 1 ? array[i - 1, j - 1] : 0; 
                var ne = i >= 1 && j < length - 1 ? array[i - 1, j + 1] : 0;
                var sw = i < height - 1 && j >= 1 ? array[i + 1, j - 1] : 0;
                var se = i < height - 1 && j < length - 1 ? array[i + 1, j + 1] : 0;

                var canMove = (w + e + n + s + nw + ne + sw + se) < 4 ? 1 : 0; 

                arrayUpdated[i, j] = array[i, j] == 0 ? 0 : canMove;
                Console.Write(arrayUpdated[i, j]);
                sum = sum + arrayUpdated[i, j]; 
            }
        }

        Console.WriteLine(sum); 

        return 0; 
    }

    public static long Question2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);

        var length = File.ReadLines(filePath).First().Length;
        var height = File.ReadLines(filePath).Count();

        var array = new int[height, length];
        int h = 0;
        var sum = 0; 

        foreach (var line in File.ReadLines(filePath))
        {
            for (int l = 0; l < length; l++)
            {
                array[h, l] = line[l] == '.' ? 0 : line[l] == '@' ? 1 : throw new ArgumentException();

            }

            h++;
        }

        while (true)
        {
            Console.WriteLine($"\n\n\n");
            Console.WriteLine(sum); 
            
            var sumNew= Remove(array, height, length);

            if (sumNew == 0)
            {
                break;
            }

            sum += sumNew;
        }

        Console.WriteLine(sum);

        return sum; 
    }

    public static int Remove(int[,] array, int height, int length)
    {
        var arrayUpdated = new int[height, length];

        var sum = 0;

        for (int i = 0; i < height; i++)
        {
            Console.WriteLine();
            for (int j = 0; j < length; j++)
            {
                var w = j >= 1 ? array[i, j - 1] : 0;
                var e = j < length - 1 ? array[i, j + 1] : 0;
                var n = i >= 1 ? array[i - 1, j] : 0;
                var s = i < height - 1 ? array[i + 1, j] : 0;

                var nw = i >= 1 && j >= 1 ? array[i - 1, j - 1] : 0;
                var ne = i >= 1 && j < length - 1 ? array[i - 1, j + 1] : 0;
                var sw = i < height - 1 && j >= 1 ? array[i + 1, j - 1] : 0;
                var se = i < height - 1 && j < length - 1 ? array[i + 1, j + 1] : 0;

                var canMove = (w + e + n + s + nw + ne + sw + se) < 4 ? 1 : 0;

                arrayUpdated[i, j] = array[i, j] == 0 ? 0 : canMove;
                Console.Write(arrayUpdated[i, j]);
                sum = sum + arrayUpdated[i, j];
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (arrayUpdated[i, j] == 1)
                    array[i, j] = 0; 
            }
        }

        return sum; 
    }
}
