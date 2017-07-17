using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument {
	public class CSGlobal : CSScope {
		public CSGlobal (TextWriter w) {
			this.w = w;
		}
	}
}
