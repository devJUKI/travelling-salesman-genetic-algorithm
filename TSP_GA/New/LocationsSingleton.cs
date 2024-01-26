namespace TSP_GA.New;

public class LocationsSingleton
{
    private static LocationsSingleton? instance = null;

    private List<Location> locations = new();

    public static LocationsSingleton GetInstance()
    {
        return instance ??= new LocationsSingleton();
    }

    public void SetLocations(List<Location> locations) => this.locations = locations;

    public List<Location> GetLocations() => locations;
    public Location GetLocation(int index) => locations[index];
}
