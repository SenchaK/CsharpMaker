using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDoc
{
	public class CSMethod : CSScope
	{
		AccessLevel accessLevel = AccessLevel.Private;
		string type = "";
		MethodKeyword keyword = MethodKeyword.Nothing;
		List<CSVar> args = new List<CSVar>();
		bool isStatic = false;

		public CSMethod(string name, AccessLevel accessLevel, string type, MethodKeyword keyword)
		{
			this.Initialize(name, accessLevel, type, keyword, false);
		}

		public CSMethod(string name, AccessLevel accessLevel, string type, MethodKeyword keyword, bool isStatic)
		{
			this.Initialize(name, accessLevel, type, keyword, isStatic);
		}

		void Initialize(string _name, AccessLevel _accessLevel, string _type, MethodKeyword _keyword, bool _isStatic)
		{
			this.name = _name;
			this.accessLevel = _accessLevel;
			this.type = _type;
			this.keyword = _keyword;
		}

		public override void Write()
		{
			this.WriteComment();
			string query = AccessLevelToString(this.accessLevel);
			if (!string.IsNullOrEmpty(query))
			{
				query += " ";
			}
			if (this.isStatic)
			{
				query += "static ";
			}
			if (this.keyword != MethodKeyword.Nothing)
			{
				query += MethodKeywordToString(this.keyword) + " ";
			}
			query += this.type + " " + this.name + "(";

			if (this.args.Count > 0)
			{
				for (int i = 0; i < this.args.Count; i++)
				{
					query += this.args[i].GetExpressionString();
					if (i != this.args.Count - 1)
					{
						query += ", ";
					}
				}
			}
			query += ")";

			this.StartScope(query);
			base.Write();
			this.EndScope();
			this.WriteNewLine();
		}

		public CSReturn Return()
		{
			return this.AddChild<CSReturn>();
		}

		public CSReturn YieldReturn()
		{
			return this.AddChild<CSReturn>(true);
		}

		public CSVar Var(string variableName)
		{
			return this.AddChild<CSVar>(variableName);
		}

		public CSCallMethod CallMethod(string methodName)
		{
			return this.AddChild<CSCallMethod>(methodName);
		}

		public CSVar Arg(string variable, string type)
		{
			var e = this.CreateInstance<CSVar>(variable, type);
			this.args.Add(e);
			return e;
		}

		public CSSymbol Symbol(string name)
		{
			return this.AddChild<CSSymbol>(name);
		}
	}
}
