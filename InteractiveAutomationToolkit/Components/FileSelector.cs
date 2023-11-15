namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Widget that can be used to upload files to the DMA.
	/// </summary>
	public class FileSelector : InteractiveWidget
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileSelector"/> class.
		/// </summary>
		public FileSelector()
		{
			Type = UIBlockType.FileSelector;
			IsRequired = false;
			BlockDefinition.InitialValue = String.Empty;
		}

		/// <summary>
		/// Gets or sets a value indicating whether multiple files can be selected.
		/// Default value: false.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.1.8 and Main Release 10.2.0 onwards.</remarks>
		public bool AllowMultipleFiles
		{
			get
			{
				return BlockDefinition.AllowMultipleFiles;
			}

			set
			{
				BlockDefinition.AllowMultipleFiles = value;
			}
		}

		/// <summary>
		/// Gets or sets the allowed file name extensions (i.e. '.txt', '.csv', ...).
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.1.12 and Main Release 10.2.0 onwards.</remarks>
		public ICollection<string> AllowedFileNameExtensions
		{
			get
			{
				return BlockDefinition.AllowedFileNameExtensions;
			}

			set
			{
				BlockDefinition.AllowedFileNameExtensions = value.ToList();
			}
		}

		/// <summary>
		/// Gets or sets the maximum allowed file size. An error will be thrown when you try to add a file that is larger than the allowed file size.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.1.12 and Main Release 10.2.0 onwards.</remarks>
		public long MaxFileSizeInBytes
		{
			get
			{
				return BlockDefinition.MaxFileSizeInBytes;
			}

			set
			{
				BlockDefinition.MaxFileSizeInBytes = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the script is allowed to continue without having a file uploaded.
		/// Default value: false.
		/// </summary>
		/// <remarks>
		/// Available from DataMiner Feature Release 10.1.10 and Main Release 10.2.0 onwards.
		/// This value has no effect in Cube. A file upload is always required.
		/// </remarks>
		public bool IsRequired
		{
			get
			{
				return BlockDefinition.IsRequired;
			}

			set
			{
				BlockDefinition.IsRequired = value;
			}
		}

		/// <summary>
		/// Gets the paths to the uploaded files if any have been uploaded.
		/// </summary>
		public string[] UploadedFilePaths { get; private set; } = new string[0];

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		public string Tooltip
		{
			get
			{
				return BlockDefinition.TooltipText;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the text that should be displayed as a placeholder.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		public string PlaceHolder
		{
			get
			{
				return BlockDefinition.PlaceholderText;
			}

			set
			{
				BlockDefinition.PlaceholderText = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		/// 	This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		public UIValidationState ValidationState
		{
			get
			{
				return BlockDefinition.ValidationState;
			}

			set
			{
				BlockDefinition.ValidationState = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the text that is shown if the validation state is invalid.
		/// 	This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		public string ValidationText
		{
			get
			{
				return BlockDefinition.ValidationText;
			}

			set
			{
				BlockDefinition.ValidationText = value;
			}
		}

		/// <summary>
		/// Copies the uploaded files to the specified folder.
		/// If the folder does not exist, a new one will be created.
		/// </summary>
		/// <param name="folderPath">Path of the folder to where the uploaded files should be copied.</param>
		public void CopyUploadedFiles(string folderPath)
		{
			if (String.IsNullOrWhiteSpace(folderPath))
			{
				throw new ArgumentException("folderPath");
			}

			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}

			foreach (string filePath in UploadedFilePaths)
			{
				FileInfo fileInfo = new FileInfo(filePath);
				string destFileName = directoryInfo.FullName.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + fileInfo.Name;
				fileInfo.CopyTo(destFileName);
			}
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		internal override void LoadResult(UIResults uiResults)
		{
			UploadedFilePaths = uiResults.GetUploadedFilePaths(this) ?? new string[0];
		}

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="InteractiveWidget.LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		internal override void RaiseResultEvents()
		{
			// Nothing to do
		}
	}
}
