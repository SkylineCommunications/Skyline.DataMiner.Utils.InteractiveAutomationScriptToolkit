namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	/// <summary>
	///     Widget to show/edit a time duration.
	/// </summary>
	[Obsolete("Use TimeSpanP")]
	public class Time : TimeSpanPicker
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Time" /> class.
		/// </summary>
		/// <param name="timeSpan">The timespan displayed in the time widget.</param>
		public Time(TimeSpan timeSpan) : base(timeSpan)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Time" /> class.
		/// </summary>
		public Time()
		{
		}
	}
}