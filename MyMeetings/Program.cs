using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MyMeetings
{
	internal class Program
	{
		private static Dictionary<string, string> _commandStrings = new Dictionary<string, string>() {
				{ "/add","Добавить встречу" },
				{ "/change", "Изменить встречу" },
				{ "/delete","Удалить встречу" },
				{ "/view","Просмотр списка встреч" },
				{ "/tofile","Просмотр списка встреч" },
			};
		//public static Dictionary<int, Meeting> _meetingDictionary = new Dictionary<int, Meeting>();
		private static MyMeetingDictionary _myMeetingDictionary = new MyMeetingDictionary();
		static async Task Main(string[] args)
		{
			ICommandList _commandList = new CommandList();
			IReadWriteToFile _readWriteToFile = new ReadWriteToFile();
			IRemindTimer _remindTimer = new RemindTimer();		
			


			string json = await _readWriteToFile.ReadFromJsonFile();
			if (!string.IsNullOrEmpty(json))
			{
				_myMeetingDictionary.MeetingDictionary = JsonSerializer.Deserialize<Dictionary<int, Meeting>>(json);
			}
			else
			{
				_myMeetingDictionary.MeetingDictionary = new Dictionary<int, Meeting>();
			}

			Task.Run(async () => await _remindTimer.StarTimer(_myMeetingDictionary));

			while (true)
			{
				Console.WriteLine("Введите команду для продолжения работы!");
				foreach (var commandString in _commandStrings)
				{
					Console.WriteLine($"command: {commandString.Key} - {commandString.Value}");
				}
				string? command = Console.ReadLine();

				Console.WriteLine("----------------------");

				switch (command)
				{
					case "/add":
						_myMeetingDictionary.MeetingDictionary = await _commandList.Add(_myMeetingDictionary.MeetingDictionary);
						break;
					case "/change":
						_myMeetingDictionary.MeetingDictionary = await _commandList.Change(_myMeetingDictionary.MeetingDictionary);
						break;
					case "/delete":
						_myMeetingDictionary.MeetingDictionary = await _commandList.Delete(_myMeetingDictionary.MeetingDictionary);
						break;
					case "/view":
						{
							Console.WriteLine("Введите дату за который хотие просмотреть список: ");
							Console.WriteLine($"Формат ввода {DateTime.Now.ToString("dd.MM.yyyy")}");
							string date = Console.ReadLine();
							await _commandList.ListView(_myMeetingDictionary.MeetingDictionary, date);
						}
						break;
					case "/tofile":
						{
							Console.WriteLine("Введите дату за который хотие просмотреть список: ");
							Console.WriteLine($"Формат ввода {DateTime.Now.ToString("dd.MM.yyyy")}");
							string date = Console.ReadLine();
							await _readWriteToFile.WriteToFile(_myMeetingDictionary.MeetingDictionary, date);
						}
						break;
					default: Console.WriteLine("Команда не верна!");
						break;
				}

				await _readWriteToFile.WriteToJsonFile(_myMeetingDictionary.MeetingDictionary);

				Console.WriteLine("----------------------");
			}			
		}
	}
}