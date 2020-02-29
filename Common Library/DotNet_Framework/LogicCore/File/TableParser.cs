using GameKernel;
using LogicCore.Debug;
using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.File
{
	/// <summary>
	/// Table parsing helper, Coverage(CSV, XML, JSON)
	/// </summary>
	public class TableParser : Singleton.INode
	{
		public interface IPostLoading
		{
			void Process();
		}

		private readonly HashSet<string> _postLoadingTables = new HashSet<string>();
		private readonly List<string> _loadingErrorMessages = new List<string>();

		public TableParser()
		{
			JsonLoader.SetExceptionHandler((exception) =>
			{
				_loadingErrorMessages.Add($"Table Exception [{GameDatabase.LastLoadingTableName}] " +
					$"Exception in JsonLoader { exception.Message }");
			});
		}

		#region CSV
		/// <summary>
		/// Parse one csv format file
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="recordType"></param>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public bool TryLoadSimpleCsv(string tableName, Type recordType, byte[] buffer)
		{
			try
			{
				if (GameDatabase.LoadSimpleCsv(tableName, recordType, buffer))
					throw new Exception("load table Failedd by unknown problem");

				if (typeof(IPostLoading).IsAssignableFrom(recordType))
					_postLoadingTables.Add(tableName);

				return true;
			}
			catch (Exception ex)
			{
				_loadingErrorMessages.Add($"CSV table: {tableName} - {ex.Message}");
			}
			return false;
		}
		#endregion

		#region JSON
		/// <summary>
		/// Deserialize Json file to Object(T)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="buffer"></param>
		/// <param name="supportTypes"></param>
		/// <returns></returns>
		public T DeserializeJson<T>(byte[] buffer, IList<(Type, string)> supportTypes) where T : IRecord
		{
			JsonLoader loader = new JsonLoader(typeof(T));

			if (supportTypes != null)
			{
				foreach (var type in supportTypes)
				{
					string typeName = type.Item2 ?? type.Item1.Name;
					loader.AddSupportType(type.Item1, typeName);
				}
			}

			return (T)loader.Deserialize(buffer);
		}
		
		/// <summary>
		/// Parse Json Table
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="rootArrayName"></param>
		/// <param name="recordType"></param>
		/// <param name="path"></param>
		/// <param name="buffer"></param>
		/// <param name="supportTypes"></param>
		/// <param name="indexField"></param>
		/// <param name="additive"></param>
		/// <returns></returns>
		public bool TryLoadJsonTable(string tableName, string rootArrayName, Type recordType, byte[] buffer,
			IList<(Type, string)> supportTypes = null, string indexField = "Index", bool additive = false)
		{
			try
			{
				JsonLoader loader = new JsonLoader(recordType.MakeArrayType(), indexField, rootArrayName);
				if (supportTypes != null)
				{
					foreach (var type in supportTypes)
					{
						string typeName = type.Item2 ?? type.Item1.Name;
						loader.AddSupportType(type.Item1, typeName);
					}
				}

				if (!GameDatabase.LoadTableJson(tableName, loader, buffer, IndexFieldName: indexField, additive: additive))
					throw new Exception("load table Failedd by unknown problem");
				
				if (typeof(IPostLoading).IsAssignableFrom(recordType))
					_postLoadingTables.Add(tableName);

				return true;
			}
			catch (Exception ex)
			{
				_loadingErrorMessages.Add($"JSON table: {tableName} - {ex.Message}");
			}

			return false;
		}

		/// <summary>
		/// 테이블은 이미 로드된 상태에서 로우데이터 하나 파싱할 경우 사용
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="path"></param>
		/// <param name="recordType"></param>
		/// <param name="buffer"></param>
		/// <param name="supportTypes"></param>
		/// <param name="indexField"></param>
		/// <returns></returns>
		public bool TryLoadJsonSingleRow(string tableName, Type recordType, byte[] buffer, 
			IList<(Type, string)> supportTypes, string indexField = "Index")
		{
			try
			{
				JsonLoader loader = new JsonLoader(recordType, indexField);
				if (supportTypes != null)
				{
					foreach (var type in supportTypes)
					{
						string typeName = type.Item2 ?? type.Item1.Name;
						loader.AddSupportType(type.Item1, typeName);
					}
				}

				if (!GameDatabase.LoadSingleJson(tableName, loader, buffer))
					throw new Exception("load table Failedd by unknown problem");

				if (typeof(IPostLoading).IsAssignableFrom(recordType))
					_postLoadingTables.Add(tableName);

				return true;
			}
			catch (Exception ex)
			{
				_loadingErrorMessages.Add($"JSON single row: {tableName} - {ex.Message}");
			}

			return false;
		}
		#endregion

		#region XML
		public bool TryLoadSimpleXml(string tableName, string recordName, Type recordType, byte[] buffer)
		{
			try
			{
				if (!GameDatabase.LoadSimpleXml(tableName, recordName, recordType, buffer))
					throw new Exception("load table Failedd by unknown problem");

				if (typeof(IPostLoading).IsAssignableFrom(recordType))
					_postLoadingTables.Add(tableName);
				
				return false;
			}
			catch (Exception ex)
			{
				_loadingErrorMessages.Add($"XML table: {tableName} - {ex.Message}");
			}
			return false;
		}
		#endregion

		#region Table Post Load
		public void PostLoadingTables()
		{
			foreach (var tableName in this._postLoadingTables)
			{
				Dev.DebugString($"PostLoading Start { tableName } ");

				this.PostLoading(tableName);

				Dev.DebugString($"PostLoading End { tableName } ");
			}

			this._postLoadingTables.Clear();
		}

		private void PostLoading(string tableName)
		{
			try
			{
				var table = GameDatabase.FindTable(tableName);
				if (table == null)
					throw new Exception("Cannot find Table");

				if (table.Values.Count == 0)
				{
					return;
				}

				var records = table.GetRecords<IRecord>();
				foreach (var record in records)
				{
					var item = (IPostLoading)record;
					item.Process();
				}
			}
			catch(Exception ex)
			{
				_loadingErrorMessages.Add($"PostLoading table: {tableName} - {ex.Message}");
			}
		}
		#endregion

		#region Utility
		public void PrintErrorMessage()
		{
			if (_loadingErrorMessages.Count == 0)
				return;

			foreach (var msg in _loadingErrorMessages)
				Dev.DebugString(msg, ConsoleColor.Red);

			_loadingErrorMessages.Clear();
		}
		#endregion
	}
}
