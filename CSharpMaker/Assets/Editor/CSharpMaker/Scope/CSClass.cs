using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument
{
	public class CSClass : CSScope
	{
		public List<CSAttribute> attribtues = new List<CSAttribute>();
		public string baseClassName = "";
		public List<string> interfaceNames = new List<string>();
		public List<CSField> fields = new List<CSField>();
		public List<CSMethod> methods = new List<CSMethod>();
		public bool isPartial = false;
		public AccessLevel accessLevel;
		public bool isStatic = false;

		public CSClass(string name, string baseClassName, List<string> interfaceNames, bool isPartial, AccessLevel accessLevel)
		{
			this.name = name;
			this.baseClassName = baseClassName;
			this.interfaceNames = interfaceNames;
			this.isPartial = isPartial;
			this.accessLevel = accessLevel;
		}

		public CSField Field(string fieldName, string type, AccessLevel accessLevel)
		{
			return this.AddChild<CSField>(fieldName, type, accessLevel);
		}

		public CSMethod Method(string methodName, string type, AccessLevel accessLevel, MethodKeyword keyword)
		{
			return this.AddChild<CSMethod>(methodName, accessLevel, type, keyword);
		}

		public CSMethod Method(string methodName, string type, AccessLevel accessLevel, MethodKeyword keyword, bool isStatic)
		{
			return this.AddChild<CSMethod>(methodName, accessLevel, type, keyword, isStatic);
		}

		public void Attribute(string typeName)
		{
			this.attribtues.Add(this.CreateInstance<CSAttribute>(typeName, new List<object>(), this));
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
			string s = this.accessLevel.AccessLevelToString();
			if (!string.IsNullOrEmpty(s))
			{
				s += " ";
			}
			if (this.isStatic)
			{
				s += "static ";
			}
			if (this.isPartial)
			{
				s += "partial ";
			}
			s += "class " + this.name;

			bool extend = false;
			if (!string.IsNullOrEmpty(baseClassName))
			{
				extend = true;
				s += " : " + this.baseClassName;
			}
			for (int i = 0; i < interfaceNames.Count; i++)
			{
				var n = interfaceNames[i];
				s += !extend ? " : " + n : "," + n;
			}

			this.StartScope(s);
			base.Write();
			foreach (var field in this.fields)
			{
				field.Write();
			}
			foreach (var method in this.methods)
			{
				method.Write();
			}
			this.EndScope();
			this.WriteNewLine();
		}
	}
}
