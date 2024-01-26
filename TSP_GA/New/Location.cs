namespace TSP_GA.New;

public class Location
{
    public string Name { get; set; }
    public Coordinates Coordinates { get; set; }

    public Location(string name, float longitude, float latitude)
    {
        Name = name;
        Coordinates = new Coordinates(latitude, longitude);
    }

    public Location(string line)
    {
        string[] content = line.Split('\t');
        Name = content[0];
        Console.WriteLine(line);
        float longitude = float.Parse(content[2]);
        float latitude = float.Parse(content[3]);
        Coordinates = new Coordinates(latitude, longitude);
    }
}
