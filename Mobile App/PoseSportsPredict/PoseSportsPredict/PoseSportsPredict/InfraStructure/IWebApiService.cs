using System.Threading.Tasks;
using WebServiceShare.ServiceContext;

namespace PoseSportsPredict.InfraStructure
{
    public interface IWebApiService
    {
        Task<TOut> RequestAsync<TOut>(WebRequestContext reqContext);

        Task<TOut> RequestAsyncWithToken<TOut>(WebRequestContext reqContext);

        Task<byte[]> RequestRawAsyncWithToken(WebRequestContext reqContext);
    }
}