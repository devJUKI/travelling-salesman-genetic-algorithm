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
private List<T> Shuffle<T> (List<T> list) {
    List<T> temp = new List<T>(list); // c1 | 1
    Random random = new Random(); // c2 | 1
    int n = temp.Count; // c3 | 1
    while (n > 1) { // c4 | n + 1
        n--; // c5 | n
        int k = random.Next(n + 1); // c6 | n
        (temp[n], temp[k]) = (temp[k], temp[n]); // c7 | n
    }
    return temp; // c8 | 1
}
```

<i>Shuffle()</i> time complexity is:
