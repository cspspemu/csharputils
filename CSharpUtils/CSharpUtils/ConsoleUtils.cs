using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	public class ConsoleUtils
	{
		static public void SaveRestoreConsoleState(Action Action)
		{
			lock (Console.Out)
			{
				var BackBackgroundColor = Console.BackgroundColor;
				var BackForegroundColor = Console.ForegroundColor;
				try
				{
					Action();
				}
				finally
				{
					Console.BackgroundColor = BackBackgroundColor;
					Console.ForegroundColor = BackForegroundColor;
				}
			}
		}

		static public String CaptureOutput(Action Action)
		{
			var OldOut = Console.Out;
			var StringWriter = new StringWriter();
			try
			{
				Console.SetOut(StringWriter);
				Action();
			}
			finally
			{
				Console.SetOut(OldOut);
			}
			try
			{
				return StringWriter.ToString();
			}
			catch
			{
				return "";
			}
		}
	}
}
