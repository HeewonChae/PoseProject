using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
	public class P_EXECUTE_QUERY : MysqlQuery<string, int>
	{
		public override void OnAlloc()
		{
			base.OnAlloc();
		}

		public override void OnFree()
		{
			base.OnFree();
		}

		public override void BindParameters()
		{
			// if you need Binding Parameters, write here
		}

		public override int OnQuery()
		{
			DapperFacade.DoWithDBContext(
					null,
					(Contexts.FootballDB footballDB) =>
					{
						if (!string.IsNullOrEmpty(_input))
							_output = footballDB.ExecuteSQL(_input);
					},
					this.OnError);

			return _output;
		}

		public override void OnError(EntityStatus entityStatus)
		{
			base.OnError(entityStatus);

			// Error Control
		}
	}
}
