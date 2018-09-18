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

        private Stack<MethodResult> threadMethods;
        private List<MethodResult> tracedMethods;

        // ID потока 
        public int ThreadID { get; set; }

        public long ExecutionTime
        {
            get
            {
                // Получаем время выполнения всех методов

                
            }
        }


    }
}
