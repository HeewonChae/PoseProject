//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Xml.Serialization;
//using Android.App;
//using Android.Content;
//using Android.Media;
//using Android.OS;
//using Android.Runtime;
//using Android.Support.V4.App;
//using Android.Views;
//using Android.Widget;
//using Xamarin_Tutorial.Droid.Implementations;
//using Xamarin_Tutorial.Models;

//namespace Xamarin_Tutorial.Droid
//{
//	[BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
//	public class ScheduledAlarmHandler : BroadcastReceiver
//	{
//		private const string CHANNEL_ID = "default";
//		private const string CHANNEL_NAME = "Default";
//		private const string CHANNEL_DESC = "The default channel for notifications.";
//		public const string LOCALNOTI_KEY = "LocalNotification";

//		private bool _channelInitialized = false;
//		private NotificationManager _manager;

//		public override void OnReceive(Context context, Intent intent)
//		{
//			if (!_channelInitialized)
//			{
//				CreateNotificationChannel();
//			}

//			var extra = intent.GetStringExtra(LOCALNOTI_KEY);
//			var notification = DeserializeNotification(extra);

//			var resultIntent = AndroidLocalNotificationService.GetLauncherActivity();
//			resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
//			var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Application.Context);
//			stackBuilder.AddNextIntent(resultIntent);

//			var resultPendingIntent =
//				stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.Immutable);

//			//Generating notification
//			var builder = new NotificationCompat.Builder(Application.Context, CHANNEL_ID)
//				.SetContentIntent(resultPendingIntent)
//				.SetContentTitle(notification.Title)
//				.SetContentText(notification.Body)
//				.SetSmallIcon(notification.IconId)
//				.SetDefaults((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate));

//			// Sending notification
//			var notificationManager = NotificationManagerCompat.From(Application.Context);
//			notificationManager.Notify(notification.Id, builder.Build());
//		}

//		private void CreateNotificationChannel()
//		{
//			_manager = (NotificationManager)Application.Context.GetSystemService(Application.NotificationService);

//			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
//			{
//				var channelNameJava = new Java.Lang.String(CHANNEL_NAME);
//				var channel = new NotificationChannel(CHANNEL_ID, channelNameJava, NotificationImportance.Default)
//				{
//					Description = CHANNEL_DESC
//				};

//				_manager.CreateNotificationChannel(channel);
//			}

//			_channelInitialized = true;
//		}

//		private LocalNotification DeserializeNotification(string notificationString)
//		{
//			var xmlSerializer = new XmlSerializer(typeof(LocalNotification));
//			using (var stringReader = new StringReader(notificationString))
//			{
//				var notification = (LocalNotification)xmlSerializer.Deserialize(stringReader);
//				return notification;
//			}
//		}
//	}
//}