namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
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
			BlockDefinition.InitialValue = String.Empty;
		}

		/// <summary>
		/// Default value: false
		/// </summary>
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
		///		Gets or sets the text that should be displayed as a placeholder.
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
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
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
		///		Gets or sets the text that is shown if the validation state is invalid.
		///		This should be used by the client to add a visual marker on the input field.
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

		internal override void LoadResult(UIResults uiResults)
		{
			UploadedFilePaths = uiResults.GetUploadedFilePaths(this);
		}

		internal override void RaiseResultEvents()
		{
			// Nothing to do
		}
	}
}
