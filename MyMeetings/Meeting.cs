using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMeetings
{
	public class Meeting
	{
		[Description("Название встречи")]
		public string Event { get; set; }

		[Description("Описание встречи")]
		public string? Description { get; set; }

		[Description("Дату и время начала встречи")]
		public DateTime DateTimeStart { get; set; }

		[Description("Дату и время окончания встречи")]
		public DateTime DateTimeEnd { get; set; }

		[Description("Напомнить о встречи за")]
		public Remind Remind { get; set; }
	}

	public class Remind
	{
		public string TimeUnit { get; set; }
		public int Value { get; set; }
	}

	public class TimeUnits
	{
		public static string[] Units = { "day", "hour", "min" };

		public static string UnitDay = Units[0];
		public static string UnitHour = Units[1];
		public static string UnitMinute = Units[2];
	}
}
