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

			autoCreatedClass.Field("field1", "int", AccessLevel.Private);
			autoCreatedClass.Field("field2", "int", AccessLevel.Private);
			autoCreatedClass.Field("field3", "int", AccessLevel.Private);
			autoCreatedClass.Field("field4", "int", AccessLevel.Private);

			var method01 = autoCreatedClass.Method("Method01", "void", AccessLevel.Public, MethodKeyword.Nothing);
			method01.Var("sample01").Literal(15);
			method01.Var("sample02").Symbol("sample01").Add<CSSymbol>("sample01");
			var exp = method01.CreateExpression();
			exp.Literal(10).Add<CSSymbol>("sample02");
			var pExp = method01.CreateExpression<CSPriorityExpression>(exp);
			pExp.Mul<CSSymbol>("sample01");
			method01.Symbol("sample01").Asign<CSPriorityExpression>(pExp).Add<CSLiteral>(100);
			method01.Symbol("Debug").CallMethod("Log").AddArgument<CSLiteral>("Hello").Add<CSLiteral>("World!!");

			maker.Make();
		}
	}
}
