//using Shiny;
//using Shiny.Infrastructure;
//using Shiny.Jobs;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xamarin_Tutorial.Models.Shiny;

//namespace Xamarin_Tutorial.Utilities.Shiny
//{
//	public class JobLoggerTask : IShinyStartupTask
//	{
//		private readonly IJobManager jobManager;
//		private readonly Shiny_SqliteConnection conn;
//		private readonly ISerializer serializer;

//		public JobLoggerTask(IJobManager jobManager,
//							 ISerializer serializer,
//							 Shiny_SqliteConnection conn)
//		{
//			this.jobManager = jobManager;
//			this.serializer = serializer;
//			this.conn = conn;
//		}

//		public void Start()
//		{
//			this.jobManager.JobStarted.SubscribeAsync(args => this.conn.InsertAsync(new JobLog
//			{
//				JobIdentifier = args.Identifier,
//				JobType = args.Type.FullName,
//				Started = true,
//				Timestamp = DateTime.Now,
//				Parameters = this.serializer.Serialize(args.Parameters)
//			}));

//			this.jobManager.JobFinished.SubscribeAsync(args => this.conn.InsertAsync(new JobLog
//			{
//				JobIdentifier = args.Job.Identifier,
//				JobType = args.Job.Type.FullName,
//				Error = args.Exception?.ToString(),
//				Parameters = this.serializer.Serialize(args.Job.Parameters),
//				Timestamp = DateTime.Now
//			}));
//		}
//	}
//}