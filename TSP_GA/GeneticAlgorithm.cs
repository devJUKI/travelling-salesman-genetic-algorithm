using TSP_GA.Entities;
using TSP_GA.Interfaces;
using TSP_GA.Utilities;

namespace TSP_GA
{
    internal class GeneticAlgorithm : IJob, IGenetic
    {
        public delegate void BestSolutionHandler(Population population);
        public event IGenetic.BestSolutionHandler? OnBestSolutionChanged;
        public delegate void HundredthGeneration(int generation, Population population);
        public event IGenetic.HundredthGeneration? OnHundredthGeneration;
        public delegate void JobDoneHandler(Population bestPopulation);
        public event IGenetic.JobDoneHandler? OnJobDone;

        private readonly int generationSize;
        private readonly int generationCount;
        private readonly int travellerCount;
        private readonly bool singleThread;

        private readonly int placeCount;

        private Population? bestPopulation;
        private Population? bestGenPopulation;
        private Generation currGeneration;

        public GeneticAlgorithm(int genSize, int genCount, int travellerCount, bool singleThread = true) {
            this.travellerCount = travellerCount;
            this.singleThread = singleThread;
            generationSize = genSize;
            generationCount = genCount;
            placeCount = Program.Places.Count;

            currGeneration = GetInitialGeneration();
            CheckBestPopulation(currGeneration);
        }

        public void Start() {
            if (singleThread) {
                for (int i = 1; i <= generationCount; i++) {
                    ExecuteStep(currGeneration, i);
                }
            } else {
                Parallel.For(1, generationCount, (i) => {
                    ExecuteStep(currGeneration, i);
                });
            }

            OnJobDone?.Invoke(bestPopulation!);
        }

        private void ExecuteStep(Generation generation, int index) {
            currGeneration = GetNextGeneration(generation);
            currGeneration.NormalizeFitnesses();

            if (index % 100 == 0) {
                OnHundredthGeneration?.Invoke(index, bestGenPopulation!);
            }

            CheckBestPopulation(currGeneration);
        }

        private void CheckBestPopulation(Generation generation) {
            bestGenPopulation = generation.GetBestPopulation();

            if (bestGenPopulation < bestPopulation!) {
                bestPopulation = bestGenPopulation;
                OnBestSolutionChanged?.Invoke(bestPopulation);
            }
        }

        private Generation GetInitialGeneration() {
            Generation generation = new();

            for (int i = 0; i < generationSize; i++) {
                Population random = Population.GenerateRandom(travellerCount, placeCount);
                generation.AddPopulation(random);
            }

            generation.NormalizeFitnesses();
            bestPopulation = generation.Populations[0];

            return generation;
        }

        private Generation GetNextGeneration(Generation currGeneration) {
            Generation nextGen = new();
            Population A = currGeneration.GetRandomPopulation();
            Population B = currGeneration.GetRandomPopulation();
            for (int j = 0; j < currGeneration.Populations.Count; j++) {
                Population population = Crossover(A, B);
                population = Mutate(population);
                nextGen.AddPopulation(population);
            }

            return nextGen;
        }

        private Population Mutate(Population population) {
            List<List<int>> orders = new();
            for (int i = 0; i < population.Paths.Count; i++) {
                List<int> shuffledOrder = population.Paths[i].Shuffle();
                // Make sure 0 is always the first element
                shuffledOrder.Remove(0);
                shuffledOrder.Insert(0, 0);
                orders.Add(shuffledOrder);
            }
            return new Population(orders);
        }

        private Population Crossover(Population A, Population B) {
            List<List<int>> orders = new();

            Random random = new();
            for (int k = 0; k < A.Paths.Count; k++) {
                int start = random.Next(0, A.Paths[k].Count);
                int end = random.Next(start + 1, B.Paths[k].Count);
                List<int> order = A.Paths[k].GetRange(start, end - start);

                int left = Program.Places.Count - order.Count;
                for (int i = 0; i < left; i++) {
                    for (int j = 0; j < B.Paths[k].Count; j++) {
                        if (!order.Contains(B.Paths[k][j])) {
                            order.Add(B.Paths[k][j]);
                        }
                    }
                }

                // Make sure 0 is always the first element
                order.Remove(0);
                order.Insert(0, 0);
                orders.Add(order);
            }

            return new Population(orders);
        }
    }
}
