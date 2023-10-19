namespace TSP_GA.Utilities
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> temp = new(list);
            Random random = new();
            int n = temp.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (temp[n], temp[k]) = (temp[k], temp[n]);
            }
            return temp;
        }
    }
}
