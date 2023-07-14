using System.Runtime.CompilerServices;

namespace MaximumZoekenTasks
{
    class WorkToDo
    {
        public WorkToDo(int lower, int higher)
        {
            this.lower = lower;
            this.higher = higher;
        }
        public int lower;
        public int higher;

    }
    internal class Program
    {
        private static List<int> list;
        public static int FindMaxInterval(object ?Work)
        {
            if (Work == null)
                return -1;
            WorkToDo workToDo = (WorkToDo)Work;
            int maxIndex;
            if (workToDo.lower > workToDo.higher)
            {
                return -1;
            }
            else if (workToDo.lower == workToDo.higher)
            {
                return workToDo.lower;
            }
            else
                maxIndex = workToDo.lower;
            for (int i = workToDo.lower + 1; i < workToDo.higher; i++)
            {
                if (list[i] > list[maxIndex])
                    maxIndex = i;
            }
            return maxIndex;
        }
        public static int FindMaximum(List<int> list)
        {
            Program.list = list;
            if (list.Count > 10)
            {
                int TaskCount = list.Count / 5;
                WorkToDo [] workToDoList = new WorkToDo[TaskCount];
                Task<int>[] tList = new Task<int>[TaskCount]; 
                for (int i = 0; i < TaskCount; i++)
                {
                    workToDoList[i] = new WorkToDo(list.Count * i / TaskCount, list.Count * (i+1)/TaskCount);
                    tList[i] = new Task<int>(FindMaxInterval, workToDoList[i]);
                    tList[i].Start();

                }
                Task.WaitAll(tList);
                int maxIndex = tList[0].Result;
                for (int i = 1; i < TaskCount; i++)
                    if (list[tList[i].Result] > list[maxIndex])
                        maxIndex = tList[i].Result;
                return maxIndex;
            }
            else
            {
                WorkToDo workToDo = new WorkToDo(0, list.Count);
                return FindMaxInterval(workToDo);

            }    
        }

        static void Main(string[] args)
        {
            List<int> list = new List<int>();
//            list.Add(1000);
            list.AddRange(Enumerable.Range(0, 100).ToList());
            list.AddRange(Enumerable.Range(0, 99).ToList());
            list.AddRange(Enumerable.Range(-100, 150));
            int maxIndex = FindMaximum(list);

            Console.WriteLine($"1: {maxIndex}");
        }
    }
}