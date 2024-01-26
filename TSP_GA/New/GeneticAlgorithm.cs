namespace TSP_GA.New;

public class GeneticAlgorithm
{
    private Population population;
    private Chromosome? bestChromosome;

    private GeneticAlgorithmOptions options;

    public GeneticAlgorithm(GeneticAlgorithmOptions options)
    {
        this.options = options;

        var locations = LocationsSingleton.GetInstance().GetLocations();
        population = Population.Create(options.GenerationSize, options.TravellerCount, locations.Count);
    }

    public void Start()
    {
        int bestCount = options.GenerationSize / 4;
        for (int i = 0; i < options.GenerationCount; i++)
        {
            List<Chromosome> chromosomes = new();

            chromosomes.AddRange(population.SelectBestChromosomes(bestCount));

            for (int j = 0; j < options.GenerationSize - bestCount; j++)
            {
                var chromosome1 = population.SelectRandomChromosome();
                var chromosome2 = population.SelectRandomChromosome();

                var crossedChromosome = chromosome1.Crossover(chromosome2);
                crossedChromosome = crossedChromosome.Mutate();
                chromosomes.Add(crossedChromosome);
            }

            population = new Population(chromosomes);

            var bestGensChromosome = population.SelectBestChromosomes(1)[0];
            if (bestChromosome == null || bestGensChromosome.GetFitness() > bestChromosome.GetFitness())
            {
                bestChromosome = bestGensChromosome;
            }

            Console.WriteLine(bestChromosome.GetFitness());
            //Console.WriteLine(bestGensChromosome.GetFitness());
            Console.WriteLine(bestChromosome);
        }
    }
}
