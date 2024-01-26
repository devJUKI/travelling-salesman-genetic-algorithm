using TSP_GA.Old.Entities;

namespace TSP_GA.Old.Utilities
{
    public static class IOUtilities
    {
        public static List<New.Location> ReadData(string path)
        {
            string text = File.ReadAllText(path);
            List<string> lines = text.Split('\n').ToList();

            List<New.Location> places = new();
            lines.ForEach(line => places.Add(new New.Location(line)));

            return places;
        }
    }
}
