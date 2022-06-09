namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.IO;

	using Automation;

	/// <summary>
	/// Widget that can be used to upload files to the DMA.
	/// </summary>
	public class FileSelector : InteractiveWidget, IFileSelector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileSelector"/> class.
		/// </summary>
		public FileSelector()
		{
			Type = UIBlockType.FileSelector;
			BlockDefinition.InitialValue = String.Empty;
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
		public string[] UploadedFilePaths { get; private set; } = Array.Empty<string>();

		/// <inheritdoc />
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
					throw new ArgumentNullException(nameof(value));
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults uiResults)
		{
			UploadedFilePaths = uiResults.GetUploadedFilePaths(this) ?? Array.Empty<string>();
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			// Nothing to do
		}
	}
}
