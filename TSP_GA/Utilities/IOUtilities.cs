using TSP_GA.Entities;

namespace TSP_GA
{
    public class IOUtilities
    {
        private const string START_PLACE = "Kauno m. centras\t " +
                                            ".\t 495677,140236133" +
                                            "\t 6084803,83132167";

        public static List<Location> ReadData(string path) {
            string text = File.ReadAllText(path);
            List<string> lines = text.Split('\n').ToList();

            List<Location> places = new();
            places.Add(new Location(START_PLACE));
            lines.ForEach(line => places.Add(new Location(line)));

            return places;
        }
    }
}
