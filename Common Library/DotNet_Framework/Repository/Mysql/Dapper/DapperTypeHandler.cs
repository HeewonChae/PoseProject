using Dapper;
using LogicCore.Debug;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.Dapper.DapperTypeHandler
{
	internal class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
	{
		public override void SetValue(IDbDataParameter parameter, DateTime value)
		{
			Dev.Assert(value.Kind != DateTimeKind.Unspecified);

			parameter.Value = value.ToUniversalTime();
		}

		public override DateTime Parse(object value)
		{
			return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
		}
	}
}
