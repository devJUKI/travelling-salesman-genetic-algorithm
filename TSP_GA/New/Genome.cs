using TSP_GA.Old.Utilities;

namespace TSP_GA.New;

public class Genome
{
    private List<int> locationIndexes = new();

    public Genome(List<int> locationIndexes)
    {
        this.locationIndexes = locationIndexes;
    }

    public Genome Crossover(Genome secondPath)
    {
        var locationIndexesCopy = new List<int>(locationIndexes);
        
        int middlePoint = locationIndexes.Count / 2;
        int genesCount = locationIndexes.Count - middlePoint;
        var genesToReplace = secondPath.locationIndexes.GetRange(middlePoint, genesCount);

        locationIndexesCopy.RemoveRange(middlePoint, genesCount);
        locationIndexesCopy.AddRange(genesToReplace);
        
        return new Genome(locationIndexesCopy);
    }

    public Genome Mutate()
    {
        List<int> locationIndexesCopy = new(locationIndexes);

        locationIndexesCopy = locationIndexesCopy.Shuffle();
        locationIndexesCopy.Remove(0);
        locationIndexesCopy.Insert(0, 0);

        return new Genome(locationIndexesCopy);
    }

    public double CalculateTime()
    {
        var locationsSingleton = LocationsSingleton.GetInstance();

        List<double> times = new();

        // 1 km = 1 min
        for (int i = 0; i < locationIndexes.Count - 1; i++)
        {
            var location1 = locationsSingleton.GetLocation(locationIndexes[i]);
            var location2 = locationsSingleton.GetLocation(locationIndexes[i + 1]);
            double distance = HaversineDistance(location1.Coordinates, location2.Coordinates);
            times.Add(distance);
        }

        // Adding rest times
        for (int i = 0; i < times.Count; i++)
        {
            int timeInPlaces = locationIndexes.Count * 60;
            int restTimes = (int)Math.Floor(times[i] / 16);
            times[i] = times[i] + restTimes * 8 * 60 + timeInPlaces;
        }

        return times.Max();
    }

    static double HaversineDistance(Coordinates coord1, Coordinates coord2)
    {
        // Radius of the Earth in kilometers
        const double R = 6371.0;

        // Convert latitude and longitude from degrees to radians
        double lat1 = ToRadians(coord1.Latitude);
        double lon1 = ToRadians(coord1.Longitude);
        double lat2 = ToRadians(coord2.Latitude);
        double lon2 = ToRadians(coord2.Longitude);

        // Differences in coordinates
        double dlat = lat2 - lat1;
        double dlon = lon2 - lon1;

        // Haversine formula
        double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Calculate the distance
        double distance = R * c;

        return distance;
    }

    static double ToRadians(double degree)
    {
        return degree * Math.PI / 180.0;
    }

    public static bool operator ==(Genome genome1, Genome genome2)
    {
        return genome1.locationIndexes.SequenceEqual(genome2.locationIndexes);
    }

    public static bool operator !=(Genome genome1, Genome genome2)
    {
        return !genome1.locationIndexes.SequenceEqual(genome2.locationIndexes);
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        string output = "";
        locationIndexes.ForEach(l => output += string.Join(',', l) + "\n");
        return output;
    }
}
