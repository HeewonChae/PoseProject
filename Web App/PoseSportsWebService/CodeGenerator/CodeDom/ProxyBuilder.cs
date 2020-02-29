using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PacketBuilder.CodeDom
{
	public class ProxyBuilder
	{
		private readonly CodeCompileUnit _targetUnit;
		private readonly CodeTypeDeclaration _targetClass;
		private readonly string _outputFileName;
		private readonly string _filePath = ".\\Proxy\\";
		private readonly TypeInfo _assambly;
		private readonly string _className;

		public ProxyBuilder(TypeInfo assambly, string className, string nameSpace)
		{
			Console.WriteLine($"Start Make Proxy {className}");

			_assambly = assambly;
			_className = className;
			_outputFileName = $"{className}Proxy.cs";

			_targetUnit = new CodeCompileUnit();
			CodeNamespace proxyCode = new CodeNamespace(nameSpace);
			proxyCode.Imports.Add(new CodeNamespaceImport("System"));
			_targetClass = new CodeTypeDeclaration($"{className}Proxy");
			_targetClass.IsClass = true;
			_targetClass.TypeAttributes = TypeAttributes.Public;

			// Mark as static class
			//_targetClass.StartDirectives.Add(new CodeRegionDirective(
			//  CodeRegionMode.Start, Environment.NewLine + "static"));
			//_targetClass.EndDirectives.Add(new CodeRegionDirective(
			//		CodeRegionMode.End, string.Empty));

			proxyCode.Types.Add(_targetClass);
			_targetUnit.Namespaces.Add(proxyCode);
		}

		public void AddFields()
		{
			Console.WriteLine($"{_className} Proxy - AddFields");

			// Declare serviceUrl field.
			CodeMemberField serviceUrl = new CodeMemberField
			{
				Attributes = MemberAttributes.Private | MemberAttributes.Static,
				Type = new CodeTypeReference(typeof(string)),
				Name = "_serviceUrl",
				InitExpression = new CodePrimitiveExpression($"Services/{_className}.svc")
			};
			_targetClass.Members.Add(serviceUrl);

			Console.WriteLine($"{_className} Proxy - Add ServiceUrl field : Services/{_className}.svc");

			var methods = _assambly.DeclaredMethods;
			foreach (var declaredMethod in _assambly.DeclaredMethods)
			{
				var attrData = declaredMethod.CustomAttributes.Where(elem => elem.AttributeType.Name.Equals("WebInvokeAttribute")).FirstOrDefault();
				if (attrData != null)
				{
					string packetUrl = (string)attrData.NamedArguments.Where(elem =>
					elem.MemberName.Equals("UriTemplate")).First().TypedValue.Value;
					string webMethodType = (string)attrData.NamedArguments.Where(elem =>
					elem.MemberName.Equals("Method")).First().TypedValue.Value;

					// Declare the method name field
					CodeMemberField methodNameField = new CodeMemberField
					{
						Attributes = MemberAttributes.Private | MemberAttributes.Static,
						Type = new CodeTypeReference(typeof(string)),
						Name = $"_{declaredMethod.Name}",
						InitExpression = new CodePrimitiveExpression(packetUrl)
					};
					_targetClass.Members.Add(methodNameField);

					Console.WriteLine($"{_className} Proxy - Add method field : _{declaredMethod.Name}");
				}
			}
		}

		public void AddProperties()
		{
			Console.WriteLine($"{_className} Proxy - AddProperties");

			// Declare the read-only ServiceUrl property.
			CodeMemberProperty serviceUrlProperty = new CodeMemberProperty
			{
				Attributes = MemberAttributes.Public | MemberAttributes.Static,
				HasGet = true,
				Type = new CodeTypeReference(typeof(string)),
				Name = "ServiceUrl"
			};
			serviceUrlProperty.Comments.Add(new CodeCommentStatement("Service base url"));
			serviceUrlProperty.GetStatements.Add(new CodeMethodReturnStatement(
				new CodeFieldReferenceExpression(null, "_serviceUrl")));
			_targetClass.Members.Add(serviceUrlProperty);

			Console.WriteLine($"{_className} Proxy - Add ServiceUrl Property : ServiceUrl");

			foreach (var declaredMethod in _assambly.DeclaredMethods)
			{
				var attrData = declaredMethod.CustomAttributes.Where(elem => elem.AttributeType.Name.Equals("WebInvokeAttribute")).FirstOrDefault();
				if (attrData != null)
				{
					string packetUrl = (string)attrData.NamedArguments.Where(elem =>
					elem.MemberName.Equals("UriTemplate")).First().TypedValue.Value;
					string webMethodType = (string)attrData.NamedArguments.Where(elem =>
					elem.MemberName.Equals("Method")).First().TypedValue.Value;

					// Find same name command
					Type inputType = null;
					Type outputType = null;
					foreach (var declaredClass in _assambly.Assembly.DefinedTypes)
					{
						if (string.Equals(declaredMethod.Name, declaredClass.Name))
						{
							var WebModelTypeAttr = declaredClass.CustomAttributes.Where(elem => elem.AttributeType.Name.Equals("WebModelTypeAttribute")).FirstOrDefault();
							if (WebModelTypeAttr != null)
							{
								inputType = (Type)WebModelTypeAttr.NamedArguments.Where(elem =>
									elem.MemberName.Equals("InputType")).FirstOrDefault().TypedValue.Value;

								outputType = (Type)WebModelTypeAttr.NamedArguments.Where(elem =>
									elem.MemberName.Equals("OutputType")).FirstOrDefault().TypedValue.Value;
							}
						}
					}

					// Declare the read-only method name property.
					CodeMemberProperty methodNameProperty = new CodeMemberProperty
					{
						Attributes = MemberAttributes.Public | MemberAttributes.Static,
						HasGet = true,
						Type = new CodeTypeReference(typeof(string)),
						Name = declaredMethod.Name
					};
					methodNameProperty.Comments.Add(
						new CodeCommentStatement($"MethodType: {webMethodType}, Segment: {packetUrl} \n InputType: {inputType?.FullName ?? "null"} \n OutputType: {outputType?.FullName ?? "null"}"));
					methodNameProperty.GetStatements.Add(new CodeMethodReturnStatement(
						new CodeFieldReferenceExpression(null, $"_{declaredMethod.Name}")));
					_targetClass.Members.Add(methodNameProperty);

					Console.WriteLine($"{_className} Proxy - Add Method Property : {declaredMethod.Name}");
				}
			}
		}

		public void GenerateCSharpCode()
		{
			Directory.CreateDirectory(_filePath);

			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
			CodeGeneratorOptions options = new CodeGeneratorOptions();
			options.BracingStyle = "C";
			using (StreamWriter sourceWriter = new StreamWriter($"{_filePath}{_outputFileName}"))
			{
				provider.GenerateCodeFromCompileUnit(
					_targetUnit, sourceWriter, options);
			}
		}
	}
}