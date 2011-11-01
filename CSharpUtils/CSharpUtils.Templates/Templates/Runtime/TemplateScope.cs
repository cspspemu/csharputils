using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Templates.Runtime
{
	sealed public class TemplateScope
	{
		public TemplateScope ParentScope;
		public Dictionary<String, dynamic> Items;

		public TemplateScope(Dictionary<String, dynamic> Items, TemplateScope ParentScope = null)
		{
			this.ParentScope = ParentScope;
			this.Items = Items;
		}

		public TemplateScope(TemplateScope ParentScope = null)
		{
			this.ParentScope = ParentScope;
			this.Items = new Dictionary<String, dynamic>();
		}

		public dynamic this[String Index]
		{
			set
			{
				Items[Index] = value;
			}
			get
			{
				if (Items.ContainsKey(Index))
				{
					return Items[Index];
				}

				if (ParentScope != null)
				{
					return ParentScope[Index];
				}
				return null;
			}
		}
	}
}
