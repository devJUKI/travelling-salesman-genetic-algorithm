namespace TSP_GA.Old.Entities;

public class Chromosome
{
    public List<List<int>> LocationIndexes { get; set; } = new(); // Genes
    public double Time { get; set; } = 0; // Fitness

    public Chromosome(List<List<int>> locationIndexes)
    {
        LocationIndexes = locationIndexes;


    }

    private double GetFitness()
    {
        List<double> times = new();
        // 1 km = 1 min
        //Paths.ForEach(order => times.Add(GetDistance(order)));
        // Adding rest times
        for (int i = 0; i < times.Count; i++)
        {
            //int timeInPlaces = Paths[i].Count * 60;
            int restTimes = (int)Math.Floor(times[i] / 16);
            times[i] = times[i] + restTimes * 8 * 60;
        }

        return times.Max();
    }
}
