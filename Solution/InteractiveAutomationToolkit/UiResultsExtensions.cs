namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.AutomationUI.Objects;

	internal static class UiResultsExtensions
	{
		public static bool GetChecked(this UIResults uiResults, CheckBox checkBox)
		{
			return uiResults.GetChecked(checkBox.DestVar);
		}

		public static DateTime GetDateTime(this UIResults uiResults, DateTimePicker dateTimePicker)
		{
			return uiResults.GetDateTime(dateTimePicker.DestVar);
		}

		public static string GetString(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.GetString(interactiveWidget.DestVar);
		}

		public static string GetUploadedFilePath(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.GetUploadedFilePath(interactiveWidget.DestVar);
		}

		public static bool WasButtonPressed(this UIResults uiResults, Button button)
		{
			return uiResults.WasButtonPressed(button.DestVar);
		}

		public static bool WasCollapseButtonPressed(this UIResults uiResults, CollapseButton button)
		{
			return uiResults.WasButtonPressed(button.DestVar);
		}

		public static bool WasOnChange(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.WasOnChange(interactiveWidget.DestVar);
		}

		public static TimeSpan GetTime(this UIResults uiResults, Time time)
		{
			return DateTime.Parse(uiResults.GetString(time), CultureInfo.InvariantCulture).TimeOfDay;
		}

		public static TimeSpan GetTime(this UIResults uiResults, TimePicker time)
		{
			return DateTime.Parse(uiResults.GetString(time), CultureInfo.InvariantCulture).TimeOfDay;
		}

		public static IEnumerable<string> GetExpandedItemKeys(this UIResults uiResults, TreeView treeView)
		{
			return uiResults.GetExpanded(treeView.DestVar);
		}

		public static IEnumerable<string> GetCheckedItemKeys(this UIResults uiResults, TreeView treeView)
		{
			return uiResults.GetString(treeView.DestVar).Split(';');
		}
	}
}
