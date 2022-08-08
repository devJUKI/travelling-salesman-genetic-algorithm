using TSP_GA.Entities;

namespace TSP_GA.Interfaces
{
    public interface IGenetic
    {
        public delegate void BestSolutionHandler(Population population);
        public event BestSolutionHandler? OnBestSolutionChanged;
        public delegate void HundredthGeneration(int generation, Population population);
        public event HundredthGeneration? OnHundredthGeneration;
        public delegate void JobDoneHandler(Population bestPopulation);
        public event JobDoneHandler? OnJobDone;
    }
}
