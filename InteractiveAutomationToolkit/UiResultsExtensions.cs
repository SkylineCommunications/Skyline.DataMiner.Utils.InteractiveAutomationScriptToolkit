namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
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
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="checkBox">The checkbox widget.</param>
		/// <returns>The state of the checkbox.</returns>
		public static bool GetChecked(this UIResults uiResults, CheckBox checkBox)
		{
			return uiResults.GetChecked(checkBox.DestVar);
		}

		/// <summary>
		/// Gets the value of a date time picker according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="dateTimePicker">The date time picker widget.</param>
		/// <returns>The value of the date time picker.</returns>
		public static DateTime GetDateTime(this UIResults uiResults, DateTimePicker dateTimePicker)
		{
			return uiResults.GetDateTime(dateTimePicker.DestVar);
		}

		/// <summary>
		/// Gets the string representation of the value of an interactive widget according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">The interactive widget.</param>
		/// <returns>The string representation of the value of the interactive widget.</returns>
		public static string GetString(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.GetString(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets the path of a file that was selected according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">The file selector widget.</param>
		/// <returns>The path of a file that was selected.</returns>
		public static string GetUploadedFilePath(this UIResults uiResults, FileSelector interactiveWidget)
		{
			return uiResults.GetUploadedFilePath(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets the paths of all files that were selected according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">the file selector widget.</param>
		/// <returns>All files that were selected.</returns>
		public static string[] GetUploadedFilePaths(this UIResults uiResults, FileSelector interactiveWidget)
		{
			return uiResults.GetUploadedFilePaths(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets whether a button was pressed according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="button">The button widget.</param>
		/// <returns>Whether the button was pressed.</returns>
		public static bool WasButtonPressed(this UIResults uiResults, Button button)
		{
			return uiResults.WasButtonPressed(button.DestVar);
		}

		/// <summary>
		/// Gets whether a collapse button was pressed according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="button">The collapse button widget.</param>
		/// <returns>Whether the button was pressed.</returns>
		public static bool WasCollapseButtonPressed(this UIResults uiResults, CollapseButton button)
		{
			return uiResults.WasButtonPressed(button.DestVar);
		}

		/// <summary>
		/// Gets whether a user performed any interaction with the widget according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">The interactive widget.</param>
		/// <returns>Whether the user performed any interaction.</returns>
		public static bool WasOnChange(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.WasOnChange(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets whether use switched focus away from a widget according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="interactiveWidget">The interactive widget.</param>
		/// <returns>Whether use switched focus.</returns>
		public static bool WasOnFocusLost(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.WasOnFocusLost(interactiveWidget.DestVar);
		}

		/// <summary>
		/// Gets the value of a time widget according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="time">The time widget.</param>
		/// <returns>The value the a time widget.</returns>
		public static TimeSpan GetTime(this UIResults uiResults, Time time)
		{
			string receivedTime = uiResults.GetString(time);
			TimeSpan result;

			// This try catch is here because of a bug in Dashboards
			// The string that is received from Dashboards is a DateTime (e.g. 2021-11-16T00:00:16.0000000Z), while the string from Cube is an actual TimeSpan (e.g. 1.06:00:03).
			// This means that when using the Time component from Dashboards, you are restricted to 24h and can only enter HH:mm times.
			// See task: 171211
			if (TimeSpan.TryParse(receivedTime, out result))
			{
				return result;
			}
			else
			{
				return DateTime.Parse(receivedTime, CultureInfo.InvariantCulture).TimeOfDay;
			}
		}

		/// <summary>
		/// Gets the value of a time picker widget according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="time">The time picker widget.</param>
		/// <returns>The value of the time picker widget.</returns>
		public static TimeSpan GetTime(this UIResults uiResults, TimePicker time)
		{
			return DateTime.Parse(uiResults.GetString(time), CultureInfo.InvariantCulture).TimeOfDay;
		}

		/// <summary>
		/// Gets the names of tree view items that are expanded according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="treeView">The tree view widget.</param>
		/// <returns>The names of tree view items that are expanded.</returns>
		public static IEnumerable<string> GetExpandedItemKeys(this UIResults uiResults, TreeView treeView)
		{
			string[] expandedItems = uiResults.GetExpanded(treeView.DestVar);
			if (expandedItems == null)
			{
				return Array.Empty<string>();
			}

			return expandedItems.Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
		}

		/// <summary>
		/// Gets the names of the tree view items that are checked according to the UI results.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <param name="treeView">The tree view widget.</param>
		/// <returns>The names of tree view items that are checked.</returns>
		public static IEnumerable<string> GetCheckedItemKeys(this UIResults uiResults, TreeView treeView)
		{
			string result = uiResults.GetString(treeView.DestVar);
			if (String.IsNullOrEmpty(result))
			{
				return Array.Empty<string>();
			}

			return result.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
