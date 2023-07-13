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
        public int maxIndex;

    }
    internal class Program
    {
        private static List<int> list;
        public static void FindMaxInterval(object ?Work)
        {
            if (Work == null)
                return;
            WorkToDo workToDo = (WorkToDo)Work;
            int maxIndex;
            if (workToDo.lower > workToDo.higher)
            {
                workToDo.maxIndex = -1;
                return;
            }
            else if (workToDo.lower == workToDo.higher)
            {
                workToDo.maxIndex = workToDo.lower;
                return;
            }
            else
                maxIndex = workToDo.lower;
            for (int i = workToDo.lower + 1; i < workToDo.higher; i++)
            {
                if (list[i] > list[maxIndex])
                    maxIndex = i;
            }
            workToDo.maxIndex = maxIndex;
        }
        public static int FindMaximum(List<int> list)
        {
            Program.list = list;
            if (list.Count > 10)
            {
                int TaskCount = list.Count / 5;
                WorkToDo [] workToDoList = new WorkToDo[TaskCount];
                Task[] tList = new Task[TaskCount]; 
                for (int i = 0; i < TaskCount; i++)
                {
                    workToDoList[i] = new WorkToDo(list.Count * i / TaskCount, list.Count * (i+1)/TaskCount);
                    tList[i] = new Task(FindMaxInterval, workToDoList[i]);
                    tList[i].Start();

                }
                Task.WaitAll(tList);
                int maxIndex = workToDoList[0].maxIndex;
                if (maxIndex == -1)
                    return maxIndex; 
                for (int i = 1; i < TaskCount; i++)
                    if (list[workToDoList[i].maxIndex] > list[maxIndex])
                        maxIndex = workToDoList[i].maxIndex;
                return maxIndex;
            }
            else
            {
                WorkToDo workToDo = new WorkToDo(0, list.Count);
                FindMaxInterval(workToDo);
                return workToDo.maxIndex;

            }    
        }

        static void Main(string[] args)
        {
            List<int> list = new List<int>();
 //           list.Add(1000);
            list.AddRange(Enumerable.Range(0, 100).ToList());
            list.AddRange(Enumerable.Range(0, 99).ToList());
            list.AddRange(Enumerable.Range(-100, 0));
            int maxIndex = FindMaximum(list);

            Console.WriteLine($"1: {maxIndex}");
        }
    }
}