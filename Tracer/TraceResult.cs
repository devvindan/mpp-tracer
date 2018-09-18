using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tracer
{
    public class TraceResult
    {
        private ConcurrentDictionary<int, ThreadTracingResult> threadResults;

        internal TraceResult()
        {
            threadResults = new ConcurrentDictionary<int, ThreadTracingResult>();
        }
    }
}