using TSP_GA.Entities;

namespace TSP_GA.DTOs
{
    public record GeneticAlgorithmParams(List<Location> Locations, int GenerationSize, int GenerationCount, int TravellerCount, bool MultipleThreads);
}
