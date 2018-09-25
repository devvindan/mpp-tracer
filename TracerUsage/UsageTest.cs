﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer;
using System.Threading;

namespace TracerUsage
{
    public class UsageTest
    {
        // Класс с тестами для методов
        public class ExampleTests
        {
            private static ITracer _tracer;

            // Простой метод 
            public void BasicMethod()
            {
                _tracer.StartTrace();
                Thread.Sleep(new Random().Next(100, 1000));
                _tracer.StopTrace();
            }

            public void AdvancedMethod()
            {
                _tracer.StartTrace();
                BasicMethod();
                BasicMethod();
                Thread.Sleep(new Random().Next(100, 1000));
                _tracer.StopTrace();
            }

            public void LongMethod()
            {
                _tracer.StartTrace();
                BasicMethod();
                AdvancedMethod();
                AdvancedMethod();
                Thread.Sleep(new Random().Next(100, 1000));
                _tracer.StopTrace();
            }

            // Проверка многопоточных методов
            public void MultiThreadedMethod()
            {
                _tracer.StartTrace();
                var threads = new List<Thread>();
                threads.Add(new Thread(BasicMethod));
                threads.Add(new Thread(AdvancedMethod));
                threads.Add(new Thread(LongMethod));

                // запуск потоков
                foreach (Thread thread in threads)
                {
                    thread.Start();
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

                Thread.Sleep(new Random().Next(100, 1000));
                _tracer.StopTrace();
            }

            public ExampleTests(ITracer tracer)
            {
                _tracer = tracer;
            }
        }

        private static Tracer.Tracer exampleTracer;

        static void Main(string[] args)
        {

            Console.WriteLine("Start testing Tracer Usage:");

            exampleTracer = new Tracer.Tracer();
            new ExampleTests(exampleTracer).MultiThreadedMethod();

            IWriter writer;

            // Вывод в консоль, сериализация в JSON
            writer = new ConsoleWriter();
            writer.WriteResult(exampleTracer.GetTraceResult(), new JsonSerializer());

            // XML-сериализация
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            writer = new FileWriter(desktopPath + "\\xmlSerialized.xml");
            writer.WriteResult(exampleTracer.GetTraceResult(), new XmlSerializer());

            Console.WriteLine("Finished testing Tracer Usage.")

            Console.ReadKey();
        }
    }
}
