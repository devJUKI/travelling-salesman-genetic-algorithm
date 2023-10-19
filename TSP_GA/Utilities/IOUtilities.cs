using TSP_GA.Entities;

namespace TSP_GA
{
    public class IOUtilities
    {
        public static List<Location> ReadData(string path)
        {
            string text = File.ReadAllText(path);
            List<string> lines = text.Split('\n').ToList();

            List<Location> places = new() { new Location(Constants.START_PLACE) };
            lines.ForEach(line => places.Add(new Location(line)));

            return places;
        }
    }
}
