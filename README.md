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
```

But to calculate the complexity of this method, we first need to calculate the complexity of the methods that it calls.
Let's dive into <i>ExecuteStep</i> method.

### ExecuteStep()

```cs
private void ExecuteStep(Generation generation, int index) {
    currGeneration = GetNextGeneration(generation);                 // c1 | P * T * L^2
    currGeneration.NormalizeFitnesses();                            // c2 | P

    if (index % 100 == 0) {
        OnHundredthGeneration?.Invoke(index, bestGenPopulation!);
    }

    CheckBestPopulation(currGeneration);                            // c3 | P
}
```

<i>ExecuteStep</i> method consists of 3 main methods - <i>GetNextGeneration</i>, <i>NormalizeFitnesses</i> and <i>CheckBestPopulation</i>. We will start from the smallest, independent methods and work our way up to the main methods to calculate their time complexities.

## GetNextGeneration()

```cs
private Generation GetNextGeneration(Generation currGeneration) {
    Generation nextGen = new();                                     // c1 | 1
    Population A = currGeneration.GetRandomPopulation();            // c2 | 1 * P^2
    Population B = currGeneration.GetRandomPopulation();            // c3 | 1 * P^2
    for (int j = 0; j < currGeneration.Populations.Count; j++) {    // c4 | P + 1
        Population population = Crossover(A, B);                    // c5 | P * T * L^2
        population = Mutate(population);                            // c6 | P * TL
        nextGen.AddPopulation(population);                          // c7 | P
    }

    return nextGen;                                                 // c8 | 1
}
```

We can see which methods need their time complexities to be calculated, so let's get started.

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

It's time complexity would be:
> T(P) = P^2 + 1 + 1 + 1 + P + P + P + 1 = P^2 + 3P = P^2 = O(P^2)

### 

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
    List<List<int>> orders = new();                                 // c1  | 1 
    Random random = new();                                          // c2  | 1

    for (int k = 0; k < A.Paths.Count; k++) {                       // c3  | T + 1
        int start = random.Next(0, A.Paths[k].Count);               // c4  | T
        int end = random.Next(start + 1, B.Paths[k].Count);         // c5  | T
        List<int> order = A.Paths[k].GetRange(start, end - start);  // c6  | T

        int left = Program.Places.Count - order.Count;              // c7  | T
        for (int i = 0; i < left; i++) {                            // c8  | T * (L + 1)
            for (int j = 0; j < B.Paths[k].Count; j++) {            // c9  | TL * (L + 1)
                if (!order.Contains(B.Paths[k][j])) {               // c10 | T * L^2
                    order.Add(B.Paths[k][j]);                       // c11 | T * L^2
                }
            }
        }

        // Make sure 0 is always the first element
        order.Remove(0);                                            // c12 | T
        order.Insert(0, 0);                                         // c13 | T
        orders.Add(order);                                          // c14 | T
    }

    return new Population(orders);                                  // c15 | 1 * TL
}
```

<i>Crossover()</i> time complexity is:

<p align="center">
  <img src="https://github.com/devJUKI/TSP_GA/blob/main/img6.png" alt="drawing" width="450"/>
</p>
