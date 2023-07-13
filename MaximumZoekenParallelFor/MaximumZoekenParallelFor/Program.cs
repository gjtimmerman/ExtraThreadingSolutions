namespace MaximumZoekenParallelFor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            list.AddRange(Enumerable.Range(0, 100));
            list.AddRange(Enumerable.Range(-100, 100));
            list.AddRange(Enumerable.Range(0, 90));
            list.Add(99);
            int maxIndex;
            if (list.Count == 0 )
            {
                maxIndex = -1;
            }
            else
            {
                maxIndex = list[0];
            }
//            object lockObject = new object();
            ParallelLoopResult result = Parallel.For(1, list.Count, i =>
            {
                //if (list[i] > list[maxIndex])
                //{
                //    lock (lockObject)
                //    {
                //        if (list[i] > list[maxIndex])
                //            maxIndex = i;

                //    }
                //}
                int initialValue;
                do
                {
                    initialValue = maxIndex;
                    if (list[i] <= list[initialValue])
                        break;
                } while (Interlocked.CompareExchange(ref maxIndex, i, initialValue) != initialValue);
                
            });
            Console.WriteLine(maxIndex);
        }
    }
}