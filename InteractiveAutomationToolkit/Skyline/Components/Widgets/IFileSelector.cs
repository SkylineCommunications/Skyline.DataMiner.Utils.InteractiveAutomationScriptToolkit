namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	/// <summary>
	///     Represents a widget that can be used to upload files.
	/// </summary>
	public interface IFileSelector : IInteractiveWidget, IValidate
	{
		/// <summary>
		///     Gets the paths to the uploaded files if any have been uploaded.
		/// </summary>
		string[] UploadedFilePaths { get; }

		/// <summary>
		///     Gets or sets a value indicating whether multiple files can be uploaded.
		///     Default value: false.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.1.8 and Main Release 10.2.0 onwards.</remarks>
		bool AllowMultipleFiles { get; set; }

		/// <summary>
		///     Gets or sets the text that should be displayed as a placeholder.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		string PlaceHolder { get; set; }

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		///     Copies the uploaded files to the specified folder.
		///     If the folder does not exist, a new one will be created.
		/// </summary>
		/// <param name="folderPath">Path of the folder to where the uploaded files should be copied.</param>
		void CopyUploadedFiles(string folderPath);
	}
}