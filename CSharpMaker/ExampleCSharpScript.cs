using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SenchaK
{
	public class AutoCreatedClass : MonoBehaviour
	{
		int field1;
		int field2;
		int field3;
		int field4;
		public void Method01()
		{
			var sample01 = 15;
			var sample02 = sample01 + sample01;
			sample01 = ((10 + sample02) * sample01) + 100;
			Debug.Log("Hello" + "World!!");
		}
	}
}
