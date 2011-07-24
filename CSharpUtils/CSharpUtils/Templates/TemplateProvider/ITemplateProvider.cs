using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Templates.TemplateProvider
{
	public interface ITemplateProvider
	{
		Stream GetTemplate(String Name);
	}
}
