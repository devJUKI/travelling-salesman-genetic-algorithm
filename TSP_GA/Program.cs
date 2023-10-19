using TSP_GA.Entities;

namespace TSP_GA
{
    public class Program
    {
        private const string DATA_PATH = "data.txt";
        private const string RESULTS_FILE = "output.txt";

        private static List<Location> places = new();

        private static void Main() {
            places = IOUtilities.ReadData(DATA_PATH);

            var algorithm = ExecuteDialog();

            algorithm.OnBestSolutionChanged += OnBestSolutionChanged;
            algorithm.OnHundredthGeneration += OnHundredthGeneration;
            algorithm.OnJobDone += OnJobDone;

            algorithm.Start();
        }

        private static GeneticAlgorithm ExecuteDialog() {
            Console.Clear();
            Console.WriteLine("Travelling Salesman Problem (with Genetic Algorithm)");
            Console.WriteLine("\nWhat do you want to execute?:");
            Console.WriteLine("1. Genetic Algorithm (single thread)");
            Console.WriteLine("2. Genetic Algorithm (multiple threads)\n");
            char pick = Console.ReadKey().KeyChar;
            Console.Clear();

            Console.Write($"You chose option {pick} - ");

            switch (pick) {
                case '1':
                    Console.WriteLine("Genetic Algorithm (single thread)\n");
                    return new GeneticAlgorithm(10, 500, 5);
                case '2':
                    Console.WriteLine("Genetic Algorithm (multiple threads)\n");
                    return new GeneticAlgorithm(10, 500, 5, true);
                default:
                    return ExecuteDialog();
            }
        }

        private static void OnBestSolutionChanged(Population population) {
            Console.WriteLine($"New best population found. New best fitness {population.Fitness:F2}");
        }

        private static void OnHundredthGeneration(int generation, Population population) {
            Console.WriteLine($"Generation: {generation}. Best generation's fitness: {population.Fitness:F2}");
        }

        private static void OnJobDone(Population population) {
            Console.WriteLine("\nBest population found:");
            Console.WriteLine(population);
            File.WriteAllText(RESULTS_FILE, population.ToString());
        }
    }
}
