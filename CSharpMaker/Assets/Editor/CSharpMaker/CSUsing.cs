using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument {
	public class CSUsing : CSElement {
		public CSUsing (string name) {
			this.name = name;
		}
		public override void Write () {
			this.WriteLine ("using " + this.name + ";");
		}
	}
}
