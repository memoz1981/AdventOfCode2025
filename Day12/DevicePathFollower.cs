using System.Text;

namespace Day11;

public class DevicePathFollower
{
    public static long Question1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);
        var all = ReadDevices(filePath);

        var (found, totalCount) = FindTotalWays(all["you"], new(), 0, false);

        return totalCount;
    }

    public static long Question2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);
        var all = ReadDevices(filePath);

        var (found, totalCount) = FindTotalWays(all["out"], new(), 0, true, "svr", true);

        return totalCount;
    }

    //public static HashSet<string> BackToSvr(Device startDevice, Dictionary<string, HashSet<string>> pathTraversed)
    //{
    //    if (startDevice.Name == "svr")
    //    {
    //        for(int i = 0; i <)
    //    }
    //}

    private static Dictionary<string, Device> ReadDevices(string filePath)
    {
        var all = new Dictionary<string, Device>();
        all["out"] = new Device("out"); //manually add as it doesn't have any next

        // add keys
        foreach (var line in File.ReadLines(filePath))
        {
            var lines = line.Split(' ', StringSplitOptions.TrimEntries).ToArray();

            var name = lines[0].Substring(0, lines[0].Length - 1);

            if (!all.TryGetValue(name, out var current))
            {
                current = new Device(name);
                all[name] = current;
            }
        }

        // add nexts
        foreach (var line in File.ReadLines(filePath))
        {
            var lines = line.Split(' ', StringSplitOptions.TrimEntries).ToArray();

            var name = lines[0].Substring(0, lines[0].Length - 1);

            if (!all.TryGetValue(name, out var current))
            {
                throw new ArgumentException();
            }

            for (int i = 1; i < lines.Length; i++)
            {
                var next = lines[i];

                if (!all.TryGetValue(next, out var nextDevice))
                    throw new ArgumentException();

                current.Next.Add(nextDevice);

                nextDevice.Parents.Add(current);
            }

            all[name] = current;
        }

        return all;
    }

    public static (bool found, int totalCountReturned) FindTotalWays(Device startDevice, HashSet<string> traversed, int totalCount,
        bool mustIncludeFftDac, string destination = "out", bool shouldGoToNext = true)
    {
        if (startDevice.Name == destination)
        {
            if (mustIncludeFftDac)
            {
                if((traversed.Contains("dac") && traversed.Contains("fft")))
                    return (true, totalCount + 1);

                return (false, totalCount);
            }

            return (true, totalCount + 1);
        }

        if (traversed.Contains(startDevice.Name))
            return (false, totalCount);

        traversed.Add(startDevice.Name);

        var sumCount = 0;
        var foundAny = false;

        var itemsToTraverse = shouldGoToNext ? startDevice.Parents : startDevice.Next;

        foreach(var next in itemsToTraverse)
        {
            if (traversed.Contains(next.Name))
                continue;

            var nextCount = totalCount;

            var traversedNew = traversed.Select(x => x).ToHashSet(); 
            var (found, totalCountNext) = FindTotalWays(next, traversedNew, nextCount, mustIncludeFftDac, destination, shouldGoToNext);

            if (!found)
                continue;

            sumCount += totalCountNext;
            foundAny = true; 
        }

        return (foundAny, sumCount); 
    }

    //public static (bool found, int totalCountReturned) FindTotalWays(Device startDevice, HashSet<string> traversed, int totalCount,
    //    HashSet)
    //{
    //    if (startDevice.Name == destination)
    //    {
    //        if (mustIncludeFftDac)
    //        {
    //            if ((traversed.Contains("dac") && traversed.Contains("fft")))
    //                return (true, totalCount + 1);

    //            return (false, totalCount);
    //        }

    //        return (true, totalCount + 1);
    //    }

    //    if (traversed.Contains(startDevice.Name))
    //        return (false, totalCount);

    //    traversed.Add(startDevice.Name);

    //    var sumCount = 0;
    //    var foundAny = false;

    //    var itemsToTraverse = shouldGoToNext ? startDevice.Parents : startDevice.Next;

    //    foreach (var next in itemsToTraverse)
    //    {
    //        if (traversed.Contains(next.Name))
    //            continue;

    //        var nextCount = totalCount;

    //        var traversedNew = traversed.Select(x => x).ToHashSet();
    //        var (found, totalCountNext) = FindTotalWays(next, traversedNew, nextCount, mustIncludeFftDac, destination, shouldGoToNext);

    //        if (!found)
    //            continue;

    //        sumCount += totalCountNext;
    //        foundAny = true;
    //    }

    //    return (foundAny, sumCount);
    //}
}

public record struct Device(string Name)
{
    public List<Device> Next { get; set; } = new();

    public List<Device> Parents { get; set; } = new();  

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append($"Name: {Name}");

        foreach (var next in Next)
        {
            builder.Append($" {next.Name} ");
        }

        return builder.ToString();
    }
}
