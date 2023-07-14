namespace ParallelMergeSortTasks
{
    internal class WorkToDo
    {
        public WorkToDo(int lower, int upper)
        {
            this.lower = lower;
            this.upper = upper;
        }
        public int lower;
        public int upper;
    }
    internal class Program
    {
        private static int[] numbers;

        public static List<int> MergeSort(object? obj)
        {
            if (obj == null)
                return null;
            WorkToDo workToDo = (WorkToDo)obj;
            List<int> result = new List<int>();
            
            if (workToDo.lower > workToDo.upper - 1)
                return result;
            if (workToDo.lower == workToDo.upper - 1)
            {
                result.Add(numbers[workToDo.lower]);
                return result;
            }
            if (workToDo.upper - workToDo.lower == 2)
            {
                if (numbers[workToDo.lower] <= numbers[workToDo.lower + 1])
                {
                    result.Add(numbers[workToDo.lower]);
                    result.Add(numbers[workToDo.lower + 1]);
                }
                else
                {
                    result.Add(numbers[workToDo.lower + 1]);
                    result.Add(numbers[workToDo.lower]);

                }
                return result;
            }
            else
            {
                WorkToDo workToDo1 = new WorkToDo(workToDo.lower, workToDo.lower + (workToDo.upper - workToDo.lower) / 2);
                WorkToDo workToDo2 = new WorkToDo(workToDo.lower + (workToDo.upper - workToDo.lower) / 2, workToDo.upper);
                Task<List<int>> t1 = new Task<List<int>>(MergeSort,workToDo1);
                t1.Start();
                Task<List<int>> t2 = new Task<List<int>>(MergeSort, workToDo2);
                t2.Start();
                t1.Wait();
                t2.Wait();
                int i;
                int j;
                for (i = 0, j = 0; i < t1.Result.Count && j < t2.Result.Count;)
                {
                    if (t1.Result[i] <= t2.Result[j])
                        result.Add(t1.Result[i++]);
                    else
                        result.Add(t2.Result[j++]);
                }
                if (i == t1.Result.Count)
                    result.AddRange(t2.Result.GetRange(j, t2.Result.Count - j));
                else
                    result.AddRange(t1.Result.GetRange(i, t1.Result.Count - i));
                return result;
            }
        }
        static void Main(string[] args)
        {
            List<int> ints = new List<int>();
            ints.AddRange(Enumerable.Range(0, 100).Reverse());
            ints.AddRange(Enumerable.Range(-10, 200));
            numbers = ints.ToArray();
            WorkToDo workToDo = new WorkToDo(0, numbers.Length);
            List<int> result = MergeSort(workToDo);
            foreach (int i in result)
                Console.WriteLine(i);
            Console.WriteLine();
            Console.WriteLine(result.Count);
        }
    }
}