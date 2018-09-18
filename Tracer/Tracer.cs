using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    class Tracer : ITracer
    {

        protected TraceResult traceResult;

        // вызывается в начале замеряемого метода
        public void StartTrace()
        {

        }
    ​
        // вызывается в конце замеряемого метода
        public void StopTrace()
        {

        }
    ​
        // получить результаты измерений
        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public Tracer()
        {
            traceResult = new TraceResult();
        }

    }
}
