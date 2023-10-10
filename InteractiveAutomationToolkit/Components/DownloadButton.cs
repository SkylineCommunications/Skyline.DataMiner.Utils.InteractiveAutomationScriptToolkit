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
		private AutomationDownloadButtonOptions downloadButtonOptions;
		private bool downloadStarted;
		private ButtonStyle style;

		/// <summary>
		///     Initializes a new instance of the <see cref="DownloadButton" /> class.
		/// </summary>
		public DownloadButton() : this("Download")
		{

		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DownloadButton" /> class.
		/// </summary>
		public DownloadButton(string text)
		{
			Type = UIBlockType.DownloadButton;
			Text = text;
			DownloadButtonOptions = new AutomationDownloadButtonOptions();
		}

		/// <summary>
		///		Triggered when the file starts downloading in the browser.
		/// </summary>
		public event EventHandler<EventArgs> DownloadStarted
		{
			add
			{
				OnDownloadStarted += value;
				DownloadButtonOptions.ReturnWhenDownloadIsStarted = true;
			}

			remove
			{
				OnDownloadStarted -= value;
				if (OnDownloadStarted == null || !OnDownloadStarted.GetInvocationList().Any())
				{
					DownloadButtonOptions.ReturnWhenDownloadIsStarted = false;
				}
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
		///		Relative: path must start with either / or ./ or ../
		///		"/Documents/MyElement/MyDocument.txt" will download the file hosted on URL http(s)://yourdma/Documents/MyElement/MyDocument.txt, which is the file located in C:\Skyline DataMiner\Documents\MyElement\MyDocument.txt".
		///		Absolute: link to a file that is public accessible on the web.
		///		"https://dataminer.services/install/DataMinerCube.exe" will download the latest Cube from DataMiner Services.
		/// </summary>
		public string RemoteFilePath
		{
			get => DownloadButtonOptions.Url;
			set => DownloadButtonOptions.Url = value ?? throw new ArgumentNullException("value");
		}

		/// <summary>
		///		The filename that will be saved. By default this is the same as the filename of the file on the remote location, but it can be overriden.
		///		Note: overriding the filename is blocked by some browsers when the file to download is on another host (so not on the DataMiner agent). In this case the original filename will be used.
		/// </summary>
		public string DownloadedFileName
		{
			get => DownloadButtonOptions.FileNameToSave;
			set => DownloadButtonOptions.FileNameToSave = value;
		}

		/// <summary>
		///		If set to true (the default is false), the download will start immediately when the widget is displayed. The button stays visible and can be clicked to download the file again.
		/// </summary>
		public bool StartDownloadImmediately
		{
			get => DownloadButtonOptions.StartDownloadImmediately;
			set => DownloadButtonOptions.StartDownloadImmediately = value;
		}

		/// <summary>
		///		Gets or sets the configuration of this <see cref="AutomationDownloadButtonOptions" /> instance.
		/// </summary>
		private AutomationDownloadButtonOptions DownloadButtonOptions
		{
			get
			{
				return downloadButtonOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				downloadButtonOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		protected internal override void LoadResult(UIResults uiResults)
		{
			if (DownloadButtonOptions.ReturnWhenDownloadIsStarted)
			{
				downloadStarted = uiResults.HasDownloadStarted(this);
			}
		}

		protected internal override void RaiseResultEvents()
		{
			if (downloadStarted)
			{
				OnDownloadStarted?.Invoke(this, EventArgs.Empty);
			}

			downloadStarted = false;
		}
	}
}
