using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyMeetings
{
	public interface ICommandList
	{
		Task<Dictionary<int, Meeting>> Add(Dictionary<int, Meeting> meetingDictionary);
		Task<Dictionary<int, Meeting>> Change(Dictionary<int, Meeting> meetingDictionary);
		Task<Dictionary<int, Meeting>> Delete(Dictionary<int, Meeting> meetingDictionary);
		Task ListView(Dictionary<int, Meeting> meetingDictionary, string? date);
	}
	public class CommandList : ICommandList
	{
		IValidationsAndChecks _validationsAndChecks = new ValidationsAndChecks();
		public async Task<Dictionary<int, Meeting>> Add(Dictionary<int, Meeting> meetingDictionary)
		{
			try
			{
				Meeting meeting = new Meeting();
				int key = 0;
				if (meetingDictionary.Count != 0)
					key = meetingDictionary.Keys.Last();

				Console.WriteLine("----------------------");
				Console.WriteLine("Поля помеченные звездочкой обязательны к заполнению!");
				meeting.Event = await _validationsAndChecks.MeetingEventCheck();
				Console.Write("Введите описание встречи: ");
				meeting.Description = Console.ReadLine();
				meeting.DateTimeStart = await _validationsAndChecks.DateTimeCheck(true, meetingDictionary);
				do
				{
					meeting.DateTimeEnd = await _validationsAndChecks.DateTimeCheck(false, meetingDictionary);
					if (meeting.DateTimeEnd < meeting.DateTimeStart) {
						Console.WriteLine("Дата и время окончания встречи на могут быть раньше даты и времени начала встречи!");
					}
				} while (meeting.DateTimeEnd < meeting.DateTimeStart);
				meeting.Remind = await _validationsAndChecks.RemindCheck();

				meetingDictionary.Add(key + 1, meeting);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return meetingDictionary;
		}
		public async Task<Dictionary<int, Meeting>> Change(Dictionary<int, Meeting> meetingDictionary)
		{
			try
			{
				Console.WriteLine("Введите дату за который хотие просмотреть список: ");
				Console.WriteLine($"Формат ввода {DateTime.Now.ToString("dd.MM.yyyy")}");
				string date = Console.ReadLine();
				Console.WriteLine("Список встреч с ключами:");
				await ListView(meetingDictionary, date);
				Console.WriteLine("Оттедактировать встечу можжно выбрав её ключ!");
				string keyString = Console.ReadLine();
				if (int.TryParse(keyString, out int key))
				{
					Meeting meeting = new Meeting();

					Console.WriteLine("----------------------");
					Console.WriteLine("Поля помеченные звездочкой обязательны к заполнению!");
					meeting.Event = await _validationsAndChecks.MeetingEventCheck();
					Console.Write("Введите описание встречи: ");
					meeting.Description = Console.ReadLine();
					meeting.DateTimeStart = await _validationsAndChecks.DateTimeCheck(true, meetingDictionary);
					do
					{
						meeting.DateTimeEnd = await _validationsAndChecks.DateTimeCheck(false, meetingDictionary);
						if (meeting.DateTimeEnd < meeting.DateTimeStart)
						{
							Console.WriteLine("Дата и время окончания встречи на могут быть раньше даты и времени начала встречи!");
						}
					} while (meeting.DateTimeEnd < meeting.DateTimeStart);
					meeting.Remind = await _validationsAndChecks.RemindCheck();

					meetingDictionary[key] = meeting;
				}	
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return meetingDictionary;
		}
		public async Task<Dictionary<int, Meeting>> Delete(Dictionary<int, Meeting> meetingDictionary)
		{
			try
			{
				Console.WriteLine("Введите дату за который хотие просмотреть список: ");
				Console.WriteLine($"Формат ввода {DateTime.Now.ToString("dd.MM.yyyy")}");
				string date = Console.ReadLine();
				Console.WriteLine("Список встреч с ключами:");
				await ListView(meetingDictionary, date);				
				Console.WriteLine("Удалить встечу можжно выбрав её ключ!");
				string keyString = Console.ReadLine();
				if (int.TryParse(keyString, out int key))
					meetingDictionary.Remove(key);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return meetingDictionary;
		}


		public async Task ListView(Dictionary<int, Meeting> meetingDictionary, string? date)
		{
			DateTime dateTime = DateTime.Now;
			if (!string.IsNullOrEmpty(date))
			{
				if (await _validationsAndChecks.DateTimeFormatCheck(date))
				{
					dateTime = DateTime.Parse(date);

					foreach (var meeting in meetingDictionary)
					{
						if (meeting.Value.DateTimeStart.Date == dateTime.Date)
						{
							Console.WriteLine("----------------------");
							Console.WriteLine($"Ключ: {meeting.Key}");
							Type type = typeof(Meeting);
							Console.WriteLine($"{type.GetProperty("Event").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Event}");
							Console.WriteLine($"{type.GetProperty("Description").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Description}");
							Console.WriteLine($"{type.GetProperty("DateTimeStart").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeStart.ToString("dd.MM.yyyy HH:mm")}");
							Console.WriteLine($"{type.GetProperty("DateTimeEnd").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeEnd.ToString("dd.MM.yyyy HH:mm")}");
							Console.WriteLine($"{type.GetProperty("Remind").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Remind.Value} {meeting.Value.Remind.TimeUnit}");
						}
						
					}
				}
				else
				{
					Console.WriteLine("Не верный формат даты!");
				}
			}
			else
			{
				foreach (var meeting in meetingDictionary)
				{
					Console.WriteLine("----------------------");
					Console.WriteLine($"Ключ: {meeting.Key}");
					Type type = typeof(Meeting);
					Console.WriteLine($"{type.GetProperty("Event").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Event}");
					Console.WriteLine($"{type.GetProperty("Description").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Description}");
					Console.WriteLine($"{type.GetProperty("DateTimeStart").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeStart.ToString("dd.MM.yyyy HH:mm")}");
					Console.WriteLine($"{type.GetProperty("DateTimeEnd").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeEnd.ToString("dd.MM.yyyy HH:mm")}");
					Console.WriteLine($"{type.GetProperty("Remind").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Remind.Value} {meeting.Value.Remind.TimeUnit}");

				}
			}
		}
	}
}
