using System.Security.Cryptography.X509Certificates;

namespace Day8;

public class JunctionBoxDistanceFinder
{
    private const int GROUP_CNT = 1000;
    public static long Question1()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);


        var junctionBoxes = new List<JunctionBox>();

        foreach (var line in File.ReadAllLines(filePath))
        {
            var nums = line.Split(',').Select(t => int.Parse(t)).ToArray();

            junctionBoxes.Add(new JunctionBox(nums[0], nums[1], nums[2]));
        }

        var distances = new List<(double distance, JunctionBox from, JunctionBox to)>();

        foreach (var jb in junctionBoxes)
        {
            foreach (var jb2 in junctionBoxes)
            {
                if (jb2 == jb || jb.Distances.Contains(jb2) || jb2.Distances.Contains(jb))
                    continue;

                var distance = JunctionBox.Distance(jb, jb2);


                distances.Add(new (distance, jb, jb2)); 
                jb.Distances.Add(jb2);

            }
        }

        distances = distances.OrderBy(d => d.distance).ToList();

        var count = junctionBoxes.Count; 

        for (int i = 0; i < GROUP_CNT; i++)
        {
            var distance = distances[i];

            distance.from.Group(distance.to);

        }

        var included = new HashSet<JunctionBox>();
        long result = 1;

        var numLargestCircuitsIncluded = 0;

        var jbs = junctionBoxes.Where(jb => jb.IsGrouped).OrderByDescending(jb => jb.GroupCount).ToList();

        foreach (var jb in junctionBoxes.Where(jb => jb.IsGrouped).OrderByDescending(jb => jb.GroupCount))
        {
            if (numLargestCircuitsIncluded >= 3)
                break; 
            
            if (included.Contains(jb))
                continue;

            result *= (jb.GroupCount + 1);
            numLargestCircuitsIncluded++; 


            included.Add(jb);

            foreach(var gr in jb.Groups)
                included.Add(gr);
        }

        return result; 
    }

    public static long Question2()
    {
        var fileName = "1.txt";
        var location = AppContext.BaseDirectory;
        var filePath = Path.Combine(location, fileName);


        var junctionBoxes = new List<JunctionBox>();

        foreach (var line in File.ReadAllLines(filePath))
        {
            var nums = line.Split(',').Select(t => int.Parse(t)).ToArray();

            junctionBoxes.Add(new JunctionBox(nums[0], nums[1], nums[2]));
        }

        var distances = new List<(double distance, JunctionBox from, JunctionBox to)>();

        foreach (var jb in junctionBoxes)
        {
            foreach (var jb2 in junctionBoxes)
            {
                if (jb2 == jb || jb.Distances.Contains(jb2) || jb2.Distances.Contains(jb))
                    continue;

                var distance = JunctionBox.Distance(jb, jb2);

                distances.Add(new(distance, jb, jb2));
                jb.Distances.Add(jb2);

            }
        }

        distances = distances.OrderBy(d => d.distance).ToList();

        var count = junctionBoxes.Count;

        long result = 0; 

        for (int i = 0; i < distances.Count; i++)
        {
            var distance = distances[i];

            var countBefore = Math.Max(distance.from.GroupCount, distance.to.GroupCount);

            distance.from.Group(distance.to);

            var countAfter = Math.Max(distance.from.GroupCount, distance.to.GroupCount);

            if (countBefore == junctionBoxes.Count - 2 && countAfter == junctionBoxes.Count - 1)
            {
                result = distance.from.X * distance.to.X;
                break;
            }
        }

        return result;
        
    }
}

public record JunctionBox(long X, long Y, long Z)
{
    public static double Distance(JunctionBox a, JunctionBox b)
    {
        return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2) + Math.Pow((a.Z - b.Z), 2));  
    }

    public HashSet<JunctionBox> Distances = new();
    private HashSet<JunctionBox> _grouped = new();

    public HashSet<JunctionBox> Groups => _grouped;

    public void Group(JunctionBox b)
    {
        if (_grouped.Contains(b) || this == b)
            return;

        _grouped.Add(b);
        b.Group(this);

        for (int i = 0; i < _grouped.Count; i++)
        {
            _grouped.ElementAt(i).Group(b);
            b.Group(_grouped.ElementAt(i));
        }

        for (int i = 0; i < b.Groups.Count; i++)
        {
            b.Groups.ElementAt(i).Group(this);
            Group(b.Groups.ElementAt(i));
        }
    }

    public bool IsGrouped => _grouped.Any();
    public int GroupCount => _grouped.Count();
    public bool IsGroupedWith(JunctionBox b) => _grouped.Contains(b);

    public override string ToString() => $"{X}-{Y}-{Z}-{GroupCount}";
}
