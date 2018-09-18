using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Tracer
{
    public class ThreadTracingResult
    {

        private Stack<MethodTracingResult> threadMethods;
        private List<MethodTracingResult> tracedMethods;

        // ID потока 
        public int ThreadID { get; set; }

        public long ExecutionTime
        {
            get
            {
                // Получаем время выполнения всех методов

                
            }
        }

        public ThreadTracingResult(int id)
        {
            // TO be continued
        }


    }
}
