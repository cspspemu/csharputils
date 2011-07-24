using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Templates.Tokenizers
{
	public class TemplateToken
	{
		public String Text;
		//public String OriginalText;
	}

	public class RawTemplateToken : TemplateToken
	{
	}

	public class OpenTagTemplateToken : TemplateToken
	{
	}

	public class CloseTagTemplateToken : TemplateToken
	{
	}

	public class StringLiteralTemplateToken : TemplateToken
	{
	}

	public class NumericLiteralTemplateToken : TemplateToken
	{
	}

	public class OperatorTemplateToken : TemplateToken
	{
	}

	public class IdentifierTemplateToken : TemplateToken
	{
	}
}
