namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	internal static class UiResultsExtensions
	{
		public static bool GetChecked(this UIResults results, CheckBox checkBox)
		{
			return results.GetChecked(checkBox.DestVar);
		}

		public static string[] GetCheckedItemKeys(this UIResults results, TreeView treeView)
		{
			string result = results.GetString(treeView.DestVar);
			if (String.IsNullOrEmpty(result))
			{
				return Array.Empty<string>();
			}

			return result.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		}

		public static DateTime GetDateTime(this UIResults results, DateTimePicker dateTimePicker)
		{
			return results.GetDateTime(dateTimePicker.DestVar);
		}

		public static string[] GetExpandedItemKeys(this UIResults results, TreeView treeView)
		{
			string[] expandedItems = results.GetExpanded(treeView.DestVar);
			if (expandedItems == null)
			{
				return Array.Empty<string>();
			}

			return expandedItems.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
		}

		public static string GetString(this UIResults results, InteractiveWidget interactiveWidget)
		{
			return results.GetString(interactiveWidget.DestVar);
		}

		public static TimeSpan GetTime(this UIResults results, Time time)
		{
			string receivedTime = results.GetString(time);

			// This try catch is here because of a bug in Dashboards
			// The string that is received from Dashboards is a DateTime (e.g. 2021-11-16T00:00:16.0000000Z), while the string from Cube is an actual TimeSpan (e.g. 1.06:00:03).
			// This means that when using the Time component from Dashboards, you are restricted to 24h and can only enter HH:mm times.
			// See task: 171211
			if (TimeSpan.TryParse(receivedTime, out TimeSpan result))
			{
				return result;
			}

			return DateTime.Parse(receivedTime, CultureInfo.InvariantCulture).TimeOfDay;
		}

		public static TimeSpan GetTime(this UIResults results, TimePicker time)
		{
			return DateTime.Parse(results.GetString(time), CultureInfo.InvariantCulture).TimeOfDay;
		}

		public static string GetUploadedFilePath(this UIResults results, FileSelector interactiveWidget)
		{
			return results.GetUploadedFilePath(interactiveWidget.DestVar);
		}

		public static string[] GetUploadedFilePaths(this UIResults results, FileSelector interactiveWidget)
		{
			return results.GetUploadedFilePaths(interactiveWidget.DestVar);
		}

		public static bool WasButtonPressed(this UIResults results, Button button)
		{
			return results.WasButtonPressed(button.DestVar);
		}

		public static bool WasCollapseButtonPressed(this UIResults results, CollapseButton button)
		{
			return results.WasButtonPressed(button.DestVar);
		}

		public static bool WasOnChange(this UIResults results, InteractiveWidget interactiveWidget)
		{
			return results.WasOnChange(interactiveWidget.DestVar);
		}
	}
}