using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDoc
{
	public class CSScope : CSElement
	{
		public CSScope()
		{
		}

		public CSClass Class(string className, string baseClassName, List<string> interfaceNames, bool isPartial, AccessLevel accessLevel)
		{
			return this.AddChild<CSClass>(className, baseClassName, interfaceNames, isPartial, accessLevel);
		}
	}
}
