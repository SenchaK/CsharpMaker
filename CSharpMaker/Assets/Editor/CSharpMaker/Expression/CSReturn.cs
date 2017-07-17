using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument
{
	public class CSReturn : CSExpression
	{
		public bool isYield = false;

		public CSReturn()
		{
		}

		public CSReturn(bool isYield)
		{
			this.isYield = isYield;
		}

		public override void Write()
		{
			var query = "";
			if (this.isYield)
			{
				query += "yield ";
			}

			query += "return ";
			query += base.GetExpressionString();
			query += ";";
			this.WriteLine(query);
		}
	}
}

