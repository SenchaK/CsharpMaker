using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument {
	public class CSAttribute : CSElement {
		public CSElement owner;
		public List<object> args = new List<object> ();

		public CSAttribute (string name, List<object> args, CSElement owner) {
			this.name = name;
			this.args = args;
			this.owner = owner;
			this.SetParent (this.owner.parent);
		}

		public override void Write () {
			string s = "[" + this.name;
			if (this.args.Count > 0) {
				s += "(";
				for (int i = 0; i < args.Count; i++) {
					s += args [i];
					if (i != args.Count - 1) {
						s += ",";
					}
				}
				s += ")";
			}
			s += "]";
			this.WriteLine (s);
		}
	}
}
