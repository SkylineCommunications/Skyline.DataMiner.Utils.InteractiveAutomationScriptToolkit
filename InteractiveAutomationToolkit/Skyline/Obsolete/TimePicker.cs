namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	///     Widget to show/edit a time of day.
	/// </summary>
	[Obsolete("Use TimeOfDayPicker instead")]
	public class TimePicker : TimeOfDayPicker
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TimePicker"/> class.
		/// </summary>
		/// <param name="time">Time displayed in the time picker.</param>
		public TimePicker(TimeSpan time)
			: base(time)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TimePicker" /> class.
		/// </summary>
		public TimePicker()
		{
		}
	}
}