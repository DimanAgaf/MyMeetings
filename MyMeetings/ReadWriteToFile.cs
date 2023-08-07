using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyMeetings
{
	public interface IReadWriteToFile
	{
		Task WriteToJsonFile(Dictionary<int, Meeting> meetingDictionary);
		Task<string> ReadFromJsonFile();
		Task WriteToFile(Dictionary<int, Meeting> meetingDictionary, string? date);
	}
	public class ReadWriteToFile : IReadWriteToFile
	{
		IValidationsAndChecks _validationsAndChecks = new ValidationsAndChecks();
		string path = @"note1.txt";
		public async Task WriteToJsonFile(Dictionary<int, Meeting> meetingDictionary) 
		{
			var options = new JsonSerializerOptions { WriteIndented = true };
			string json = JsonSerializer.Serialize(meetingDictionary, options);

			using (StreamWriter writer = new StreamWriter(path, false))
			{
				await writer.WriteLineAsync(json);
			}
		}

		public async Task<string> ReadFromJsonFile()
		{
			using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
			{
				using (StreamReader reader = new StreamReader(fileStream))
				{
					return await reader.ReadToEndAsync();
				}
			}
		}

		public async Task WriteToFile(Dictionary<int, Meeting> meetingDictionary, string? date)
		{
			try
			{
				DateTime dateTime = DateTime.Now;
				if (await _validationsAndChecks.DateTimeFormatCheck(date))
				{
					dateTime = DateTime.Parse(date);
				}
				else
				{
					Console.WriteLine("Не верный формат даты!");
					return;
				}

				string pf = @"files/" + dateTime.Date.ToString("dd.MM.yyyy") + ".txt";

				if (!Directory.Exists(pf))
				{ Directory.CreateDirectory(Path.GetDirectoryName(pf)); }
				using (FileStream fileStream = new FileStream(pf, FileMode.OpenOrCreate))
				{
					using (StreamWriter writer = new StreamWriter(fileStream))
					{
						foreach (var meeting in meetingDictionary)
						{
							if (meeting.Value.DateTimeStart.Date == dateTime.Date)
							{
								await writer.WriteLineAsync($"Ключ: {meeting.Key}");
								Type type = typeof(Meeting);
								await writer.WriteLineAsync($"{type.GetProperty("Event").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Event}");
								await writer.WriteLineAsync($"{type.GetProperty("Description").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Description}");
								await writer.WriteLineAsync($"{type.GetProperty("DateTimeStart").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeStart.ToString("dd.MM.yyyy HH:mm")}");
								await writer.WriteLineAsync($"{type.GetProperty("DateTimeEnd").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.DateTimeEnd.ToString("dd.MM.yyyy HH:mm")}");
								await writer.WriteLineAsync($"{type.GetProperty("Remind").GetCustomAttribute<DescriptionAttribute>().Description}: {meeting.Value.Remind.Value} {meeting.Value.Remind.TimeUnit}");
								await writer.WriteLineAsync("----------------------");
							}

						}
					}
				}

				Console.WriteLine($"Файл сохранен по адресу {Path.GetFullPath(pf)}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
