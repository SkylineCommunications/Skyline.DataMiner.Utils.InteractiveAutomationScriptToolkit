namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Components.Widgets.Generics;

	/// <summary>
	/// A radio button list that allows users to select a value defined by an enum.
	/// </summary>
	/// <typeparam name="T">Enum type to represent with the radio button list.</typeparam>
	/// /// <example>
	///	<code>
	///	public enum Capitol
	/// {
	///		None,
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
	/// var capitolRadioButtonList = new EnumRadioButtonList&lt;Capitol&gt;(new [] { Capitol.None });
	/// Capitol selectedCapitol = capitolRadioButtonList.Selected;
	/// capitolRadioButtonList.Changed += (s, e) =>
	/// {
	///		engine.Log($"Capitol changed from {e.Previous} to {e.Selected}");
	///		selectedCapitol = e.Selected;
	/// };
	///	</code>
	/// </example>
	public class EnumRadioButtonList<T> : RadioButtonList<T> where T : struct, Enum
	{
		private readonly Func<T, string> convertValueToString;

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumRadioButtonList{T}"/>
		/// </summary>
		/// <param name="exclude">Values of the enum to exclude as options from the radio button list.</param>
		/// <remarks>
		/// The display value of the enum is determined by the presence of the <see cref="DescriptionAttribute"/>.
		/// If this attribute isn't defined, the .ToString() representation is used.
		/// </remarks>
		public EnumRadioButtonList(ICollection<T> exclude = null) : this(DefaultConversion, exclude) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumRadioButtonList{T}"/>
		/// </summary>
		/// <param name="convertValueToString">Function to map the enum value to its string representation.</param>
		/// <param name="exclude">Values of the enum to exclude as options from the radio button list.</param>
		public EnumRadioButtonList(Func<T, string> convertValueToString, ICollection<T> exclude = null)
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

		public override void SetOptions(IEnumerable<T> options)
		{
			base.SetOptions(options.Select(x => new Option<T>(convertValueToString(x), x)));
		}

		public override IEnumerable<T> Values
		{
			get => base.Values;
			set
			{
				if (value == null) throw new ArgumentNullException(nameof(value));
				SetOptions(value.Select(x => new Option<T>(convertValueToString(x), x)));
			}
		}

		private static string DefaultConversion(T value)
		{
			return value.GetDescription();
		}
	}
}
