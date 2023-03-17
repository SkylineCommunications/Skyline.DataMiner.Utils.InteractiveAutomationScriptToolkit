namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Provides a set of static methods for getting the UI changes that occured for a given widget.
	/// </summary>
	internal static class UiResultsExtensions
	{
		/// <summary>
		/// Gets the state of a checkbox according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="checkBox">The checkbox widget.</param>
		/// <returns>The state of the checkbox.</returns>
		public static bool GetChecked(this UIResults results, CheckBox checkBox)
		{
			return results.GetChecked(checkBox.DestVar);
		}

		/// <summary>
		/// Gets the names of the tree view items that are checked according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="treeView">The tree view widget.</param>
		/// <returns>The names of tree view items that are checked.</returns>
		public static string[] GetCheckedItemKeys(this UIResults results, TreeView treeView)
		{
			string result = results.GetString(treeView.DestVar);
			if (String.IsNullOrEmpty(result))
			{
				return Array.Empty<string>();
			}

			return result.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// Gets the value of a date time picker according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="dateTimePicker">The date time picker widget.</param>
		/// <returns>The value of the date time picker.</returns>
		public static DateTime GetDateTime(this UIResults results, DateTimePicker dateTimePicker)
		{
			return results.GetDateTime(dateTimePicker.DestVar);
		}

		/// <summary>
		/// Gets the names of tree view items that are expanded according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="treeView">The tree view widget.</param>
		/// <returns>The names of tree view items that are expanded.</returns>
		public static string[] GetExpandedItemKeys(this UIResults results, TreeView treeView)
		{
			string[] expandedItems = results.GetExpanded(treeView.DestVar);
			if (expandedItems == null)
			{
				return Array.Empty<string>();
			}

			return expandedItems.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
		}

		/// <summary>
		/// Gets the string representation of the value of an interactive widget according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">The interactive widget.</param>
		/// <returns>The string representation of the value of the interactive widget.</returns>
		public static string GetString(this UIResults results, InteractiveWidget interactiveWidget)
		{
			return results.GetString(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets the value of a timespan widget according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="time">The time widget.</param>
		/// <returns>The value the a timespan widget.</returns>
		public static TimeSpan GetTime(this UIResults results, TimeSpanPicker time)
		{
			string receivedTime = results.GetString(time);

			// This try catch is here because of a bug in Dashboards
			// The string that is received from Dashboards is a DateTime (e.g. 2021-11-16T00:00:16.0000000Z), while the string from Cube is an actual TimeSpan (e.g. 1.06:00:03).
			// This means that when using the Time component from Dashboards, you are restricted to 24h and can only enter HH:mm times.
			// See task: 171211
			// ReSharper disable once RedundantNameQualifier
			// DIS code generation fails to properly generate this code if TimeSpan type is not fully qualified
			if (TimeSpan.TryParse(receivedTime, out System.TimeSpan result))
			{
				return result;
			}

			return DateTime.Parse(receivedTime, CultureInfo.InvariantCulture).TimeOfDay;
		}

		/// <summary>
		/// Gets the value of a time of day picker widget according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="time">The time picker widget.</param>
		/// <returns>The value of the time of day picker widget.</returns>
		public static TimeSpan GetTime(this UIResults results, TimeOfDayPicker time)
		{
			return DateTime.Parse(results.GetString(time), CultureInfo.InvariantCulture).TimeOfDay;
		}

		/// <summary>
		/// Gets the path of a file that was selected according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">The file selector widget.</param>
		/// <returns>The path of a file that was selected.</returns>
		public static string GetUploadedFilePath(this UIResults results, FileSelector interactiveWidget)
		{
			return results.GetUploadedFilePath(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets the paths of all files that were selected according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">the file selector widget.</param>
		/// <returns>All files that were selected.</returns>
		public static string[] GetUploadedFilePaths(this UIResults results, FileSelector interactiveWidget)
		{
			return results.GetUploadedFilePaths(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets whether a button was pressed according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="button">The button widget.</param>
		/// <returns>Whether the button was pressed.</returns>
		public static bool WasButtonPressed(this UIResults results, Button button)
		{
			return results.WasButtonPressed(button.DestVar);
		}

		/// <summary>
		/// Gets whether a collapse button was pressed according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="button">The collapse button widget.</param>
		/// <returns>Whether the button was pressed.</returns>
		public static bool WasCollapseButtonPressed(this UIResults results, CollapseButton button)
		{
			return results.WasButtonPressed(button.DestVar);
		}

		/// <summary>
		/// Gets whether a user performed any interaction with the widget according to the UI results.
		/// </summary>
		/// <param name="results">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">The interactive widget.</param>
		/// <returns>Whether the user performed any interaction.</returns>
		public static bool WasOnChange(this UIResults results, InteractiveWidget interactiveWidget)
		{
			return results.WasOnChange(interactiveWidget.DestVar);
		}
	}
}