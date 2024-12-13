namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Defines the base functionality for a checkboxlist widget, including properties for state management.
	/// </summary>
	public interface ICheckBoxListBase : IValidationWidget
	{
		/// <summary>
		/// Gets or sets a value indicating whether the checkboxlist is read-only.
		/// </summary>
		bool IsReadOnly { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the options in the checkboxlist are sorted.
		/// </summary>
		bool IsSorted { get; set; }

		/// <summary>
		/// Gets or sets the tooltip text associated with the checkboxlist.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		/// Checks all options in the checkboxlist.
		/// </summary>
		void CheckAll();

		/// <summary>
		/// Unchecks all options in the checkboxlist.
		/// </summary>
		void UncheckAll();
	}

}