namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	/// <summary>
	/// A checkbox list that allows users to select values defined by a flags enum.
	/// </summary>
	/// <typeparam name="T">Flags enum type to represent with the checkbox list.</typeparam>
	/// <example>
	///	<code>
	///	[Flags]
	///	public enum DaysOfWeek
	/// {
	///		None = 0,
	///		[Description("Monday")]
	///		Monday = 1,
	///		[Description("Tuesday")]
	///		Tuesday = 2,
	///		[Description("Wednesday")]
	///		Wednesday = 4,
	///		[Description("Thursday")]
	///		Thursday = 8,
	///		[Description("Friday")]
	///		Friday = 16,
	///		[Description("Saturday")]
	///		Saturday = 32,
	///		[Description("Sunday")]
	///		Sunday = 64
	/// }
	///
	/// var daysCheckBoxList = new EnumCheckBoxList&lt;DaysOfWeek&gt;(new [] { DaysOfWeek.None });
	/// DaysOfWeek selectedDays = daysCheckBoxList.CheckedFlags;
	/// daysCheckBoxList.Changed += (s, e) =>
	/// {
	///		engine.Log($"Day {e.Value} changed to {e.IsChecked}");
	///		selectedDays = daysCheckBoxList.CheckedFlags;
	/// };
	///	</code>
	/// </example>
	public class EnumCheckBoxList<T> : CheckBoxList<T> where T : struct, Enum
	{
		private readonly Func<T, string> convertValueToString;

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumCheckBoxList{T}"/>
		/// </summary>
		/// <param name="exclude">Values of the enum to exclude as options from the checkbox list.</param>
		/// <remarks>
		/// The display value of the enum is determined by the presence of the <see cref="DescriptionAttribute"/>.
		/// If this attribute isn't defined, the .ToString() representation is used.
		/// </remarks>
		public EnumCheckBoxList(ICollection<T> exclude = null) : this(DefaultConversion, exclude) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumCheckBoxList{T}"/>
		/// </summary>
		/// <param name="convertValueToString">Function to map the enum value to its string representation.</param>
		/// <param name="exclude">Values of the enum to exclude as options from the checkbox list.</param>
		public EnumCheckBoxList(Func<T, string> convertValueToString, ICollection<T> exclude = null)
		{
			this.convertValueToString = convertValueToString ?? throw new ArgumentNullException(nameof(convertValueToString));

			var options = new List<Option<T>>();

			var type = typeof(T);
			var values = Enum.GetValues(type).Cast<T>();

			foreach (var value in values)
			{
				if (exclude != null && exclude.Contains(value))
				{
					continue;
				}

				options.Add(new Option<T>(convertValueToString(value), value));
			}

			Options = options;
		}

		/// <summary>
		/// Gets or sets the combined flags value of all checked options.
		/// </summary>
		public T CheckedFlags
		{
			get
			{
				var checkedValues = Checked.ToList();
				if (!checkedValues.Any())
				{
					return default;
				}

				// Combine all checked flags using bitwise OR
				int result = 0;
				foreach (var value in checkedValues)
				{
					result |= Convert.ToInt32(value);
				}

				return (T)Enum.ToObject(typeof(T), result);
			}

			set
			{
				// Uncheck all first
				UncheckAll();

				// Get all individual flags from the value
				var allValues = Enum.GetValues(typeof(T)).Cast<T>();
				foreach (var enumValue in allValues)
				{
					int enumInt = Convert.ToInt32(enumValue);
					int valueInt = Convert.ToInt32(value);

					// Check if this flag is set (and it's not zero unless value is zero)
					if (enumInt != 0 && (valueInt & enumInt) == enumInt)
					{
						try
						{
							Check(enumValue);
						}
						catch (ArgumentException)
						{
							// Value might be excluded, skip it
						}
					}
					else if (enumInt == 0 && valueInt == 0)
					{
						// Handle zero/None case
						try
						{
							Check(enumValue);
						}
						catch (ArgumentException)
						{
							// Value might be excluded, skip it
						}
					}
				}
			}
		}

		public override IEnumerable<Option<T>> Options
		{
			get => base.Options;
			set
			{
				if (value == null) throw new ArgumentNullException(nameof(value));
				base.SetOptions(value);
			}
		}

		public override IEnumerable<T> Values
		{
			get => base.Values;
			set
			{
				if (value == null) throw new ArgumentNullException(nameof(value));
				base.SetOptions(value.Select(x => new Option<T>(convertValueToString(x), x)));
			}
		}

		private static string DefaultConversion(T value)
		{
			return value.GetDescription();
		}
	}
}
