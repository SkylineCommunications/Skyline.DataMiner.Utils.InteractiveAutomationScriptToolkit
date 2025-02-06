namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.ComponentModel;
	using System.Linq;

	internal static class EnumExtensions
	{
		public static string GetDescription(this Enum value)
		{
			var enumField = value.GetType().GetField(value.ToString()) ?? throw new InvalidOperationException($"Value '{value}' is not a valid enum value");

			var attribute = enumField.GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault() as DescriptionAttribute;

			return attribute == null ? value.ToString() : attribute.Description;
		}
	}
}
