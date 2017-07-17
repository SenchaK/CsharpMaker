using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument
{
	public class CSScope : CSElement
	{
		public List<CSElement> childs = new List<CSElement>();
		public CSScope()
		{
		}

		public CSClass Class(string className, string baseClassName, List<string> interfaceNames, bool isPartial, AccessLevel accessLevel)
		{
			return this.AddChild<CSClass>(className, baseClassName, interfaceNames, isPartial, accessLevel);
		}

		public T AddChild<T>(params object[] args) where T : CSElement
		{
			var child = this.CreateInstance<T>(args) as T;
			child.SetParent(this);
			this.childs.Add(child);
			return child;
		}

		public override void Write()
		{
			foreach (var child in this.childs)
			{
				child.Write();
			}
		}

		public void StartScope(string scopeLabel)
		{
			this.WriteLine(scopeLabel);
			this.WriteLine("{");
		}

		public void EndScope()
		{
			this.WriteLine("}");
		}
	}
}
