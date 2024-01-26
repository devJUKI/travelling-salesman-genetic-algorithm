namespace TSP_GA.New;

public class Chromosome
{
    private List<Genome> genomes = new(); // Paths

    private Chromosome(List<Genome> genomes)
    {
        this.genomes = genomes;
    }
    
    public Chromosome Crossover(Chromosome secondChromosome)
    {
        var newPaths = genomes.Zip(secondChromosome.genomes, (a, b) => a.Crossover(b)).ToList();
        return new Chromosome(newPaths);
    }

    public Chromosome Mutate()
    {
        var mutadedGenomes = genomes.Select(p => p.Mutate()).ToList();
        return new Chromosome(mutadedGenomes);
    }

    public double GetFitness()
    {
        return genomes.Max(p => p.CalculateTime());
    }

    public double GetNormalizedFitness(double min, double max)
    {
        return (GetFitness() - min) / (max - min);
    }

    public static Chromosome Create(int travellerCount, int locationCount)
    {
        var locationIndexes = Enumerable.Range(1, locationCount - 1).ToList();

        var paths = SplitList(locationIndexes, travellerCount);
        var genomes = paths.Select(p => new Genome(p)).ToList();

        return new Chromosome(genomes);
    }

    static List<List<T>> SplitList<T>(List<T> source, int parts)
    {
        int sourceCount = source.Count;
        int elementsPerPart = sourceCount / parts;
        int remainder = sourceCount % parts;

        List<List<T>> result = new(parts);

        int startIndex = 0;
        for (int i = 0; i < parts; i++)
        {
            int partSize = elementsPerPart + (i < remainder ? 1 : 0);
            List<T> part = source.GetRange(startIndex, partSize);
            result.Add(part);

            startIndex += partSize;
        }

        return result;
    }

    public static bool operator ==(Chromosome chromosome1, Chromosome chromosome2)
    {
        if (chromosome1 is null && chromosome2 is null)
            return true;

        if (chromosome1 is null || chromosome2 is null)
            return false;

        return chromosome1.genomes == chromosome2.genomes;
    }

    public static bool operator !=(Chromosome chromosome1, Chromosome chromosome2)
    {
        return chromosome1.genomes != chromosome2.genomes;
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
        genomes.ForEach(g => output += g + "\n");
        return output;
    }
}
