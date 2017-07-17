using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument {
	public class CSComment : CSElement {
		public string commentText;

		public CSComment (string comment) {
			this.commentText = comment;
		}

		public override void Write () {
			var s = this.commentText.Split ('\n').Where ((arg) => !string.IsNullOrEmpty (arg));
			if (s.Count () > 0) {
				this.WriteLine ("/// <summary>");
				foreach (var text in s) {
					this.WriteLine ("/// " + text);
				}
				this.WriteLine ("/// </summary>");
			}
		}
	}
}
