namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	using Automation;

	public static class UIBlockDefinitionExtensions
	{
		private static readonly Type UIBlockDefinitionType = typeof(UIBlockDefinition);

		private static readonly FieldInfo DropdownOptionsField =
			UIBlockDefinitionType.GetField("_dropDownOptions", BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly ConstructorInfo DropdownOptionsFieldDefaultConstructor =
			DropdownOptionsField.FieldType.GetConstructor(Type.EmptyTypes);

		/// <summary>
		/// Exposes the private _dropDownOptions field used by Dropdown, CheckBoxList and RadioButtonList Widgets.
		/// </summary>
		/// <param name="definition">A <see cref="UIBlockDefinition"/> to get the options list from.</param>
		/// <returns>An <see cref="ICollection{T}"/> containing all options.</returns>
		public static ICollection<string> GetOptionsCollection(this UIBlockDefinition definition)
		{
			var options = (ICollection<string>)DropdownOptionsField.GetValue(definition);
			if (options != null)
			{
				return options;
			}

			// field is not initialized during construction of UIBlockDefinition.
			// So we need to take care of it ourselves.
			// Use reflection to construct, so we don't make any assumptions about the actual type.
			// This is just in case they decide to change the type to something other than List.
			options = (ICollection<string>)DropdownOptionsFieldDefaultConstructor.Invoke(Array.Empty<object>());
			DropdownOptionsField.SetValue(definition, options);

			return options;
		}
	}
}