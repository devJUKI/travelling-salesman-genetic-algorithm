using TSP_GA.Old.Entities;

namespace TSP_GA.Old.DTOs
{
    public record GeneticAlgorithmParams(List<Location> Locations, int GenerationSize, int GenerationCount, int TravellerCount, bool MultipleThreads);
}
