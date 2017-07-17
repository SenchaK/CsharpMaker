using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument {
	public class CSField : CSElement {
		public List<CSAttribute> attribtues = new List<CSAttribute> ();
		public string type = "";
		public AccessLevel accessLevel;

		public CSField (string name, string type, AccessLevel accessLevel) {
			this.name = name;
			this.type = type;
			this.accessLevel = accessLevel;
		}

		public void Attribute (string typeName, List<object> args) {
			this.attribtues.Add (this.CreateInstance<CSAttribute> (typeName, args, this));
		}

		public override void Write () {
			this.WriteComment ();
			foreach (var attribute in this.attribtues) {
				attribute.Write ();
			}
			if (accessLevel == AccessLevel.Private) {
				this.WriteLine (string.Format ("{0} {1};", type, this.name));
				return;
			}
			this.WriteLine (string.Format ("{0} {1} {2};", AccessLevelToString (accessLevel), type, this.name));
		}
	}
}
