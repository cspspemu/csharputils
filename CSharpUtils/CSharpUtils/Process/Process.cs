using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;

namespace CSharpUtils.Process
{
    abstract class Process
    {
        [DllImport("kernel32.dll")]
        extern static IntPtr ConvertThreadToFiber(int fiberData);

        [DllImport("kernel32.dll")]
        extern static IntPtr CreateFiber(int size, System.Delegate function, int handle);

        [DllImport("kernel32.dll")]
        extern static IntPtr SwitchToFiber(IntPtr fiberAddress);

        [DllImport("kernel32.dll")]
        extern static void DeleteFiber(IntPtr fiberAddress);

        [DllImport("kernel32.dll")]
        extern static int GetLastError();

        public enum State
        {
            Stopped,
            Running,
            Ended,
        }

        public State state;
        static IntPtr mainFiberHandle = IntPtr.Zero;
        IntPtr fiberHandle;

        delegate void RunDelegate();

        public Process()
        {
            state = State.Stopped;
            if (mainFiberHandle == IntPtr.Zero)
            {
                mainFiberHandle = ConvertThreadToFiber(0);
            }
            fiberHandle = CreateFiber(500, new RunDelegate(() =>
            {
                state = State.Running;
                main();
                state = State.Ended;
                Yield();
            }), 0);
        }

        ~Process()
        {
            DeleteFiber(fiberHandle);
        }

        public void SwitchTo()
        {
            if (state != State.Ended)
            {
                SwitchToFiber(fiberHandle);
            }
        }

        protected void Yield()
        {
            SwitchToFiber(mainFiberHandle);
        }

        abstract protected void main();
    }

	/*
    abstract class MyProcess : Process {
        int n = 0;

        protected void increment()
        {
            while (n < +4)
            {
                n++;
                Print();
                Yield();
            }
        }

        protected void decrement()
        {
            while (n > -4)
            {
                n--;
                Print();
                Yield();
            }
        }

        void Print()
        {
            Console.WriteLine(n);
        }

    }

    class MyProcess1 : MyProcess
    {
        override protected void main()
        {
            increment();
            decrement();
        }
    }

    class MyProcess2 : MyProcess
    {
        override protected void main()
        {
            decrement();
            increment();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var f1 = new MyProcess1();
            var f2 = new MyProcess2();
            for (int n = 0; n < 20; n++)
            {
                f1.SwitchTo();
                f2.SwitchTo();
            }
            Console.ReadKey();
        }
    }
	*/
}
