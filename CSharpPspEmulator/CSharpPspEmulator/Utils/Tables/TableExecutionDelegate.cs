//#define DEBUG_INSTRUCTION_GENERATE_TABLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Utils.Tables
{
    public class TableExecutionDelegate<TExecutionDelegate>
    {
        public delegate TExecutionDelegate CreateExecutionDelegateDelegate(TableExecutionDelegate<TExecutionDelegate> TableExecutionDelegate);

        public class TableExecutionItem
        {
            public ITableItem TableItem;
            public TExecutionDelegate ExecutionDelegate;
        }

        CreateExecutionDelegateDelegate CreateExecutionDelegate;
        IEnumerable<TableExecutionItem> TableExecutionList;
        public int Offset;
        public uint BaseMask;
        public uint Mask;
        public uint Level;
        public bool[] SettedDelegates;
        public TExecutionDelegate[] Delegates;
        public TExecutionDelegate DefaultExecutionDelegate;

        public TableExecutionDelegate(IEnumerable<TableExecutionItem> TableExecutionList, CreateExecutionDelegateDelegate CreateExecutionDelegate, TExecutionDelegate DefaultExecutionDelegate, uint BaseMask = 0xFFFFFFFF, uint Level = 0)
        {
            this.DefaultExecutionDelegate = DefaultExecutionDelegate;
            this.CreateExecutionDelegate = CreateExecutionDelegate;
            this.TableExecutionList = TableExecutionList;
            Offset = 0;
            Mask = 0;
            this.BaseMask = BaseMask;
            this.Level = Level;
            if (Level > 6) throw (new Exception("Too much recursion generating tables"));
            Delegates = null;
            GetOffsetMask();
#if DEBUG_INSTRUCTION_GENERATE_TABLE
                    Console.WriteLine("TableExecutionDelegate Offset:{0,8:X} Mask:{1,8:X} BaseMask:{2,8:X} Level:{3}", Offset, Mask, BaseMask, Level);
#endif
            FillDelegates();
        }

        private void GetOffsetMask()
        {
            Mask = GetCommonMask(TableExecutionList);
            Mask &= BaseMask;
            if (Mask == 0) throw (new Exception("Empty CommonMask"));
            Offset = 0;
            while ((Mask & 1) == 0)
            {
                Offset++;
                Mask >>= 1;
            }
            Delegates = new TExecutionDelegate[1 << SizeBitcount(Mask)];
            SettedDelegates = new bool[Delegates.Length];
            for (int n = 0; n < Delegates.Length; n++) Delegates[n] = DefaultExecutionDelegate;
            Mask &= (uint)(Delegates.Length - 1);
        }

        private void FillDelegates()
        {
            var TableExecutionListPerOffset = new Dictionary<uint, List<TableExecutionItem>>();

            foreach (var TableExecutionItem in TableExecutionList)
            {
                var TableOffset = (TableExecutionItem.TableItem.TableValue >> Offset) & Mask;
                if (!TableExecutionListPerOffset.ContainsKey(TableOffset))
                {
                    TableExecutionListPerOffset[TableOffset] = new List<TableExecutionItem>();
                }
                TableExecutionListPerOffset[TableOffset].Add(TableExecutionItem);
            }

            foreach (var Pair in TableExecutionListPerOffset)
            {
                var TableOffset = Pair.Key;
                var TableExecutionListForThisOffset = Pair.Value;

                if (TableOffset < 0) throw (new Exception("Invalid Instruction Offset (I)"));
                if (TableOffset >= Delegates.Length) throw (new Exception("Invalid Instruction Offset (II)"));
                if (SettedDelegates[TableOffset]) throw (new Exception("Repeated Instruction"));

                if (TableExecutionListForThisOffset.Count == 0)
                {
                    throw (new Exception("No Items in list"));
                }
                else if (TableExecutionListForThisOffset.Count > 1)
                {
                    Delegates[TableOffset] = new TableExecutionDelegate<TExecutionDelegate>(TableExecutionListForThisOffset, CreateExecutionDelegate, DefaultExecutionDelegate, BaseMask & ~(Mask << Offset), Level + 1);
                    //throw(new NotImplementedException());
                }
                // One in list.
                else
                {
#if DEBUG_INSTRUCTION_GENERATE_TABLE
                            Console.WriteLine("{0}", InstructionListForThisOffset[0]);
#endif
                    Delegates[TableOffset] = TableExecutionListForThisOffset[0].ExecutionDelegate;
                }

                SettedDelegates[TableOffset] = true;
            }
        }

        static int SizeBitcount(uint n)
        {
            int count = 0;
            while ((n & 1) != 0)
            {
                count++;
                n >>= 1;
            }
            return count;
        }

        static private uint GetCommonMask(IEnumerable<TableExecutionItem> TableExecutionList)
        {
            return TableExecutionList.Aggregate((uint)0xFFFFFFFF, (Previous, Item) => Previous & Item.TableItem.TableMask);
        }

        static public implicit operator TExecutionDelegate(TableExecutionDelegate<TExecutionDelegate> TableExecutionDelegate)
        {
            return TableExecutionDelegate.CreateTExecutionDelegate();
        }

        public TExecutionDelegate GetDelegateByValue(uint Value)
        {
            return Delegates[(Value >> Offset) & Mask];
        }

        TExecutionDelegate CreateTExecutionDelegate()
        {
            return CreateExecutionDelegate(this);
            //return Delegates[(Value >> Offset) & Mask];
        }
    }
}
