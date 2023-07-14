using System.Globalization;

namespace ParallelMergeSort
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
        public List<int> mergedNumbers;
    }
    internal class Program
    {
        private static int[] numbers;

        public static void MergeSort(object ? obj)
        {
            if (obj == null)
                return;
            WorkToDo workToDo = (WorkToDo)obj;
            workToDo.mergedNumbers = new List<int>();
            if (workToDo.lower > workToDo.upper - 1)
                return;
            if (workToDo.lower ==  workToDo.upper - 1)
            {
                workToDo.mergedNumbers.Add(numbers[workToDo.lower]);
                return;
            }
            if (workToDo.upper - workToDo.lower == 2)
            {
                if (numbers[workToDo.lower] <= numbers[workToDo.lower+1])
                {
                    workToDo.mergedNumbers.Add(numbers[workToDo.lower]);
                    workToDo.mergedNumbers.Add(numbers[workToDo.lower+1]);
                }
                else
                {
                    workToDo.mergedNumbers.Add(numbers[workToDo.lower + 1]);
                    workToDo.mergedNumbers.Add(numbers[workToDo.lower]);

                }
                return;
            }
            else
            {
                WorkToDo workToDo1 = new WorkToDo(workToDo.lower, workToDo.lower + (workToDo.upper - workToDo.lower) / 2);
                WorkToDo workToDo2 = new WorkToDo(workToDo.lower + (workToDo.upper - workToDo.lower) / 2, workToDo.upper);
                Thread th1 = new Thread(MergeSort);
                th1.Start(workToDo1 );
                Thread th2 = new Thread(MergeSort);
                th2.Start(workToDo2 );
                th1.Join();
                th2.Join();
                int i;
                int j;
                for (i = 0, j = 0; i < workToDo1.mergedNumbers.Count && j < workToDo2.mergedNumbers.Count;)
                {
                    if (workToDo1.mergedNumbers[i] <= workToDo2.mergedNumbers[j])
                        workToDo.mergedNumbers.Add(workToDo1.mergedNumbers[i++]);
                    else
                        workToDo.mergedNumbers.Add(workToDo2.mergedNumbers[j++]);
                }
                if (i == workToDo1.mergedNumbers.Count)
                    workToDo.mergedNumbers.AddRange(workToDo2.mergedNumbers.GetRange(j, workToDo2.mergedNumbers.Count - j));
                else
                    workToDo.mergedNumbers.AddRange(workToDo1.mergedNumbers.GetRange(i, workToDo1.mergedNumbers.Count - i));
                return;
            }
        }
        static void Main(string[] args)
        {
            List<int> ints = new List<int>();
            ints.AddRange(Enumerable.Range(0, 100).Reverse());
            ints.AddRange(Enumerable.Range(-10, 200));
            numbers = ints.ToArray();
            WorkToDo workToDo = new WorkToDo(0, numbers.Length);
            MergeSort(workToDo);
            foreach (int i in workToDo.mergedNumbers)
                Console.WriteLine(i);
        }
    }
}