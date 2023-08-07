using MyMeetings;

namespace MyMeetingsTest
{
	public class ValidationsAndChecksTest
	{
		IValidationsAndChecks _validationsAndChecks = new ValidationsAndChecks();	
		
		[Fact]
		public async void MeetingEventCheckTest()
		{
			var input = new StringReader("Meet1");
			Console.SetIn(input);

			string value = await _validationsAndChecks.MeetingEventCheck();

			Assert.Equal("Meet1", value);
		}

		[Fact]
		public async void DateTimeCheckTest()
		{
			Dictionary<int, Meeting> meetingDictionary = new Dictionary<int, Meeting>()
			{
				{ 1, new Meeting() { 
					Event = "1",
					Description = "D1",
					DateTimeStart = DateTime.Now,
					DateTimeEnd = DateTime.Now.AddMinutes(5),
					Remind = new Remind() {
						TimeUnit = "min",
						Value = 5
					}}
				}
			};

			DateTime dateTime = DateTime.Now.AddDays(1);
			var input = new StringReader(dateTime.ToString("dd.MM.yyyy HH:mm"));
			Console.SetIn(input);

			DateTime value = await _validationsAndChecks.DateTimeCheck(true, meetingDictionary);

			Assert.Equal(dateTime.ToString("dd.MM.yyyy HH:mm"), value.ToString("dd.MM.yyyy HH:mm"));
		}

		[Fact]
		public async void RemindCheckTest()
		{
			Remind remindEqual = new Remind()
			{
				TimeUnit = "min",
				Value = 5
			};

			var input = new StringReader(remindEqual.TimeUnit + "\r" + remindEqual.Value);
			Console.SetIn(input);

			Remind value = await _validationsAndChecks.RemindCheck();

			Assert.Equal(remindEqual.TimeUnit, value.TimeUnit);
			Assert.Equal(remindEqual.Value, value.Value);
		}

		[Fact]
		public async void DateTimeFormatCheckTest()
		{
			Assert.Equal(true, await _validationsAndChecks.DateTimeFormatCheck("04.08.2023 12:00"));
			Assert.Equal(false, await _validationsAndChecks.DateTimeFormatCheck("04.08.2023 12.00"));
		}
	}
}