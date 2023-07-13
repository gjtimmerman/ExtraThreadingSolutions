using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace LangsteWoordZoekenThreads
{
    class WorkToDo
    {
        public WorkToDo(int lower, int upper) {
            this.lower = lower;
            this.upper = upper;
        }
        public int lower;
        public int upper;
        public int indexLongestWord;
    }
    internal class Program
    {
        internal static MatchCollection matchCollection;
        internal static void FindLongestWordInterval(object? todo)
        {
            if (todo == null)
                return;
            WorkToDo workToDo = (WorkToDo)todo;
            if (workToDo.lower > workToDo.upper)
            {
                workToDo.indexLongestWord = -1;
                return;
            }
            else if (workToDo.lower == workToDo.upper)
            {
                workToDo.indexLongestWord = workToDo.lower;
                return;
            }
            else
                workToDo.indexLongestWord = workToDo.lower;
            for (int i = workToDo.lower + 1;  i < workToDo.upper; i++)
            {
                if (matchCollection[i].Value.Length > matchCollection[workToDo.indexLongestWord].Value.Length)
                    workToDo.indexLongestWord = i;
            }
        }

        public static int FindLongestWord(string text)
        {
            Regex regex = new Regex(@"\w+");
            matchCollection = regex.Matches(text);
            if (matchCollection.Count > 10)
            {
                int ThreadCount = matchCollection.Count / 5;
                Thread[] thlist = new Thread[ThreadCount];
                WorkToDo[] todolist = new WorkToDo[ThreadCount];
                for (int i = 0; i < ThreadCount; i++)
                {
                    todolist[i] = new WorkToDo(matchCollection.Count * i / ThreadCount,matchCollection.Count * (i + 1)/ThreadCount);
                    thlist[i] = new Thread(FindLongestWordInterval);
                    thlist[i].Start(todolist[i]);
                    
                }
                foreach (Thread th in thlist)
                    th.Join();
                if (todolist[0].indexLongestWord == -1)
                    return -1;
                else
                {
                    int indexLongestWord = todolist[0].indexLongestWord;
                    for (int i = 1; i < ThreadCount; i++)
                        if (matchCollection[i].Value.Length > matchCollection[indexLongestWord].Value.Length)
                            indexLongestWord = i;
                    return matchCollection[indexLongestWord].Index;
                }
            }
            else
            {
                WorkToDo workToDo = new WorkToDo(0, matchCollection.Count);
                FindLongestWordInterval(workToDo);
                if (workToDo.indexLongestWord == -1)
                    return -1;
                else
                    return matchCollection[workToDo.indexLongestWord].Index;
            }
        }
        static void Main(string[] args)
        {
            
            Console.WriteLine(FindLongestWord("Dit is een string,met.allerlei\nwoorden"));
        }
    }
}