using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tracer
{
    public class TraceResult
    {
        private ConcurrentDictionary<int, ThreadTracingResult> threadResults;

        // Свойство доступа к словарю потоков

        public List<ThreadTracingResult> ThreadResults
        {
            get
            {
                return new List<ThreadTracingResult>(new SortedDictionary<int, ThreadTracingResult>(threadResults).Values);
            }

            set
            {
                // для сериализации
            }
        }

        // Добавляет или возвращает уже существующий поток
        public ThreadTracingResult AddThreadResult(int id)
        {
            ThreadTracingResult result;


            // если поток уже есть, он заносится в result
            if (!threadResults.TryGetValue(id, out result))
            {
                result = new ThreadTracingResult(id);
                threadResults[id] = result;
            }
            return result;
        }

        public ThreadTracingResult GetThreadResult(int id)
        {
            return threadResults[id];
        }



        public TraceResult()
        {
            threadResults = new ConcurrentDictionary<int, ThreadTracingResult>();
        }
    }
}