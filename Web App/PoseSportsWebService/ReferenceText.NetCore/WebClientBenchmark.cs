//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ReferenceText.NetCore
//{
//	class Program
//	{
//		static void Main(string[] args)
//		{
//			var StopWatch = new System.Diagnostics.Stopwatch();
//			StopWatch.Reset();

//			// WCF proxy clinet
//			{
//				//	StopWatch.Start();

//				//	List<Task> taskList = new List<Task>();
//				//	byte[] credentials = WCFProxyTest_Auth().Result;
//				//	for(int i =0; i < 2000; i++)
//				//	{
//				//		taskList.Add(WCFProxyTest_Hello(credentials, i));

//				//		if (i % 100 == 0)
//				//			Console.WriteLine($"WCF proxy clinet {}");
//				//	}

//				//	Task.WaitAll(taskList.ToArray());

//				//	StopWatch.Stop();
//				//	Console.WriteLine($"WCF proxy clinet End deltaTiem: {StopWatch.ElapsedMilliseconds}ms");
//			}

//			//StopWatch.Reset();

//			//// RestSharp
//			//{
//			//	StopWatch.Start();

//			//	List<Task> taskList = new List<Task>();
//			//	ClientContext.Credentials = RestSharpTest_Auth().Result;

//			//	for (int i = 0; i < 5000; i++)
//			//	{
//			//		taskList.Add(SharpTestTest_Hello());

//			//		if (i % 100 == 0)
//			//			Console.WriteLine($"RestSharp clinet {i}");
//			//	}

//			//	Task.WaitAll(taskList.ToArray());

//			//	StopWatch.Stop();
//			//	Console.WriteLine($"RestSharp End deltaTiem: {StopWatch.ElapsedMilliseconds}ms");
//			//}

//			//StopWatch.Reset();

//			//// Dalsoft.RestClient
//			//{
//			//	StopWatch.Start();

//			//	List<Task> taskList = new List<Task>();
//			//	ClientContext.Credentials = Dalsoft_RestClientTest_Auth().Result;

//			//	for (int i = 0; i < 5000; i++)
//			//	{
//			//		taskList.Add(Dalsoft_RestClientTest_Hello());

//			//		if (i % 100 == 0)
//			//			Console.WriteLine($"Dalsoft.RestClient clinet {i}");
//			//	}

//			//	Task.WaitAll(taskList.ToArray());

//			//	StopWatch.Stop();
//			//	Console.WriteLine($"RestSharp End deltaTiem: {StopWatch.ElapsedMilliseconds}ms");
//			//}

//			//StopWatch.Reset();

//			// Flurl.Http
//			{
//				StopWatch.Start();

//				List<Task> taskList = new List<Task>();
//				ClientContext.Credentials = Flurl_HttpTest_Auth().Result;

//				var ret = Flurl_HttpTest_Hello().Result;

//				for (int i = 0; i < 5000; i++)
//				{
//					taskList.Add(Flurl_HttpTest_HelloPost());

//					if (i % 100 == 0)
//						Console.WriteLine($"Dalsoft.RestClient clinet {i}");
//				}

//				Task.WaitAll(taskList.ToArray());

//				StopWatch.Stop();
//				Console.WriteLine($"RestSharp End deltaTiem: {StopWatch.ElapsedMilliseconds}ms");
//			}
//		}
//		#region WCF
//		public static async Task<byte[]> WCFProxyTest_Auth()
//		{
//			var authClient = new PoseWebService.AuthClient(
//				new BasicHttpBinding()
//				, new EndpointAddress("http://192.168.0.157:8888/Service/Auth.svc"));

//			return await Task.Factory.FromAsync(authClient.BeginGetCredentials, authClient.EndGetCredentials, "Admin", TaskCreationOptions.None);
//		}

//		public static Task WCFProxyTest_Hello(byte[] credentials, int index)
//		{
//			var helloWorldClient = new PoseWebService.HelloWorldClient(
//					new BasicHttpBinding()
//					, new EndpointAddress("http://192.168.0.157:8888/Service/HelloWorld.svc"));

//			using (new OperationContextScope(helloWorldClient.InnerChannel))
//			{
//				PoseHeader serviceHeader = new PoseHeader()
//				{ Credentials = credentials };

//				// Add the custom header to the request.
//				PoseHeaderMessage header = new PoseHeaderMessage(serviceHeader);
//				OperationContext.Current.OutgoingMessageHeaders.Add(header);

//				return Task.Factory.FromAsync(helloWorldClient.BeginHello, helloWorldClient.EndHello, "HeeWon", TaskCreationOptions.None);
//			}
//		}
//		#endregion

//		#region RestSharp
//		public static async Task<byte[]> RestSharpTest_Auth()
//		{
//			var client = new RestSharp.RestClient("http://192.168.0.157:8888/Service/Auth.svc/");
//			var request = new RestSharp.RestRequest("GetStringCredentials/{name}", RestSharp.Method.GET);
//			request.AddUrlSegment("name", "Admin");

//			var returnValue = await client.ExecuteAsync<string>(request);

//			return Convert.FromBase64String(returnValue.Data);
//		}

//		public static async Task<PoseWebService.HelloObject> SharpTestTest_Hello()
//		{
//			PoseHeader serviceHeader = new PoseHeader();
//			ClientContext.CopyTo(serviceHeader);

//			var client = new RestSharp.RestClient("http://192.168.0.157:8888/Service/HelloWorld.svc/");
//			var request = new RestSharp.RestRequest("Hello/{name}", RestSharp.Method.GET);
//			request.AddParameter(PoseHeaderMessage.HEADER_NAME, serviceHeader, RestSharp.ParameterType.HttpHeader);
//			request.AddUrlSegment("name", "HeeWon");

//			return (await client.ExecuteAsync<PoseWebService.HelloObject>(request)).Data;
//		}

//		public static async Task<PoseWebService.HelloObject> SharpTestTest_HelloPost()
//		{
//			PoseHeader serviceHeader = new PoseHeader();
//			ClientContext.CopyTo(serviceHeader);

//			var input = new PoseWebService.HelloObject()
//			{
//				Name1 = "HeeWon",
//				DateTime = DateTime.UtcNow,
//				ByteArray = new byte[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
//			};

//			var client = new RestSharp.RestClient("http://192.168.0.157:8888/Service/HelloWorld.svc/HelloPost");
//			var request = new RestSharp.RestRequest("HelloPost", RestSharp.Method.POST);
//			request.AddParameter(PoseHeaderMessage.HEADER_NAME, serviceHeader, RestSharp.ParameterType.HttpHeader);
//			request.AddParameter("application/json; charset=utf-8", input, RestSharp.ParameterType.GetOrPost);
//			request.RequestFormat = RestSharp.DataFormat.Json;

//			var ret = await client.ExecuteAsync<PoseWebService.HelloObject>(request);
//			return ret.Data;
//		}
//		#endregion

//		#region Dalsoft.RestClient
//		public static async Task<byte[]> Dalsoft_RestClientTest_Auth()
//		{
//			var config = new DalSoft.RestClient.Config()
//			{
//				JsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } },
//				Timeout = TimeSpan.FromSeconds(10.0),
//			};

//			dynamic client = new DalSoft.RestClient.RestClient("http://192.168.0.157:8888/Service/Auth.svc/", config);

//			string returnValue = await client.GetStringCredentials("Admin").Get();
//			returnValue = returnValue.Replace("\"", "");

//			return Convert.FromBase64String(returnValue);
//		}

//		public static async Task<PoseWebService.HelloObject> Dalsoft_RestClientTest_Hello()
//		{
//			var config = new DalSoft.RestClient.Config()
//			{
//				JsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } },
//				Timeout = TimeSpan.FromSeconds(10.0),
//			};

//			PoseHeader serviceHeader = new PoseHeader();
//			ClientContext.CopyTo(serviceHeader);

//			dynamic client = new DalSoft.RestClient.RestClient("http://192.168.0.157:8888/Service/HelloWorld.svc/",
//				new Dictionary<string, string> { { PoseHeaderMessage.HEADER_NAME, serviceHeader.ToString() } }, config);

//			return await client.Hello("HeeWon").Get();
//		}
//		#endregion

//		#region Flurl.Http
//		public static async Task<byte[]> Flurl_HttpTest_Auth()
//		{
//			//var getResp = await "http://192.168.0.157:8888/Service/Auth.svc/GetStringCredentials/Admin".GetJsonAsync<string>();
//			//return Convert.FromBase64String(getResp);

//			// Byte Array 쌉가능
//			var getResp2 = await new Flurl.Url("http://192.168.0.157:8888/Service/Auth.svc/")
//				.AppendPathSegment("GetCredentials")
//				.AppendPathSegment("Admin")
//				.GetJsonAsync<byte[]>();

//			return getResp2;
//		}

//		public static async Task<PoseWebService.HelloObject> Flurl_HttpTest_Hello()
//		{
//			PoseHeader serviceHeader = new PoseHeader();
//			ClientContext.CopyTo(serviceHeader);

//			string input = "HeeWon";

//			var getResp = await new Flurl.Url("http://192.168.0.157:8888/Service/HelloWorld.svc/")
//				.AppendPathSegment("Hello")
//				.AppendPathSegment(input)
//				.WithHeader(PoseHeaderMessage.HEADER_NAME, serviceHeader)
//				.GetJsonAsync<PoseWebService.HelloObject>();

//			return getResp;
//		}

//		public static async Task<PoseWebService.HelloObject> Flurl_HttpTest_HelloPost()
//		{
//			PoseHeader serviceHeader = new PoseHeader();
//			ClientContext.CopyTo(serviceHeader);

//			var input1 = new PoseWebService.HelloObject()
//			{
//				Name1 = "HeeWon",
//				DateTime = DateTime.UtcNow,
//				ByteArray = new byte[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
//			};

//			var inputstr = JsonConvert.SerializeObject(input1);

//			var getResp = await new Flurl.Url("http://192.168.0.157:8888/Service/HelloWorld.svc/")
//				.AppendPathSegments("HelloPost")
//				.WithHeader(PoseHeaderMessage.HEADER_NAME, serviceHeader)
//				.PostJsonAsync(inputstr)
//				.ReceiveJson<PoseWebService.HelloObject>();

//			return getResp;
//		}
//		#endregion
//	}
//}
