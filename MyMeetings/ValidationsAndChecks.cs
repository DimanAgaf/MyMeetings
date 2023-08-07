using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMeetings
{
	public interface IValidationsAndChecks
	{
		Task<string> MeetingEventCheck();
		Task<DateTime> DateTimeCheck(bool startDt, Dictionary<int, Meeting> meetingDictionary);
		Task<Remind> RemindCheck();
		Task<bool> DateTimeFormatCheck(string? date);
	}
	public class ValidationsAndChecks : IValidationsAndChecks
	{
		public Task<string> MeetingEventCheck()
		{
			string? _input = String.Empty;
			while (string.IsNullOrEmpty(_input))
			{
				Console.Write("*Введите название встречи: ");
				_input = Console.ReadLine();
			}
			return Task.FromResult(_input);
		}

		public async Task<DateTime> DateTimeCheck(bool startDt, Dictionary<int, Meeting> meetingDictionary)
		{
			string? _input = String.Empty;
			DateTime dateTime = DateTime.Now;
			bool dateIntersection = false;
			do
			{
				switch (startDt)
				{
					case true:
						Console.WriteLine("*Введите дату и время начала встречи: ");
						break;
					case false:
						Console.WriteLine("*Введите дату и время окончания встречи: ");
						break;
				}
				Console.WriteLine($"Формат ввода {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}");
				_input = Console.ReadLine();
				if (await DateTimeFormatCheck(_input))
				{
					dateTime = DateTime.Parse(_input);

					if (dateTime < DateTime.Now)
					{
						Console.WriteLine("Дата и время должны быть больше текущей даты и времени!");
					}

					foreach (var meeting in meetingDictionary)
					{
						dateIntersection = await DateIntersection(dateTime, meeting.Value.DateTimeStart, meeting.Value.DateTimeEnd);
						if (dateIntersection == true)
						{
							Console.WriteLine("Дату или время пересекается уже с существующем!");
							break;
						}
					}
				}
			}
			while ((!DateTime.TryParse(_input, out dateTime)) || (dateTime < DateTime.Now) || (dateIntersection == true));
			return dateTime;
		}

		private Task<bool> DateIntersection(DateTime dateToCheck, DateTime startDate, DateTime endDate)
		{
			return Task.FromResult(dateToCheck >= startDate && dateToCheck < endDate);
		}

		public Task<Remind> RemindCheck()
		{
			Remind remind = new Remind();
			string _string;
			int _number;

			do
			{
				Console.WriteLine("Введите период за который напомнить о встречи: ");
				Console.WriteLine($"Формат ввода: {TimeUnits.Units[0]} или {TimeUnits.Units[1]} или {TimeUnits.Units[2]}");
				remind.TimeUnit = Console.ReadLine();
			} while ((string.IsNullOrEmpty(remind.TimeUnit)) || (!TimeUnits.Units.Any(remind.TimeUnit.Contains)));

			do
			{
				Console.WriteLine("Введите число за какое время напомнить о встречи(число должно быть целым): ");
				_string = Console.ReadLine();
				if (int.TryParse(_string, out _number))
				{
					remind.Value = int.Parse(_string);
				}
			} while (!int.TryParse(_string, out _number));

			return Task.FromResult(remind);
		}


		public async Task<bool> DateTimeFormatCheck(string? date)
		{
			DateTime dateTime = DateTime.Now;
			if (DateTime.TryParse(date, out dateTime))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
