namespace Skyline.DataMiner.Utils.InteractiveAutomationScript.Components
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.ReportsAndDashboards;

	public class DownloadButton : InteractiveWidget
	{
		//private AutomationDownloadButtonOptions downloadButtonOptions;

		private bool downloadStarted;
		private ButtonStyle style;

		public DownloadButton() : this("Download")
		{

		}

		public DownloadButton(string text)
		{
			Type = UIBlockType.DownloadButton;
			Text = text;
			DownloadButtonOptions = new AutomationDownloadButtonOptions();
		}

		public event EventHandler<EventArgs> DownloadStarted
		{
			add
			{
				OnDownloadStarted += value;
			}

			remove
			{
				OnDownloadStarted -= value;
			}
		}

		private event EventHandler<EventArgs> OnDownloadStarted;

		/// <summary>
		///     Gets or sets the text style of the DownloadButton.
		/// </summary>
		public ButtonStyle Style
		{
			get
			{
				return style;
			}

			set
			{
				style = value;
				BlockDefinition.Style = ButtonStyleConverter.StyleToUiString(value);
			}
		}

		/// <summary>
		///     Gets or sets the text displayed in the DownloadButton.
		/// </summary>
		public string Text
		{
			get
			{
				return BlockDefinition.Text;
			}

			set
			{
				BlockDefinition.Text = value;
			}
		}

		/// <summary>
		/// Relative:
		/// path must start with either / or ./ or ../
		/// "/Documents/MyElement/MyDocument.txt" will download the file hosted on URL http(s)://yourdma/Documents/MyElement/MyDocument.txt, which is the file located in C:\Skyline DataMiner\Documents\MyElement\MyDocument.txt".
		/// Absolute:
		/// link to a file that is public accessible on the web
		/// "https://dataminer.services/install/DataMinerCube.exe" will download the latest Cube from DataMiner Services.
		/// </summary>
		public string SourcePath
		{
			get => DownloadButtonOptions.Url;
			set => DownloadButtonOptions.Url = value ?? throw new ArgumentNullException("value");
		}

		public string FileName
		{
			get => DownloadButtonOptions.FileNameToSave;
			set => DownloadButtonOptions.FileNameToSave = value ?? throw new ArgumentNullException("value");
		}

		public bool StartDownloadImmediately
		{
			get => DownloadButtonOptions.StartDownloadImmediately;
			set => DownloadButtonOptions.StartDownloadImmediately = value;
		}

		// Similar to WantsOnChange?
		public bool ReturnWhenDownloadIsStarted
		{
			get => DownloadButtonOptions.ReturnWhenDownloadIsStarted;
			set => DownloadButtonOptions.ReturnWhenDownloadIsStarted = value;
		}

		/// <summary>
		/// Gets or sets the configuration of this <see cref="TimePickerBase" /> instance.
		/// </summary>
		private AutomationDownloadButtonOptions DownloadButtonOptions
		{
			get
			{
				return BlockDefinition.ConfigOptions as AutomationDownloadButtonOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				//downloadButtonOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		internal override void LoadResult(UIResults uiResults)
		{
			downloadStarted = uiResults.HasDownloadStarted(this);
		}

		internal override void RaiseResultEvents()
		{
			if ((OnDownloadStarted != null) && downloadStarted)
			{
				OnDownloadStarted(this, EventArgs.Empty);
			}

			downloadStarted = false;
		}
	}
}
