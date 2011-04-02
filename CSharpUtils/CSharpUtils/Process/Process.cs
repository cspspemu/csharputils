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
            Started,
            Running,
            Ended,
        }

        static protected Process currentExecutingProcess = null;

        protected int priority = 0;
        protected double x = 0, y = 0, z = 0;
        protected Process parent = null;
        protected LinkedList<Process> childs;

        public State state { private set; get; }
        private static IntPtr mainFiberHandle = IntPtr.Zero;
        private IntPtr fiberHandle;
        private delegate void RunDelegate();

        static private LinkedList<Process> _allProcesses = new LinkedList<Process>();

        static public LinkedList<Process> allProcesses
        {
            get
            {
                return new LinkedList<Process>(_allProcesses);
            }
        }

        static public void _removeOld()
        {
            foreach (var process in allProcesses)
            {
                if (process.state == State.Ended) process._remove();
            }
        }

        protected void _ExecuteProcessBefore()
        {
            foreach (var process in childs.Where(process => process.priority < 0).OrderBy(process => process.priority)) process._ExecuteProcess();
        }

        protected void _ExecuteProcessAfter()
        {
            foreach (var process in childs.Where(process => process.priority >= 0).OrderBy(process => process.priority)) process._ExecuteProcess();
        }

        public void _ExecuteProcess()
        {
            if (state == State.Ended) return;

            currentExecutingProcess = this;

            this._ExecuteProcessBefore();
            this.SwitchTo();
            this._ExecuteProcessAfter();
        }

        protected void _DrawProcessBefore()
        {
            foreach (var process in childs.Where(process => process.z < 0).OrderBy(process => process.z)) process._DrawProcess();
        }

        protected void _DrawProcessAfter()
        {
            foreach (var process in childs.Where(process => process.z >= 0).OrderBy(process => process.z)) process._DrawProcess();
        }

        public void _DrawProcess()
        {
            this._DrawProcessBefore();
            this.Draw();
            this._DrawProcessAfter();
        }

        virtual protected void Draw()
        {
            //Console.WriteLine(this);
        }

        public Process()
        {
            _allProcesses.AddLast(this);
            childs = new LinkedList<Process>();
            parent = currentExecutingProcess;
            if (parent != null)
            {
                parent.childs.AddLast(this);
            }
            //Console.WriteLine(this + " : " + parent);
            state = State.Started;
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
            _remove();
        }

        private void _remove()
        {
            state = State.Ended;
            if (parent != null)
            {
                parent.childs.Remove(this);
            }
            _allProcesses.Remove(this);
            if (fiberHandle != IntPtr.Zero)
            {
                DeleteFiber(fiberHandle);
                fiberHandle = IntPtr.Zero;
            }
        }

        public void SwitchTo()
        {
            if (state != State.Ended)
            {
                SwitchToFiber(fiberHandle);
            }
        }

        protected void Yield(int count = 1)
        {
            for (int n = 0; n < count; n++) SwitchToFiber(mainFiberHandle);
        }

        abstract protected void main();
    }
}
