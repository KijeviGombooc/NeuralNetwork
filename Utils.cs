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
}