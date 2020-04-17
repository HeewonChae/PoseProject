//using PosePacket.Header;
//using PosePacket.Proxy;
//using PosePacket.Service.Football;
//using ReferenceText.NetCore;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using WebServiceShare.ServiceContext;
//using WebServiceShare.WebServiceClient;

//namespace ReferenceTest.NetCore
//{
//    public class SerializerBenchmark
//    {
//        public void StartBenchmark()
//        {
//            ThreadPool.SetMinThreads(4, 64);

//            var StopWatch = new System.Diagnostics.Stopwatch();

//            int concurrencyLevel = 50;
//            int totalCall = 10000;

//            StopWatch.Reset();
//            StopWatch.Start();
//            TestJsonSerialize(totalCall, concurrencyLevel);
//            StopWatch.Stop();

//            var json_elapsedTime = StopWatch.ElapsedMilliseconds;
//            var json_timePerCall = json_elapsedTime / 1000;

//            StopWatch.Reset();
//            StopWatch.Start();
//            TestMessagePackSerialize(totalCall, concurrencyLevel);
//            StopWatch.Stop();

//            var mp_elapsedTime = StopWatch.ElapsedMilliseconds;
//            var mp_timePerCall = mp_elapsedTime / 1000;

//            Console.WriteLine($"Test Concurrency Level {concurrencyLevel}");

//            Console.WriteLine($"JsonSerialize {totalCall} Calls ElapsedTime: {json_elapsedTime} [ms]");
//            Console.WriteLine($"JsonSerialize Time Per Call {json_timePerCall} [ms]");

//            Console.WriteLine($"MessagePackSerialize {totalCall} Calls ElapsedTime: {mp_elapsedTime} [ms]");
//            Console.WriteLine($"MessagePackSerialize Time Per Call {mp_timePerCall} [ms]");
//        }

//        private void TestJsonSerialize(int totalCall, int concurrencyLevel)
//        {
//            for (int i = 0; i < totalCall / concurrencyLevel; i++)
//            {
//                List<Task> taskList = new List<Task>();
//                for (int j = 0; j < concurrencyLevel; j++)
//                {
//                    taskList.Add(Task.Run(() =>
//                    {
//                        O_GET_FIXTURES_BY_DATE login_output = WebClient.RequestAsync<O_GET_FIXTURES_BY_DATE>(new WebRequestContext()
//                        {
//                            SerializeType = SerializeType.Json,
//                            MethodType = WebMethodType.POST,
//                            BaseUrl = Program.ServiceBaseUrl,
//                            ServiceUrl = FootballProxy.ServiceUrl,
//                            SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_DATE_JSON,
//                            PostData = new I_GET_FIXTURES_BY_DATE
//                            {
//                                StartTime = DateTime.Now.Date.AddDays(1),
//                                EndTime = DateTime.Now.Date.AddDays(2).AddSeconds(-1),
//                            }
//                        },
//                        (PoseHeader.HEADER_NAME, ClientContext.Header)).Result;
//                    }));
//                }

//                Task.WaitAll(taskList.ToArray());
//                int completeCnt = concurrencyLevel * (i + 1);
//                Console.WriteLine($"JsonSerialize Complete {completeCnt}");
//            }
//        }

//        private void TestMessagePackSerialize(int totalCall, int concurrencyLevel)
//        {
//            for (int i = 0; i < totalCall / concurrencyLevel; i++)
//            {
//                List<Task> taskList = new List<Task>();
//                for (int j = 0; j < concurrencyLevel; j++)
//                {
//                    taskList.Add(Task.Run(() =>
//                    {
//                        O_GET_FIXTURES_BY_DATE login_output = WebClient.RequestAsync<O_GET_FIXTURES_BY_DATE>(new WebRequestContext()
//                        {
//                            SerializeType = SerializeType.MessagePack,
//                            MethodType = WebMethodType.POST,
//                            BaseUrl = Program.ServiceBaseUrl,
//                            ServiceUrl = FootballProxy.ServiceUrl,
//                            SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_DATE_MessagePack,
//                            PostData = new I_GET_FIXTURES_BY_DATE
//                            {
//                                StartTime = DateTime.Now.Date.AddDays(1),
//                                EndTime = DateTime.Now.Date.AddDays(2).AddSeconds(-1),
//                            }
//                        },
//                        (PoseHeader.HEADER_NAME, ClientContext.Header)).Result;
//                    }));
//                }

//                Task.WaitAll(taskList.ToArray());
//                int completeCnt = concurrencyLevel * i;
//                Console.WriteLine($"MessagePackSirialize Complete {completeCnt}");
//            }
//        }
//    }
//}