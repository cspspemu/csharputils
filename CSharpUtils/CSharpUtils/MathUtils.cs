using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
    static public class MathUtils
    {
        static public T Clamp<T>(T Value, T Min, T Max)
        {
            if ((dynamic)Value < (dynamic)Min) return Min;
            if ((dynamic)Value > (dynamic)Max) return Max;
            return Value;
        }

    }
}
