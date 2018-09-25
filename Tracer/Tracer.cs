using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

namespace Tracer
{
    public class Tracer : ITracer
    {

        protected TraceResult traceResult;

        
        public void StartTrace()
        {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            MethodTracingResult methodResult = new MethodTracingResult
            {
                ClassName = methodBase.ReflectedType.Name,
                MethodName = methodBase.Name
            };
            ThreadTracingResult curThreadResult = traceResult.AddThreadResult(Thread.CurrentThread.ManagedThreadId);
            curThreadResult.StartTracingMethod(methodResult);
        }
        
        
        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                traceResult.GetThreadResult(threadId).StopTracingMethod();
            }
            catch (KeyNotFoundException e)
            {
                //
            }
        }
       
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
