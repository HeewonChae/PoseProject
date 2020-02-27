//using Android.App;
//using Android.Content;
//using Android.Support.V4.App;
//using Java.Lang;
//using System;
//using System.IO;
//using System.Xml.Serialization;
//using Xamarin_Tutorial.Droid.Implementations;
//using Xamarin_Tutorial.Models;
//using Xamarin_Tutorial.Services.LocalNotification;

//[assembly: Xamarin.Forms.Dependency(typeof(AndroidLocalNotificationService))]

//namespace Xamarin_Tutorial.Droid.Implementations
//{
//	public class AndroidLocalNotificationService : ILocalNotificationService
//	{
//		private int _notificationIconId { get; set; }
//		private readonly DateTime _jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

//		public void LocalNotification(string title, string body, int id, DateTime notifyTime)
//		{
//			long totalMilliSeconds = System.Math.Max(0, (long)(notifyTime.ToUniversalTime() - DateTime.UtcNow).TotalMilliseconds);

//			var localNotification = new LocalNotification()
//			{
//				Title = title,
//				Body = body,
//				Id = id,
//				NotifyTime = notifyTime
//			};

//			if (_notificationIconId != 0)
//			{
//				localNotification.IconId = _notificationIconId;
//			}
//			else
//			{
//				localNotification.IconId = Resource.Drawable.ic_stat;
//			}

//			var serializedNotification = SerializeNotification(localNotification);
//			var intent = CreateIntent(localNotification.Id);
//			intent.PutExtra(ScheduledAlarmHandler.LOCALNOTI_KEY, serializedNotification);

//			var pendingIntent = PendingIntent.GetBroadcast(Application.Context, localNotification.Id, intent, PendingIntentFlags.OneShot);
//			var alarmManager = GetAlarmManager();
//			alarmManager.Set(AlarmType.RtcWakeup, totalMilliSeconds, pendingIntent);
//		}

//		public void Cancel(int id)
//		{
//			var intent = CreateIntent(id);
//			var pendingIntent = PendingIntent.GetBroadcast(Application.Context, id, intent, PendingIntentFlags.Immutable);

//			var alarmManager = GetAlarmManager();
//			alarmManager.Cancel(pendingIntent);

//			var notificationManager = NotificationManagerCompat.From(Application.Context);
//			notificationManager.CancelAll();
//			notificationManager.Cancel(id);
//		}

//		public static Intent GetLauncherActivity()
//		{
//			var packageName = Application.Context.PackageName;
//			return Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
//		}

//		private Intent CreateIntent(int id)
//		{
//			return new Intent(Application.Context, typeof(ScheduledAlarmHandler))
//				.SetAction("LocalNotifierIntent" + id);
//		}

//		private AlarmManager GetAlarmManager()
//		{
//			var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
//			return alarmManager;
//		}

//		private string SerializeNotification(LocalNotification notification)
//		{
//			var xmlSerializer = new XmlSerializer(notification.GetType());

//			using (var stringWriter = new StringWriter())
//			{
//				xmlSerializer.Serialize(stringWriter, notification);
//				return stringWriter.ToString();
//			}
//		}
//	}
//}