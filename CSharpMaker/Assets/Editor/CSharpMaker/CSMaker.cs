using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument
{
	public enum AccessLevel
	{
		Private,
		Public,
		Protected,
	}

	public enum MethodKeyword
	{
		Nothing,
		Virtual,
		Abstract,
		Override,
	}

	public class CSGlobal : CSElement
	{
		public CSGlobal(TextWriter w)
		{
			this.w = w;
		}
	}

	public class CSAttribute : CSElement
	{
		public CSElement owner;
		public List<object> args = new List<object>();

		public CSAttribute(string name, List<object> args, CSElement owner)
		{
			this.name = name;
			this.args = args;
			this.owner = owner;
			this.SetParent(this.owner.parent);
		}

		public override void Write()
		{
			string s = "[" + this.name;
			if (this.args.Count > 0)
			{
				s += "(";
				for (int i = 0; i < args.Count; i++)
				{
					s += args[i];
					if (i != args.Count - 1)
					{
						s += ",";
					}
				}
				s += ")";
			}
			s += "]";
			this.WriteLine(s);
		}
	}

	public class CSComment : CSElement
	{
		public string commentText;

		public CSComment(string comment)
		{
			this.commentText = comment;
		}

		public override void Write()
		{
			var s = this.commentText.Split('\n').Where((arg) => !string.IsNullOrEmpty(arg));
			if (s.Count() > 0)
			{
				this.WriteLine("/// <summary>");
				foreach (var text in s)
				{
					this.WriteLine("/// " + text);
				}
				this.WriteLine("/// </summary>");
			}
		}
	}

	public class CSField : CSElement
	{
		public List<CSAttribute> attribtues = new List<CSAttribute>();
		public string type = "";
		public AccessLevel accessLevel;

		public CSField(string name, string type, AccessLevel accessLevel)
		{
			this.name = name;
			this.type = type;
			this.accessLevel = accessLevel;
		}

		public void Attribute(string typeName, List<object> args)
		{
			this.attribtues.Add(this.CreateInstance<CSAttribute>(typeName, args, this));
		}

		public override void Write()
		{
			this.WriteComment();
			foreach (var attribute in this.attribtues)
			{
				attribute.Write();
			}
			if (accessLevel == AccessLevel.Private)
			{
				this.WriteLine(string.Format("{0} {1};", type, this.name));
				return;
			}
			this.WriteLine(string.Format("{0} {1} {2};", AccessLevelToString(accessLevel), type, this.name));
		}
	}

	public class CSInterface : CSElement
	{
		[Obsolete("Dont Call Constructor", true)]
		public CSInterface()
		{
		}
	}

	public class CSUsing : CSElement
	{
		public CSUsing(string name)
		{
			this.name = name;
		}
		public override void Write()
		{
			this.WriteLine("using " + this.name + ";");
		}
	}


	public class CSMaker : IDisposable
	{
		public delegate void OnNamespace(CSMaker maker, CSNamespace current);
		System.IO.TextWriter writer;
		CSGlobal globalScope;

		public CSMaker(string fileName)
		{
			this.writer = System.IO.File.CreateText(fileName);
			this.globalScope = new CSGlobal(this.writer);
		}

		public void Using(string namespaceName)
		{
			this.globalScope.AddChild<CSUsing>(namespaceName);
		}

		public CSNamespace Namespace(string namespaceName)
		{
			return this.globalScope.AddChild<CSNamespace>(namespaceName);
		}

		public void Make()
		{
			this.globalScope.Write();
		}

		public void Dispose()
		{
			this.writer.Close();
		}
	}
}