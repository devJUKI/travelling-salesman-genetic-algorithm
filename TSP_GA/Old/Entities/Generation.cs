namespace TSP_GA.Old.Entities
{
    internal class Generation
    {
        public List<Population> Populations { get; private set; }

        public Generation(List<Population> populations)
        {
            Populations = populations;
            NormalizeFitnesses();
        }

        public void AddPopulation(Population population)
        {
            Populations.Add(population);
        }

        public void NormalizeFitnesses()
        {
            double sum = Populations.Sum(p => p.Fitness);
            Populations.ForEach(p => p.SetNormalizeFitness(sum));
        }

        public Population GetRandomPopulation()
        {
            List<Population> sortedPopulations = Populations
                .OrderBy(x => x.Fitness)
                .ToList();

            // Find random population based on fitness value
            double random = new Random().NextDouble();
            Population currPopulation = sortedPopulations[0];
            int index = 0;

            while (currPopulation.NormalizedFitness > 1 - random)
            {
                random -= currPopulation.NormalizedFitness;
                currPopulation = sortedPopulations[++index];
            }

            return currPopulation;
        }

        public Population GetBestPopulation()
        {
            return Populations.OrderBy(x => x.Fitness).First();
        }
    }
}
