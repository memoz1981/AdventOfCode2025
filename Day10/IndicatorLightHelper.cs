using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

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

        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];    
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

            Console.WriteLine($"Line {i}, depth: {depth}, result: {result}");
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

        foreach (var num in numbers)
        {
            foreach (var item in line.Buttons)
            {
                var sum = num ^ item;

                numbersNew.Add(sum);
            }
        }

        return (false, numbersNew);
    }

    public static long Question2()
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
        var stopwatch = Stopwatch.StartNew(); 

        for (int i = 0; i < lines.Count; i++)
        {
            var stopwatchLoop = Stopwatch.StartNew();
            var line = lines[i]; 

            var (found, depthNew) = GetDepth(line.Joltages, line.TargetJoltage, 0);

            if (!found)
                throw new ArgumentException();

            result += depthNew;

            Console.WriteLine($"Line {i}: Depth {depthNew}; totaldepth: {result}, seconds: {stopwatchLoop.Elapsed.TotalSeconds}, total seconds: {stopwatch.Elapsed.TotalSeconds}"); 
        }

        return result; 
        

        //for(int i = 0; i < lines.Count; i++)
        //{
        //    var line = lines[i];
        //    var depth = 1;
        //    var joltages = line.Joltages;

        //    var index = line.ReturnJoltageIndexLeastUsed(); 



        //    while (true)
        //    {
        //        var (resultFound, joltagesNew) = ReturnNewNumbersOrResultQ2(joltages, line);

        //        if (resultFound)
        //        {
        //            result += depth;
                    
        //            break;
        //        }

        //        depth++;
        //        joltages = joltagesNew;
        //    }
        //    Console.WriteLine($"Line {i}, depth: {depth}, result: {result}");
        //}

        //return result;
    }



    public static (bool isFound, int depthNew) GetDepth(HashSet<Joltage> joltages, 
        Joltage targetJoltage, int depth)
    {
        if (targetJoltage == new Joltage(new int[targetJoltage.Numbers.Length]))
            return (true, depth);
        
        
        if (!joltages.Any())
            return (false, depth);

        var joltageSum = new Joltage(new int[joltages.First().Numbers.Length]);

        foreach (var j in joltages)
        {
            joltageSum += j;
        }

        if (!(targetJoltage % joltageSum))
            return (false, depth);

        //for (int i = 0; i < joltageSum.Numbers.Length; i++)
        //{
        //    if (joltageSum.Numbers[i] == 1 && targetJoltage.Numbers[i] > 0)
        //    {
        //        foreach (var joltage in joltages)
        //        {
        //            if (joltage.Numbers[i] == 1)
        //            {
        //                var div = targetJoltage / joltage;

        //                if (div == 0)
        //                    break;

        //                var depthCopy = depth + div;

        //                var newTargetJoltage = targetJoltage - joltage.Multiply(div);

        //                var joltagesNew2 = joltages.Select(j => j).ToHashSet();
        //                joltagesNew2.Remove(joltage);

        //                return GetDepth(joltagesNew2, newTargetJoltage, depthCopy);
        //            }
        //        }
        //    }
        //}

        var maxJoltage = joltages.OrderByDescending(j => j.Sum).First();
        var division = targetJoltage / maxJoltage;

        var joltagesNew = joltages.Select(j => j).ToHashSet();
        var removed = joltagesNew.Remove(maxJoltage);

        if (!removed)
            throw new ArgumentException(); 

        for (int d = division; d >= 0; d--)
        {
            var depthCopy = depth; 
            var joltage = maxJoltage.Multiply(d);
            
            var newTargetJoltage = targetJoltage - joltage;

            depthCopy += d;

            // if we reached zeros
            if (newTargetJoltage == new Joltage(new int[joltage.Numbers.Length]))
                return (true, depthCopy);

            if (!joltagesNew.Any())
                continue; 

            var (isFound, depthNew) = GetDepth(joltagesNew, newTargetJoltage, depthCopy);

            if (isFound)
                return (true, depthNew);
        }
        return (false, -1);
    }


    public static (bool resultFound, HashSet<Joltage> numbersNew) ReturnNewNumbersOrResultQ2(HashSet<Joltage> joltages, Line line)
    {
        foreach (var joltage in joltages)
        {
            if (joltage == line.TargetJoltage)
                return (true, []);
        }

        var joltagesNew = new HashSet<Joltage>();

        foreach (var joltage in joltages)
        {
            foreach (var item in line.Joltages)
            {
                var sumJoltage = joltage + item;

                if (sumJoltage > line.TargetJoltage)
                    continue; 

                joltagesNew.Add(sumJoltage);
            }
        }

        return (false, joltagesNew);
    }


    public class Line
    {
        public int Mod { get; set; }
        public HashSet<int> Buttons { get; set; } = new();
        public int TargetSum { get; set; }
        public Joltage TargetJoltage { get; set; }
        public HashSet<Joltage> Joltages { get; set; } = new();
        public Line(string line)
        {
            var parts = line.Split(' ').ToArray();


            for (int i = parts.Length - 1; i >= 0; i--)
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
                    var nums = substr.Substring(1, substr.Length - 2).Split(',').Select(n => int.Parse(n)).ToArray();
                    TargetJoltage = new Joltage(nums);
                }
                else
                {
                    var numsStr = substr.Substring(1, substr.Length - 2);

                    var button = numsStr.Split(',').Select(x => int.Parse(x)).Sum(x => (int)Math.Pow(2, x));

                    Buttons.Add(button);

                    var numbers = numsStr.Split(',').Select(n => int.Parse(n)).ToHashSet();

                    var numsArray = new int[TargetJoltage.Numbers.Length];
                    long sum = 0; 
                    for (int n = 0; n < TargetJoltage.Numbers.Length; n++)
                    {
                        if (numbers.Contains(n))
                        {
                            numsArray[n] = 1;
                            sum += (long)Math.Pow(2, n); 
                        }
                    }
                    Joltages.Add(new Joltage(numsArray));
                }

            }
        }

        public int ReturnIndexWithMinUsage()
        {
            var joltage = new Joltage(new int[Joltages.First().Numbers.Length]);
            foreach (var j in Joltages)
            {
                joltage += j;
            }
            var min = int.MaxValue;

            for (int i = 0; i < Joltages.First().Numbers.Length; i++)
            {
                min = Math.Min(min, joltage.Numbers[i]);
            }

            return min; 
        }


        public int ReturnJoltageIndexLeastUsed()
        {
            var min = long.MaxValue;
            var joltage = new Joltage(new int[Joltages.First().Numbers.Length]); 
            foreach (var j in Joltages)
            {
                joltage += j; 
            }
            var index = -1; 
            for (int i = 0; i < Joltages.First().Numbers.Length; i++)
            {
                var value = (long)Math.Pow(TargetJoltage.Numbers[i], joltage.Numbers[i]);

                if (value < min)
                {
                    min = value;
                    index = i;
                }
            }

            return index; 
        }
    }

    public class Joltage : IEquatable<Joltage>
    {
        public int[] Numbers { get; init; }
        public int Sum { get; set; }
        public Joltage(int[] numbers)
        {
            Numbers = numbers;
            Sum = Numbers.Sum(); 
        }
        
        public static Joltage operator +(Joltage a, Joltage b)
        {
            if(a.Numbers.Length != b.Numbers.Length)
                throw new ArgumentException();

            var numbers = new int[a.Numbers.Length];

            for (int i = 0; i < a.Numbers.Length; i++)
            {
                numbers[i] = a.Numbers[i] + b.Numbers[i];
            }

            return new Joltage(numbers);
        }

        public static Joltage operator -(Joltage a, Joltage b)
        {
            if (a.Numbers.Length != b.Numbers.Length)
                throw new ArgumentException();

            var numbers = new int[a.Numbers.Length];

            for (int i = 0; i < a.Numbers.Length; i++)
            {
                numbers[i] = a.Numbers[i] - b.Numbers[i];
            }

            return new Joltage(numbers);
        }

        public static bool operator %(Joltage a, Joltage b)
        {
            if (a.Numbers.Length != b.Numbers.Length)
                throw new ArgumentException();

            for (int i = 0; i < a.Numbers.Length; i++)
            {
                if (a.Numbers[i] != 0 && b.Numbers[i] == 0)
                    return false;

                //if (b.Numbers[i] != 0 && a.Numbers[i] == 0)
                //    return false;
            }

            return true; 
        }

        public Joltage Multiply(int factor)
        {
            var numbers = new int[Numbers.Length];
            for (int i = 0; i < Numbers.Length; i++)
                numbers[i] = Numbers[i] * factor;

            return new Joltage(numbers);
        }

        public static int operator /(Joltage a, Joltage b)
        {
            if (a.Numbers.Length != b.Numbers.Length)
                throw new ArgumentException();

            var minDivision = int.MaxValue;

            for (int i = 0; i < a.Numbers.Length; i++)
            {
                if (b.Numbers[i] == 0)
                    continue;
                if (a.Numbers[i] % b.Numbers[i] != 0 || a.Numbers[i] < b.Numbers[i])
                    return 0;

                

                minDivision = Math.Min(minDivision, a.Numbers[i] / b.Numbers[i]);
            }

            return minDivision;
        }

        public static bool operator >(Joltage a, Joltage targetJoltage)
        {
            if (a.Numbers.Length != targetJoltage.Numbers.Length)
                throw new ArgumentException();

            for (int i = 0; i < a.Numbers.Length; i++)
            {
                if (a.Numbers[i] > targetJoltage.Numbers[i])
                    return true; 
            }

            return false; 
        }

        public static bool operator ==(Joltage a, Joltage targetJoltage)
        {
            for (int i = 0; i < a.Numbers.Length; i++)
                if (a.Numbers[i] != targetJoltage.Numbers[i])
                    return false;

            return true; 
        }
        public static bool operator !=(Joltage a, Joltage targetJoltage)
            => throw new NotImplementedException();

        public static bool operator <(Joltage a, Joltage targetJoltage)
        {
            throw new NotImplementedException(); 
        }

        public override string ToString()
        {
            var nums = string.Join(',', Numbers);

            return $"{nums}";
        }

        public override int GetHashCode()
        {
            int hash = 88888777;
            foreach (var n in Numbers)
                hash = HashCode.Combine(hash, n);
            return hash;
        }

        public bool Equals(Joltage? other)
        {
            return other is Joltage jol &&
                   Numbers.SequenceEqual(jol.Numbers);
        }
    }
}
