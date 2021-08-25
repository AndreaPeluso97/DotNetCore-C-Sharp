using System;
using System.Collections.Generic;
using System.Linq;

namespace app1
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new[] { 5, 1, 4, 3, 2, 10, 32, 22, 104, 14, 1 };
            var result = CalculateTest(1, 4, numbers);
            Console.WriteLine($"Sum: {result.Sum:000} - Product: {result.Product:000}");
        }

        static (int Sum, int Product) CalculateTest(int startIndex, int count, IEnumerable<int> numbers)
            {
            if (startIndex < 0) throw new ArgumentException($"{nameof(startIndex)} must be greater than zero");
            if (count % 2 != 0) throw new ArgumentException($"{nameof(count)} must be even");
            if (startIndex + count > (numbers?.Count() ?? default)) throw new ArgumentException($"Not enough elements in {nameof(numbers)}");

            int Calculate(Func<int> getA, Func<int> getB, out int product, out int difference)
            {
                var a = getA?.Invoke() ?? default;
                var b = getB?.Invoke() ?? default;
                product = a * b;
                difference = b - a;
                return a + b;
            }

            var sumOfProducts = 0;
            var sumOfSums = numbers
                .Skip(startIndex)
                .Take(count)
                .Select((element, index) => new { element, index })
                .GroupBy(a => a.index / 2)
                .Select(group => new { A = group.First().element, B = group.Last().element })
                .Select(item =>
                {
                    var sum = Calculate(() => item.A, () => item.B, out var product, out _);
                    sumOfProducts += product;
                    return sum;
                })
                .Sum();

            return (sumOfSums, sumOfProducts);
        }
    }
}
