using System;
using System.Collections.Generic;
using CSDocument;
using UnityEditor;

public class Example
{
	[MenuItem("SenchaK/ExampleCsharpMaker")]
	public static void ExampleCsharpMaker()
	{
		using (CSharpMaker maker = new CSDocument.CSharpMaker("ExampleCSharpScript.cs"))
		{
			maker.Using("System");
			maker.Using("System.Collections");
			maker.Using("System.Collections.Generic");
			maker.Using("UnityEngine");
			var senchaK = maker.Namespace("SenchaK");
			var autoCreatedClass = senchaK.Class("AutoCreatedClass", "MonoBehaviour", new List<string>(), false, CSDocument.AccessLevel.Public);

			autoCreatedClass.Field("field1", "int", CSDocument.AccessLevel.Private);
			autoCreatedClass.Field("field2", "int", CSDocument.AccessLevel.Private);
			autoCreatedClass.Field("field3", "int", CSDocument.AccessLevel.Private);
			autoCreatedClass.Field("field4", "int", CSDocument.AccessLevel.Private);

			var method01 = autoCreatedClass.Method("Method01", "void", CSDocument.AccessLevel.Public, CSDocument.MethodKeyword.Nothing);
			method01.Var("sample01").Literal(0);
			method01.Var("sample02").Symbol("sample01").Add<CSSymbol>("sample01");
			method01.Symbol("Debug").CallMethod("Log").AddArgument<CSLiteral>("Hello World!!");

			maker.Make();
		}
	}
}
