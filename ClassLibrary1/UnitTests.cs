﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;
using Tracer;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        private static Tracer.Tracer tracer;
        private static int waitTime = 100;
        private static int threadsCount = 2;

       // Однопоточный метод для тесирования
        private void SingleThreadedMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(waitTime);
            tracer.StopTrace();
        }

        // Многопоточный метод для тестирования
        private void MultiThreadedMethod()
        {
            var threads = new List<Thread>();
            Thread newThread;
            for (int i = 0; i < threadsCount; i++)
            {
                newThread = new Thread(SingleThreadedMethod);
                threads.Add(newThread);
            }
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            tracer.StartTrace();
            Thread.Sleep(waitTime);
            tracer.StopTrace();
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        // Тестирование затраченного времени
        private void TestExecutionTime(long actualTime, long expectedTime)
        {
            Assert.IsTrue(actualTime >= expectedTime);
        }


        // Тестирование времени выполнения однопоточного метода
        [TestMethod]
        public void SingleThreadTest()
        {
            tracer = new Tracer.Tracer();

            tracer.StartTrace();
            Thread.Sleep(waitTime);
            tracer.StopTrace();

            long actual = tracer.GetTraceResult().ThreadTracingResults[0].ExecutionTime;

            TestExecutionTime(actual, waitTime);
        }


        // Тестирование времени выполнения многопоточного метода
        [TestMethod]
        public void MultiThreadTest()
        {
            tracer = new Tracer.Tracer();

            var threads = new List<Thread>();

            long expectedTime = 0;

            Thread newThread;
            for (int i = 0; i < threadsCount; i++)
            {
                newThread = new Thread(SingleThreadedMethod);
                threads.Add(newThread);
                newThread.Start();
                expectedTime += waitTime;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            long actual = 0;

            foreach (ThreadTracingResult threadResult in tracer.GetTraceResult().ThreadTracingResults)
            {
                actual += threadResult.ExecutionTime;
            }

            TestExecutionTime(actual, expectedTime);
        }

        // Тестирование времени и имён классов/методов для вложенности
        [TestMethod]
        public void NestedMultiThreadTest()
        {
            tracer = new Tracer.Tracer();

            var threads = new List<Thread>();
            long expectedTime = 0;

            Thread newThread;
            for (int i = 0; i < threadsCount; i++)
            {
                newThread = new Thread(MultiThreadedMethod);
                threads.Add(newThread);
                newThread.Start();
                expectedTime += waitTime * (threadsCount + 1);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            long actualTime = 0;
            TraceResult result = tracer.GetTraceResult();
            foreach (ThreadTracingResult threadResult in result.ThreadTracingResults)
            {
                actualTime += threadResult.ExecutionTime;
            }

            TestExecutionTime(actualTime, expectedTime);

            // Тестирование количества общего числа потоков

            Assert.AreEqual(threadsCount * threadsCount + threadsCount, result.ThreadTracingResults.Count);
            int multiThreadedMethodCounter = 0, singleThreadedMethodCounter = 0;

            MethodTracingResult methodResult;
            foreach (ThreadTracingResult threadResult in result.ThreadTracingResults)
            {
                // Тестирование количества методов в потоках
                Assert.AreEqual(threadResult.ThreadMethods.Count, 1);
                methodResult = threadResult.ThreadMethods[0];

                // Тестирование количества вложенных методов
                Assert.AreEqual(0, methodResult.InnerMethods.Count);

                // Тестирование имени класса
                Assert.AreEqual(nameof(UnitTests), methodResult.ClassName);

                // Тестирование времени выполнения
                TestExecutionTime(methodResult.ExecutionTime, waitTime);

                if (methodResult.MethodName == nameof(MultiThreadedMethod))
                    multiThreadedMethodCounter++;
                if (methodResult.MethodName == nameof(SingleThreadedMethod))
                    singleThreadedMethodCounter++;
            }

            // Тестирование количества многопоточных и однопоточных методов
            Assert.AreEqual(threadsCount, multiThreadedMethodCounter);
            Assert.AreEqual(threadsCount * threadsCount, singleThreadedMethodCounter);
        }


        // Тестирование вложенных методов
        [TestMethod]
        public void InnerMethodTest()
        {
            tracer = new Tracer.Tracer();

            tracer.StartTrace();
            Thread.Sleep(waitTime);
            SingleThreadedMethod();
            tracer.StopTrace();

            TraceResult traceResult = tracer.GetTraceResult();

            Assert.AreEqual(1, traceResult.ThreadTracingResults.Count);
            TestExecutionTime(tracer.GetTraceResult().ThreadTracingResults[0].ExecutionTime, waitTime * 2);
            Assert.AreEqual(1, traceResult.ThreadTracingResults[0].ThreadMethods.Count);

            MethodTracingResult methodResult = traceResult.ThreadTracingResults[0].ThreadMethods[0];

            Assert.AreEqual(nameof(UnitTests), methodResult.ClassName);
            Assert.AreEqual(nameof(InnerMethodTest), methodResult.MethodName);
            TestExecutionTime(methodResult.ExecutionTime, waitTime * 2);
            Assert.AreEqual(1, methodResult.InnerMethods.Count);
            MethodTracingResult innerMethodResult = methodResult.InnerMethods[0];
            Assert.AreEqual(nameof(UnitTests), innerMethodResult.ClassName);
            Assert.AreEqual(nameof(SingleThreadedMethod), innerMethodResult.MethodName);
            TestExecutionTime(innerMethodResult.ExecutionTime, waitTime);
        }
    }
}   