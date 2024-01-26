namespace TSP_GA.New;

public class Population
{
    private List<Chromosome> chromosomes;

    public Population(List<Chromosome> chromosomes)
    {
        this.chromosomes = chromosomes;
    }

    public List<Chromosome> SelectBestChromosomes(int count)
    {
        return chromosomes
            .OrderBy(c => c.GetFitness())
            .Take(count)
            .ToList();
    }

    public Chromosome SelectRandomChromosome()
    {
        List<Chromosome> sortedChromosomes = chromosomes
            .OrderBy(c => c.GetFitness())
            .ToList();

        double maxFitness = sortedChromosomes.Max(c => c.GetFitness());
        double minFitness = sortedChromosomes.Min(c => c.GetFitness());

        // Find random population based on fitness value
        double random = new Random().NextDouble();
        Chromosome currChromosome = sortedChromosomes[0];
        int index = 0;

        while (currChromosome.GetNormalizedFitness(minFitness, maxFitness) > 1 - random)
        {
            random -= currChromosome.GetNormalizedFitness(minFitness, maxFitness);
            currChromosome = sortedChromosomes[++index];
        }

        return currChromosome;
    }

    public static Population Create(int chromosomeCount, int travellerCount, int locationCount)
    {
        var chromosomes = new List<Chromosome>();

        for (int i = 0; i < chromosomeCount; i++)
        {
            var chromosome = Chromosome.Create(travellerCount, locationCount);
            chromosomes.Add(chromosome);
        }

        return new Population(chromosomes);
    }
}
