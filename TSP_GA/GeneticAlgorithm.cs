﻿using TSP_GA.DTOs;
using TSP_GA.Entities;
using TSP_GA.Utilities;

namespace TSP_GA
{
    public class GeneticAlgorithm
    {
        public delegate void BestSolutionHandler(Population population);
        public event BestSolutionHandler? OnBestSolutionChanged;
        public delegate void HundredthGeneration(int generation, Population population);
        public event HundredthGeneration? OnHundredthGeneration;
        public delegate void JobDoneHandler(Population bestPopulation);
        public event JobDoneHandler? OnJobDone;

        private readonly GeneticAlgorithmParams @params;

        private Population? bestPopulation;
        private Population? bestGenPopulation;
        private Generation currGeneration;

        public GeneticAlgorithm(GeneticAlgorithmParams @params)
        {
            this.@params = @params;

            currGeneration = GetInitialGeneration();
            CheckBestPopulation(currGeneration);
        }

        public void Start()
        {
            if (@params.MultipleThreads == false)
            {
                for (int i = 1; i <= @params.GenerationCount; i++)
                {
                    ExecuteStep(currGeneration, i);
                }
            }
            else
            {
                Parallel.For(1, @params.GenerationCount, (i) =>
                {
                    ExecuteStep(currGeneration, i);
                });
            }

            OnJobDone?.Invoke(bestPopulation!);
        }

        private void ExecuteStep(Generation generation, int index)
        {
            currGeneration = GetNextGeneration(generation);
            currGeneration.NormalizeFitnesses();

            if (index % 100 == 0)
            {
                OnHundredthGeneration?.Invoke(index, bestGenPopulation!);
            }

            CheckBestPopulation(currGeneration);
        }

        private void CheckBestPopulation(Generation generation)
        {
            bestGenPopulation = generation.GetBestPopulation();

            if (bestGenPopulation < bestPopulation!)
            {
                bestPopulation = bestGenPopulation;
                OnBestSolutionChanged?.Invoke(bestPopulation);
            }
        }

        private Generation GetInitialGeneration()
        {
            Generation generation = new();

            for (int i = 0; i < @params.GenerationSize; i++)
            {
                Population random = Population.GenerateRandom(@params.TravellerCount, @params.Locations.Count);
                generation.AddPopulation(random);
            }

            generation.NormalizeFitnesses();
            bestPopulation = generation.Populations[0];

            return generation;
        }

        private Generation GetNextGeneration(Generation currGeneration)
        {
            Generation nextGen = new();
            Population A = currGeneration.GetRandomPopulation();
            Population B = currGeneration.GetRandomPopulation();
            for (int j = 0; j < currGeneration.Populations.Count; j++)
            {
                Population population = Crossover(A, B);
                population = Mutate(population);
                nextGen.AddPopulation(population);
            }

            return nextGen;
        }

        private Population Mutate(Population population)
        {
            List<List<int>> orders = new();
            for (int i = 0; i < population.Paths.Count; i++)
            {
                List<int> shuffledOrder = population.Paths[i].Shuffle();
                // Make sure 0 is always the first element
                shuffledOrder.Remove(0);
                shuffledOrder.Insert(0, 0);
                orders.Add(shuffledOrder);
            }
            return new Population(orders);
        }

        private Population Crossover(Population A, Population B)
        {
            List<List<int>> orders = new();

            Random random = new();
            for (int k = 0; k < A.Paths.Count; k++)
            {
                int start = random.Next(0, A.Paths[k].Count);
                int end = random.Next(start + 1, B.Paths[k].Count);
                List<int> order = A.Paths[k].GetRange(start, end - start);

                int left = @params.Locations.Count - order.Count;
                for (int i = 0; i < left; i++)
                {
                    for (int j = 0; j < B.Paths[k].Count; j++)
                    {
                        if (!order.Contains(B.Paths[k][j]))
                        {
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
