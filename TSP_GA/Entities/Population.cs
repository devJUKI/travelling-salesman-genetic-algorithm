using TSP_GA.Utilities;

namespace TSP_GA.Entities
{
    public class Population
    {
        public List<List<int>> Paths { get; private set; }
        public double Fitness { get; private set; }
        public double NormalizedFitness { get; private set; }
        public double Price { get; private set; }

        public Population(List<List<int>> paths) {
            Paths = paths;
            Fitness = GetFitness();
            Price = GetPrice();
        }

        public static Population GenerateRandom(int travellerCount, int placeCount) {
            List<List<int>> paths = new();

            int placesPerPerson = (int)Math.Ceiling((placeCount - 1f) / travellerCount);
            List<int> ascNumbers = Enumerable.Range(1, placeCount - 1).ToList();

            for (int j = 0; j < travellerCount; j++) {
                int pathsToAdd = j == travellerCount - 1 ? ascNumbers.Count : placesPerPerson;
                paths.Add(ascNumbers.GetRange(0, pathsToAdd));
                ascNumbers.RemoveRange(0, pathsToAdd);
            }

            for (int j = 0; j < paths.Count; j++) {
                paths[j] = paths[j].Shuffle();
                paths[j].Insert(0, 0);
            }

            return new Population(paths);
        }

        private double GetFitness() {
            List<double> times = new();
            // 1 km = 1 min
            Paths.ForEach(order => times.Add(GetDistance(order)));
            // Adding rest times
            for (int i = 0; i < times.Count; i++) {
                int timeInPlaces = Paths[i].Count * 60;
                int restTimes = (int)Math.Floor(times[i] / 16);
                times[i] = times[i] + restTimes * 8 * 60 + timeInPlaces;
            }
            return times.Max();
        }

        public void SetNormalizeFitness(double sum) {
            NormalizedFitness = Fitness / sum;
        }

        private double GetPrice() {
            List<double> distances = new();
            Paths.ForEach(order => distances.Add(GetDistance(order)));
            double price = 0;
            distances.ForEach(d => price += Math.Sqrt(d));
            return price;
        }

        private double GetDistance(List<int> order) {
            double distance = 0;
            for (int i = 0; i < order.Count - 1; i++) {
                Location place1 = Program.Places[order[i]];
                Location place2 = Program.Places[order[i + 1]];
                // Convert to km
                double xDiff = (place1.X - place2.X) / 1000;
                double yDiff = (place1.Y - place2.Y) / 1000;
                distance += Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
            }
            return distance;
        }

        public override string ToString() {
            string orderString = "";
            //for (int i = 0; i < Orders.Count; i++) {
            //    orderString += $"Order {i}: ";
            //    for (int j = 0; j < Orders[i].Count; j++) {
            //        orderString += Orders[i][j];
            //        if (j != Orders[i].Count - 1) {
            //            orderString += "-";
            //        }
            //    }
            //    orderString += "\n\n";
            //}
            orderString += $"Fitness: {Fitness:F2}\n";
            return orderString;
        }

        public static bool operator <(Population left, Population right) {
            if (left.Fitness < right.Fitness)
                return true;

            if (left.Fitness == right.Fitness && left.Price < right.Price) {
                return true;
            }

            return false;
        }

        public static bool operator >(Population left, Population right) {
            if (left.Fitness > right.Fitness)
                return true;

            if (left.Fitness == right.Fitness && left.Price > right.Price) {
                return true;
            }

            return false;
        }

        public static bool operator ==(Population left, Population right) {
            return left.Fitness == right.Fitness && left.Price == right.Price;
        }

        public static bool operator !=(Population left, Population right) {
            return !(left == right);
        }

        public override bool Equals(object? obj) {
            if (obj is Population pop) {
                return pop.Paths.SequenceEqual(Paths);
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
