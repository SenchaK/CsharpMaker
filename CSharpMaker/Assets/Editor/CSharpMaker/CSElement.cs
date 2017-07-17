using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument
{
	public class CSElement
	{
		protected System.IO.TextWriter w;
		public string name = "";
		public CSElement parent = null;
		public CSComment comment = null;
		public int line = 0;

		public CSElement()
		{
		}

		int NestCount(CSElement _parent, int count)
		{
			if (_parent == null || _parent is CSGlobal)
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

		public void Comment(string comment)
		{
			this.comment = this.CreateInstance<CSComment>(comment);
			this.comment.parent = this.parent;
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

		public T CreateInstance<T>(params object[] args) where T : CSElement
		{
			var instance = Activator.CreateInstance(typeof(T), args) as T;
			instance.w = this.w;
			return instance;
		}
	}
}
