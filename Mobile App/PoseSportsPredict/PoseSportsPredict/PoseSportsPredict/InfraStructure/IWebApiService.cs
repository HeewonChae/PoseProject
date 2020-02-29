using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;

namespace PoseSportsPredict.InfraStructure
{
	public interface IWebApiService
	{
		Task<TOut> RequestAsync<TOut>(WebRequestContext reqContext, bool isIndicateLoading = true);

		Task<TOut> RequestAsyncWithToken<TOut>(WebRequestContext reqContext, bool isIndicateLoading = true);
	}
}