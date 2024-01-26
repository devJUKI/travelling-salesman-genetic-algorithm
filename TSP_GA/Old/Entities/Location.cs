namespace TSP_GA.Old.Entities
{
    public class Location
    {
        public string Name { get; set; }
        public float Longtitude { get; set; }
        public float Latitude { get; set; }

        public Location(string name, float longtitude, float latitude)
        {
            Name = name;
            Longtitude = longtitude;
            Latitude = latitude;
        }

        public Location(string line)
        {
            string[] content = line.Split('\t');
            Name = content[0];
            Longtitude = float.Parse(content[2]);
            Latitude = float.Parse(content[3]);
        }
    }
}
