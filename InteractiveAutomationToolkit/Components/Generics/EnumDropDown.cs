namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	/// <summary>
	///  A dropdown that allows users to select a value defined by an enum.
	/// </summary>
	/// <typeparam name="T">Enum type to represent with the dropdown.</typeparam>
	/// <example>
	///	<code>
	///	public enum Capitol
	/// {
	///		Brussels,
	///		[Description("Washington DC")]
	///		WashingtonDc,
	///		[Description("Buenos Aires")]
	///		BuenosAires,
	///		Lisbon,
	///		Cairo,
	///		Paris
	/// }
	///
	/// var capitolDropDown = new EnumDropDown&lt;Capitol&gt;();
	/// var selectedCapitol = capitolDropDown.Selected;
	/// capitolDropDown.Changed += (s, e) =>
	/// {
	///		engine.Log($"Capitol changed from {e.Previous} to {e.Selected}");
	///		selectedCapitol = e.Selected;
	/// };
	///	</code>
	/// </example>
	public class EnumDropDown<T> : DropDown<T> where T : struct, Enum
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EnumDropDown{T}"/>
		/// </summary>
		/// <param name="exclude">Values of the enum to exclude as options from the dropdown.</param>
		/// <remarks>
		/// The display value of the enum is determined by the presence of the <see cref="DescriptionAttribute"/>.
		/// If this attribute isn't defined, the .ToString() representation is used.
		/// </remarks>
		public EnumDropDown(ICollection<T> exclude = null)
		{
			var options = new List<Option<T>>();

			var type = typeof(T);
			var values = Enum.GetValues(type).Cast<T>();

			foreach (var value in values)
			{
				if (exclude != null && exclude.Contains(value))
				{
					continue;
				}

				options.Add(new Option<T>(value.GetDescription(), value));
			}

			Options = options;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumDropDown{T}"/>
		/// </summary>
		/// <param name="convertValueToString">Function to map the enum value to its string representation.</param>
		/// <param name="exclude">Values of the enum to exclude as options from the dropdown.</param>
		public EnumDropDown(Func<T, string> convertValueToString, ICollection<T> exclude = null)
		{
			if (convertValueToString == null)
			{
				throw new ArgumentNullException(nameof(convertValueToString));
			}

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
	}
}
