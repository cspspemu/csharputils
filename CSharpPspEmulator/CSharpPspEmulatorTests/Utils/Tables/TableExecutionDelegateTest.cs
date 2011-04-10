using CSharpPspEmulator.Utils.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CSharpPspEmulatorTests.Utils.Tables
{
    [TestClass]
    public class TableExecutionDelegateTest
    {
        [TestMethod]
        public void TableExecutionDelegateConstructorTest()
        {
            var Output = new List<String>();
            var Table = new List<TableExecutionDelegate<ExecutionDelegate>.TableExecutionItem>();

            var Item1 = new TableExecutionDelegate<ExecutionDelegate>.TableExecutionItem();
            Item1.TableItem = new TableItem(0x00000001, 0x000000FF);
            Item1.ExecutionDelegate = delegate(ExecutionState ExecutionState)
            {
                Output.Add("1");
            };
            Table.Add(Item1);

            var Item2 = new TableExecutionDelegate<ExecutionDelegate>.TableExecutionItem();
            Item2.TableItem = new TableItem(0x00000002, 0x000000FF);
            Item2.ExecutionDelegate = delegate(ExecutionState ExecutionState)
            {
                Output.Add("2");
            };
            Table.Add(Item2);

            ExecutionDelegate Executor = new TableExecutionDelegate<ExecutionDelegate>(Table, CurrentTable => delegate(ExecutionState ExecutionState)
            {
                CurrentTable.GetDelegateByValue(ExecutionState.Value)(ExecutionState);
            }, delegate(ExecutionState ExecutionState)
            {
                Output.Add("-");
            });
            Executor(new ExecutionState(0));
            Executor(new ExecutionState(1));
            Executor(new ExecutionState(2));
            Executor(new ExecutionState(3));
            Executor(new ExecutionState(0xFFF));

            Assert.AreEqual("-,1,2,-,-", String.Join(",", Output));
        }

        [TestMethod]
        public void TableExecutionDelegateVeryLongTest()
        {
            var Output = new List<String>();
            var Table = new List<TableExecutionDelegate<ExecutionDelegate>.TableExecutionItem>();

            var Item2 = new TableExecutionDelegate<ExecutionDelegate>.TableExecutionItem();
            Item2.TableItem = new TableItem(0x00000002, 0x03FFFFFF);
            Item2.ExecutionDelegate = delegate(ExecutionState ExecutionState)
            {
                Output.Add("2");
            };
            Table.Add(Item2);

            ExecutionDelegate Executor = new TableExecutionDelegate<ExecutionDelegate>(Table, CurrentTable => delegate(ExecutionState ExecutionState)
            {
                CurrentTable.GetDelegateByValue(ExecutionState.Value)(ExecutionState);
            }, delegate(ExecutionState ExecutionState)
            {
                Output.Add("-");
            });
            Executor(new ExecutionState(0));
            Executor(new ExecutionState(2));
            Executor(new ExecutionState(0xFFF));

            Assert.AreEqual("-,2,-", String.Join(",", Output));
        }
    }

    delegate void ExecutionDelegate(ExecutionState ExecutionState);

    class TableItem : ITableItem
    {
        uint _TableValue;
        uint _TableMask;

        public TableItem(uint _TableValue, uint _TableMask)
        {
            this._TableValue = _TableValue;
            this._TableMask = _TableMask;
        }

        public uint TableValue
        {
            get { return _TableValue; }
        }

        public uint TableMask
        {
            get { return _TableMask; }
        }
    }

    class ExecutionState
    {
        public uint Value;

        public ExecutionState(uint Value)
        {
            this.Value = Value;
        }
    }
}
