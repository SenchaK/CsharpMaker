using System;
using System.Collections.Generic;


namespace CSDocument
{
	public class CSExpression : CSElement
	{
		CSExpression nextExpression = null;
		public CSExpression()
		{
		}

		public CSExpression(CSExpression nextExpression)
		{
			this.nextExpression = nextExpression;
		}

		public virtual string GetExpressionString()
		{
			if (this.nextExpression != null)
			{
				return this.nextExpression.GetExpressionString();
			}
			return "";
		}

		public T Asign<T>(params object[] args) where T : CSExpression
		{
			var t = this.CreateInstance<T>(args);
			var e = this.CreateInstance<CSAsign>(t);
			this.nextExpression = e;
			return t;
		}

		public T Add<T>(params object[] args) where T : CSExpression
		{
			var t = this.CreateInstance<T>(args);
			var e = this.CreateInstance<CSAdd>(t);
			this.nextExpression = e;
			return t;
		}

		public T Mul<T>(params object[] args) where T : CSExpression
		{
			var t = this.CreateInstance<T>(args);
			var e = this.CreateInstance<CSMul>(t);
			this.nextExpression = e;
			return t;
		}

		public CSNullableOperation NullableOperation<T>(params object[] args) where T : CSExpression
		{
			var e = this.CreateInstance<CSNullableOperation>(this.CreateInstance<T>(args));
			this.nextExpression = e;
			return e;
		}

		public CSLiteral Literal(object value)
		{
			var e = this.CreateInstance<CSLiteral>(value);
			this.nextExpression = e;
			return e;
		}

		public CSNew New(string typeName)
		{
			var e = this.CreateInstance<CSNew>(typeName);
			this.nextExpression = e;
			return e;
		}

		public CSSymbol Symbol(string symbolName)
		{
			var e = this.CreateInstance<CSSymbol>(symbolName);
			this.nextExpression = e;
			return e;
		}

	}


	public class CSSymbol : CSExpression
	{
		CSSymbol member = null;
		CSCallMethod method = null;

		[Obsolete("Dont Call this Method!!", true)]
		public CSSymbol(string name)
		{
			this.name = name;
		}

		public CSSymbol Member(string name)
		{
			var m = this.CreateInstance<CSSymbol>(name);
			this.member = m;
			return m;
		}

		public override string GetExpressionString()
		{
			var query = this.name;
			if (this.member != null)
			{
				query += "." + this.member.GetExpressionString();
			}
			if (this.method != null)
			{
				query += "." + this.method.GetExpressionString();
			}
			query += base.GetExpressionString();
			return query;
		}

		public override void Write()
		{
			this.WriteLine(string.Format("{0};", this.GetExpressionString()));
		}

		public CSCallMethod CallMethod(string methodName)
		{
			var e = this.CreateInstance<CSCallMethod>(methodName);
			this.method = e;
			return e;
		}
	}

	public class CSAsign : CSExpression
	{
		public CSAsign(CSExpression exp) : base(exp)
		{
		}

		public override void Write()
		{
			this.WriteLine(this.GetExpressionString() + ";");
		}

		public override string GetExpressionString()
		{
			return " = " + base.GetExpressionString();
		}
	}

	public class CSAdd : CSExpression
	{
		public CSAdd(CSExpression exp) : base(exp)
		{
		}

		public override void Write()
		{
			this.WriteLine(this.GetExpressionString() + ";");
		}

		public override string GetExpressionString()
		{
			return " + " + base.GetExpressionString();
		}
	}

	public class CSMul : CSExpression
	{
		public CSMul(CSExpression exp) : base(exp)
		{
		}

		public override void Write()
		{
			this.WriteLine(this.GetExpressionString() + ";");
		}

		public override string GetExpressionString()
		{
			return " * " + base.GetExpressionString();
		}
	}

	public class PriorityExpression : CSExpression
	{
		public delegate void InnerExpressionDelegate(CSExpression exp);
		InnerExpressionDelegate func = null;

		[Obsolete("Dont Call this Method!!", true)]
		public PriorityExpression(InnerExpressionDelegate func)
		{
			this.func = func;
		}

		public override void Write()
		{
			this.WriteLine(this.GetExpressionString() + ";");
		}

		public override string GetExpressionString()
		{
			this.func?.Invoke(this);
			return base.GetExpressionString();
		}
	}

	public class CSNullableOperation : CSExpression
	{
		[Obsolete("Dont Call this Method!!", true)]
		public CSNullableOperation(CSExpression exp) : base(exp)
		{
		}

		public override void Write()
		{
			this.WriteLine(this.GetExpressionString() + ";");
		}

		public override string GetExpressionString()
		{
			return " ?? " + base.GetExpressionString();
		}
	}

	public class CSCallMethod : CSExpression
	{
		string method = "";
		List<CSExpression> args = new List<CSExpression>();

		[Obsolete("Dont Call this Method!!", true)]
		public CSCallMethod(string method)
		{
			this.method = method;
		}

		public override void Write()
		{
			var query = this.GetExpressionString() + ";";
			this.WriteLine(query);
		}

		public override string GetExpressionString()
		{
			string query = this.method + "(";
			for (int i = 0; i < this.args.Count; i++)
			{
				query += this.args[i].GetExpressionString();
				if (i != this.args.Count - 1)
				{
					query += ", ";
				}
			}
			query += ")";
			return query;
		}

		public T AddArgument<T>(params object[] args) where T : CSExpression
		{
			var t = this.CreateInstance<T>(args);
			this.args.Add(t);
			return t;
		}
	}


	public class CSNew : CSCallMethod
	{
		[Obsolete("Dont Call this Method!!", true)]
		public CSNew(string method) : base(method)
		{
		}

		public override string GetExpressionString()
		{
			return "new " + base.GetExpressionString();
		}
	}

	public class CSLiteral : CSExpression
	{
		object value;
		public CSLiteral(object value)
		{
			this.value = value;
		}

		public override string GetExpressionString()
		{
			var query = "";
			if (value is string)
			{
				query = "\"" + value + "\"";
			}
			else
			{
				if (value == null)
				{
					query = "null";
				}
				else
				{
					query = string.Format("{0}", value);
				}
			}
			query += base.GetExpressionString();
			return query;
		}
	}

	public class CSPriorityExpression : CSExpression
	{
		CSExpression innerExpression = null;
		public CSPriorityExpression(CSExpression innerExpression) : base()
		{
			this.innerExpression = innerExpression;
		}

		public override string GetExpressionString()
		{
			var query = "(" + this.innerExpression.GetExpressionString() + ")" + base.GetExpressionString();
			return query;
		}
	}

	public class CSVar : CSExpression
	{
		string type = "";

		public CSVar(string name)
		{
			this.name = name;
		}

		public CSVar(string name, string type)
		{
			this.name = name;
			this.type = type;
		}

		public override void Write()
		{
			var query = this.GetExpressionString() + ";";
			this.WriteLine(query);
		}

		public override string GetExpressionString()
		{
			string query = string.IsNullOrEmpty(this.type) ? "var" : this.type;
			query += " " + this.name;
			var expression = base.GetExpressionString();

			if (!string.IsNullOrEmpty(expression))
			{
				query += " = " + expression;
			}
			return query;
		}
	}
}
