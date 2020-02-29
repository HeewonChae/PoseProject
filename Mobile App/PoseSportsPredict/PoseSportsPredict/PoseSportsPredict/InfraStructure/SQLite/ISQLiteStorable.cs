using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure.SQLite
{
	public interface ISQLiteStorable
	{
		int GetPrimaryKey();
	}
}