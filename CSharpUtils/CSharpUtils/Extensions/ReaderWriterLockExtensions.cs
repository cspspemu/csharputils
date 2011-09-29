using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharpUtils.Extensions
{
	static public class ReaderWriterLockExtensions
	{
		static public void ReaderLock(this ReaderWriterLock This, Action Callback)
		{
			This.AcquireReaderLock(int.MaxValue);
			try
			{
				Callback();
			}
			finally
			{
				This.ReleaseReaderLock();
			}
		}

		static public void WriterLock(this ReaderWriterLock This, Action Callback)
		{
			This.AcquireWriterLock(int.MaxValue);
			try
			{
				Callback();
			}
			finally
			{
				This.ReleaseWriterLock();
			}
		}
	}
}
