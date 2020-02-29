using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PacketBuilder.CodeDom
{
	public class CodeJsonDescription
	{
		public int ErrorCode;
		public string Description;
	}

	public class CodeJsonRoot
	{
		public List<CodeJsonDescription> Items;
	}

	public class ErrorCodeDescriptionBuilder
	{
		private readonly string _outputFileName = "ErrorCodeDescription.json";
		private readonly string _filePath = ".\\ErrorDescription\\";
		private readonly Assembly _assambly;
		private readonly CodeJsonRoot _codeJsonRoot = new CodeJsonRoot();

		public ErrorCodeDescriptionBuilder(Assembly assambly)
		{
			_assambly = assambly;
			_codeJsonRoot.Items = new List<CodeJsonDescription>();
		}

		public void ParseErrorCode()
		{
			foreach (var declareType in _assambly.DefinedTypes)
			{
				foreach (var codeJsonDesc in GetErrorCodeDesc(declareType))
				{
					if (_codeJsonRoot.Items.Find(elem => elem.ErrorCode == codeJsonDesc.ErrorCode) != null)
					{
						throw new Exception($"Alread Exist ErrorCode ErrorCode: {codeJsonDesc.ErrorCode}, Description: {codeJsonDesc.Description}");
					}

					_codeJsonRoot.Items.Add(codeJsonDesc);
				}
			}
		}

		public void GenerateJsonFile()
		{
			//var serializeString = JsonConvert.SerializeObject(_codeJsonRoot, Formatting.Indented);
			//FileFacade.MakeSimpleTextFile(_filePath, "invalidTeams.json", serializeString);

			Directory.CreateDirectory(_filePath);

			using (FileStream stream = new FileStream($"{_filePath}{_outputFileName}", FileMode.Create))
			{
				using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(stream)))
				{
					JsonSerializer serial = new JsonSerializer();
					serial.Serialize(writer, _codeJsonRoot);
				}
			}
		}

		private IEnumerable<CodeJsonDescription> GetErrorCodeDesc(TypeInfo type)
		{
			foreach (var fieldInfo in type.GetFields())
			{
				var attributes = (System.ComponentModel.DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute));
				if (attributes.Length == 0)
					continue;

				if (fieldInfo.IsStatic == false)
				{
					continue;
				}

				if (fieldInfo.FieldType != typeof(int))
				{
					continue;
				}

				CodeJsonDescription result = new CodeJsonDescription();
				result.ErrorCode = (int)fieldInfo.GetValue(null);
				result.Description = attributes[0].Description;

				yield return result;
			}
		}
	}
}