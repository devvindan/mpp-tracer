using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;


namespace Tracer
{
    public class MethodTracingResult
    {

        // Таймер, список внутренних методов

        private Stopwatch timer;
        private List<MethodTracingResult> innerMethods;

        // Поля с именем класса и метода

        public string ClassName { get; set; }
        public string MethodName { get; set; }

        // Свойства доступа к списку внутренних методов и времени выполнения.

        public long ExecutionTime
        {
            get
            {
                return timer.ElapsedMilliseconds;
            }
        }

        public List<MethodTracingResult> InnerMethods
        {
            get
            {
                return new List<MethodTracingResult>(innerMethods);
            }

            private set
            {
                // для сериализации
            }
        }


        // Строковое представление времени

        public string ExecutionTimeString
        {
            get
            {
                return ExecutionTime.ToString() + " ms";
            }

            private set
            {
                // для сериализации
            }
        }


        public MethodTracingResult()
        {
            innerMethods = new List<MethodTracingResult>();
            timer = new Stopwatch();
        }

        public void addInnerMethod(MethodTracingResult method)
        {
            innerMethods.Add(method);
        }

        // Методы для работы с таймером

        public void startTracing()
        {
            timer.Start();
        }

        public void stopTracing()
        {
            timer.Stop();
        }

    }
}
