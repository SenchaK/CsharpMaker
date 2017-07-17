using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace CSDocument
{
	public enum AccessLevel
	{
		Private,
		Public,
		Protected,
	}

	public enum MethodKeyword
	{
		Nothing,
		Virtual,
		Abstract,
		Override,
	}

	public static class CsharpMakerExtension
	{
		public static string AccessLevelToString(this AccessLevel level)
		{
			switch (level)
			{
				case AccessLevel.Private:
				default:
					return "";
				case AccessLevel.Public:
					return "public";
				case AccessLevel.Protected:
					return "protected";
			}
		}

		public static string MethodKeywordToString(this MethodKeyword keyword)
		{
			switch (keyword)
			{
				case MethodKeyword.Nothing:
				default:
					return "";
				case MethodKeyword.Virtual:
					return "virtual";
				case MethodKeyword.Abstract:
					return "abstract";
				case MethodKeyword.Override:
					return "override";
			}
		}
	}

	public class CSharpMaker : IDisposable
	{
		System.IO.TextWriter writer;
		CSGlobal globalScope;

		public CSharpMaker(string fileName)
		{
			this.writer = System.IO.File.CreateText(fileName);
			this.globalScope = new CSGlobal(this.writer);
		}

		public void Using(string namespaceName)
		{
			this.globalScope.AddChild<CSUsing>(namespaceName);
		}

		public CSNamespace Namespace(string namespaceName)
		{
			return this.globalScope.AddChild<CSNamespace>(namespaceName);
		}

		public void Make()
		{
			this.globalScope.Write();
		}

		public void Dispose()
		{
			this.writer.Close();
		}
	}
}