using System;
using System.IO;
using System.Text;

namespace LogicCore.File
{
	public static class FileFacade
	{
		/// <summary>
		/// Convert file contents to byte array
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static byte[] FileToByte(string filePath)
		{
			if (!Directory.Exists(Path.GetDirectoryName(filePath)))
				throw new Exception($"Directory not exist - {Path.GetDirectoryName(filePath)}");

			using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					return reader.ReadBytes((int)reader.BaseStream.Length);
				}
			}
		}

		public static void MakeSimpleTextFile(string filePath, string fileNameWithFormat, string contents)
		{
			Directory.CreateDirectory(filePath);

			System.IO.File.AppendAllText($"{filePath}\\{fileNameWithFormat}", contents, Encoding.UTF8);
		}
	}
}