using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Utils.Tables
{
    public interface ITableItem
    {
        uint TableValue { get; }
        uint TableMask { get; }
    }
}
