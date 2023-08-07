using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyMeetings
{
	public interface IRemindTimer
	{
		Task StarTimer(MyMeetingDictionary myMeetingDictionary);
	}
	public class RemindTimer : IRemindTimer
	{
		public async Task StarTimer(MyMeetingDictionary myMeetingDictionary)
		{
			while (true)
			{
				if (myMeetingDictionary.MeetingDictionary.Count > 0)
				{
					foreach (var meeting in myMeetingDictionary.MeetingDictionary)
					{
						switch (meeting.Value.Remind.TimeUnit)
						{
							case "day":
								{
									if (DateTime.Now.AddDays(meeting.Value.Remind.Value).ToString("yyyy-MM-dd HH:mm") == meeting.Value.DateTimeStart.ToString("yyyy-MM-dd HH:mm"))
									{
										await Remind(meeting);
									}
								}
								break;
							case "hour":
								{
									if (DateTime.Now.AddHours(meeting.Value.Remind.Value).ToString("yyyy-MM-dd HH:mm") == meeting.Value.DateTimeStart.ToString("yyyy-MM-dd HH:mm"))
									{
										await Remind(meeting);
									}
								}
								break;
							case "min":
								{
									if (DateTime.Now.AddMinutes(meeting.Value.Remind.Value).ToString("yyyy-MM-dd HH:mm") == meeting.Value.DateTimeStart.ToString("yyyy-MM-dd HH:mm"))
									{
										await Remind(meeting);
									}
								}
								break;
						}
					}
					await Task.Delay(60000);
				}
			}
		}

		private async Task Remind(KeyValuePair<int, Meeting> meeting)
		{
			Console.WriteLine("----------------------");
			Console.WriteLine("У вас запланирована встреча!");
			Type type = typeof(Meeting);			
			Console.WriteLine($"{type.GetProperty("DateTimeStart").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeStart.ToString("dd.MM.yyyy HH:mm")}");
			Console.WriteLine($"{type.GetProperty("Event").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Event}");
			Console.WriteLine($"{type.GetProperty("Description").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Description}");
			Console.WriteLine($"{type.GetProperty("DateTimeEnd").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeEnd.ToString("dd.MM.yyyy HH:mm")}");
			Console.WriteLine("----------------------");
		}
	}
}
