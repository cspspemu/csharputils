using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Getopt
{
    public class Getopt
    {
        Queue<string> Items;

        public Getopt(string[] _Items)
        {
            this.Items = new Queue<string>(_Items);
        }

        public bool HasMore
        {
            get
            {
                return Items.Count > 0;
            }
        }

        public string Next
        {
            get
            {
                return this.Items.Dequeue();
            }
        }
    }
}
