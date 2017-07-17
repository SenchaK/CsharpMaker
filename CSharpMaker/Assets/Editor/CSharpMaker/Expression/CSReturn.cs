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
		public CSExpression exp = null;
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
			if (this.exp != null)
			{
				query += this.exp.GetExpressionString();
			}
			else
			{
				query += "null";
			}
			query += ";";
			this.WriteLine(query);
		}

		public CSLiteral Literal(object value)
		{
			var e = this.CreateInstance<CSLiteral>(value);
			this.exp = e;
			return e;
		}
	}
}

