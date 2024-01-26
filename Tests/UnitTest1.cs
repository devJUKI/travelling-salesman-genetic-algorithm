using TSP_GA.New;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Chromosome_Mutate_NotEqual()
        {
            var chromosome = Chromosome.Create(5, 17);
            var mutatedChromosome = chromosome.Mutate();
            Assert.NotEqual(chromosome, mutatedChromosome);
        }
    }
}