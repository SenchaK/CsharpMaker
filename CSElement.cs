﻿using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDoc
{
	public class CSElement
	{
		protected System.IO.TextWriter w;
		public string name = "";
		public List<CSElement> childs = new List<CSElement>();
		public CSElement parent = null;
		public CSComment comment = null;
		public int line = 0;

		public CSElement()
		{
		}

		public T AddChild<T>(params object[] args) where T : CSElement
		{
			var child = this.CreateInstance<T>(args) as T;
			child.SetParent(this);
			this.childs.Add(child);
			return child;
		}

		int NestCount(CSElement _parent, int count)
		{
			if (_parent == null)
			{
				return count;
			}
			count++;
			return NestCount(_parent.parent as CSScope, count);
		}

		protected int NestCount()
		{
			return this.NestCount(this.parent as CSScope, 0);
		}

		public void AddLine()
		{
			this.line++;
		}

		public void SetParent(CSElement e)
		{
			this.parent = e;
		}

		public virtual void Write()
		{
			foreach (var child in this.childs)
			{
				child.Write();
			}
		}

		public void WriteNewLine()
		{
			for (int i = 0; i < this.line; i++)
			{
				this.WriteLine("");
			}
		}

		public void WriteComment()
		{
			if (this.comment != null)
			{
				this.comment.Write();
			}
		}

		public void StartScope(string scopeLabel)
		{
			this.WriteLine(scopeLabel);
			this.WriteLine("{");
		}

		public void Comment(string comment)
		{
			this.comment = this.CreateInstance<CSComment>(comment);
			this.comment.parent = this.parent;
		}

		public void EndScope()
		{
			this.WriteLine("}");
		}

		public void WriteLine(string s)
		{
			string tab = "";
			int nestCount = this.NestCount();
			for (int i = 0; i < nestCount; i++)
			{
				tab += "\t";
			}
			this.w.WriteLine(tab + s);
		}

		public string AccessLevelToString(AccessLevel level)
		{
			switch (level)
			{
				case AccessLevel.Private:
				default:
					return "";
				case AccessLevel.Public:
					return "public";
				case AccessLevel.Protected:
					return "protected";
			}
		}

		public string MethodKeywordToString(MethodKeyword keyword)
		{
			switch (keyword)
			{
				case MethodKeyword.Nothing:
				default:
					return "";
				case MethodKeyword.Virtual:
					return "virtual";
				case MethodKeyword.Abstract:
					return "abstract";
				case MethodKeyword.Override:
					return "override";
			}
		}

		public T CreateInstance<T>(params object[] args) where T : CSElement
		{
			var instance = Activator.CreateInstance(typeof(T), args) as T;
			instance.w = this.w;
			return instance;
		}
	}
}
