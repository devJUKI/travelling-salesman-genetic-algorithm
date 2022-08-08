namespace TSP_GA.Entities
{
    public class Location
    {
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Location(string name, float x, float y) {
            Name = name;
            X = x;
            Y = y;
        }

        public Location(string line) {
            string[] content = line.Split('\t');
            Name = content[0];
            X = float.Parse(content[2]);
            Y = float.Parse(content[3]);
        }
    }
}
