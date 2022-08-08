namespace TSP_GA.Entities
{
    internal class Generation
    {
        public List<Population> Populations { get; private set; }

        public Generation() {
            Populations = new();
        }

        public void AddPopulation(Population population) {
            Populations.Add(population);
        }

        public void NormalizeFitnesses() {
            double sum = 0;
            Populations.ForEach(gen => sum += gen.Fitness);
            for (int i = 0; i < Populations.Count; i++) {
                Populations[i].SetNormalizeFitness(sum);
            }
        }

        /// <summary>
        /// Gets random population based on fitness values
        /// </summary>
        public Population GetRandomPopulation() {
            List<Population> sortedPopulations = Populations.OrderBy(x => x.Fitness).ToList();
            double random = new Random().NextDouble();
            int index = 0;
            Population currPopulation = sortedPopulations[0];
            while (currPopulation.NormalizedFitness > 1 - random) {
                random -= currPopulation.NormalizedFitness;
                currPopulation = sortedPopulations[++index];
            }
            return currPopulation;
        }

        public Population GetBestPopulation() {
            return Populations.OrderBy(x => x.Fitness).First();
        }
    }
}
