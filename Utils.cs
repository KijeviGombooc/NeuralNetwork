using System.Text;

class Utils
{
    public static Random Random { get; private set; } = new Random(1);
    
    static void SetRandomSeed(int seed)
    {
        Random = new Random(seed);
    }
    
    public static List<T> GenerateList<T>(int count, Func<int, T> func)
    {
        List<T> list = new List<T>(count);
        for (int i = 0; i < count; i++)
            list.Add(func(i));
        return list;
    }

    public static string ListToString<T>(List<T> list)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count - 1; i++)
        {
            sb.Append(list[i]);
            sb.Append("; ");
        }
        sb.Append(list.Last());
        return sb.ToString();
    }
}