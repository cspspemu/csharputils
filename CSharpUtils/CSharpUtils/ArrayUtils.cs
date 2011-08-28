using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	static public class ArrayUtils
	{
        static public IEnumerable<int> Range(int From, int To)
        {
            for (int n = From; n < To; n++)
            {
                yield return n;
            }
        }

        static public IEnumerable<int> Range(int To)
        {
            return Range(0, To);
        }
    }
}
