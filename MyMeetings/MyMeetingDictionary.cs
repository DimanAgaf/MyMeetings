using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMeetings
{
	public class MyMeetingDictionary
	{
		private Dictionary<int, Meeting> _meetingDictionary;

		public Dictionary<int, Meeting> MeetingDictionary { get { return _meetingDictionary; } set { _meetingDictionary = value; } }
	}
}
