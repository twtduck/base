using System;
using System.Windows.Media;
using Google.Apis.Calendar.v3.Data;

namespace OneTooCalendar
{
	public class GoogleCalendarEventDataModel : IEventDataModel
	{
		public GoogleCalendarEventDataModel(ICalendarDataModel sourceCalendar)
		{
			Calendar = sourceCalendar;
		}

		public bool AllDayEvent { get; init; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string Title { get; set; } = "";
		public string Location { get; set; } = "";
		public string? Description { get; set; }
		public Color Color => ThemeHelper.TryGetEventBackgroundColor(CustomEventColorId) ?? ThemeHelper.GetCalendarBackgroundColor(Calendar);
		public ICalendarDataModel Calendar { get; }
		public string EventId { get; set; } = Guid.NewGuid().ToString();
		public bool EventIdIsTemporary { get; set; } = true;
		public int? CustomEventColorId { get; set; }
		public Event SourceEvent { get; init; } = null!;
	}
}