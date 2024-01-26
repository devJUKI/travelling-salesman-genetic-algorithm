using TSP_GA.Old.Utilities;

namespace TSP_GA.Old.Entities;

public class PopulationNew
{
    public List<Chromosome> Chromosomes { get; set; } = new();

    private PopulationNew(List<List<Location>> paths)
    {

    }

    public static PopulationNew Create(List<Location> locations, int travellerCount)
    {
        List<List<Location>> paths = new();

        int placesPerPerson = (int)Math.Ceiling((locations.Count - 1f) / travellerCount);
        List<int> locationIndexPool = Enumerable.Range(1, locations.Count - 1).ToList();

        for (int j = 0; j < travellerCount; j++)
        {
            int pathsToAdd = j == travellerCount - 1 ? locationIndexPool.Count : placesPerPerson;
            locationIndexPool.GetRange(0, pathsToAdd);
            locationIndexPool.RemoveRange(0, pathsToAdd);
        }

        //for (int j = 0; j < paths.Count; j++)
        //{
        //    paths[j] = paths[j].Shuffle();
        //    paths[j].Insert(0, 0);
        //}

        return new PopulationNew(paths);
    }
}
