namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.IO;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget that can be used to upload files to the DMA.
	/// </summary>
	public class FileSelector : InteractiveWidget, IFileSelector
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="FileSelector" /> class.
		/// </summary>
		public FileSelector()
		{
			Type = UIBlockType.FileSelector;
			BlockDefinition.InitialValue = String.Empty;
		}

		/// <inheritdoc />
		public bool AllowMultipleFiles
		{
			get => BlockDefinition.AllowMultipleFiles;
			set => BlockDefinition.AllowMultipleFiles = value;
		}

		/// <inheritdoc />
		public string PlaceHolder
		{
			get => BlockDefinition.PlaceholderText;
			set => BlockDefinition.PlaceholderText = value;
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;

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
		public string[] UploadedFilePaths { get; private set; } = Array.Empty<string>();

		/// <inheritdoc />
		public UIValidationState ValidationState
		{
			get => BlockDefinition.ValidationState;
			set => BlockDefinition.ValidationState = value;
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => BlockDefinition.ValidationText;
			set => BlockDefinition.ValidationText = value;
		}

		/// <inheritdoc />
		public void CopyUploadedFiles(string folderPath)
		{
			if (String.IsNullOrWhiteSpace(folderPath))
			{
				throw new ArgumentException("folderPath");
			}

			var directoryInfo = new DirectoryInfo(folderPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}

			foreach (string filePath in UploadedFilePaths)
			{
				var fileInfo = new FileInfo(filePath);
				string destFileName = directoryInfo.FullName.TrimEnd(Path.DirectorySeparatorChar) +
					Path.DirectorySeparatorChar + fileInfo.Name;
				fileInfo.CopyTo(destFileName);
			}
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			UploadedFilePaths = results.GetUploadedFilePaths(this) ?? Array.Empty<string>();
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			// Nothing to do
		}
	}
}