namespace ParallelQuickSort
{
    public struct Boundaries
    {

        public int lower;
        public int upper;
    }
    internal class Program
    {
        public static int[] numbers;

        public static void swap(ref int x, ref int y)
        {
            int tmp = x;
            x = y;
            y = tmp;
        }
        public static void swap(ref int x, ref int y, ref int z)
        {
            int tmp = x;
            x = y;
            y = z;
            z = tmp;
        }
        public static int Partition(int lower, int upper)
        {
            int pivot = lower;
            for (int i = lower + 1; i < upper; i++)
            {
                if (numbers[i] < numbers[pivot])
                {
                    swap(ref numbers[pivot], ref numbers[pivot + 1], ref numbers[i]);
                    pivot++;
                }
            }
            return pivot;
        }
        public static void QuickSort(object ?obj)
        {
            if (obj == null)
                return;
            Boundaries boundaries = (Boundaries)obj;
            if (boundaries.lower >= (boundaries.upper - 1))
                return;
            if ((boundaries.upper - boundaries.lower) == 2)
            {
                if (numbers[boundaries.lower] <= numbers[boundaries.upper-1])
                    return;
                swap(ref numbers[boundaries.lower], ref numbers[boundaries.upper-1]);
            }
            int pivot = Partition(boundaries.lower, boundaries.upper);
            Thread th1 = new Thread(QuickSort);
            Boundaries boundariesTh1;
            boundariesTh1.lower = boundaries.lower;
            boundariesTh1.upper = pivot;
            th1.Start(boundariesTh1);
            Thread th2 = new Thread(QuickSort);
            Boundaries boundariesTh2;
            boundariesTh2.lower = pivot + 1;
            boundariesTh2.upper = boundaries.upper;
            th2.Start(boundariesTh2);
            th1.Join();
            th2.Join();

        }
        static void Main(string[] args)
        {
            List<int> ints = new List<int>();
            ints.AddRange(Enumerable.Range(0,100).Reverse());
            ints.AddRange(Enumerable.Range(-10, 200));
            numbers = ints.ToArray();
            Boundaries boundaries;
            boundaries.lower = 0;
            boundaries.upper = numbers.Length;
            QuickSort(boundaries);
            foreach (int i in numbers)
                Console.WriteLine(i);
        }
    }
}