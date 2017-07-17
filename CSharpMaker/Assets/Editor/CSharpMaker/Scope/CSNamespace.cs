using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument
{
	public class CSNamespace : CSScope
	{
		public CSNamespace(string name)
		{
			this.name = name;
		}

		public override void Write()
		{
			this.WriteComment();
			this.StartScope("namespace " + this.name);
			base.Write();
			this.EndScope();
			this.WriteNewLine();
		}
	}
}
