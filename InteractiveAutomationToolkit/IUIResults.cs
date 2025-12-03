namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Runtime.InteropServices;

	public interface IUIResults
	{
		//
		// Summary:
		//     Gets the string value from the specified destination variable that is linked
		//     to a dialog box item (typically a text box).
		//
		// Parameters:
		//   key:
		//     The name of the destination variable.
		//
		// Returns:
		//     The string value from the specified destination variable that is linked to a
		//     dialog box item.
		string GetString(string key);

		//
		// Summary:
		//     Retrieves the selected upload file path.
		//
		// Parameters:
		//   key:
		//     The key of the file selection UI component to retrieve the selected upload file
		//     paht from.
		//
		// Returns:
		//     The selected upload file path. If the specified key was not found, null is returned.
		//
		//
		// Remarks:
		//     When you have selected a file, the actual upload will only start after you click
		//     a button to make the script continue (e.g.Close, Next, etc.). Once the upload
		//     has started, a Cancel option will appear, allowing you to abort the upload operation.
		//
		//
		//     All files uploaded by users will by default be placed in the C:\Skyline DataMiner\TempDocuments
		//     folder, which is automatically cleared at every DataMiner startup.
		//
		//     Feature introduced in DataMiner 10.0.2 (RN 23950).
		string GetUploadedFilePath(string key);

		string[] GetUploadedFilePaths(string key);

		//
		// Summary:
		//     Returns a value indicating whether a specific button was clicked.
		//
		// Parameters:
		//   key:
		//     The destination variable that is linked to the button.
		//
		// Returns:
		//     true if the button was clicked; otherwise, false.
		bool WasButtonPressed(string key);

		//
		// Summary:
		//     Returns a value indicating whether the user clicked the Back button.
		//
		// Returns:
		//     true if the user clicked the Back button; otherwise, false.
		//
		// Remarks:
		//     Only applicable for scripts that support this feature.
		//
		//     To enable the Back and Forward buttons, from the General Page in the Automation
		//     App, expand Show Details and check the "Supports back/forward buttons in interactive
		//     mode" checkbox.
		bool WasBack();

		//
		// Summary:
		//     Returns a value indicating whether the user clicked the Forward button.
		//
		// Returns:
		//     true if the user clicked the Forward button; otherwise, false.
		//
		// Remarks:
		//     Only applicable for scripts that support this feature.
		//
		//     To enable the Back and Forward buttons, from the General Page in the Automation
		//     App, expand Show Details and check the "Supports back/forward buttons in interactive
		//     mode" checkbox.
		bool WasForward();

		//
		// Summary:
		//     Returns a value indicating whether the user changed the value of a specific dialog
		//     box item.
		//
		// Parameters:
		//   key:
		//     The destination variable that is linked to the specific dialog box item.
		//
		// Returns:
		//     true if the user changed the value; otherwise, false.
		//
		// Remarks:
		//     Use this method to check whether or not the user changed the value of a particular
		//     dialog box item:
		//
		//     • Did the user enter text in a text box?
		//     • Did the user select a value in a selection box?
		//     • Did the user specify a date/time in a calendar item?
		//     • ...
		//
		//     For a .WasOnChange to work, you have to put .WantsOnChange to true. See the example.
		bool WasOnChange(string key);

		//
		// Summary:
		//     Returns true if a dialog box item with the given destination variable and with
		//     the property WantsOnFocusLoss set to true has lost focus.
		//
		// Parameters:
		//   key:
		//     The destination variable that is linked to the specific dialog box item.
		//
		// Returns:
		//     true if the dialog box item has lost focus; otherwise, false.
		//
		// Remarks:
		//     For this method ever to return true, you have to set .WantsOnFocusLoss to true.
		bool WasOnFocusLost(string key);

		//
		// Summary:
		//     Returns true if a download button with the given destination variable and with
		//     the property ReturnWhenDownloadIsStarted on the AutomationDownloadButtonOptions
		//     set to true has started the file download.
		//
		// Parameters:
		//   key:
		//     The destination variable that is linked to the specific download button.
		//
		// Returns:
		//     true if the download button has started the download; otherwise, false.
		//
		// Remarks:
		//     For this method ever to return true, you have to set .ReturnWhenDownloadIsStarted
		//     to true on the AutomationDownloadButtonOptions, in the ConfigOptions of the UIBlockDefinition.
		//     Once the download is started, the control is given to the browser, so there is
		//     no way to know when the download is finished.
		bool WasOnDownloadStarted(string key);

		//
		// Summary:
		//     Returns true if a filter value was entered for a specific dialog box item.
		//
		// Parameters:
		//   key:
		//     The destination variable that is linked to the specific dialog box item.
		//
		// Returns:
		//     true if the user entered a filter value; otherwise, false.
		//
		// Remarks:
		//		Applicable only in case Type is set to DropDown.
		//		Available from DataMiner 10.5.8/10.6.0 onwards, in Automation scripts launched from web apps and specifying the useNewIASInputComponents=true URL parameter.
		//		For WasOnFilter(string) to work, WantsOnFilter has to be set to true. Use GetFilterString(string) to get the filter value. See example.
		//		When the options are filtered, the currently selected option (if still relevant) needs to be inserted as an option, regardless of whether the filter matches. Otherwise, the dropdown will consider that value incorrect, and the dropdown will be cleared.
		bool WasOnFilter(string key);

		//
		// Summary:
		//     Gets a value indicating whether the specified destination variable that is linked
		//     to a checkbox was selected.
		//
		// Parameters:
		//   key:
		//     The destination variable name.
		//
		// Returns:
		//     true if the specified checkbox is selected; otherwise, false.
		bool GetChecked(string key);

		//
		// Summary:
		//     Gets a value indicating whether the specified checkbox list item of the specified
		//     destination variable that is linked to a checkbox list was selected.
		//
		// Parameters:
		//   key:
		//     The value of the checkbox.
		//
		//   value:
		//     true if the specified checkbox list item is selected; otherwise, false.
		//
		// Returns:
		//     true if the specified checkbox list item is selected; otherwise, false.
		bool GetChecked(string key, string value);

		//
		// Summary:
		//     Gets the date/time that was selected in the specified destination variable that
		//     is linked to a Calendar item.
		//
		// Parameters:
		//   key:
		//     The name of the destination variable.
		//
		// Returns:
		//     The date/time that was selected in the specified destination variable that is
		//     linked to a Calendar item.
		//
		// Remarks:
		//     Prior to DataMiner 10.0.12, this method only supports the parsing of datetimes
		//     in the format dd/MM/yyyy HH:mm:ss. From DataMiner 10.0.12 onwards, ISO format
		//     is supported.
		DateTime GetDateTime(string key);

		//
		// Summary:
		//     Gets the key values of the tree view items that are expanded for the specified
		//     key.
		//
		// Parameters:
		//   key:
		//     The name of the destination variable.
		//
		// Returns:
		//     The keys of all expanded tree view items which have the SupportsLazyLoading property
		//     enabled.
		//
		// Remarks:
		//     This method can be used to check whether a tree view node is collapsed or expanded.
		//
		//
		//     Feature introduced in DataMiner 10.1.2 (RN 28132).
		string[] GetExpanded(string key);

		//
		// Summary:
		//     Will return the time zone info the client is in for the UI block with the specified destVar.
		//
		// Parameters:
		//   key:
		//     The name of the destination variable.
		//
		// Returns:
		//     The time zone info for the timezone the client is in.
		//
		// Remarks:
		//     The returned value will be null if the component doesn't exist, ClientTimeInfo isn't set to UIClientTimeInfo.Return or the component doesn't support the information.
		//     When the time zone info provided by the client cannot be deserialized back into a TimeZoneInfo object, a SerializationException will be thrown.
		//
		//     Feature introduced in DataMiner 10.5.4 (RN 42064).
		TimeZoneInfo GetClientTimeZoneInfo(string key);

		//
		// Summary:
		//    Gets the date/time as displayed in the client from the UI block with the specified destVar.
		//
		// Parameters:
		//   key:
		//     The name of the destination variable.
		//
		// Returns:
		//     The date/time that was selected for the specified destination variable, as displayed in the client, which is linked to a Calendar and Time item.
		//
		// Remarks:
		//     Feature introduced in DataMiner 10.5.4 (RN 42064).
		DateTimeOffset GetClientDateTime(string key);

		//
		// Summary:
		//    Gets the filter value from the specified destination variable that is linked to a dialog box item.
		//
		// Parameters:
		//   key:
		//     The name of the destination variable.
		//
		// Returns:
		//     The filter value from the specified destination variable that is linked to a dialog box item.
		//
		// Remarks:
		//		Applicable only in case Type is set to DropDown.
		//		Available from DataMiner 10.5.8/10.6.0 onwards, in Automation scripts launched from web apps and specifying the useNewIASInputComponents=true URL parameter.
		//		When the options are filtered, the currently selected option (if still relevant) needs to be inserted as an option, regardless of whether the filter matches. Otherwise, the dropdown will consider that value incorrect, and the dropdown will be cleared.
		string GetFilterString(string key);
	}
}