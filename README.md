# TSP_GA

Travelling salesman problem using genetic algorithm

## Algorithm flowchart

<p align="center">
  <img src="https://github.com/devJUKI/TSP_GA/blob/main/img1.png" alt="drawing" width="500"/>
</p>

## Time complexity

Genetic algorithm starts from this method

```cs
public void Start() {
    List < Population > generation = GetInitialGeneration(); // c1 | 1
    for (int i = 1; i <= generationCount; i++) { // c2 | n + 1
        generation = GetNextGeneration(generation); // c3 | n
        NormalizeFitness(generation); // c4 | n
        CheckBestPopulation(generation); // c5 | n
    }
    OnJobDone?.Invoke(bestPopulation); // c6 | n
}
```

But to calculate the complexity of this method, you first need to calculate the complexity of the methods that it calls.
Let's start with a method whose performance is independent from other methods.

### Shuffle()

```cs
public static List<T> Shuffle<T>(this List<T> list) {
    List<T> temp = new(list);                     // c1 | 1
    Random random = new();                        // c2 | 1
    int n = temp.Count;                           // c3 | 1
    while (n > 1) {                               // c4 | n + 1
        n--;                                      // c5 | n
        int k = random.Next(n + 1);               // c6 | n
        (temp[n], temp[k]) = (temp[k], temp[n]);  // c7 | n
    }
    return temp;                                  // c8 | 1
}
```

<i>Shuffle()</i> time complexity is:

<p align="center">
  <img src="https://github.com/devJUKI/TSP_GA/blob/main/img2.png" alt="drawing" width="450"/>
</p>

### GetRandomPopulation()

```cs
public Population GetRandomPopulation() {
    List<Population> sortedPopulations = Populations.OrderBy(x => x.Fitness).ToList();  // c1 | P^2
    double random = new Random().NextDouble();                                          // c2 | 1
    int index = 0;                                                                      // c3 | 1
    Population currPopulation = sortedPopulations[0];                                   // c4 | 1
    while (currPopulation.NormalizedFitness > 1 - random) {                             // c5 | P + 1
        random -= currPopulation.NormalizedFitness;                                     // c6 | P
        currPopulation = sortedPopulations[++index];                                    // c7 | P
    }
    return currPopulation;                                                              // c8 | 1
}
```

<i>GetRandomPopulation()</i> time complexity is:

<p align="center">
  <img src="https://github.com/devJUKI/TSP_GA/blob/main/img3.png" alt="drawing" width="450"/>
</p>

### NormalizeFitness()

```cs
private void NormalizeFitness(List < Population > generation) {
    double sum = 0; // c1 | 1
    generation.ForEach(gen => sum += gen.Fitness); // c2 | 1
    for (int i = 0; i < generation.Count; i++) { // c3 | n + 1
        generation[i].SetNormalizeFitness(sum); // c4 | n
    }
}
```

<i>NormalizeFitness()</i> time complexity is:

<p align="center">
  <img src="https://github.com/devJUKI/TSP_GA/blob/main/img4.png" alt="drawing" width="450"/>
</p>

### Mutate()

```cs
private Population Mutate(Population population) {
    List < List < int >> orders = new List < List < int >> (); // c1 | 1
    for (int i = 0; i < population.Orders.Count; i++) { // c2 | n + 1
        List < int > shuffledOrder = Shuffle(population.Orders[i]); // c3 | n
        // Make sure 0 is always the first element
        shuffledOrder.Remove(0); // c4 | n
        shuffledOrder.Insert(0, 0); // c5 | n
        orders.Add(shuffledOrder); // c6 | n
    }
    return new Population(orders); // c7 | 1
}
```

<i>Mutate()</i> time complexity is:

<p align="center">
  <img src="https://github.com/devJUKI/TSP_GA/blob/main/img5.png" alt="drawing" width="450"/>
</p>

### Crossover()

```cs
private Population Crossover(Population A, Population B) {
    List < List < int >> orders = new List < List < int >> (); // c1 | 1

    Random random = new Random(); // c2 | 1
    for (int k = 0; k < A.Orders.Count; k++) { // c3 | n + 1
        int start = random.Next(0, A.Orders[k].Count); // c4 | n
        int end = random.Next(start + 1, B.Orders[k].Count); // c5 | n
        List < int > order = A.Orders[k].GetRange(start, end - start); // c6 | n
        int left = Program.Places.Count - order.Count; // c7 | n
        for (int i = 0; i < left; i++) { // c8 | n(n + 1)
            for (int j = 0; j < B.Orders[k].Count; j++) { // c9 | n^2(n + 1)
                if (!order.Contains(B.Orders[k][j])) { // c10 | n^3
                    order.Add(B.Orders[k][j]); // c11 | n^3
                }
            }
        } // Make sure 0 is always the first element
        order.Remove(0); // c12 | n
        order.Insert(0, 0); // c13 | n
        orders.Add(order); // c14 | n
    }
    return new Population(orders); // c15 | 1
}
```

<i>Crossover()</i> time complexity is:

<p align="center">
  <img src="https://github.com/devJUKI/TSP_GA/blob/main/img6.png" alt="drawing" width="450"/>
</p>
